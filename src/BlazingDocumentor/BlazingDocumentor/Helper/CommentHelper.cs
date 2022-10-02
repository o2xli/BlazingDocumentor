using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BlazingDocumentor.Helper
{
	public static class CommentCreator
	{
		public static string CreateClass(string name)
		{
			return CreateCommon(name);
		}

		public static string CreateField(string name)
		{
			return CreateCommon(name);
		}

		public static string CreateConstructor(string name, bool isPrivate)
		{
			if (isPrivate)
			{
				return $"Prevents a new instance of the <see cref=\"{name}\"/> class from being created.";
			}
			else
			{
				return $"Initializes a new instance of the <see cref=\"{name}\"/> class.";
			}
		}

		public static string CreateInterface(string name)
		{
			List<string> parts = WordSplitter.Split(name).ToLower(false).ToList();
			if (parts[0]=="I")
			{
				parts.RemoveAt(0);
			}

			parts.Insert(0,"The");
			return $"{string.Join(" ",parts)}.";
		}

		public static string CreateEnum(string name)
		{
			return CreateCommon(name);
		}

		public static string CreateProperty(string name, bool isBoolean, bool hasSetter)
		{
			var Builder = new StringBuilder();
            Builder.Append("Gets");
			if (hasSetter)
			{
                Builder.Append(" or sets");
			}

			if (isBoolean)
			{
                Builder.Append(CreatePropertyBooleanPart(name));
			}
			else
			{
                Builder.Append($" the {string.Join(" ", WordSplitter.Split(name).ToLower(false))}");
			}
			Builder.Append(".");

			return Builder.ToString();
		}

		public static string CreateMethod(string name)
		{
			List<string> parts = WordSplitter.Split(name).ToLower(false).ToList();
			parts[0] = Pluralizer.Pluralize(parts[0]);
			parts.Insert(1, "the");
			return $"{string.Join(" ", parts)}.";
        }

		public static string CreateParameter(ParameterSyntax parameter)
		{
			bool isBoolean = false;
			if (parameter.Type.IsKind(SyntaxKind.PredefinedType))
			{
				isBoolean = (parameter.Type as PredefinedTypeSyntax).Keyword.IsKind(SyntaxKind.BoolKeyword);
			}
			else if (parameter.Type.IsKind(SyntaxKind.NullableType))
			{
				var type = (parameter.Type as NullableTypeSyntax).ElementType as PredefinedTypeSyntax;

				if (type != null)
				{
					isBoolean = type.Keyword.IsKind(SyntaxKind.BoolKeyword);
				}
			}

			if (isBoolean)
			{
				return $"If true, {string.Join(" ", WordSplitter.Split(parameter.Identifier.ValueText).ToLower(true))}.";
			}
			
			return CreateCommon(parameter.Identifier.ValueText);
			
		}

		public static bool HasAllready(DocumentationCommentTriviaSyntax commentTriviaSyntax)
		{
			return 				
				commentTriviaSyntax
				.ChildNodes()
				.OfType<XmlElementSyntax>()
				.Any(o => o.StartTag.Name.ToString().Equals(DocumentationCommentHelper.Summary)) 
				|| commentTriviaSyntax
				.ChildNodes()
				.OfType<XmlEmptyElementSyntax>()
				.Any(o => o.Name.ToString().Equals(DocumentationCommentHelper.InheritDoc));			
		}

		private static string CreatePropertyBooleanPart(string name)
		{
			var parts = WordSplitter.Split(name).ToLower(true).ToList();

			string isWord = parts.FirstOrDefault(o => o == "is");
			if (isWord != null)
			{
				parts.Remove(isWord);
				parts.Insert(parts.Count - 1, isWord);
			}

			var booleanPart = $" a value indicating whether {string.Join(" ", parts)}";
			return booleanPart;
		}

		private static string CreateCommon(string name)
		{
			return $"The {string.Join(" ", WordSplitter.Split(name).ToLower(true))}.";
		}

		
	}
}
