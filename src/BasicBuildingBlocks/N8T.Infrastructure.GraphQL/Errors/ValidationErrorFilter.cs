using HotChocolate;
using N8T.Infrastructure.Validator;

namespace N8T.Infrastructure.GraphQL.Errors
{
    public class ValidationErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Exception is ValidationException exception)
            {
                return error.AddExtension("ValidationError",
                    exception.ValidationResultModel.ToString());
            }

            return error;
        }
    }
}