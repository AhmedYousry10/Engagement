using EngagementApi.Application.Details;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngagementApi.Api.Controllers;

[ApiController]
[Route("api/details")]
public class DetailsController : ControllerBase
{
    private readonly IDetailsService _detailsService;

    public DetailsController(IDetailsService detailsService)
    {
        _detailsService = detailsService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<DetailCardDto>>> GetAll()
    {
        return Ok(await _detailsService.GetAllAsync());
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<DetailCardDto>> Create(DetailCardUpsertDto dto)
    {
        var result = await _detailsService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult<DetailCardDto>> Update(int id, DetailCardUpsertDto dto)
    {
        var result = await _detailsService.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _detailsService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
