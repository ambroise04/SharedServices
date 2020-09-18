using System;

namespace SharedServices.DAL.Entities
{
    public class UserSession
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Hostname { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Loc { get; set; }
        public string Org { get; set; }
        public string Postal { get; set; }
        public DateTime SessionDate { get; set; }
    }
}