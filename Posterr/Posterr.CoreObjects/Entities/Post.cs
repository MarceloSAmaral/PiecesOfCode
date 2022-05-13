using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.CoreObjects.Entities
{
    public enum TypesOfPosts : int
    {
        NotDefined = 0,
        Original = 1,
        Repost = 2,
        Quote = 3,
    }

    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public TypesOfPosts PostType { get; set; } = TypesOfPosts.NotDefined;

        public string Content { get; set; }

        public DateTime PostedAt { get; set; }

        public Guid? RepostFrom { get; set; }

        public Guid? QuoteFrom { get; set; }
    }

}
