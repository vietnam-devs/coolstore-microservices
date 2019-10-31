using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Nest;
using VND.CoolStore.Search.DataContracts.Api.V1;
using VND.CoolStore.Search.DataContracts.Dto.V1;

namespace VND.CoolStore.Search.Usecases.SearchProduct
{
    public class SearchProductHandler : IRequestHandler<SearchProductRequest, SearchProductResponse>
    {
        private readonly IConfiguration _config;

        public SearchProductHandler(IConfiguration config)
        {
            _config = config;
        }

        public async Task<SearchProductResponse> Handle(SearchProductRequest request, CancellationToken cancellationToken)
        {
            var connString = _config.GetValue<string>("ElasticSearch:Connection");

            var settings = new ConnectionSettings(new Uri(connString))
                .DefaultMappingFor<SearchProductModel>(i=>i
                    .IndexName("product")
                )
                .PrettyJson();

            var client = new ElasticClient(settings);
            

            // index
            //await IndexData(client);

            // search
            var result = await client.SearchAsync<SearchProductModel>(s => s
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Query(request.Query)
                            .Fields(f => f
                                .Fields(f1 => f1.Name, f2 => f2.Description))
                    )
                    && q
                    .Range(ra => ra
                        .Field(f => f.Price)
                        .LessThanOrEquals(request.Price)
                    )
                )
                .Aggregations(a => a
                    .Terms("tags", t => t
                        .Field(f => f.Category.Name.Suffix("keyword"))
                    )
                )
                .From(request.Page - 1)
                .Size(request.PageSize)
            );

            var tags = result
                .Aggregations
                .Terms("tags")
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
                        Id = x.Source.Category.Id.ToString(),
                        Name = x.Source.Category.Name
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
            response.CategoryTags.AddRange(tags.ToArray());

            return response;
        }

        // TODO: move to migration project
        private async Task IndexData(ElasticClient client)
        {
            var products = new List<SearchProductModel>
            {
                new SearchProductModel{
                    Id = Guid.NewGuid().ToString(),
                    Name = "product a",
                    Description = "this is a product a",
                    Price = 100,
                    ImageUrl = "http://a.com/1.jpg",
                    Category = new SearchCategoryModel{
                        Id = Guid.NewGuid().ToString(),
                        Name = "cat1"
                    }
                },
                new SearchProductModel{
                    Id = Guid.NewGuid().ToString(),
                    Name = "product b",
                    Description = "this is a product b",
                    Price = 120,
                    ImageUrl = "http://a.com/2.jpg",
                    Category = new SearchCategoryModel{
                        Id = Guid.NewGuid().ToString(),
                        Name = "cat2"
                    }
                },
                new SearchProductModel{
                    Id = Guid.NewGuid().ToString(),
                    Name = "product c",
                    Description = "this is a product c",
                    Price = 200,
                    ImageUrl = "http://a.com/3.jpg",
                    Category = new SearchCategoryModel{
                        Id = Guid.NewGuid().ToString(),
                        Name = "cat1"
                    }
                }
            };

            var a = await client.IndexDocumentAsync(products[0]);
            var b = await client.IndexDocumentAsync(products[1]);
            var c = await client.IndexDocumentAsync(products[2]);

            await Task.CompletedTask;
        }
    }
}
