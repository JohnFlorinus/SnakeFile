using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SnakeFileAPI.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SnakeFileAPI.Controllers
{
    [Route("files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileRepository repo;
        public FileController(IFileRepository fileRepository) {
            repo = fileRepository;
        }

        [RequestSizeLimit(100 * 1000 * 1000)]
        [HttpPost("upload")]
        [EnableRateLimiting("UploadLimit")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            string ID;
            try
            {
                ID = await repo.UploadFile(file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(ID);
        }

        [HttpGet("download/{ID}")]
        public async Task<IActionResult> DownloadFile(string ID)
        {
            byte[] returnedBytes;
            try
            {
                returnedBytes = await repo.DownloadFile(ID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            var base64 = Convert.ToBase64String(returnedBytes);
            return Ok(base64);
        }
    }
}