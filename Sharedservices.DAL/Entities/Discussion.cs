using System;

namespace SharedServices.DAL.Entities
{
    public class Discussion
    {
        public int Id { get; set; }
        public string Emitter { get; set; }
        public string Receiver { get; set; }
        public string Message { get; set; }
        public DateTime DateHour { get; set; }
    }
}