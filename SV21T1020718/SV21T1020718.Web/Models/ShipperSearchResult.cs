using SV21T1020718.DomainModels;

namespace SV21T1020718.Web.Models
{
    public class ShipperSearchResult : PaginationSearchResult
    {
        public required List<Shipper> Data { get; set; }

    }
}
