using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Posterr.CoreObjects.Entities
{
    public class UserFollowings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid UserId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid FollowsThisId { get; set; }

        public DateTime Since { get; set; }
    }

    public class UserFollowingKey
    {
        public Guid UserId { get; set; }
        public Guid FollowsThisId { get; set; }
    }

}
