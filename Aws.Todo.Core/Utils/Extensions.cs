using Aws.Todo.Model;
using System.Collections.Generic;
using System.Linq;

namespace Aws.Todo.Core
{
    public static class Extensions
    {
        public static TodoResponse CreateSuccessResponse(this TodoCommand cmd)
        {
            return new TodoResponse()
            {
                OperationUUID = cmd.OperationUUID,
                IsSuccess = true,
                Operation = cmd.Operation                 
            };
        }

        public static TodoResponse CreateSuccessResponse(this TodoCommand cmd, IEnumerable<ITodoItem> list)
        {
            return new TodoResponse()
            {
                OperationUUID = cmd.OperationUUID,
                IsSuccess = true,
                Operation = cmd.Operation,                
                TaskList = list.Cast<TodoItem>().ToList() 
            };
        }

        public static TodoResponse CreateErrorResponse(this TodoCommand cmd, string error)
        {
            return new TodoResponse()
            {
                OperationUUID = cmd.OperationUUID,
                IsSuccess = false,
                Operation = cmd.Operation,
                ErrorMessage = error
            };
        }

    }
}
