using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace ProductCatalogService.Application.SearchProducts
{
    public class GetCategoriesAuthzHandler : AuthorizationHandler<SearchProductsQuery>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SearchProductsQuery requirement)
        {
            if (context.User.HasClaim("user_role", "sys_admin") is true)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class SearchProductsHandler : IRequestHandler<SearchProductsQuery, SearchProductsResponse>
    {
        public async Task<SearchProductsResponse> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new SearchProductsResponse());
        }
    }
}
