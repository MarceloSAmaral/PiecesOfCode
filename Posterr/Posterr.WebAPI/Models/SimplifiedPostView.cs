using System;

namespace Posterr.WebAPI.Models
{
    public class SimplifiedPostView
    {
        public Guid Id { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime PostedAt { get; set; }
    }
}
