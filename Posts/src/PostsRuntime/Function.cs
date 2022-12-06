using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.SQS;
using PostsLibrary;
using PostsRuntime;
using System.Text.Json;
using System.Text.Json.Serialization;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(SourceGeneratorLambdaJsonSerializer<LambdaRuntimeFunctionJsonSerializerContext>))]

namespace PostsRuntime;

public class Function
{
    private readonly PostRepository _postRepository;

    private readonly PostPublisher _postPublisher;

    public Function()
    {
        _postRepository = new PostRepository(new AmazonDynamoDBClient());

        _postPublisher = new PostPublisher(new AmazonSQSClient());
    }

    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest input, ILambdaContext context)
    {
        var post = JsonSerializer.Deserialize(input.Body, FunctionJsonSerializerContext.Default.Post)!;

        await _postRepository.Save(post);

        await _postPublisher.Publish(post);

        var body = JsonSerializer.Serialize(post, FunctionJsonSerializerContext.Default.Post);

        return new APIGatewayHttpApiV2ProxyResponse
        {
            Body = body,
            StatusCode = 200,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}

[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest))]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse))]
public partial class LambdaRuntimeFunctionJsonSerializerContext : JsonSerializerContext
{
    // By using this partial class derived from JsonSerializerContext, we can generate reflection free JSON Serializer code at compile time
    // which can deserialize our class and properties. However, we must attribute this class to tell it what types to generate serialization code for.
    // See https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-source-generation
}