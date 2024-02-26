using BlogService.Common.Dtos.Comments;
using BlogService.Common.Dtos.General;
using BlogService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("Post/{postId}")]
        public async Task<IActionResult> GetPostComments(int postId)
        {
            var comments = await _commentService.GetComments(postId);

            return Ok(comments);
        }

        [HttpGet("{id}", Name = "GetCommentById")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _commentService.GetComment(id);

            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto dto)
        {
            int authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var comment = await _commentService.CreateComment(authId, dto);

            return CreatedAtRoute("GetCommentById", new { id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] ValueDto<string> dto)
        {
            int authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _commentService.UpdateComment(authId, id, dto.Value);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            int authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _commentService.DeleteComment(authId, id);

            return NoContent();
        }
    }
}
