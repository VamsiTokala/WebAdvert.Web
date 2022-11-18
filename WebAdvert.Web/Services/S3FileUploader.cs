using System.Net;
using Amazon.S3;
using Amazon.S3.Model;

namespace WebAdvert.Web.Services
{
    public class S3FileUploader : IFileUploader
    {
        private readonly IConfiguration _configuration;
        public S3FileUploader(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> UploadFileAsync(string fileName, Stream storageStream)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("File name must be specified.");

            var bucketName = _configuration.GetValue<string>("ImageBucket");

            using (var client = new AmazonS3Client())
            {
                if (storageStream.Length > 0)
                    if (storageStream.CanSeek)
                        storageStream.Seek(0, SeekOrigin.Begin);

                var request = new PutObjectRequest
                {
                    AutoCloseStream = true,
                    BucketName = bucketName,//name of bucket
                    InputStream = storageStream,
                    Key = fileName //filename
                };
                var response = await client.PutObjectAsync(request).ConfigureAwait(false); //it works only if the file in't too big
                return response.HttpStatusCode == HttpStatusCode.OK;
            }
        }
    }
}
