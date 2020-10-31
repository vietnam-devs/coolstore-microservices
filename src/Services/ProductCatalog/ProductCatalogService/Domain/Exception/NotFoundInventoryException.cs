using System;
using N8T.Domain;

namespace ProductCatalogService.Domain.Exception
{
    public class NotFoundInventoryException : CoreException
    {
        public NotFoundInventoryException(Guid id) : this($"The inventory with id={id} is not found.")
        {

        }

        public NotFoundInventoryException(string message) : base(message)
        {
        }
    }
}
