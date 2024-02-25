using BlogService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }


    }
}
