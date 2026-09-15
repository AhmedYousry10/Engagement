using EngagementApi.Application.SiteContents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngagementApi.Api.Controllers;

[ApiController]
[Route("api/site-content")]
public class SiteContentController : ControllerBase
{
    private readonly ISiteContentService _siteContentService;

    public SiteContentController(ISiteContentService siteContentService)
    {
        _siteContentService = siteContentService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<SiteContentDto>> Get()
    {
        var content = await _siteContentService.GetAsync();
        return content is null ? NotFound() : Ok(content);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<SiteContentDto>> Update(SiteContentDto dto)
    {
        var updated = await _siteContentService.UpdateAsync(dto);
        return updated is null ? NotFound() : Ok(updated);
    }
}
