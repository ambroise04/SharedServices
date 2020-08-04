namespace SharedServices.UI.Models.RequestViewModels
{
    public class CreateRequestVM
    {
        public int Service { get; set; }
        public string Description { get; set; }
        public int Point { get; set; }
        public int Duration { get; set; }
        public string Date { get; set; }
        public string Flag { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int PostalCode { get; set; }
    }
}