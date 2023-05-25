using System.Threading.Tasks;

namespace DonationNation.Communication.SMS
{
    public interface ISNSService
    {
        Task<string> SendSMS(string phoneNumber, string textMessage);
    }
}
