using OpenAI_API;
using System;
using System.Data.SqlTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace BlazingDocumentor.OpenAI
{
    
    public class Commentor
    {
        public string GetMethodSummary(string sharpCode)
        {
            OpenAIAPI api = new OpenAIAPI();
            var chat = api.Chat.CreateConversation();

            chat.AppendSystemMessage("Based on the source code provided, give me a brief summary of a C# method.");

            chat.AppendUserInput($"Here is the source code of the method:\n{sharpCode}");

            return chat.GetResponseFromChatbotAsync().GetAwaiter().GetResult();

        }



    public DocumentationCommentTriviaSyntax GetMethodXmlDoc(string sharpCode)
        {
            OpenAIAPI api = new OpenAIAPI();
            var chat = api.Chat.CreateConversation();

            chat.AppendSystemMessage("Based on the source code provided, give me a XML comment of a C# method.");

            chat.AppendUserInput($"Here is the source code of the method:\n{sharpCode}");

            var result = chat.GetResponseFromChatbotAsync().GetAwaiter().GetResult()+"\r\n";

            DocumentationCommentTriviaSyntax commentTrivia = SyntaxFactory.ParseLeadingTrivia(result)
                .Select(trivia => trivia.GetStructure())
                .OfType<DocumentationCommentTriviaSyntax>()
                .FirstOrDefault();

            return commentTrivia;
        }
    }
}
