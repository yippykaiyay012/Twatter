using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Twatter.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePictureURL { get; set; }

        public ApplicationUser()
        {
            this.ProfilePictureURL = "/img/profile-placeholder.png";
        }
    }
}
