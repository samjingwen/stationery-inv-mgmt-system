using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7ADProject.Validation
{
    public class UniqueSupplier : ValidationAttribute
    {
        public List<string> CompareProperties { get; set; }
        public UniqueSupplier(string compareproperty)
        {
            CompareProperties = compareproperty.Split(',').ToList();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (var compareProperty in CompareProperties)
            {
                var otherProperty = validationContext.ObjectType.GetProperty(compareProperty);
                if (otherProperty == null)
                    return new ValidationResult(String.Format("Unknown property: {0}.", otherProperty));
                // get the other value
                var other = otherProperty.GetValue(validationContext.ObjectInstance, null);

                if (other.Equals(value))
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }
}