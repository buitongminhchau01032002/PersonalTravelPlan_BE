using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalTravelPlan_BE.Dtos;

namespace PersonalTravelPlan_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase {

        public static IWebHostEnvironment? _environment;
        public UploadController(IWebHostEnvironment environment) {
            _environment = environment;
        }


        // POST api/<UploadController>
        [HttpPost]
        public ActionResult<FilePathDto> Upload([FromForm] UploadDto upload) {

            IFormFile? file = upload.file;
            if (file == null || file.FileName == null || file.FileName.Length == 0) {
                return BadRequest();
            }

            if (_environment == null) {
                return StatusCode(500);
            }

            try {

                string relativePath = "Upload\\" + Guid.NewGuid() + file.FileName;

                string path = Path.Combine(_environment.WebRootPath, relativePath);

                using (FileStream stream = new FileStream(path, FileMode.Create)) {
                    file.CopyTo(stream);
                    stream.Close();
                }
                FilePathDto res = new FilePathDto(relativePath);
                
                return Ok(res);
            } catch (Exception ex) {
                return StatusCode(500);
            }


        }
    }
}
