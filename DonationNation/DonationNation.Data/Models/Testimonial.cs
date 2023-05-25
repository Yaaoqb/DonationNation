namespace DonationNation.Data.Models
{
    public class Testimonial : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Message { get; set; }
    }
}
