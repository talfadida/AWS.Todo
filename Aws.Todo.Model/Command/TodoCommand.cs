using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aws.Todo.Model
{

    [Serializable]
    public class TodoCommand : TodoOperation
    {
        public string Title { get; set; }
        public string NewTitle { get; set; }
        public bool IsCompleted { get; set; }


    }


}
