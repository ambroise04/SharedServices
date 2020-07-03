﻿namespace SharedServices.DAL.Entities
{
    public class Place
    {
        public int Id { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}