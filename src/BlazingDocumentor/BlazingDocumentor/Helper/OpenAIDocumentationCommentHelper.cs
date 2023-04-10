using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API;

namespace BlazingDocumentor.Helper
{
    public static class OpenAIDocumentationCommentHelper
    {

        public static async Task<string> GetClassCommentAsync(string declarationString)
        {
            OpenAIAPI api = new OpenAIAPI();
            var chat = api.Chat.CreateConversation();

            chat.AppendSystemMessage("Based on the source code provided, give me a XML comment of a C# class. Limit the max length of a line to 100 chars. Use the same identation.");

            chat.AppendUserInput($"Here is the source code of the class:\n{declarationString}");

            var result = await chat.GetResponseFromChatbotAsync() + "\n";

            return result;
        }

        public static async Task<string> GetMethodCommentAsync(string declarationString)
        {
            OpenAIAPI api = new OpenAIAPI();
            var chat = api.Chat.CreateConversation();

            chat.AppendSystemMessage("Based on the source code provided, give me a XML comment of a C# method. Limit the max length of a line to 100 chars. Add links to object types.");

            chat.AppendUserInput($"Here is the source code of the method:\n{declarationString}");

            var result = await chat.GetResponseFromChatbotAsync() + "\n";

            return result;
        }
    }
}
