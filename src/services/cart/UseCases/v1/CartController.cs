using Microsoft.AspNetCore.Mvc;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.EfCore.Repository;

namespace VND.CoolStore.Services.Cart.UseCases.v1
{
		[ApiVersion("1.0")]
		[Route("api/v{api-version:apiVersion}/carts")]
		public class CartController : CrudControllerBase<Domain.Cart>
		{
				public CartController(
						IEfQueryRepository<Domain.Cart> queryRepository,
						IEfRepositoryAsync<Domain.Cart> mutateRepository) 
						: base(queryRepository, mutateRepository)
				{
				}
    }
}