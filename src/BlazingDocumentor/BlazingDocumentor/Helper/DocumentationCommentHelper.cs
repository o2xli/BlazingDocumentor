using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlazingDocumentor.Helper
{
	public static class DocumentationCommentHelper
	{
		public const string Category = "DocumentationComment";
		public const string Summary = "summary";
		public const string InheritDoc = "inheritdoc";

		public static DocumentationCommentTriviaSyntax CreateOnlySummaryDocumentationCommentTrivia(string content)
		{
			SyntaxList<XmlNodeSyntax> list = SyntaxFactory.List(CreateSummaryPartNodes(content));
			return SyntaxFactory.DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia, list);
		}

		public static XmlNodeSyntax[] CreateSummaryPartNodes(string content)
		{
			return new XmlNodeSyntax[]
			{
				CreateLineStartTextSyntax(),
				CreateSummaryElementSyntax(content),
				CreateLineEndTextSyntax()
			};
		}

		public static XmlNodeSyntax[] CreateParameterPartNodes(string parameterName, string parameterContent)
		{
            return new XmlNodeSyntax[] 
			{ 
				CreateLineStartTextSyntax(),
				CreateParameterElementSyntax(parameterName, parameterContent),
				CreateLineEndTextSyntax()
			};
		}

		public static XmlNodeSyntax[] CreateReturnPartNodes(string content)
		{
            return new XmlNodeSyntax[] 
			{
				CreateLineStartTextSyntax(),
				CreateReturnElementSyntax(content),
				CreateLineEndTextSyntax(),
			};
		}

		private static XmlElementSyntax CreateSummaryElementSyntax(string content)
		{
			var xmlName = SyntaxFactory.XmlName(SyntaxFactory.Identifier(DocumentationCommentHelper.Summary));
			
			return SyntaxFactory.XmlElement
			(
                SyntaxFactory.XmlElementStartTag(xmlName),
				SyntaxFactory.SingletonList<XmlNodeSyntax>(CreateSummaryTextSyntax(content)),
                SyntaxFactory.XmlElementEndTag(xmlName)
			);
		}

		private static XmlElementSyntax CreateParameterElementSyntax(string parameterName, string parameterContent)
		{
			var paramName = SyntaxFactory.XmlName("param");
			var paramAttribute = SyntaxFactory.XmlNameAttribute(parameterName);
			var content = SyntaxFactory.XmlText(parameterContent);
		
			return SyntaxFactory.XmlElement
			(
				SyntaxFactory.XmlElementStartTag(paramName, SyntaxFactory.SingletonList<XmlAttributeSyntax>(paramAttribute)),
				SyntaxFactory.SingletonList<SyntaxNode>(content),
				SyntaxFactory.XmlElementEndTag(paramName)
			);
		}

		private static XmlElementSyntax CreateReturnElementSyntax(string content)
		{
			var xmlName = SyntaxFactory.XmlName("returns");
            var contentText = SyntaxFactory.XmlText(content);

			return SyntaxFactory.XmlElement
			(
				SyntaxFactory.XmlElementStartTag(xmlName),
				SyntaxFactory.SingletonList<XmlNodeSyntax>(contentText),
				SyntaxFactory.XmlElementEndTag(xmlName)
			);
		}

		private static XmlTextSyntax CreateSummaryTextSyntax(string content)
		{
			var leadingTrivia = CreateCommentExterior();
            var leadingTrivia2 = CreateCommentExterior();

			return SyntaxFactory.XmlText
			(
                CreateNewLineToken(),
                SyntaxFactory.XmlTextLiteral(leadingTrivia, $" {content}", $" {content}", SyntaxFactory.TriviaList()),
                CreateNewLineToken(),
                SyntaxFactory.XmlTextLiteral(leadingTrivia2, " ", " ", SyntaxFactory.TriviaList())
            );
		}

		private static XmlTextSyntax CreateLineStartTextSyntax()
		{
            var xmlTextLeading = CreateCommentExterior();
            var xmlTextLiteralToken = SyntaxFactory.XmlTextLiteral(xmlTextLeading, " ", " ", SyntaxFactory.TriviaList());
            var xmlText = SyntaxFactory.XmlText(xmlTextLiteralToken);
			return xmlText;
		}

		private static XmlTextSyntax CreateLineEndTextSyntax()
		{
			return SyntaxFactory.XmlText(CreateNewLineToken());
		}

		private static SyntaxToken CreateNewLineToken()
		{
			return SyntaxFactory.XmlTextNewLine(Environment.NewLine, false);
		}

		private static SyntaxTriviaList CreateCommentExterior()
		{
			return SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("///"));
		}
	}
}
