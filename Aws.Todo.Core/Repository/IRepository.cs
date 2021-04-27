using Aws.Todo.Model;
using System;
using System.Collections.Generic;

namespace Aws.Todo.Core
{
    public interface IRepository
    {


        //define crud operations
        bool TryAdd(ITodoItem item);
        void Update(string key, ITodoItem item);
        void Delete(string key);

        IEnumerable<ITodoItem> Get(Func<ITodoItem, bool> predicate);

        IEnumerable<ITodoItem> Get();
        
    }
}