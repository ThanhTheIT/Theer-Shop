using SV21T1020718.DomainModels;

namespace SV21T1020718.Web.Models
{
    public class EmployeeSearchResult : PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }

    }
}
