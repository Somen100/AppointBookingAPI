using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Appointment.BAL
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly int _timeout;

        public EmailService(IConfiguration configuration)
        {
            _smtpHost = configuration["EmailSettings:SmtpHost"];
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
            _smtpUsername = configuration["EmailSettings:From"];
            _smtpPassword = configuration["EmailSettings:FromPassword"];
            _timeout = int.Parse(configuration["EmailSettings:Timeout"]);
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpHost, _smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = true;
                client.Timeout = _timeout;

                using (var message = new MailMessage(_smtpUsername, toEmail))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception ex)
                    {
                        // Handle exception if needed
                        // You can log the exception or throw it to the caller
                        // For demonstration purposes, I'll just rethrow it
                        throw;
                    }
                }
            }
        }
    }
}
