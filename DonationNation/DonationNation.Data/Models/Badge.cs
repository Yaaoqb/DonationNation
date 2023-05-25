using System.Collections.Generic;

namespace DonationNation.Data.Models
{
    public class Badge : BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
