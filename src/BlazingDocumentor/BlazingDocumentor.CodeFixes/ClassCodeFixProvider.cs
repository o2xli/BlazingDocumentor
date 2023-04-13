using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazingDocumentor.Helper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OpenAI_API.Moderation;

namespace BlazingDocumentor
{
	[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ClassCodeFixProvider)), Shared]
	public class ClassCodeFixProvider : CodeFixProvider
	{
		private const string Title = "Add documentation header to this class";

		public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ClassAnalyzer.DiagnosticId);

		public sealed override FixAllProvider GetFixAllProvider()
		{
			return WellKnownFixAllProviders.BatchFixer;
		}

		public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

			Diagnostic diagnostic = context.Diagnostics.First();
			Microsoft.CodeAnalysis.Text.TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

			ClassDeclarationSyntax declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();

			context.RegisterCodeFix(
				CodeAction.Create(
					title: Title,
					createChangedDocument: c => this.AddDocumentationHeaderAsync(context.Document, root, declaration, c),
					equivalenceKey: Title),
				diagnostic);
		}

		private async Task<Document> AddDocumentationHeaderAsync(Document document, SyntaxNode root, ClassDeclarationSyntax declarationSyntax, CancellationToken cancellationToken)
		{
			SyntaxTriviaList leadingTrivia = declarationSyntax.GetLeadingTrivia();
            SyntaxTrivia indentTrivia = leadingTrivia.LastOrDefault(trivia => trivia.IsKind(SyntaxKind.WhitespaceTrivia));

            var result = await OpenAIDocumentationCommentHelper.GetClassCommentAsync(declarationSyntax.ToFullString(), indentTrivia.ToFullString());

            string comment = CommentCreator.CreateClass(declarationSyntax.Identifier.ValueText);
            DocumentationCommentTriviaSyntax commentTrivia = SyntaxFactory.ParseLeadingTrivia(result)
                       .Select(trivia => trivia.GetStructure())
                       .OfType<DocumentationCommentTriviaSyntax>()
                       .FirstOrDefault();


			SyntaxTriviaList newLeadingTrivia = leadingTrivia.Insert(0, SyntaxFactory.Trivia(commentTrivia));
            newLeadingTrivia = newLeadingTrivia.Insert(0, indentTrivia);

            ClassDeclarationSyntax newDeclaration = declarationSyntax.WithLeadingTrivia(newLeadingTrivia);

			SyntaxNode newRoot = root.ReplaceNode(declarationSyntax, newDeclaration).NormalizeWhitespace();
			return document.WithSyntaxRoot(newRoot);
		}
	}
}
