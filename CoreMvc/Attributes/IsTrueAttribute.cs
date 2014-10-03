using System;
using System.ComponentModel.DataAnnotations;

namespace CoreMvc.Attributes
{
    /// <summary>
    /// Attribute for validating that a boolean property has a value of TRUE.
    /// </summary>
    public class IsTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value.GetType() != typeof(bool))
            {
                throw new InvalidOperationException("Can only be used on boolean properties.");
            }

            return (bool)value;
        }
    }
}
