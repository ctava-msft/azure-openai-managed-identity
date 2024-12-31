using System;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Azure.Identity;
using DotNetEnv;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

// Program class
class Program
{
    // Main method
    static async Task Main(string[] args)
    {
        // Load the .env file
        Env.Load();

        // Load Azure OpenAI endpoint from config file
        string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");

        // Load Model Deployment name from config file
        string model_deployment_name = Environment.GetEnvironmentVariable("MODEL_DEPLOYMENT_NAME");

        // Load Model Id from config file
        string model_id = Environment.GetEnvironmentVariable("MODEL_ID");

        // Specify the model version
        string model_version = Environment.GetEnvironmentVariable("MODEL_VERSION");

        // Create a new Chat Completion Service
        AzureOpenAIChatCompletionService chatService = new(
            deploymentName: model_deployment_name,
            endpoint: endpoint,
            credentials: new DefaultAzureCredential(),
            modelId: model_id,
            apiVersion: model_version);

        // Send a message
        var chatHistory = new ChatHistory("You are a comedian, expert about being funny");
        chatHistory.AddUserMessage("Tell me a joke.");
        // Get the reply
        var reply = await chatService.GetChatMessageContentAsync(chatHistory);

        // Get message details
        var replyInnerContent = reply.InnerContent as OpenAI.Chat.ChatCompletion;

        // Output message details
        Program.OutputInnerContent(replyInnerContent!);
    }

    /// <summary>
    /// Retrieve extra information from a <see cref="ChatMessageContent"/> inner content of type <see cref="OpenAI.Chat.ChatCompletion"/>.
    /// </summary>
    /// <param name="innerContent">An instance of <see cref="OpenAI.Chat.ChatCompletion"/> retrieved as an inner content of <see cref="ChatMessageContent"/>.</param>
    /// <remarks>
    /// This is a breaking glass scenario, any attempt on running with different versions of OpenAI SDK that introduces breaking changes
    /// may break the code below.
    /// </remarks>
    static void OutputInnerContent(OpenAI.Chat.ChatCompletion innerContent)
    {
        Console.WriteLine($"=================================");
        Console.WriteLine($"Message role: {innerContent.Role}"); // Available as a property of ChatMessageContent
        Console.WriteLine($"Message content: {innerContent.Content[0].Text}"); // Available as a property of ChatMessageContent

        Console.WriteLine($"Model: {innerContent.Model}"); // Model doesn't change per chunk, so we can get it from the first chunk only
        Console.WriteLine($"Created At: {innerContent.CreatedAt}");

        Console.WriteLine($"Finish reason: {innerContent.FinishReason}");
        Console.WriteLine($"Input tokens usage: {innerContent.Usage.InputTokenCount}");
        Console.WriteLine($"Output tokens usage: {innerContent.Usage.OutputTokenCount}");
        Console.WriteLine($"Total tokens usage: {innerContent.Usage.TotalTokenCount}");
        Console.WriteLine($"Refusal: {innerContent.Refusal} ");
        Console.WriteLine($"Id: {innerContent.Id}");
        Console.WriteLine($"System fingerprint: {innerContent.SystemFingerprint}");

        if (innerContent.ContentTokenLogProbabilities.Count > 0)
        {
            Console.WriteLine("Content token log probabilities:");
            foreach (var contentTokenLogProbability in innerContent.ContentTokenLogProbabilities)
            {
                Console.WriteLine($"Token: {contentTokenLogProbability.Token}");
                Console.WriteLine($"Log probability: {contentTokenLogProbability.LogProbability}");

                Console.WriteLine("   Top log probabilities for this token:");
                foreach (var topLogProbability in contentTokenLogProbability.TopLogProbabilities)
                {
                    Console.WriteLine($"   Token: {topLogProbability.Token}");
                    Console.WriteLine($"   Log probability: {topLogProbability.LogProbability}");
                    Console.WriteLine("   =======");
                }

                Console.WriteLine("--------------");
            }
        }

        if (innerContent.RefusalTokenLogProbabilities.Count > 0)
        {
            Console.WriteLine("Refusal token log probabilities:");
            foreach (var refusalTokenLogProbability in innerContent.RefusalTokenLogProbabilities)
            {
                Console.WriteLine($"Token: {refusalTokenLogProbability.Token}");
                Console.WriteLine($"Log probability: {refusalTokenLogProbability.LogProbability}");

                Console.WriteLine("   Refusal top log probabilities for this token:");
                foreach (var topLogProbability in refusalTokenLogProbability.TopLogProbabilities)
                {
                    Console.WriteLine($"   Token: {topLogProbability.Token}");
                    Console.WriteLine($"   Log probability: {topLogProbability.LogProbability}");
                    Console.WriteLine("   =======");
                }
            }
        }
    }


}