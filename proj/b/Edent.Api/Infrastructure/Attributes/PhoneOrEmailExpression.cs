using Edent.Api.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Edent.Api.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class PhoneOrEmailExpression : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(string.Format("Invalid format {0}", validationContext.DisplayName));

            string filed = value.ToString();
            if (filed.ToLower().Equals("admin"))
            {
                return ValidationResult.Success;
            }

            bool result = Regex.IsMatch(filed, RegexPatterns.Email) || Regex.IsMatch(filed, RegexPatterns.PhoneNumber);

            if (!result)
            {
                return new ValidationResult(string.Format("Invalid format {0}", validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
