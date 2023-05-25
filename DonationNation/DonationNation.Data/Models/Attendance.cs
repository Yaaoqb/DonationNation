namespace DonationNation.Data.Models
{
    public class Attendance : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public bool isPresent { get; set; }
    }
}
