using System.Collections.Generic;
using FluentValidation;
using MediatR;
using N8T.Infrastructure.Auth;
using ProductCatalogService.Application.Common;
using ProductCatalogService.Domain.Dto;

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
}
