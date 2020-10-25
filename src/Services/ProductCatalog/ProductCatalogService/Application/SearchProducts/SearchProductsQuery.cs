using System.Collections.Generic;
using FluentValidation;
using MediatR;
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

    public class SearchProductsResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public IEnumerable<ProductDto> Results { get; set; } = new List<ProductDto>();
        public IEnumerable<SearchAggsByTagsDto> CategoryTags { get; set; } = new List<SearchAggsByTagsDto>();
        public IEnumerable<SearchAggsByTagsDto> InventoryTags { get; set; } = new List<SearchAggsByTagsDto>();
        public int ElapsedMilliseconds  { get; set; }
        public string CorrelationId { get; set; } = default!;
    }

    public class SearchProductsQueryValidator : AbstractValidator<SearchProductsQuery>
    {
    }
}
