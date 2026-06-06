using MailKit.Net.Smtp;
using MimeKit;

namespace SmartFibreAPI.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendLeadEmailAsync(string name, string surname, string email, string phone, string description)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SmartFibre", _config["EmailSettings:SenderEmail"]));
            message.To.Add(new MailboxAddress("Consultant", _config["EmailSettings:ConsultantEmail"]));
            message.Subject = "New Fibre Application - " + description;

            message.Body = new TextPart("html")
            {
                Text = $@"
                    <h2>New Fibre Application</h2>
                    <p><strong>Name:</strong> {name} {surname}</p>
                    <p><strong>Email:</strong> {email}</p>
                    <p><strong>Phone:</strong> {phone}</p>
                    <p><strong>Package:</strong> {description}</p>
                    <p><strong>Submitted:</strong> {DateTime.Now}</p>
                "
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _config["EmailSettings:SmtpServer"],
                465,
                true  // useSSL = true for port 465
            );
            await client.AuthenticateAsync(
                _config["EmailSettings:SenderEmail"],
                _config["EmailSettings:SenderPassword"]
            );
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}