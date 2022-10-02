using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace BlazingDocumentor.Extentions
{
    public static class PrivateMemberExtentions
    {        
        public static bool IsPrivate(this ClassDeclarationSyntax node) => !node.Modifiers.Any(SyntaxKind.PublicKeyword);

        public static bool IsPrivate(this FieldDeclarationSyntax node)
        {
            if (!node.Modifiers.Any(SyntaxKind.PublicKeyword))
            {
                return true;
            }

            return IsPrivate(node.Parent as ClassDeclarationSyntax);
        }

        public static bool IsPrivate(this ConstructorDeclarationSyntax node)
        {
            if (!node.Modifiers.Any(SyntaxKind.PublicKeyword))
            {
                return true;
            }

            return IsPrivate(node.Parent as ClassDeclarationSyntax);
        }

        public static bool IsPrivate(this PropertyDeclarationSyntax node)
        {
            if (!node.Modifiers.Any(SyntaxKind.PublicKeyword))
            {
                return true;
            }

            return IsPrivate(node.Parent as ClassDeclarationSyntax);
        }

        public static bool IsPrivate(this MethodDeclarationSyntax node)
        {
            if (!node.Modifiers.Any(SyntaxKind.PublicKeyword))
            {
                return true;
            }

            return IsPrivate(node.Parent as ClassDeclarationSyntax);
        }
    }
}
