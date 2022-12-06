using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostsLibrary
{
    public class PostRepository
    {
        private AmazonDynamoDBClient _amazonDynamoDB;

        public PostRepository(AmazonDynamoDBClient amazonDynamoDB)
        {
            _amazonDynamoDB = amazonDynamoDB;
        }

        public Task Save(Post post)
        {
            post.Id = Guid.NewGuid();

            var putItemRequest = new PutItemRequest
            {
                TableName = "Posts",
                Item = new Dictionary<string, AttributeValue> {
                    {
                        "id",
                        new AttributeValue {
                        S = post.Id.ToString(),
                    }
                    },
                    {
                        "name",
                        new AttributeValue {
                        S = post.Name
                        }
                    }
                }
            };

            return _amazonDynamoDB.PutItemAsync(putItemRequest);
        }
    }
}
