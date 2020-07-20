using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace GemspaceBlog.Infrastructure
{
    public class AllowedFileTypeValidator : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedFileTypeValidator(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()) && file.ContentLength > (2 * 1024 *1024))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This file is not a photo or it's larger than 2MB";
        }
    }
}