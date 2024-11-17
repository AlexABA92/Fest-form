using System.ComponentModel.DataAnnotations;

namespace Fest_form.data.Entity
{
    internal class TrimAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value is string str)
            { 
                var trimmedStr = str.Trim(); 
                var property = validationContext.ObjectType.GetProperty(validationContext.MemberName); 
                if (property != null && property.CanWrite) 
                    { property.SetValue(validationContext.ObjectInstance, trimmedStr); 
                } 
            }
            return ValidationResult.Success;
        }
    }
}