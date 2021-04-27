using System;

namespace Aws.Todo.Model
{


    [Serializable]
    public class TodoItem : ITodoItem
    {
        public string Title { get; set; }               
        public bool IsCompleted { get; set; }
        
        

    }
}
