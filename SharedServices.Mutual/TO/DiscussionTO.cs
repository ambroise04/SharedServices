using System;

namespace SharedServices.Mutual.TO
{
    public class DiscussionTO
    {
        public int Id { get; set; }
        public string Emitter { get; set; }
        public string Receiver { get; set; }
        public string EmitterName { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public string PictureSource { get; set; }
    }
}