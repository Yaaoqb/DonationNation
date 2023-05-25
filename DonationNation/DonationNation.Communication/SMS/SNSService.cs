using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DonationNation.Communication.SMS
{
    public class SNSService : ISNSService
    {
        private IConfiguration _config;

        public SNSService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> SendSMS(string phoneNumber, string textMessage)
        {
            var awsConfig = _config.GetSection("AWS");

            var accessKey = awsConfig["AccessKey"];
            var secretKey = awsConfig["SecretKey"];
            var securityToken = awsConfig["SessionToken"];

            var client = new AmazonSimpleNotificationServiceClient(accessKey, secretKey, securityToken, RegionEndpoint.USEast1);
            var messageAttributes = new Dictionary<string, MessageAttributeValue>();
            var smsType = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = "Transactional"
            };

            var senderID = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = "Donation"
            };

            messageAttributes.Add("AWS.SNS.SMS.SMSType", smsType);
            messageAttributes.Add("AWS.SNS.SMS.SenderID", senderID);

            PublishRequest request = new PublishRequest
            {
                Message = textMessage,
                PhoneNumber = phoneNumber,
                MessageAttributes = messageAttributes
            };

            return (await client.PublishAsync(request)).HttpStatusCode.ToString();

        }
    }
}
