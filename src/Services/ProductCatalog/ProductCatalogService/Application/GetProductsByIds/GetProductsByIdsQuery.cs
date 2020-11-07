using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace ProductCatalogService.Application.GetProductsByIds
{
    public class GetProductsByIdsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public List<Guid> ProductIds { get; set; } = new List<Guid>();
    }

    public class GetProductsByIdsValidator : AbstractValidator<GetProductsByIdsQuery>
    {
        public GetProductsByIdsValidator()
        {
        }
    }
}
