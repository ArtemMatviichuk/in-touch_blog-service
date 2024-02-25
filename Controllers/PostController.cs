using BlogService.Common.Dtos.Posts;
using BlogService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postsService;

        public PostController(IPostService postsService)
        {
            _postsService = postsService;
        }

        [HttpGet("My")]
        public async Task<IActionResult> GetAllMyPosts()
        {
            int authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var posts = await _postsService.GetMyPosts(authId);

            return Ok(posts);
        }

        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetAllUserPosts(int id)
        {
            var posts = await _postsService.GetPosts(id);

            return Ok(posts);
        }

        [HttpGet("{id}", Name = "GetPostById")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postsService.GetPost(id);

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
        {
            int authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var post = await _postsService.CreatePost(authId, dto);

            return CreatedAtRoute("GetPostById", new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] CreatePostDto dto)
        {
            int authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _postsService.UpdatePost(authId, id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            int authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _postsService.DeletePost(authId, id);

            return NoContent();
        }
    }
}
