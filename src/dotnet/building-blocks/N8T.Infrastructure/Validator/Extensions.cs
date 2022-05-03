using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace N8T.Infrastructure.Validator
{
    public static class Extensions
    {
        private static ValidationResultModel ToValidationResultModel(this ValidationResult validationResult)
        {
            return new(validationResult);
        }

        /// <summary>
        /// Ref https://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi
        /// </summary>
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
