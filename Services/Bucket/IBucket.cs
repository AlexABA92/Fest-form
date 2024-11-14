namespace Fest_form.Services.Bucket
{
    public interface IBucket
    {
        public Task UploadFileAsync(byte[] file,string fileName);
    }
}
