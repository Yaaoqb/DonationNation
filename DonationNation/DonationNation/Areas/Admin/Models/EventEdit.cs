using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace DonationNation.Areas.Admin.Models
{
    public class EventEdit
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string Description { get; set; }

        public IFormFile ImageFile { get; set; }

        public string Image { get; set; }
    }
}
