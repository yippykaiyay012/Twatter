using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twatter.Models
{
    public class Following
    {
        public int Id { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public ApplicationUser UserFollowing { get; set; }
        public string UserFollowingId { get; set; }
    }
}
