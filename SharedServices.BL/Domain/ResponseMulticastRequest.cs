using SharedServices.DAL;

namespace SharedServices.BL.Domain
{
    public class ResponseMulticastRequest
    {
        public int RequestMulticastId { get; set; }
        public string ApplicationUserId { get; set; }
        public RequestMulticast RequestMulticast { get; set; }
        public ApplicationUser Responder { get; set; }
        public bool Choosen { get; set; }
    }
}