using System.Collections.Generic;
using System.Linq;
using CloudNativeKit.Utils.Extensions;

namespace CloudNativeKit.Domain
{
    /// <summary>
    /// Source: https://github.com/VaughnVernon/IDDD_Samples_NET
    /// </summary>
    public abstract class ValueObjectBase
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (ReferenceEquals(null, obj))
                return false;
            if (GetType() != obj.GetType())
                return false;
            var vo = obj as ValueObjectBase;
            return GetEqualityComponents().SequenceEqual(vo.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents().CombineHashCodes();
        }
    }

    public class PaginatedItem<TResponse> : ValueObjectBase
    {
        public PaginatedItem(long totalItems, long totalPages, IReadOnlyList<TResponse> items)
        {
            TotalItems = totalItems;
            TotalPages = totalPages;
            Items = items;
        }

        public long TotalItems { get; }

        public long TotalPages { get; }

        public IReadOnlyList<TResponse> Items { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TotalItems;
            yield return TotalPages;
            yield return Items;
        }
    }
}
