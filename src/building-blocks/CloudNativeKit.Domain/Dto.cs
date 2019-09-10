namespace CloudNativeKit.Domain
{
    /// <summary>
    /// Supertype for all Dto types
    /// </summary>
    public interface IDto
    {
    }

    public class Criterion : IDto
    {
        private const int MaxPageSize = 50;
        private const int ConfigurablePageSize = 10;
        private const string DefaultSortBy = "Id";
        private const string DefaultSortOrder = "desc";

        private int _pageSize = MaxPageSize;

        private string _sortBy = DefaultSortBy;

        private string _sortOrder = DefaultSortOrder;

        public Criterion()
        {
            CurrentPage = 1;
            PageSize = ConfigurablePageSize;
        }

        public int CurrentPage { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
        }

        public string SortBy
        {
            get => _sortBy;
            set => _sortBy = string.IsNullOrEmpty(value) ? DefaultSortBy : value;
        }

        public string SortOrder
        {
            get => _sortOrder;
            set => _sortOrder = string.IsNullOrEmpty(value) ? DefaultSortOrder : value;
        }

        public Criterion SetPageSize(int pageSize)
        {
            if (pageSize <= 0)
                throw new ValidationException("PageSize could not be less than zero.");

            PageSize = pageSize;
            return this;
        }

        public Criterion SetCurrentPage(int currentPage)
        {
            if (currentPage <= 0)
                throw new ValidationException("CurrentPage could not be less than zero.");

            CurrentPage = currentPage;
            return this;
        }
    }
}
