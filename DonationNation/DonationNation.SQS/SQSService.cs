//using Amazon.SQS;
//using Amazon.SQS.Model;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace DonationNation.SQS
//{
//    public class SQSService : ISQSService
//    {
//        private readonly IAmazonSQS _sqs;

//        public SQSService(IAmazonSQS sqs)
//        {
//            _sqs = sqs;
//        }

//        public async Task<bool> SendMessageAsync(string data)
//        {
//            try
//            {
//                var queueUrl = "";
//                string message = JsonConvert.SerializeObject(data);
//                var sendRequest = new SendMessageRequest(queueUrl, message);
//                // Post message or payload to queue  
//                var sendResult = await _sqs.SendMessageAsync(sendRequest);

//                return sendResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        public async Task<List<Message>> ReceiveMessageAsync()
//        {
//            try
//            {
//                var queueUrl = "";
//                //Create New instance  
//                var request = new ReceiveMessageRequest
//                {
//                    QueueUrl = queueUrl,
//                    MaxNumberOfMessages = 10,
//                    WaitTimeSeconds = 5
//                };
//                //CheckIs there any new message available to process  
//                var result = await _sqs.ReceiveMessageAsync(request);

//                return result.Messages.Any() ? result.Messages : new List<Message>();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        public async Task<bool> DeleteMessageAsync(string messageReceiptHandle)
//        {
//            try
//            {
//                var queueUrl = "";
//                //Deletes the specified message from the specified queue  
//                var deleteResult = await _sqs.DeleteMessageAsync(queueUrl, messageReceiptHandle);
//                return deleteResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//    }
//}
