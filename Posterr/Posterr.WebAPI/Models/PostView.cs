using Posterr.CoreObjects.Entities;
using System;

namespace Posterr.WebAPI.Models
{
    public class PostView
    {
        public Guid Id { get; set; }

        public string Author { get; set; }

        public TypesOfPosts PostType { get; set; } = TypesOfPosts.NotDefined;

        public string Content { get; set; }

        public DateTime PostedAt { get; set; }

        public SimplifiedPostView Origin { get; set; }
    }
}
