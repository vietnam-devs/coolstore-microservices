using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using N8T.Infrastructure.Auth;

namespace ProductCatalogService.Application.GetProductsByPriceAndName
{
    public class GetProductsByPriceAndNameQuery : IRequest<IEnumerable<GetProductsByPriceAndNameItem>>, IAuthRequest
    {
        public int Page { get; set; }
        public double Price { get; set; }
    }

    public class GetProductsByPriceAndNameItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public double Price { get; set; }
        public string ImageUrl { get; set; } = default!;
        public Guid InventoryId  { get; set; }
        public string InventoryLocation { get; set; } = default!;
        public string InventoryWebsite { get; set; } = default!;
        public string InventoryDescription { get; set; } = default!;
        public Guid CategoryId  { get; set; }
        public string CategoryName { get; set; } = default!;
    }

    public class SearchProductsQueryValidator : AbstractValidator<GetProductsByPriceAndNameQuery>
    {
    }
}
