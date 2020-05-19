using SharedServices.DAL;
using System;

namespace SharedServices.BL.Domain
{
    public class Discussion
    {
        public int Id { get; set; }
        public ApplicationUser EmitterUser { get; set; }
        public ApplicationUser ReceiverUser { get; set; }
        public string Emitter { get; set; }
        public string Receiver { get; set; }
        public string Message { get; set; }
        public DateTime DateHour { get; set; }
    }
}