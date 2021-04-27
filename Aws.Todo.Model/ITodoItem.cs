namespace Aws.Todo.Model
{

    public interface ITodoItem
    {
       
        string Title { get; set; }
        bool IsCompleted { get; set; }
        
    }
}