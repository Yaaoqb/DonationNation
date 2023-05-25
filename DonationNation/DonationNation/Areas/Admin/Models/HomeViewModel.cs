using System.Collections.Generic;
using DonationNation.Data.Models;

namespace DonationNation.Areas.Admin.Models
{
    public class HomeViewModel
    {
        public string UsersCount { get; set; }
        public string Eventscount { get; set; }
        public string BadgesCount { get; set; }

        public ICollection<UserActivity> Activities { get; set; }
    }
}
