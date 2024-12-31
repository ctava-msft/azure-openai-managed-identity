using System;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using DotNetEnv;

class Program
{
    static async Task Main(string[] args)
    {
        Env.Load();

        // Load Azure OpenAI endpoint from config file
        string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");

        // Create a DefaultAzureCredential instance to use Managed Identity
        var credential = new DefaultAzureCredential();

        // Create an OpenAIClient instance
        var client = new OpenAIClient(new Uri(endpoint), credential);

        // Define the chat completion request
        var chatCompletionRequest = new ChatCompletionsOptions
        {
            Messages =
            {
                new ChatMessage(ChatRole.System, "You are a helpful assistant."),
                new ChatMessage(ChatRole.User, "Tell me a joke.")
            }
        };

        // Send the request and get the response
        Response<ChatCompletions> response = await client.GetChatCompletionsAsync("gpt-4o", chatCompletionRequest);

        // Output the response
        foreach (var message in response.Value.Choices)
        {
            Console.WriteLine(message.Message.Content);
        }
    }
}