using System;
using System.Collections.Generic;

namespace Bookchin.Library.API.Exceptions
{
    public class ModelValidationException : ApplicationException
    {
        public List<string> Errors { get { return _errors; } }

        private List<string> _errors;

        public ModelValidationException(List<string> errors)
        {
            _errors = errors;
        }

        public ModelValidationException(string message)
            : base(message)
        {
            _errors = new List<string> {
                message
            };
        }

        public ModelValidationException(string message, Exception inner)
            : base(message, inner)
        {
            _errors = new List<string> {
                message
            };
        }
    }
}
