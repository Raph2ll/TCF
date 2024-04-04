using System;
using System.ComponentModel.DataAnnotations;

namespace client.Models.DataAnnotations
{
    public class CustomerDateOfBirthValidation : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dob = (DateTime)value;

            if (value == null || !(value is DateTime) || dob <= DateTime.MinValue.Date)
            {
                return new ValidationResult("Date of birth is required.");
            }

            return ValidationResult.Success;
        }

    }
}
