﻿namespace SharedServices.DAL.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public ApplicationUser  User { get; set; }
    }
}