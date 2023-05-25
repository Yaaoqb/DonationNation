using Microsoft.AspNetCore.Http;

namespace DonationNation.Areas.Admin.Models
{
    public class BadgeCreate
    {
        public IFormFile Image { get; set; }
        public string Name { get; set; }
    }
}
