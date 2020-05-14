using SharedServices.DAL;

namespace SharedServices.UI.Models
{
    public class SearchModel
    {
        public PaginatedList<ApplicationUser> Users { get; set; }
    }
}