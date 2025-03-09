using SV21T1020718.DomainModels;

namespace SV21T1020718.Web.Models
{
    public class SupplierSearchResult : PaginationSearchResult
    {
        public required List<Supplier> Data { get; set; }

    }
}
