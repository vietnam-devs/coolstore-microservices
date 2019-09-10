using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace CloudNativeKit.Infrastructure.ValidationModel
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public RequestValidationBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await _validator.HandleValidation(request);
            return await next();
        }
    }
}
