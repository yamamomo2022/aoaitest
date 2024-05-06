using Azure.AI.OpenAI;
using System;
using Microsoft.Extensions.Configuration;

namespace aoaitest
{
    public class Chat
    {
        public static async Task ChatAsync()
        {
            var cbr = new ConfigurationBuilder()
            .AddUserSecrets<Program>().Build();

            string nonAzureOpenAIApiKey = cbr["AOAI_KEY"];
            var client = new OpenAIClient(nonAzureOpenAIApiKey, new OpenAIClientOptions());
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = "gpt-3.5-turbo", // Use DeploymentName for "model" with non-Azure clients
                Messages =
                {
                    new ChatRequestSystemMessage("You are a helpful assistant. You will talk like a pirate."),
                    new ChatRequestUserMessage("Can you help me?"),
                    new ChatRequestAssistantMessage("Arrrr! Of course, me hearty! What can I do for ye?"),
                    new ChatRequestUserMessage("What's the best way to train a parrot?"),
                }
            };

            await foreach (StreamingChatCompletionsUpdate chatUpdate in client.GetChatCompletionsStreaming(chatCompletionsOptions))
            {
                if (chatUpdate.Role.HasValue)
                {
                    Console.Write($"{chatUpdate.Role.Value.ToString().ToUpperInvariant()}: ");
                }
                if (!string.IsNullOrEmpty(chatUpdate.ContentUpdate))
                {
                    Console.Write(chatUpdate.ContentUpdate);
                }
            }
        }
    }
}