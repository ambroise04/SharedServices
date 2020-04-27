using SharedServices.DAL;
using System;

namespace SharedServices.BL.Domain
{
    public class Discussion
    {
        public int Id { get; set; }
        public ApplicationUser Emitter { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string Message { get; set; }
        public DateTime DateHour { get; set; }
    }
}