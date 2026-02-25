using Fest_form.data.Entity;

namespace Fest_form.Services.MailSend
{
    public interface IMailSend
    {
        public  Task SendEmailAsync(DanceTeam team, List<IFormFile>? files);
        

    }
}
