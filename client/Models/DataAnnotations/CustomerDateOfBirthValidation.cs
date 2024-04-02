using System;
using System.ComponentModel.DataAnnotations;

namespace client.Models.DataAnnotations
{
    public class CustomerDateOfBirthValidation : ValidationAttribute
    {
        public const string MINIMUM_DATE_OF_BIRTH = "The customer is younger than 18 years old";

        /// <summary>
        /// Minimum age 
        /// </summary>
        private int minAge = 18;

        /// <summary>
        /// Whether the date of birth is valid.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !(value is DateTime))
            {
                return ValidationResult.Success;
            }

            DateTime dob = (DateTime)value;

            var minDateOfBirth = DateTime.Now.Date.AddYears(-minAge);

            if (dob > minDateOfBirth)
            {
                return new ValidationResult(MINIMUM_DATE_OF_BIRTH);
            }

            return ValidationResult.Success;
        }
    }
}
