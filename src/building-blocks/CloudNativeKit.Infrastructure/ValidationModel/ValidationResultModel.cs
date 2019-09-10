using FluentValidation.Results;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CloudNativeKit.Infrastructure.ValidationModel
{
    public class ValidationResultModel
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
        public string Message { get; set; } = "Validation Failed.";

        public List<ValidationError> Errors { get; }

        public ValidationResultModel(ValidationResult validationResult = null)
        {
            Errors = validationResult.Errors
                    .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage))
                    .ToList()
                ?? new List<ValidationError>();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
