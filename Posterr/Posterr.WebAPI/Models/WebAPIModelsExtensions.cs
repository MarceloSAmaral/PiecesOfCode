using Posterr.CoreObjects.Entities;
using System;

namespace Posterr.WebAPI.Models
{
    public static class WebAPIModelsExtensions
    {
        public static Post ConvertToPost(this PostInput post, Guid userId, DateTime referenceDate)
        {
            var result = new Post()
            {
                Id = post.Id,
                Content = post.Content,
                PostedAt = referenceDate,
                PostType = post.PostType,
                QuoteFrom = post.QuoteFrom,
                RepostFrom = post.RepostFrom,
                UserId = userId
            };
            return result;
        }
    }
}
