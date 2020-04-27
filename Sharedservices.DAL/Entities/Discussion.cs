using System;
using System.ComponentModel.DataAnnotations;

namespace SharedServices.DAL.Entities
{
    public class Discussion
    {
        public int Id { get; set; }
        [Required]
        public string Emitter { get; set; }
        [Required]
        public string Receiver { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime DateHour { get; set; }
    }
}