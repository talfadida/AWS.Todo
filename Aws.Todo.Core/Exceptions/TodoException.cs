using System;
using System.Runtime.Serialization;

namespace Aws.Todo.Core
{
    [Serializable]
    internal class TodoException : Exception
    {
        public TodoException()
        {
        }

        public TodoException(string message) : base(message)
        {
        }

        public TodoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TodoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}