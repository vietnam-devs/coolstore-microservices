using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using N8T.Infrastructure.Auth;
using ProductCatalogService.Domain.Dto;

namespace ProductCatalogService.Application.GetProductsByPriceAndName
{
    public class GetProductsByPriceAndNameQuery : IRequest<IEnumerable<FlatProductDto>>, IAuthRequest
    {
        public int Page { get; set; }
        public double Price { get; set; }
    }

    public record GetInventoryByIdsRequest(IEnumerable<Guid> InventoryIds);

    public class SearchProductsQueryValidator : AbstractValidator<GetProductsByPriceAndNameQuery>
    {
    }

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
}
