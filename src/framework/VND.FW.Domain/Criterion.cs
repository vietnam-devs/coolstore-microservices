using System;
using System.Collections.Generic;

namespace VND.Fw.Domain
{
    public class Criterion : ValueObjectBase
    {
        public Criterion(int currentPage, int pageSize, PaginationOption defaultPagingOption, string sortBy = "", string sortOrder = "")
        {
            if (currentPage <= 0)
                throw new Exception("CurrentPage could not be less than zero.");

            if (pageSize <= 0)
                throw new Exception("PageSize could not be less than zero.");

            CurrentPage = currentPage - 1;
            PageSize = pageSize;
            DefaultPagingOption = defaultPagingOption;
            SortBy = sortBy;
            SortOrder = sortOrder;
        }

        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public string SortBy { get; private set; }
        public string SortOrder { get; private set; }
        public PaginationOption DefaultPagingOption { get; private set; }

        public Criterion SetPageSize(int pageSize)
        {
            if (pageSize <= 0)
                throw new Exception("PageSize could not be less than zero.");

            PageSize = pageSize;
            return this;
        }

        public Criterion SetCurrentPage(int currentPage)
        {
            if (currentPage <= 0)
                throw new Exception("CurrentPage could not be less than zero.");

            CurrentPage = currentPage;
            return this;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CurrentPage;
            yield return PageSize;
            yield return DefaultPagingOption;
            yield return SortBy;
            yield return SortOrder;
        }
    }
}