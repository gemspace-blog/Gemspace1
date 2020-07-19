using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GemspaceBlog.Infrastructure
{
    public class CategoryValidator : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((string)value == "Basketball" || (string)value == "Nature" || (string)value == "Coding" || (string)value == "Food")
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("The category that you provided is invalid.");
            }
        }
    }
}