namespace SharedServices.UI.Models.RequestMulticastViewModels
{
    public class CreateMulticastVM
    {
        public int Service { get; set; }
        public string Description { get; set; }
        public int Point { get; set; }
        public int Duration { get; set; }
        public string Date { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int PostalCode { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
}