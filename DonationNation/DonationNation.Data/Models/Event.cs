using System;
using System.Collections.Generic;

namespace DonationNation.Data.Models
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public ICollection<ApplicationUser> Donors  { get; set; }
    }
}
