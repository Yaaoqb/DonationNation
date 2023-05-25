using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DonationNation.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }

        public ICollection<Badge> Badges { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}