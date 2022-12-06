using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;
using System.Threading.Tasks;

namespace PostsLibrary
{
    public class PostPublisher
    {
        private AmazonSQSClient _amazonSQS;

        public PostPublisher(AmazonSQSClient amazonSQS)
        {
            _amazonSQS = amazonSQS;
        }

        public Task Publish(Post post)
        {
            var body = JsonSerializer.Serialize(post, FunctionJsonSerializerContext.Default.Post);

            return _amazonSQS.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = "QUEUE_URL",
                MessageBody = body
            });
        }
    }
}
