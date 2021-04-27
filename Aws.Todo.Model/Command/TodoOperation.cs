using System;

namespace Aws.Todo.Model
{

    public class TodoOperation
    {
        public Guid OperationUUID { get; set; } = Guid.NewGuid();
        public EnumOperation Operation { get; set; }
 
    }



}
