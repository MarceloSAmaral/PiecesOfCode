using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Posterr.Base;
using Posterr.CoreObjects.AppInterfaces;
using Posterr.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Posterr.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Homepage : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private IPostApplication _postApplication;
        public Homepage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _postApplication = _serviceProvider.GetService<IPostApplication>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostView>> GetPostAsync(Guid id)
        {
            var postEntry = await _postApplication.GetPostByIdAsync(id);
            if (postEntry is null) return NotFound();
            var webApiServices = _serviceProvider.GetService<IWebAPIServices>();
            return Ok(await webApiServices.ConvertToPostViewAsync(postEntry));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostView>>> Get([FromQuery] int offset = 0, [FromQuery] int fetch = 10, [FromQuery] bool justFollowing = false)
        {
            if (offset < 0) return new BadRequestObjectResult("offset must be greater or equals to zero.");
            if (fetch <= 0) return new BadRequestObjectResult("offset must be greater or equals to one.");

            var webApiServices = _serviceProvider.GetService<IWebAPIServices>();
            var currentUser = webApiServices.GetCurrentUser();
            var posts = await _postApplication.GetLatestPostsAsync(currentUser.Id, justFollowing, offset, fetch);
            List<PostView> result = new List<PostView>();
            foreach (var post in posts)
            {
                result.Add(await webApiServices.ConvertToPostViewAsync(post));
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] PostInput input)
        {
            try
            {
                if (input == null) return new BadRequestObjectResult("This post is invalid.");

                var webApiServices = _serviceProvider.GetService<IWebAPIServices>();
                var currentUser = webApiServices.GetCurrentUser();
                var postEntry = input.ConvertToPost(currentUser.Id, TimeProvider.Current.UtcNow);
                await _postApplication.AddPostAsync(postEntry);
                return CreatedAtAction(nameof(GetPostAsync), new { id = postEntry.Id }, await webApiServices.ConvertToPostViewAsync(postEntry));
            }
            catch (Exception ex)
            {
                return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(ex.Message);
            }
        }
    }
}
