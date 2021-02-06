using System;
using System.Collections.Generic;

namespace Bookchin.Library.API.Exceptions
{
    public class ModelRecordNotFoundException : ApplicationException
    {
        public ModelRecordNotFoundException(string message)
            : base(message)
        {
        }

        public ModelRecordNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
