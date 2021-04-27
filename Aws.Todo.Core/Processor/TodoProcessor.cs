using Aws.Todo.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Aws.Todo.Core
{

    public class TodoProcessor : ITodoProcessor
    {
        private readonly IRepository _repo;
        private readonly ILogger<ITodoProcessor> _logger;
        public TodoProcessor(IRepository repo, ILogger<ITodoProcessor> logger)
        {
            this._repo = repo;
            this._logger = logger;
        }

 
        public  TodoResponse Handle(TodoCommand command)
        {
            try
            {
                //we can add here some safeguard to protect heavy load on _repo..
                IEnumerable<ITodoItem> response = null; 
                switch (command.Operation)
                {
                    case EnumOperation.Add:
                        this.TrySave(command); break;
                         
                    case EnumOperation.Update:
                        this.RenameTask(command);
                        break;
                    case EnumOperation.Delete:
                        this.TryRemove(command);
                        break;
                    case EnumOperation.Complete:
                        this.SetCompletedMode(command, true);
                        break;
                    case EnumOperation.Undo:
                        this.SetCompletedMode(command, false);
                        break;
                    case EnumOperation.List:
                        response =  this.GetItems();
                        break;
                    case EnumOperation.ListCompleted:
                        response =  this.GetItems(true);
                        break;
                    default:
                        throw new Exception($"Unhandled operation {command.Operation}");
                }

                return response == null ? command.CreateSuccessResponse() : command.CreateSuccessResponse(response);                 
 
            }
            catch(Exception ex)
            {
                _logger.LogError($"TodoProcessor failed for operation {command.OperationUUID} on executing {command.Operation}: {ex.ToString()}");
                return command.CreateErrorResponse(ex.Message);
            }
        }



        private IEnumerable<ITodoItem> GetItems(bool onlyCompleted = false)
        {
           return onlyCompleted ? _repo.Get(p => p.IsCompleted) : _repo.Get();                        
        }

        private void SetCompletedMode(TodoCommand cmd, bool isCompleted)
        {
            Validate(cmd);
            var existingItem = _repo.Get(p => p.Title == cmd.Title).FirstOrDefault();
            if(existingItem == null)
                throw new TodoException($"item {cmd.Title} is not exist");
            if(existingItem.IsCompleted == isCompleted)
                throw new TodoException($"cannot set completed/undo to {cmd.Title}. It's already in the same state");

            _repo.Update(cmd.Title, new TodoItem() { Title = cmd.Title, IsCompleted = isCompleted });
        }
               
        private void TryRemove(TodoCommand cmd)
        {
            Validate(cmd);
            _repo.Delete(cmd.Title);
        }

        private void RenameTask(TodoCommand cmd)
        {
            Validate(cmd);
            if(string.IsNullOrEmpty(cmd.NewTitle))
                throw new TodoException("Cannot perform operation. NewTitle is empty");

            var existing = _repo.Get(p => p.Title == cmd.Title).FirstOrDefault();
            if(existing==null)
                throw new TodoException($"item {cmd.Title} is not exist");
            
            //let's make sure we able to add this task as no one else "catch" same name already
            _repo.TryAdd(new TodoItem()
            {
                Title = cmd.NewTitle,
                IsCompleted = existing.IsCompleted
            });
            //now let's remove the original task
            TryRemove(cmd);

        }

        private void TrySave(TodoCommand cmd)
        {
            Validate(cmd);
            _repo.TryAdd(new TodoItem()
            {
                Title = cmd.Title,
                IsCompleted = cmd.IsCompleted
            });
        }


        private void Validate(TodoCommand cmd)
        {
            if (string.IsNullOrEmpty(cmd.Title))
                throw new TodoException("Cannot perform operation. Title is empty");
        }
    }
}
