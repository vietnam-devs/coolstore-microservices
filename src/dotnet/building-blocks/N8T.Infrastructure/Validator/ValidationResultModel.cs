using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using FluentValidation.Results;

namespace N8T.Infrastructure.Validator
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
                .ToList();
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}