using BlogService.Common.Dtos.Profiles;
using BlogService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;
    public UserProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("All")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _userProfileService.GetProfiles();

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        int authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _userProfileService.GetProfile(authId);

        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileDto dto)
    {
        int authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _userProfileService.UpdateProfile(authId, dto);

        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("{publicId}/Avatar")]
    public async Task<IActionResult> GetAvatar(string publicId)
    {
        var file = await _userProfileService.GetProfileAvatar(publicId);

        return File(file.Bytes!, file.ContentType, file.FileName);
    }
}
