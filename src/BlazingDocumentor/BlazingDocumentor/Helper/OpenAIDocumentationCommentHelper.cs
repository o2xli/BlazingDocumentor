using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API;
using System.Runtime.Caching;
using System.Security.Cryptography;

namespace BlazingDocumentor.Helper
{
    public static class OpenAIDocumentationCommentHelper
    {

        public static async Task<string> GetClassCommentAsync(string declarationString, string indent)
        {
            if (TryGetCache(declarationString, out var cacheValue))
                return cacheValue;
            OpenAIAPI api = new OpenAIAPI();
            var chat = api.Chat.CreateConversation();

            chat.AppendSystemMessage("Based on the source code provided, give me a XML comment of a C# class. Limit the max length of a line to 100 chars. Use the same identation.");

            chat.AppendUserInput($"Here is the source code of the class:\n{declarationString}");

            var result = await chat.GetResponseFromChatbotAsync() + "\n";
            result = result.Replace("///", indent + "///");

            return result;
        }

        public static async Task<string> GetMethodCommentAsync(string declarationString, string indent)
        {
            if (TryGetCache(declarationString, out var cacheValue))
                return cacheValue;
                
            OpenAIAPI api = new OpenAIAPI();
            var chat = api.Chat.CreateConversation();

            chat.AppendSystemMessage("Based on the source code provided, give me a XML comment of a C# method. Limit the max length of a line to 100 chars. Add links to object types. But do not add seealso tags.");

            chat.AppendUserInput($"Here is the source code of the method:\n{declarationString}");

            var result = await chat.GetResponseFromChatbotAsync() + "\n";
            result = result.Replace("///", indent + "///");

            SetCache(declarationString, result);

            return result;
        }

        private static void SetCache(string declarationString,string result)
        {
            ObjectCache cache = MemoryCache.Default;
            var key = GetHash(declarationString);
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.SlidingExpiration = TimeSpan.FromSeconds(30);

            cache.Add(key, result, policy);

        }
        private static bool TryGetCache(string declarationString,out string value)
        {
            ObjectCache cache = MemoryCache.Default;
            var key = GetHash(declarationString);
            value = cache.Get(key) as string;
            if(value == null)
                return false;
            else
                return true;
        }

        private static string GetHash(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException(nameof(input));

            using (HashAlgorithm algorithm = SHA256.Create())
            {
                StringBuilder sb = new StringBuilder();
                foreach (byte b in algorithm.ComputeHash(Encoding.UTF8.GetBytes(input)))
                    sb.Append(b.ToString("X2"));

                return sb.ToString();
            }
        }
    }
}
