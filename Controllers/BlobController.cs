using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;

namespace MongoDbAKS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobController(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var containerClient = _blobServiceClient.GetBlobContainerClient("mycontainer");
            var blobClient = containerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return Ok(new { fileName = file.FileName, blobUri = blobClient.Uri });
        }
    }
}
   