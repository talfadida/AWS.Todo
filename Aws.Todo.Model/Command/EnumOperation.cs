using System;

namespace Aws.Todo.Model
{
    [Serializable]
    public enum EnumOperation
    {
        Add,
        Complete,
        Undo,
        Delete,
        List,
        ListCompleted,
        Update,
        Unknown,
    }


}
