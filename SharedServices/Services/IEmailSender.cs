﻿using System.Threading.Tasks;

namespace SharedServices.UI.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
