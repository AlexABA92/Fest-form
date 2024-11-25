using Fest_form.data.Entity;
using Fest_form.Services.Bucket;
using Fest_form.Services.MailSend;

using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;

namespace Fest_form.Repositories.FileRepos
{
    public class FilesRepos : IFileRepos
    {
        private IBucket _bucket;
        private readonly IMailSend _mail;
        public FilesRepos(IBucket bucket, IMailSend mail)
        {
            _bucket = bucket;
            _mail = mail;
        }
        public async Task FileSender(DanceTeam team, List<IFormFile> files)
        {
            try
            {
                List<byte[]> fileDataList = new List<byte[]>();

                using (var memoryStream = new MemoryStream())
                {
                    for (var i = 0; i < files.Count; i++)
                    {
                        byte[] fileData;
                        files[i].CopyTo(memoryStream);
                        fileData = memoryStream.ToArray(); // Store file data in a byte array
                        fileDataList.Add(fileData);
                        memoryStream.SetLength(0);
                    }
                }
                for (var i = 0; i < team.Performances.Count; i++)
                {
                    var fileData = fileDataList[i]; // Get the preloaded file data

                    await _bucket.UploadFileAsync(fileData, team.Performances[i].PhonogramFileURL);
                    await _mail.SendMultipleEmailsAsync(team, fileData, i);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async void TeamInfoMail(DanceTeam team) {
           await _mail.SendTeamInfoEmailAsync(team);
        }
    }
}
