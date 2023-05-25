using System.Collections.Generic;
using DonationNation.Data.Models;

namespace DonationNation.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Testimonial> Testimonials { get; set; }
    }
}
