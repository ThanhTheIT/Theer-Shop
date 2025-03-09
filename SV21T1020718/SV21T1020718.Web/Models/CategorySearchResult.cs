using SV21T1020718.DomainModels;

namespace SV21T1020718.Web.Models
{
    public class CategorySearchResult : PaginationSearchResult
    {
        public required List<Category> Data { get; set; }

    }
}
