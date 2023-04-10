using OpenAI_API;
using System;

namespace BlazingDocumentor.OpenAI
{
    
    public class Commentor
    {
        private const string OpenAIApiKey = "";
        public string GetMethodSummary(string sharpCode)
        {
            OpenAIAPI api = new OpenAIAPI(OpenAIApiKey);
            var chat = api.Chat.CreateConversation();

            chat.AppendSystemMessage("Based on the source code provided, give me a brief summary of a C# method.");

            chat.AppendUserInput($"Here is the source code of the method:\n{sharpCode}");

            return chat.GetResponseFromChatbotAsync().GetAwaiter().GetResult();

        }
    }
}
