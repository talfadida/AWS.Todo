using Aws.Todo.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aws.Todo.Core
{
    

    public class InMemoryRepository : IRepository
    {

        ConcurrentDictionary<string, ITodoItem> repo = new ConcurrentDictionary<string, ITodoItem>();
        private readonly ILogger<IRepository> _logger;

        public InMemoryRepository(ILogger<IRepository> logger)
        {
            this._logger = logger;
        }

        public bool TryAdd(ITodoItem todoItem)
        {
            try
            {

                if (todoItem == null)
                    throw new TodoException("todoItem is null");

                if (string.IsNullOrEmpty(todoItem.Title))
                    throw new TodoException("Item must have title");

                if (!repo.TryAdd(todoItem.Title, todoItem))
                    throw new TodoException($"item '{todoItem.Title}' already exist");

                _logger.LogInformation($"item '{todoItem.Title}' was added");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error happened: " + ex.Message);
                throw ex;
            }
        }

        public void Delete(string key)
        {
            try
            {

                if (string.IsNullOrEmpty(key))
                    throw new TodoException("title is missing");

                if (!repo.TryRemove(key, out var itemRemoved))
                    throw new TodoException($"item '{key}' is not exist");

                _logger.LogTrace($"item '{key}' was removed");


            }
            catch (Exception ex)
            {
                _logger.LogError("Error happened: " + ex.Message);
                throw ex;
            }
        }

        public IEnumerable<ITodoItem> Get(Func<ITodoItem, bool> predicate)
        {
            return repo.Values.Where(p => predicate(p));
        }
        public IEnumerable<ITodoItem> Get()
        {
            return repo.Values;
        }

        public void Update(string key, ITodoItem item)
        {
            if (!repo.ContainsKey(key))
                throw new TodoException($"item '{key}' not exist; update failed");
            repo[key] = item;

        }
    }
}