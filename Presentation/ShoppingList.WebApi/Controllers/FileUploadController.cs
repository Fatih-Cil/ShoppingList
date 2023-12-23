using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        IWebHostEnvironment _env;

        public FileUploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null)
            {
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (allowedExtensions.Contains(fileExtension))
                {

                    string uploadPath = Path.Combine("Uploads", file.FileName);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);

                    }

                    string imageUrl = "https://localhost:44344/Uploads/" + file.FileName; // Örnek URL oluşturumu
                    return Ok(new { imageUrl });
                }
                else
                {
                    return BadRequest("Uygun dosya formatı değil. Lütfen png, jpg veya jpeg formatındaki dosyaları yükleyin.");
                }
            }
            return BadRequest("Dosya eksik veya hatalı.");


        }
    }
}
