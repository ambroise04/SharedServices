using System;

namespace SharedServices.DAL.Entities
{
    public class ResponseMulticastRequest
    {
        public int RequestMulticastId { get; set; }
        public string ApplicationUserId { get; set; }
        public RequestMulticast RequestMulticast { get; set; }
        public ApplicationUser Responder { get; set; }
    }
}