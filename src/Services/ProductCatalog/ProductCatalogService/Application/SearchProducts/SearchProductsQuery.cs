using System;
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

    public record SearchProductsResponse(int Total, int Page, IEnumerable<SearchProductModel> Results,
        IEnumerable<SearchAggsByTagsDto> CategoryTags, IEnumerable<SearchAggsByTagsDto> InventoryTags, int ElapsedMilliseconds);
    public record SearchCategoryModel (Guid Id, string Name);
    public record SearchInventoryModel(Guid Id, string Location, string Website, string? Description);
    public record SearchProductModel(Guid Id, string Name, double Price, string ImageUrl, string Description,
        SearchCategoryModel Category, SearchInventoryModel Inventory);

    public class SearchProductsQueryValidator : AbstractValidator<SearchProductsQuery>
    {
    }
}
