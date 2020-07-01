using SharedServices.DAL;

namespace SharedServices.UI.Models
{
    public class SearchModel
    {
        public PaginatedRequests<ApplicationUser> Users { get; set; }
    }
}