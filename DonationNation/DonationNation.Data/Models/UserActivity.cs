namespace DonationNation.Data.Models
{
    public class UserActivity : BaseEntity
    {
        public string Url { get; set; }
        public string Data { get; set; }
        public string UserId { get; set; }
        public string IPAddress { get; set; }
        public string UserName { get; set; }
    }
}
