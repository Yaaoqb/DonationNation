using Microsoft.AspNetCore.Http;

namespace DonationNation.Areas.Admin.Models
{
    public class BadgeEdit
    {
        public int Id { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
    }
}
