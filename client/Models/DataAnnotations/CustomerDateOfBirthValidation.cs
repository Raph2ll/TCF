using System;
using System.ComponentModel.DataAnnotations;

namespace client.Models.DataAnnotations
{

    public class CustomerDateOfBirthValidation : ValidationAttribute
    {
        public const string MINIMUM_DATE_OF_BIRTH = "The customer is younger than 18 years old";
        public const string MAXIMUM_DATE_OF_BIRTH = "The customer is older than 200 years old";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is DateTime))
            {
                return new ValidationResult("Invalid date of birth format.");
            }

            DateTime dob = (DateTime)value;

            var currentDate = DateTime.Now.Date;
            var oldestAllowedDateOfBirth = currentDate.AddYears(-200);

            if (dob > currentDate || dob < oldestAllowedDateOfBirth)
            {
                return new ValidationResult("Date of birth is outside the allowed range.");
            }

            return ValidationResult.Success;
        }

    }
}
