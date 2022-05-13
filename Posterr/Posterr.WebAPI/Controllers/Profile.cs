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
    public class Profile : ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private IUserApplication _userApplication;
        private IPostApplication _postApplication;

        public Profile(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _postApplication = serviceProvider.GetService<IPostApplication>();
            _userApplication = _serviceProvider.GetService<IUserApplication>();
        }

        [HttpGet("{UserName}")]
        public async Task<ActionResult<UserView>> Get(string UserName)
        {
            if (string.IsNullOrWhiteSpace(UserName)) return new BadRequestObjectResult("This username is invalid.");

            var userRecord = await _userApplication.GetUserByUserNameAsync(UserName);
            if (userRecord is null) return NotFound();
            var userStats = await _userApplication.GetUserStatsAsync(userRecord.Id);
            if (userRecord is null) return NotFound();
            var webApiServices = _serviceProvider.GetService<IWebAPIServices>();
            var currentUser = webApiServices.GetCurrentUser();
            var userView = await webApiServices.ConvertToUserViewAsync(currentUser.Id, userRecord, userStats);
            return Ok(userView);
        }

        [HttpGet("{UserName}/Posts")]
        public async Task<ActionResult<IEnumerable<PostView>>> GetPost(string UserName, [FromQuery] int offset = 0, [FromQuery] int fetch = 5)
        {
            if (string.IsNullOrWhiteSpace(UserName)) return new BadRequestObjectResult("This username is invalid.");
            if (offset < 0) return new BadRequestObjectResult("offset must be greater or equals to zero.");
            if (fetch <= 0) return new BadRequestObjectResult("offset must be greater or equals to one.");

            var userRecord = await _userApplication.GetUserByUserNameAsync(UserName);
            var posts = await _postApplication.GetPostsByUserAsync(userRecord.Id, offset, fetch);
            List<PostView> result = new List<PostView>();
            var webApiServices = _serviceProvider.GetService<IWebAPIServices>();
            foreach (var post in posts)
            {
                result.Add(await webApiServices.ConvertToPostViewAsync(post));
            }
            return Ok(result);
        }

        [HttpPut("{UserName}/Follow")]
        public async Task<IActionResult> Follow(string UserName)
        {
            if (string.IsNullOrWhiteSpace(UserName)) return new BadRequestObjectResult("This username is invalid.");

            var userToFollow = await _userApplication.GetUserByUserNameAsync(UserName);
            if (userToFollow is null) return NotFound();
            var webApiServices = _serviceProvider.GetService<IWebAPIServices>();
            var currentUser = webApiServices.GetCurrentUser();
            await _userApplication.FollowUserAsync(currentUser.Id, userToFollow.Id);
            return NoContent();
        }

        [HttpPut("{UserName}/Unfollow")]
        public async Task<IActionResult> Unfollow(string UserName)
        {
            if (string.IsNullOrWhiteSpace(UserName)) return new BadRequestObjectResult("This username is invalid.");

            var userToUnfollow = await _userApplication.GetUserByUserNameAsync(UserName);
            if (userToUnfollow is null) return NotFound();
            var webApiServices = _serviceProvider.GetService<IWebAPIServices>();
            var currentUser = webApiServices.GetCurrentUser();
            await _userApplication.UnfollowUserAsync(currentUser.Id, userToUnfollow.Id);
            return NoContent();
        }

        [HttpGet("{UserName}/Posts/{id}")]
        public async Task<ActionResult<PostView>> GetPostAsync(string UserName, Guid id)
        {
            if (string.IsNullOrWhiteSpace(UserName)) return new BadRequestObjectResult("This username is invalid.");
            var userProfileOwner = await _userApplication.GetUserByUserNameAsync(UserName);
            if (userProfileOwner is null) return NotFound();

            var postEntry = await _postApplication.GetPostByIdAsync(id);
            if (postEntry is null) return NotFound();

            if(postEntry.UserId != userProfileOwner.Id) return NotFound();

            var webApiServices = _serviceProvider.GetService<IWebAPIServices>();
            return Ok(await webApiServices.ConvertToPostViewAsync(postEntry));
        }

        [HttpPost("{UserName}/Posts")]
        public async Task<IActionResult> AddPost(string UserName, [FromBody] PostInput input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserName)) return new BadRequestObjectResult("This username is invalid.");
                if (input == null) return new BadRequestObjectResult("This post is invalid.");

                var userProfileOwner = await _userApplication.GetUserByUserNameAsync(UserName);
                if (userProfileOwner is null) return NotFound();

                var webApiServices = _serviceProvider.GetService<IWebAPIServices>();
                var currentUser = webApiServices.GetCurrentUser();
                var referenceDate = TimeProvider.Current.UtcNow;
                var postEntry = input.ConvertToPost(currentUser.Id, referenceDate);
                await _postApplication.AddPostAsync(postEntry);
                return CreatedAtAction(nameof(GetPostAsync), new { UserName = UserName, id = input.Id }, await webApiServices.ConvertToPostViewAsync(postEntry));
            }
            catch (Exception ex)
            {
                return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(ex.Message);
            }
        }
    }
}
