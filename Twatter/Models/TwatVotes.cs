using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twatter.Models
{
    public class TwatVotes
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
