using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twatter.Models;

namespace Twatter.ViewModels
{
    public class PostsListViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public Post NewPost { get; set; }
    }
}
