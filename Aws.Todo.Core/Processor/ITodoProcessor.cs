using Aws.Todo.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aws.Todo.Core
{
    public interface ITodoProcessor
    {
        TodoResponse Handle(TodoCommand command);
    }
}
