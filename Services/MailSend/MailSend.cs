using Fest_form.data.Entity;

using Microsoft.Extensions.Configuration;

using System.Net.Mail;
using System.Net;
using System.IO;

namespace Fest_form.Services.MailSend
{
    public class MailSend : IMailSend
    {
        private readonly IConfiguration configuration;

        public MailSend(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task SendMultipleEmailsAsync(DanceTeam team, byte[] fileData, int index)
        {
            var gmailConfig = configuration.GetSection("Gmail");
            string host = gmailConfig.GetValue<string>("Host");
            string box = gmailConfig.GetValue<string>("Box");
            string key = gmailConfig.GetValue<string>("Key");
            int port = gmailConfig.GetValue<int>("Port");
            bool ssl = gmailConfig.GetValue<bool>("Ssl");
           
            using SmtpClient smtpClient = new SmtpClient(host)
            {
                Port = port,
                EnableSsl = ssl,
                Credentials = new NetworkCredential(box, key)
            };
            try
            {
                using MailMessage mailMessage = new MailMessage()
                {
                    IsBodyHtml = true,
                    From = new MailAddress(box),
                    Subject = "Новий учасник фестивалю",
                    Body = $"<h2 style=\"font-size:5em;\">{team.TeamName}</h2>" +
                    $"<p>Номер {index + 1} : {team.Performances[index].PerformanceName}</p>"
                };

                mailMessage.To.Add(new MailAddress("blacksea.patterns@gmail.com"));

                using var stream = new MemoryStream(fileData);
                var mailAttachment = new Attachment(stream, Uri.UnescapeDataString(team.Performances[index].PhonogramFileURL));
                mailMessage.Attachments.Add(mailAttachment);

                await smtpClient.SendMailAsync(mailMessage);

            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

        }
    }
}
