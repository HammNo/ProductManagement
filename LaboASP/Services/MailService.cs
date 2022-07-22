using System.Net;
using System.Net.Mail;

namespace Demo.ASP.Services
{
    public class MailService
    {
        private MailConfig _config;
        private SmtpClient _client;
        public MailService(MailConfig config, SmtpClient client)
        {
            _config = config;
            _client = client;
            _client.Host = config.Host;
            _client.Port = _config.Port;
            _client.Credentials = new NetworkCredential(_config.Email, _config.Password);
            _client.EnableSsl = true;
        }

        public void Send(string subject, string content, params string[] to)
        {

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_config.Email);
            foreach (string email in to)
            {
                message.To.Add(email);
            }
            message.Body = content;
            message.Subject = subject;
            _client.Send(message);
        }
    }
}