using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class ImageController : ControllerBase
    {
        private string AccessKey = "";
        private string SecretKey = "q572qw2XdRLOTzh9Wt1CcDnv+aqWw1Bo7czlvTSt";
        private string BucketName = "aherrera";

        [HttpPost("ClassSchedImage")]
        public async Task<IActionResult> UploadImage(IFormFile File, string FileName)
        {
            if (File == null || File .Length == 0)
            {
                return BadRequest("The File is missing or has no content.");
            }

            if (string.IsNullOrEmpty(FileName))
            {
                return BadRequest("File name must be provided to upload.");
            }

            try
            {
                using (var client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.USEast1))
                {
                    var keyName = FileName;

                    using (var stream = File.OpenReadStream())
                    {
                        var putRequest = new PutObjectRequest
                        {
                            BucketName = BucketName,
                            Key = keyName,
                            InputStream = stream,
                            ContentType = File.ContentType
                        };

                        var response = await client.PutObjectAsync(putRequest);

                        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var fileUrl = $"https://{BucketName}.s3.amazonaws.com/{keyName}";
                            return Ok(new { Message = "File Uploaded Successfully.", FileUrl = fileUrl });
                        }
                        else
                        {
                            return StatusCode((int)response.HttpStatusCode, "File Upload Failed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}