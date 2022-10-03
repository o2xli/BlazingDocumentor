using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace BlazingDocumentor.Helper
{
	public class ReturnCommentConstruction
	{
		public ReturnCommentConstruction(TypeSyntax returnType)
		{
			if (returnType is PredefinedTypeSyntax predefinedTypeSyntax)
			{
				this.Comment = GeneratePredefinedTypeComment(predefinedTypeSyntax);
			}
			else if (returnType is IdentifierNameSyntax identifierNameSyntax)
			{
				this.Comment = GenerateIdentifierNameTypeComment(identifierNameSyntax);
			}
			else if (returnType is QualifiedNameSyntax qualifiedNameSyntax)
			{
				this.Comment = GenerateQualifiedNameTypeComment(qualifiedNameSyntax);
			}
			else if (returnType is GenericNameSyntax genericNameSyntax)
			{
				this.Comment = GenerateGenericTypeComment(genericNameSyntax);
			}
			else if (returnType is ArrayTypeSyntax arrayTypeSyntax)
			{
				this.Comment = this.GenerateArrayTypeComment(arrayTypeSyntax);
			}
			else
			{
				this.Comment = GenerateGeneralComment(returnType.ToFullString());
			}
		}

		public string Comment { get; }

		private string GeneratePredefinedTypeComment(PredefinedTypeSyntax returnType)
		{
			return $"{DetermineStartedWord(returnType.Keyword.ValueText)} {returnType.Keyword.ValueText}.";
		}

		private string GenerateIdentifierNameTypeComment(IdentifierNameSyntax returnType)
		{
			return GenerateGeneralComment(returnType.Identifier.ValueText);
		}

		private string GenerateQualifiedNameTypeComment(QualifiedNameSyntax returnType)
		{
			return GenerateGeneralComment(returnType.ToString());
		}

		private string GenerateArrayTypeComment(ArrayTypeSyntax arrayTypeSyntax)
		{
			return $"An array of {DetermineSpecificObjectName(arrayTypeSyntax.ElementType)}";
		}

		private string GenerateGenericTypeComment(GenericNameSyntax returnType)
		{
			string genericTypeStr = returnType.Identifier.ValueText;
			if (genericTypeStr.Contains("ReadOnlyCollection"))
			{
				return $"A read only collection of {DetermineSpecificObjectName(returnType.TypeArgumentList.Arguments.First())}";
			}

            if (genericTypeStr == "IEnumerable" || genericTypeStr.Contains("List"))
            {
                return $"A list of {DetermineSpecificObjectName(returnType.TypeArgumentList.Arguments.First())}";
            }

            if (genericTypeStr == "Task" )
            {
                return $"A Task result of {DetermineSpecificObjectName(returnType.TypeArgumentList.Arguments.First())}";
            }

            if (genericTypeStr.Contains("Dictionary"))
			{
				return GenerateGeneralComment(genericTypeStr);
			}

			return GenerateGeneralComment(genericTypeStr);
		}

		private string GenerateGeneralComment(string returnType)
		{
			return $"{DetermineStartedWord(returnType)} {returnType}.";
		}

		private string DetermineSpecificObjectName(TypeSyntax specificType)
		{
			string result = null;
			if (specificType is IdentifierNameSyntax identifierNameSyntax)
			{
				result = Pluralizer.Pluralize(identifierNameSyntax.Identifier.ValueText);
			}
			else if (specificType is PredefinedTypeSyntax predefinedTypeSyntax)
			{
				result = predefinedTypeSyntax.Keyword.ValueText;
			}
			else if (specificType is GenericNameSyntax genericNameSyntax)
			{
				result = genericNameSyntax.Identifier.ValueText;
			}
			else
			{
				result = specificType.ToFullString();
			}
			return $"{result}.";
		}

		private string DetermineStartedWord(string returnType)
		{
			var vowelChars = new List<char>() { 'a', 'e', 'i', 'o', 'u' };
			if (vowelChars.Contains(char.ToLower(returnType[0])))
			{
				return "An";
			}
			return "A";
		}
	}
}
