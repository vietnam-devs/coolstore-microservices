using FluentValidation;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace CloudNativeKit.Infrastructure.ValidationModel
{
    /// <summary>
    /// Ref https://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi
    /// </summary>
    public static class ValidationModelExtensions
    {
        public static ValidationResultModel ToValidationResultModel(this ValidationResult validationResult)
        {
            return new ValidationResultModel(validationResult);
        }

        public static async Task HandleValidation<TRequest>(this IValidator<TRequest> validator, TRequest request)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToValidationResultModel());
            }
        }
    }
}
