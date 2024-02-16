using BlogService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;
    public UserProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        System.Console.WriteLine(User.FindFirstValue(ClaimTypes.NameIdentifier));
        foreach (var item in User.Claims)
        {
            System.Console.WriteLine(item.Value);
        }
        return Ok("It works");
    }
}
