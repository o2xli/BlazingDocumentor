﻿using System.Collections.Immutable;
using System.Linq;
using BlazingDocumentor.Extentions;
using BlazingDocumentor.Helper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace BlazingDocumentor
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class ClassAnalyzer : DiagnosticAnalyzer
	{
		private const string Title = "This class should have a documentation header.";

		private const string Category = DocumentationCommentHelper.Category;

		public const string DiagnosticId = "ClassDocumentationHeader";

		public const string MessageFormat = Title;

		private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

		public override void Initialize(AnalysisContext context)
		{
			context.EnableConcurrentExecution();
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
			context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration);
		}

		private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
		{
			var node = context.Node as ClassDeclarationSyntax;

			if (/*SyncOptionExtention.PublicMemberOnly() &&*/ node.IsPrivate())
			{
				return;
			}

			DocumentationCommentTriviaSyntax commentTriviaSyntax = node
				.GetLeadingTrivia()
				.Select(o => o.GetStructure())
				.OfType<DocumentationCommentTriviaSyntax>()
				.FirstOrDefault();

			if (commentTriviaSyntax != null && CommentCreator.HasAllready(commentTriviaSyntax))
			{
				return;
			}

			context.ReportDiagnostic(Diagnostic.Create(Rule, node.Identifier.GetLocation()));
		}
	}
}
