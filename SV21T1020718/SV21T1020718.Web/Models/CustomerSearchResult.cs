using SV21T1020718.DomainModels;

namespace SV21T1020718.Web.Models
{
    public class CustomerSearchResult : PaginationSearchResult
    {
        public required List<Customer> Data { get; set; }

    }
}
