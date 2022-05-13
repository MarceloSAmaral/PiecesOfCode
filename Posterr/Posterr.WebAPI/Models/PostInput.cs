using Posterr.CoreObjects.Entities;
using System;

namespace Posterr.WebAPI.Models
{
    public class PostInput
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public TypesOfPosts PostType { get; set; }
        public string Content { get; set; }
        public Guid? RepostFrom { get; set; }
        public Guid? QuoteFrom { get; set; }
    }
}
