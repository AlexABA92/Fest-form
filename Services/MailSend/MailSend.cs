using Fest_form.data.Entity;
using System.Net.Mail;
using System.Net;

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Web;
using Amazon.Runtime.Internal.Util;
using System.Runtime.ConstrainedExecution;
using Microsoft.Extensions.Caching.Memory;

namespace Fest_form.Services.MailSend
{
    public class MailSend : IMailSend
    {
        private readonly IConfiguration configuration;
        string? host;
        string? box;
        string? key;
        int? port;
        bool ssl;
        private IMemoryCache  _cache;
        MailAddress mailAddressTo =new MailAddress( "blacksea.patterns@gmail.com");
        public MailSend(IConfiguration _configuration, IMemoryCache cache)
        {
            _cache = cache;
            configuration = _configuration;
            var gmailConfig = configuration.GetSection("Gmail");
            host  = gmailConfig.GetValue<string>("Host");
            box   = gmailConfig.GetValue<string>("Box");
            key   = gmailConfig.GetValue<string>("Key");
            port  = gmailConfig.GetValue<int>("Port");
            ssl   = gmailConfig.GetValue<bool>("Ssl");
        }
        private string TeamInfo(DanceTeam team) {
            var mailBody = new TagBuilder("div");
            mailBody.Attributes.Add("style", "width: 100%; padding-top: 3rem; font-family:" +
                " 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;");
             
          
                var teamBody = new TagBuilder("div");
                teamBody.Attributes.Add("style", "padding - left: 5rem;border: 3px solid blue; border-radius: 15px");
                teamBody.InnerHtml.AppendHtml(
                    $"<h1 style='color: rgb(79, 79, 79);'> Назва колективу : " +
                        $"<strong style='font-family: Arial, Helvetica, sans-serif; color: black;'>{team.TeamName}</strong>" +
                    $"</h1>" +
                    $"<div style=\"font-size: 1.5rem; padding-left: 2rem;\">" +
                        $"<span style=\"color: rgb(79, 79, 79); font-weight: 600; margin: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\">" +
                        $"Керівник колективу : &nbsp;</span>" +
                        $"<span style='color: black; margin: 0; font-family: Arial, Helvetica, sans-serif;' >" +
                        $"{team.TeamLeader.PersonLastName} {team.TeamLeader.PersonName}" +
                        $" {team.TeamLeader.PersonFatherName}" +
                        $"</span>" +
                    $"</div>" +
                    $"<div style=\"font-size: 1.5rem; padding-left: 2rem;\">" +
                        $"<span style=\"color: rgb(79, 79, 79); font-weight: 600; margin: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\">" +
                        $"Mail адреса : &nbsp;</span>" +
                        $"<span style='color: black; margin: 0; font-family: Arial, Helvetica, sans-serif;' >" +
                        $"{team.Mail}" +
                        $"</span>" +
                    $"</div>" +
                    $"<div style=\"font-size: 1.5rem; padding-left: 2rem;\">" +
                        $"<span style=\"color: rgb(79, 79, 79); font-weight: 600; margin: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\">" +
                        $"Номер телефону : &nbsp;</span>" +
                        $"<span style='color: black; margin: 0; font-family: Arial, Helvetica, sans-serif;' >" +
                        $"{team.TeamPhoneNumber}" +
                        $"</span>" +
                    $"</div>" +
                    $"<div style=\"font-size: 1.5rem; padding-left: 2rem;\">" +
                        $"<span style=\"color: rgb(79, 79, 79); font-weight: 600; margin: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\">" +
                        $"Рівень підготовки : &nbsp;</span>"+
                $"<span style='color: black; margin: 0; font-family: Arial, Helvetica, sans-serif;' >");
                        var tl = team.TeamLevel == GlobalData.Enum.TeamLevelEnum.Amateur ? "Аматорський" : "Професійний";
                        teamBody.InnerHtml.AppendHtml($"{tl}" +
                        $"</span>" +
                    $"</div>");
                if (team.TeamLevel == GlobalData.Enum.TeamLevelEnum.Professional) {
                    teamBody.InnerHtml.AppendHtml(
                    $"<div style=\"font-size: 1.5rem; padding-left: 2rem;\">" +
                        $"<span style=\"color: rgb(79, 79, 79); font-weight: 600; margin: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\">" +
                        $"Організація : &nbsp;</span>" +
                        $"<span style='color: black; margin: 0; font-family: Arial, Helvetica, sans-serif;' >"+
                        $"{team.Organization}" +
                        $"</span>" +
                    $"</div>");        
                }
                mailBody.InnerHtml.AppendHtml(teamBody);
            

            var stringWriter = new StringWriter(); 
            mailBody.WriteTo(stringWriter, System.Text.Encodings.Web.HtmlEncoder.Default); 
            var mailBodyString = stringWriter.ToString();

            return mailBodyString;
        }
        private string PerformanceInfo(Performance performance, int index, string teamName)  {
            Category? ageGroup = null;
            Genre? genre = null;
            ParticipantsNumber? number = null;


            if (_cache.TryGetValue("GenreList", out List<Genre> genreList))
            {
                genre = genreList.FirstOrDefault(item => item.Id == performance.GenreId);
            }

            if (_cache.TryGetValue("CategoryList", out List<Category> categoryList))
            {
                ageGroup = categoryList.FirstOrDefault(item => item.Id == performance.CategoryId);
            }

            if (_cache.TryGetValue("ParticipantsNumberList", out List<ParticipantsNumber> partNumb))
            {
                number = partNumb.FirstOrDefault(item => item.Id == performance.ParticipantsNumberId);
            }

            var container = new TagBuilder("div");
            container.Attributes.Add("style", "font-family: Arial, sans-serif; " +
                "line-height: 1.6; color: #333; background-color: #f9f9f9; padding: 20px; max-width: 600px; margin: 0 auto;");
            
            var header = new TagBuilder("h1");
            header.InnerHtml.Append($"{teamName} - {performance.PerformanceName}");
            container.InnerHtml.AppendHtml(header);
            
            StringBuilder fullName = new StringBuilder();

            fullName.Append($"{performance.ChoreographerDirector.PersonLastName} {performance.ChoreographerDirector.PersonName} " +
                $"{performance.ChoreographerDirector.PersonFatherName}");
            AppendField(container, "Хореограф постановник", fullName.ToString());
            fullName.Clear();
            
            fullName.Append($"{performance.Concertmaster?.PersonLastName} {performance.Concertmaster?.PersonName} " +
                $"{performance.Concertmaster?.PersonFatherName}");
            AppendField(container, "Kонцертмейстер", fullName.ToString());
            fullName.Clear();

            AppendField(container, "Вікова категория", ageGroup?.Name ?? "N/A");
            AppendField(container, "Жанр", genre?.Name ?? "N/A");
            AppendField(container, "Кількість учасників", number?.Name ?? "N/A");

            if (performance.ParticipantsNameList != null) {
                if (performance.ParticipantsNameList.Person1 != null) {
                    fullName.Append($"{performance.ParticipantsNameList.Person1.PersonLastName} " +
                        $"{performance.ParticipantsNameList.Person1.PersonName} " +
                        $"{performance.ParticipantsNameList.Person1.PersonFatherName}");
                    AppendField(container, "Учасник 1", fullName.ToString());
                    fullName.Clear();
                }
                if (performance.ParticipantsNameList.Person2 != null)
                {
                    fullName.Append($"{performance.ParticipantsNameList.Person2.PersonLastName} " +
                        $"{performance.ParticipantsNameList.Person2.PersonName} " +
                        $"{performance.ParticipantsNameList.Person2.PersonFatherName}");
                    AppendField(container, "Учасник 2", fullName.ToString());
                    fullName.Clear();
                }
                if (performance.ParticipantsNameList.Person3 != null)
                {
                    fullName.Append($"{performance.ParticipantsNameList.Person3.PersonLastName} " +
                        $"{performance.ParticipantsNameList.Person3.PersonName} " +
                        $"{performance.ParticipantsNameList.Person3.PersonFatherName}");
                    AppendField(container, "Учасник 3", fullName.ToString());
                    fullName.Clear();
                }
            }
            AppendField(container, "Час виступу", performance.PerformanceTime);
            var point = performance.StartPoint == GlobalData.Enum.StartPointEnum.Point ? "З точки" : "З кулиси";
            AppendField(container, "Стартова позиція", point);
            AppendField(container, "Назва файла фонограми", HttpUtility.UrlDecode( performance.PhonogramFileURL));
            if(!string.IsNullOrEmpty( performance.YouTubeVideoURL))
                AppendField(container, "Посиланя на відео виступу", performance.YouTubeVideoURL);

            var writer = new System.IO.StringWriter();
            container.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
            return writer.ToString();
        }
        

