using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GemspaceBlog.Infrastructure
{
    public class MaxFileSizeValidator : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeValidator(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;

            if (file != null)
            {
                if (file.ContentLength > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is { _maxFileSize/1048576} MB.";
        }
    }
}