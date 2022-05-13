using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.CoreObjects.Entities
{
    public class UserStats
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid UserId { get; set; }

        public DateTime LastUpdate { get; set; }

        public int NumberOfFollowers { get; set; }

        public int NumberOfFollowing { get; set; }

        public int NumberOfPosts { get; set; }
    }

}
