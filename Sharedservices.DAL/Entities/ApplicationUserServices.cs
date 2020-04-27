namespace SharedServices.DAL.Entities
{
    public class ApplicationUserServices
    {
        public string ApplicationUserId { get; set; }
        public int ServiceId { get; set; }
        public ApplicationUser User { get; set; }
        public Service Service { get; set; }
    }
}