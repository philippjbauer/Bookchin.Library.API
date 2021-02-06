using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bookchin.Library.API.Exceptions;

namespace Bookchin.Library.API.Helpers
{
    public class ModelValidationHelpers
    {
        public static bool HasRequiredClassProperties(dynamic @class, List<string> requiredProperties)
        {
            List<string> errors = new List<string>();

            List<PropertyInfo> properties = new List<PropertyInfo>(
                    @class.GetType().GetProperties()
                )
                .Where(p => requiredProperties.Contains(p.Name))
                .ToList();

            foreach (PropertyInfo property in properties)
            {
                string value = property
                    .GetValue(@class, null)?
                    .ToString()
                    .Trim();

                if (
                    string.IsNullOrEmpty(value) == true
                    || value == Guid.Empty.ToString()
                )
                {
                    errors.Add($"Property \"{property.Name}\" of type {@class.GetType().Name} can't be null or empty.");
                }
            }

            if (errors.Count() > 0)
            {
                throw new ModelValidationException(errors);
            }

            return true;
        }
    }
}