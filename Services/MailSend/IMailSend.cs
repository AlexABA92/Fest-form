using Fest_form.data.Entity;

namespace Fest_form.Services.MailSend
{
    public interface IMailSend
    {
        public Task SendMultipleEmailsAsync(DanceTeam team, byte[] fileData, int index);
    }
}
