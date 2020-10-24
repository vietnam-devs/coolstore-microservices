using System;
using System.Linq;
using System.Linq.Expressions;
using HotChocolate;
using HotChocolate.Configuration;
using HotChocolate.Execution.Configuration;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Filters;
using HotChocolate.Types.Sorting;
using HotChocolate.Utilities;
using Microsoft.Extensions.DependencyInjection;
using N8T.Infrastructure.GraphQL.Errors;
using N8T.Infrastructure.GraphQL.OffsetPaging;

namespace N8T.Infrastructure.GraphQL
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomGraphQL(this IServiceCollection services,
            Action<ISchemaConfiguration> schemaConfiguration,
            Action<IServiceCollection> doMoreActions = null)
        {
            services.AddDataLoaderRegistry();

            services.AddGraphQL(sp => Schema.Create(c =>
                    {
                        c.RegisterServiceProvider(sp);
                        schemaConfiguration.Invoke(c);
                        c.RegisterExtendedScalarTypes();
                    }),
                    new QueryExecutionOptions
                    {
                        IncludeExceptionDetails = true, TracingPreference = TracingPreference.Always
                    })
                .AddErrorFilter<ValidationErrorFilter>();

            doMoreActions?.Invoke(services);

            return services;
        }

        public static IServiceCollection AddCustomGraphQL<TQuery, TMutation>(this IServiceCollection services,
            Action<ISchemaConfiguration> schemaConfiguration,
            Action<IServiceCollection> doMoreActions = null)
            where TQuery: ObjectType
            where TMutation: ObjectType
        {
            services.AddDataLoaderRegistry();

            services.AddGraphQL(sp => Schema.Create(c =>
                    {
                        c.RegisterServiceProvider(sp);
                        c.RegisterQueryType<TQuery>();
                        c.RegisterMutationType<TMutation>();
                        schemaConfiguration.Invoke(c);
                        c.RegisterExtendedScalarTypes();
                    }),
                    new QueryExecutionOptions
                    {
                        IncludeExceptionDetails = true, TracingPreference = TracingPreference.Always
                    })
                .AddErrorFilter<ValidationErrorFilter>();

            doMoreActions?.Invoke(services);

            return services;
        }

        public static ISchemaConfiguration RegisterObjectTypes(this ISchemaConfiguration schemaConfiguration,
            Type marker)
        {
            var objectTypes = marker
                .Assembly
                .GetTypes()
                .Where(type => typeof(ObjectType).IsAssignableFrom(type));

            foreach (var objectType in objectTypes)
            {
                schemaConfiguration.RegisterType(objectType);
            }

            return schemaConfiguration;
        }

        public static Expression<Func<TDto, bool>> GetQueryableFilterExpr<TDto>(this IResolverContext context)
        {
            Expression<Func<TDto, bool>> filter = null;
            if (context.Field?.Arguments["where"]?.Type is InputObjectType whereIot
                && whereIot is IFilterInputType whereFit)
            {
                var whereValueNode = context.Argument<IValueNode>("where");
                if (whereValueNode != null)
                {
                    var queryableFilterVisitor = new QueryableFilterVisitor(
                        whereIot,
                        whereFit.EntityType,
                        TypeConversion.Default);

                    if (whereValueNode.Kind != NodeKind.NullValue)
                    {
                        whereValueNode.Accept(queryableFilterVisitor);
                        filter = queryableFilterVisitor?.CreateFilter<TDto>();
                    }
                }
            }

            return filter;
        }

        public static QueryableSortVisitor GetQueryableSortExpr<TDto>(this IResolverContext context)
        {
            QueryableSortVisitor queryableSortVisitor = null;
            if (context.Field?.Arguments[SortObjectFieldDescriptorExtensions.OrderByArgumentName]?.Type is
                    InputObjectType iot
                && iot is ISortInputType fit)
            {
                var orderByValueNode =
                    context.Argument<IValueNode>(SortObjectFieldDescriptorExtensions.OrderByArgumentName);
                if (orderByValueNode != null)
                {
                    queryableSortVisitor = new QueryableSortVisitor(iot, fit.EntityType);
                    orderByValueNode.Accept(queryableSortVisitor);
                }
            }

            return queryableSortVisitor;
        }

        public static GraphQueryBase<TDto> ExtractParams<TDto>(this GraphQueryBase<TDto> request,
            IResolverContext context)
        {
            var page = context.Argument<int>("page") <= 0 ? 1 : context.Argument<int>("page");
            var pageSize = context.Argument<int>("pageSize") <= 0 ? 20 : context.Argument<int>("pageSize");
            var filterExpr = context.GetQueryableFilterExpr<TDto>();
            var sortExpr = context.GetQueryableSortExpr<TDto>();

            request.Page = page;
            request.PageSize = pageSize;
            request.FilterExpr = filterExpr;
            request.SortingVisitor = sortExpr;

            return request;
        }

        public static IObjectFieldDescriptor AddPagingArguments(this IObjectFieldDescriptor descriptor)
        {
            return descriptor
                .Argument("page", a => a.Type<IntType>())
                .Argument("pageSize", a => a.Type<IntType>());
        }

        public static IObjectFieldDescriptor AddSortingArguments<TSortType>(this IObjectFieldDescriptor descriptor)
            where TSortType : IInputType
        {
            return descriptor.Argument(SortObjectFieldDescriptorExtensions.OrderByArgumentName,
                a => a.Type<TSortType>());
        }

        public static OffsetPaging<TResult> WithCriterions<TResult>(this IQueryable<TResult> source, GraphQueryBase<TResult> queryBase)
        {
            // filter by GraphQL
            if (queryBase.FilterExpr != null)
            {
                source = source.Where(queryBase.FilterExpr);
            }
        
            // order_by by GraphQL 
            if (queryBase.SortingVisitor != null)
            {
                source = queryBase.SortingVisitor.Sort(source);
            }
        
            // pagination
            var totalCount = source.Count();
            source = source.Skip((queryBase.Page - 1) * queryBase.PageSize).Take(queryBase.PageSize);
        
            // build pagination model
            var model = new OffsetPaging<TResult> {TotalCount = totalCount, Edges = source.ToList()};
            return model;
        }
    }
}
