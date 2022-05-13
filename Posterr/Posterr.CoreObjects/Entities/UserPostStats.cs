using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.CoreObjects.Entities
{
    public class UserPostStats
    {
        public DateTime ReferenceDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid UserId { get; set; }

        public int PostsCounter { get; set; }
    }

    public class UserPostStatsKey
    {
        public DateTime ReferenceDate { get; set; }

        public Guid UserId { get; set; }
    }

}
