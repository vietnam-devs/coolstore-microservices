using System;
using System.Linq;
using VND.Fw.Domain;

namespace VND.CoolStore.Services.ApiGateway.Extensions
{
	internal static class CriterionExtensions
	{
		public static bool HasPrevious(this Criterion criterion)
		{
			return (criterion.CurrentPage > 1);
		}

		public static bool HasNext(this Criterion criterion, int totalCount)
		{
			return (criterion.CurrentPage < (int) GetTotalPages(criterion, totalCount));
		}

		public static double GetTotalPages(this Criterion criterion, int totalCount)
		{
			return Math.Ceiling(totalCount / (double) criterion.PageSize);
		}

		public static bool HasQuery(this Criterion criterion)
		{
			return !String.IsNullOrEmpty(criterion.SortBy);
		}

		public static bool IsDescending(this Criterion criterion)
		{
			if (!String.IsNullOrEmpty(criterion.SortOrder))
			{
				return criterion.SortOrder.Split(' ').Last().ToLowerInvariant().StartsWith("desc");
			}
			return false;
		}
	}
}
