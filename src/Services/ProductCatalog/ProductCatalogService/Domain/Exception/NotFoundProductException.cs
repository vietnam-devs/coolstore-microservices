using System;
using N8T.Domain;

namespace ProductCatalogService.Domain.Exception
{
    public class NotFoundProductException : CoreException
    {
        public NotFoundProductException(Guid id) : this($"The product with id={id} is not found.")
        {

        }

        public NotFoundProductException(string message) : base(message)
        {
        }
    }
}
