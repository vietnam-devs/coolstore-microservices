using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.Auth;
using ProductCatalogService.Application.Common;

namespace ProductCatalogService.Application.SearchProducts
{
    public class SearchProductsQuery : IRequest<SearchProductsResponse>, IAuthRequest
    {
        public string Query { get; set; } = default!;
        public double Price { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public record SearchProductsResponse(int Total, int Page, IEnumerable<ProductDto> Results,
        IEnumerable<SearchAggsByTagsDto> CategoryTags, IEnumerable<SearchAggsByTagsDto> InventoryTags, int ElapsedMilliseconds);

    public class SearchProductsQueryValidator : AbstractValidator<SearchProductsQuery>
    {
    }

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
}
