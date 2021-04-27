using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Aws.Todo.Model
{
    [Serializable]
    public class TodoResponse : TodoOperation
    {

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public List<TodoItem> TaskList { get; set; }

       
    }
}

