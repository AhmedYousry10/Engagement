using EngagementApi.Application.Common.Exceptions;
using EngagementApi.Application.Photos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngagementApi.Api.Controllers;

[ApiController]
[Route("api/photos")]
public class PhotosController : ControllerBase
{
    private const long MaxFileSizeBytes = 10 * 1024 * 1024;

    private readonly IPhotosService _photosService;

    public PhotosController(IPhotosService photosService)
    {
        _photosService = photosService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<PhotoDto>>> GetAll()
    {
        return Ok(await _photosService.GetAllAsync());
    }

    [HttpPost("upload")]
    [Authorize]
    [RequestSizeLimit(MaxFileSizeBytes)]
    public async Task<ActionResult<PhotoDto>> Upload(IFormFile? file)
    {
        if (file is null)
        {
            return BadRequest(new { message = "No file uploaded." });
        }

        try
        {
            await using var stream = file.OpenReadStream();
            var result = await _photosService.UploadAsync(stream, file.FileName, file.Length);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
        catch (PhotoValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _photosService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
