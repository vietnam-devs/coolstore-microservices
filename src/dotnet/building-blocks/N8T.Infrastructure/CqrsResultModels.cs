using System.Collections.Generic;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Http;
using N8T.Core.Domain;
using N8T.Core.Specification;

namespace N8T.Infrastructure
{
    public interface ICommand : IRequest<IResult>
    {
    }

    public interface IQuery : IRequest<IResult>
    {
    }

    public interface ICreateCommand : ICommand, ITxRequest
    {
    }

    public interface IUpdateCommand<TId> : ICommand, ITxRequest
        where TId : struct
    {
        public TId Id { get; init; }
    }

    public interface IDeleteCommand<TId> : ICommand
        where TId : struct
    {
        public TId Id { get; init; }
    }

    public interface IListQuery : IQuery
    {
        public List<string> Includes { get; init; }
        public List<FilterModel> Filters { get; init; }
        public List<string> Sorts { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
    }

    public interface IItemQuery<TId> : IQuery
        where TId : struct
    {
        public List<string> Includes { get; init; }
        public TId Id { get; init; }
    }

    public record ResultModel<T>(T Data, bool IsError = false, string ErrorMessage = default!) where T : notnull
    {
        public static ResultModel<T> Create(T data, bool isError = false, string errorMessage = default!)
        {
            return new ResultModel<T>(data, isError, errorMessage);
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public record ListResultModel<T>(List<T> Items, long TotalItems, int Page, int PageSize) where T : notnull
    {
        public static ListResultModel<T> Create(List<T> items, long totalItems = 0, int page = 1, int pageSize = 20)
        {
            return new ListResultModel<T>(items, totalItems, page, pageSize);
        }
    }
}
