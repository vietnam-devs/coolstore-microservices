using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace N8T.Infrastructure.Validator
{
    public static class Extensions
    {
        private static ValidationResultModel ToValidationResultModel(this ValidationResult validationResult)
        {
            return new ValidationResultModel(validationResult);
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

        public static IServiceCollection AddCustomValidators<TType>(this IServiceCollection services)
        {
            return services.Scan(scan => scan
                .FromAssemblyOf<TType>()
                .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }
    }
}
