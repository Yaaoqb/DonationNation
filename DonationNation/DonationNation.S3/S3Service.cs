using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DonationNation.S3
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3;
        private IConfiguration _config;

        public S3Service(IAmazonS3 s3, IConfiguration config)
        {
            _s3 = s3;
            _config = config;
        }
        public async Task<string> AddFile(IFormFile file, string type)
        {
            var S3Config = _config.GetSection("AWS_S3");

            string fileName = Guid.NewGuid().ToString();
            string objectKey = $"{type}/{fileName}";

            using (Stream fileToUpload = file.OpenReadStream())
            {
                var putObjectRequest = new PutObjectRequest();
                putObjectRequest.CannedACL = S3CannedACL.PublicRead;
                putObjectRequest.BucketName = S3Config["Bucket"];
                putObjectRequest.Key = objectKey;
                putObjectRequest.InputStream = fileToUpload;
                putObjectRequest.ContentType = file.ContentType;

                var response = await _s3.PutObjectAsync(putObjectRequest);
                return S3Config["URL"] + objectKey;
            }
        }

        public async Task<string> RemoveFile(string fileURL)
        {
            var S3Config = _config.GetSection("AWS_S3");

            var deleteObjectRequest = new DeleteObjectRequest();
            deleteObjectRequest.BucketName = S3Config["Bucket"];
            deleteObjectRequest.Key = fileURL.Replace(S3Config["URL"], "");

            var response = await _s3.DeleteObjectAsync(deleteObjectRequest);
            return response.HttpStatusCode.ToString();
        }
    }
}
