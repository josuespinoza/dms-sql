using System;
using System.Runtime.Serialization;

namespace SCG.DMSOne.Framework.MenuManager
{
    public class RecursiveMenuException : Exception
    {
        public override string Message
        {
            get { return "Invalid menu definition. FatherID is the same as MenuId."; }
        }

        public RecursiveMenuException()
        {
        }

        public RecursiveMenuException(string message) : base(message)
        {
        }

        public RecursiveMenuException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RecursiveMenuException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}