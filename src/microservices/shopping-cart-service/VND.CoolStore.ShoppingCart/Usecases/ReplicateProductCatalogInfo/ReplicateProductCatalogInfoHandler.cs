using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using MediatR;
using VND.CoolStore.ShoppingCart.Data;

namespace VND.CoolStore.ShoppingCart.Usecases.ReplicateProductCatalogInfo
{
    public class ReplicateProductCatalogInfoHandler : IRequestHandler<ReplicateProductCatalogInfo, ReplicateProductCatalogInfoResult>
    {
        private readonly IEfUnitOfWork<ShoppingCartDataContext> _unitOfWork;

        public ReplicateProductCatalogInfoHandler(IEfUnitOfWork<ShoppingCartDataContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReplicateProductCatalogInfoResult> Handle(ReplicateProductCatalogInfo request, CancellationToken cancellationToken)
        {
            var queryRepository = _unitOfWork.QueryRepository<Domain.ProductCatalog.ProductCatalog, Guid>();
            var product = await queryRepository.FindOneAsync<ShoppingCartDataContext, Domain.ProductCatalog.ProductCatalog, Guid>(x => x.ProductId == request.Id);
            if (product != null)
            {
                var repository = _unitOfWork.RepositoryAsync<Domain.ProductCatalog.ProductCatalog, Guid>();
                var updated = product.ReplicateProductCatalog(request);
                await repository.UpdateAsync(updated);
                var rowCount = await _unitOfWork.SaveChangesAsync(cancellationToken);
                if (rowCount <= 0)
                {
                    throw new Exception("Could not mark data as deleted.");
                }
            }

            return new ReplicateProductCatalogInfoResult();
        }
    }
}
