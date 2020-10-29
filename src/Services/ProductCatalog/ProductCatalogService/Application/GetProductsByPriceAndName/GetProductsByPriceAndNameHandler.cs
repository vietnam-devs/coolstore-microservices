using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Infrastructure.Persistence;

namespace ProductCatalogService.Application.GetProductsByPriceAndName
{
    public class GetCategoriesAuthzHandler : AuthorizationHandler<GetProductsByPriceAndNameQuery>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GetProductsByPriceAndNameQuery requirement)
        {
            if (context.User.HasClaim("user_role", "sys_admin") is true)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }

    public class GetProductsByPriceAndNameHandler : IRequestHandler<GetProductsByPriceAndNameQuery, IEnumerable<GetProductsByPriceAndNameItem>>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

        public GetProductsByPriceAndNameHandler(IDbContextFactory<MainDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<IEnumerable<GetProductsByPriceAndNameItem>> Handle(GetProductsByPriceAndNameQuery request, CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var products = await dbContext.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Skip(request.Page - 1)
                .Take(10)
                .Where(x => !x.IsDeleted && x.Price <= request.Price)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            return products.Select(x =>
            {
                return new GetProductsByPriceAndNameItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    CategoryId = x.Category.Id,
                    CategoryName = x.Category.Name

                    //TODO:
                    //InventoryId = inventory?.Id,
                    //InventoryLocation = inventory?.Location,
                    //InventoryWebsite = inventory?.Website,
                    //InventoryDescription = inventory?.Description
                };
            });
        }
    }
}
