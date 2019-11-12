using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CorrelationId;
using MediatR;
using Microsoft.Extensions.Configuration;
using Nest;
using VND.CoolStore.Search.DataContracts.Api.V1;
using VND.CoolStore.Search.DataContracts.Dto.V1;

namespace VND.CoolStore.Search.Usecases.SearchProduct
{
    public class SearchProductHandler : IRequestHandler<SearchProductRequest, SearchProductResponse>
    {
        private readonly IElasticClient _client;
        private readonly ICorrelationContextAccessor _correlationContext;

        public SearchProductHandler(IConfiguration config, ICorrelationContextAccessor correlationContext)
        {
            var connString = config.GetValue<string>("ElasticSearch:Connection");
            var settings = new ConnectionSettings(new Uri(connString))
                .DefaultMappingFor<SearchProductModel>(i => i
                    .IndexName("product")
                )
                .PrettyJson();
            _client = new ElasticClient(settings);
            _correlationContext = correlationContext;
        }

        public async Task<SearchProductResponse> Handle(SearchProductRequest request, CancellationToken cancellationToken)
        {
            Func<QueryContainerDescriptor<SearchProductModel>, QueryContainer> queryWithNameAndDesc = q => q
                .MultiMatch(mm => mm
                        .Query(request.Query)
                            .Fields(f => f
                                .Fields(f1 => f1.Name, f2 => f2.Description)));

            var result = await _client.SearchAsync<SearchProductModel>(s => s
                .Query(q =>
                    request.Query != "*" ?
                        queryWithNameAndDesc(q) && q.Range(ra => ra
                            .Field(f => f.Price)
                            .LessThanOrEquals(request.Price)
                        ) :
                        q.Range(ra => ra
                        .Field(f => f.Price)
                        .LessThanOrEquals(request.Price))
                )
                .Aggregations(a => a
                    .Terms("category_tags", t => t
                        .Field(f => f.Category.Name.Suffix("keyword"))
                    ) && a
                    .Terms("inventory_tags", t => t
                        .Field(f => f.Inventory.Location.Suffix("keyword"))
                    )
                )
                .From(request.Page - 1)
                .Size(request.PageSize)
            );

            if (result.ApiCall.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                throw new Exception("Could not query data");
            }

            var categoryTags = result
                .Aggregations
                .Terms("category_tags")
                .Buckets
                .Select(b => new SearchAggsByTags { Key = b.Key, Count = (int)b.DocCount })
                .ToList();

            var inventoryTags = result
                .Aggregations
                .Terms("inventory_tags")
                .Buckets
                .Select(b => new SearchAggsByTags { Key = b.Key, Count = (int)b.DocCount })
                .ToList();

            var items = result
                .Hits
                .Select(x => new SearchProductModel
                {
                    Id = x.Source.Id.ToString(),
                    Name = x.Source.Name,
                    Description = x.Source.Description,
                    Price = x.Source.Price,
                    ImageUrl = x.Source.ImageUrl,
                    Category = new SearchCategoryModel
                    {
                        Id = x.Source.Category != null ? x.Source.Category.Id.ToString() : string.Empty,
                        Name = x.Source.Category != null ? x.Source.Category.Name : string.Empty
                    },
                    Inventory = new SearchInventoryModel
                    {
                        Id = x.Source.Inventory != null ? x.Source.Inventory.Id.ToString() : string.Empty,
                        Description = x.Source.Inventory != null ? x.Source.Inventory.Description : string.Empty,
                        Location = x.Source.Inventory != null ? x.Source.Inventory.Location : string.Empty,
                        Website = x.Source.Inventory != null ? x.Source.Inventory.Website : string.Empty
                    }
                })
                .ToList();

            var response =  new SearchProductResponse
            {
                Page = (int)(result.Total / request.PageSize) + 1,
                ElapsedMilliseconds = (int)result.Took,
                Total = result.Documents.Count,
            };

            response.Results.AddRange(items.ToArray());
            response.CategoryTags.AddRange(categoryTags.ToArray());
            response.InventoryTags.AddRange(inventoryTags.ToArray());
            response.CorrelationId = _correlationContext.CorrelationContext.CorrelationId;

            return response;
        }
    }
}
