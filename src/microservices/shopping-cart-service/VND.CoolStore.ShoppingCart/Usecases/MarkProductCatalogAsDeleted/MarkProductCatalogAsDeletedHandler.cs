using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using VND.CoolStore.ShoppingCart.Data;

namespace VND.CoolStore.ShoppingCart.Usecases.MarkProductCatalogAsDeleted
{
    public class MarkProductCatalogAsDeletedHandler : IRequestHandler<MarkProductCatalogAsDeleted, MarkProductCatalogAsDeletedResult>
    {
        private readonly IEfUnitOfWork<ShoppingCartDataContext> _unitOfWork;
        private readonly ILogger<MarkProductCatalogAsDeletedHandler> _logger;

        public MarkProductCatalogAsDeletedHandler(IEfUnitOfWork<ShoppingCartDataContext> unitOfWork, ILogger<MarkProductCatalogAsDeletedHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<MarkProductCatalogAsDeletedResult> Handle(MarkProductCatalogAsDeleted request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Domain.ProductCatalog.ProductCatalog, Guid>();
            var product = await queryRepository.FindOneAsync<ShoppingCartDataContext, Domain.ProductCatalog.ProductCatalog, Guid>(x => x.ProductId == request.ProductId);
            if (product != null)
            {
                var repository = _unitOfWork.RepositoryAsync<Domain.ProductCatalog.ProductCatalog, Guid>();
                var updated = product.MarkAsDeleted();
                await repository.UpdateAsync(updated);
                var rowCount = await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"{rowCount} rows are just updated to the database.");
            }
            return new MarkProductCatalogAsDeletedResult();
        }
    }
}