        public async Task SendMultipleEmailsAsync(DanceTeam team, byte[] fileData, int index)
        {


            try
            {
                using SmtpClient smtpClient = new SmtpClient(host)
            {
                Port = port ?? throw new Exception("SendMultipleEmailsAsync : smtpClient - port is null!!!"),
                EnableSsl = ssl,
                Credentials = new NetworkCredential(box, key)
            };
          
                using MailMessage mailMessage = new MailMessage()
                {
                    IsBodyHtml = true,
                    From = new MailAddress(box),
                    Subject = index == 0 ?
                    team.TeamName + " - " + $"Номер {index + 1} : " + team.Performances[index].PerformanceName :
                    $"Номер {index + 1} : " + team.Performances[index].PerformanceName,
                    Body = PerformanceInfo(team.Performances[index],index,team.TeamName)
                }; 

                mailMessage.To.Add(mailAddressTo);

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

        public async Task SendTeamInfoEmailAsync(DanceTeam team)
        {
            using SmtpClient smtpClient = new SmtpClient(host)
            {
                Port = port ?? throw new Exception("SendMultipleEmailsAsync : smtpClient - port is null!!!"),
                EnableSsl = ssl,
                Credentials = new NetworkCredential(box, key)
            };
            using MailMessage mailMessage = new MailMessage()
            {
                IsBodyHtml = true,
                From = new MailAddress(box),
                Subject =
                    "Новий Участник : " + team.TeamName,
                Body = TeamInfo(team)
            };
            mailMessage.To.Add(mailAddressTo);
            await smtpClient.SendMailAsync(mailMessage);
        }
        private void AppendField(TagBuilder container, string fieldName, string fieldValue)
        {
            var fieldContainer = new TagBuilder("p");

            var fieldLabel = new TagBuilder("strong");
            fieldLabel.Attributes.Add("style", "color: rgb(79, 79, 79);");
            fieldLabel.InnerHtml.Append(fieldName + ": ");
            fieldContainer.InnerHtml.AppendHtml(fieldLabel);

            var fieldValueItalic = new TagBuilder("em");
            fieldValueItalic.InnerHtml.Append(fieldValue);
            fieldContainer.InnerHtml.AppendHtml(fieldValueItalic);

            container.InnerHtml.AppendHtml(fieldContainer);
        }
    }
}
