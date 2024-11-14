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

                mailMessage.To.Add(new MailAddress("alex1991020481@gmail.com"));

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



            // Create a list of tasks to send emails concurrently
            //var emailTasks = new List<Task>();

            //for (int i = 0; i < attachments.Count; i++)
            //{
            //    int index = i;
            //    var attachment = attachments[index];

            //    byte[] fileData;
            //    using (var memoryStream = new MemoryStream())
            //    {
            //       attachment.CopyTo(memoryStream);
            //       fileData = memoryStream.ToArray(); // Store file data in a byte array
            //    }
            //    // Create and start each email task
            //    emailTasks.Add(Task.Run(async () =>
            //    {
            //        using SmtpClient smtpClient = new SmtpClient(host)
            //        {
            //            Port = port,
            //            EnableSsl = ssl,
            //            Credentials = new NetworkCredential(box, key)
            //        };

            //        try
            //        {
            //            using MailMessage mailMessage = new MailMessage()
            //            {
            //                IsBodyHtml = true,
            //                From = new MailAddress(box),
            //                Subject = "Новий учасник фестивалю",
            //                Body = $"<h2 style=\"font-size:5em;\">{team.TeamName}</h2>" +
            //                $"<p>Номер {index+1} : {team.Performances[index].PerformanceName}</p>"
            //            };

            //            mailMessage.To.Add(new MailAddress("alex1991020481@gmail.com"));

            //            using var stream = new MemoryStream(fileData);
            //            var mailAttachment = new Attachment(stream, attachment.FileName);
            //            mailMessage.Attachments.Add(mailAttachment);

            //            await smtpClient.SendMailAsync(mailMessage);

            //        }
            //        catch (SmtpException smtpEx)
            //        {
            //            Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine($"General Error: {ex.Message}");
            //        }
            //    }));
            //}

            //// Wait for all email tasks to complete
            //await Task.WhenAll(emailTasks);
            //Console.WriteLine("All emails sent successfully.");
        }
    }
}
