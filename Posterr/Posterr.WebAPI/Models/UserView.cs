using System;

namespace Posterr.WebAPI.Models
{
    public class UserView
    {
        public string UserName { get; set; }

        public DateTime JoinDate { get; set; }

        public int Followers { get; set; }

        public int Follows { get; set; }

        public bool FollowingThis { get; set; }

        public int NumberOfPosts { get; set; }
    }
}
