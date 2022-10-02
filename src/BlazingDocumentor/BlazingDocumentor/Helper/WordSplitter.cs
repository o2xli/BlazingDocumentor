using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace BlazingDocumentor.Helper
{
	public static class WordSplitter
	{       
        public static IEnumerable<string> Split(string name)
        {
            StringBuilder part = new StringBuilder();
            foreach (char c in name)
            {
                if (char.IsUpper(c) && part.Length > 0)
                {
                    yield return part.ToString();
                    part.Clear();
                }
                
                part.Append(c);
            }
            if (part.Length > 0)
            {
                yield return part.ToString();
            }
            
        }

        public static IEnumerable<string> ToLower(this IEnumerable<string> words, bool isFirstCharLower)
        {
            var jumpOverFirst = !isFirstCharLower;
            foreach (var word in words)
            {
                if (jumpOverFirst)
                {
                    jumpOverFirst = false;
                    yield return word;
                    continue;
                }
                yield return word.ToLower();
            }
        }

    }
}
