using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using Fest_form.GlobalData.Collections;

using Microsoft.Extensions.Configuration;

using System;
using System.Net.Mail;

namespace Fest_form.Services.Bucket
{
    public class Bucket : IBucket
    {
        private readonly IAmazonS3 _s3Client;
        private const string _BucketName = "fest-minus-file";
        private readonly IConfiguration configuration;
        public Bucket(IConfiguration _configuration) {
            configuration = _configuration;
            var bucket = configuration.GetSection("Bucket");
            string key_id = bucket.GetValue<string>("AWS_ACCESS_KEY_ID");
            string secret_key = bucket.GetValue<string>("AWS_SECRET_ACCESS_KEY");

            _s3Client = new AmazonS3Client(key_id, secret_key, RegionEndpoint.USEast1);
           
    }

        public async Task UploadFileAsync(byte[] file, string fileName) {
           
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new Exception("File is empty or null.");
                }
                List<UploadPartResponse> uploadsResp = new();
                InitiateMultipartUploadRequest initialRequest = new InitiateMultipartUploadRequest()
                {
                    BucketName = _BucketName,
                    Key = fileName
                };

                InitiateMultipartUploadResponse initResponse = 
                    await _s3Client.InitiateMultipartUploadAsync(initialRequest);
                
                
                long contentLenght = file.Length;

                long partSize = 10 * (long)Math.Pow(2, 20);
                long filePos = 0;

               

                try
                {


                    for (int i = 1; filePos < contentLenght; i++)
                    {
                        long partLength = Math.Min(partSize, contentLenght - filePos);

                        byte[] buffer = new byte[partLength];
                        using var partStream = new MemoryStream(file, (int)filePos, (int)partLength);
                       

                        UploadPartRequest uploadPartRequest = new UploadPartRequest()
                        {
                            BucketName = _BucketName,
                            Key = fileName,
                            UploadId = initResponse.UploadId,
                            PartNumber = i,
                            PartSize = partSize,
                            InputStream = partStream
                        };
                        uploadPartRequest.StreamTransferProgress +=
                            new EventHandler<StreamTransferProgressArgs>(UploadPartProgressEventCallback!);

                        uploadsResp.Add(await _s3Client.UploadPartAsync(uploadPartRequest));
                        filePos += partLength;
                    }

                

                    CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest()
                    {
                        BucketName = _BucketName,
                        Key = fileName,
                        UploadId = initResponse.UploadId,
                    };
                    completeRequest.AddPartETags(uploadsResp);
                    CompleteMultipartUploadResponse completeUploadResponce = 
                        await _s3Client.CompleteMultipartUploadAsync(completeRequest);
                    Console.WriteLine($"Upload completed successfully {fileName}.");
                }
                catch (Exception exception)
                {
                    Console.WriteLine("An exception occurred: {0}", exception.Message);

                    // Abort the upload.
                    AbortMultipartUploadRequest abortMPURequest = new AbortMultipartUploadRequest
                    {
                        BucketName = _BucketName,
                        Key = fileName,
                        UploadId = initResponse.UploadId
                    };
                    await _s3Client.AbortMultipartUploadAsync(abortMPURequest);

                    Console.WriteLine("Upload aborted due to an error.");
                }

                
                
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
        private static void UploadPartProgressEventCallback(object sender, StreamTransferProgressArgs e)
        {
            Console.WriteLine($"Uploaded {e.TransferredBytes} of {e.TotalBytes} bytes ({e.PercentDone}% complete)");
        }
    }

}
