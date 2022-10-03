using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = BlazingDocumentor.Test.CSharpCodeFixVerifier<
    BlazingDocumentor.InterfaceAnalyzer,
    BlazingDocumentor.InterfaceCodeFixProvider>;

namespace BlazingDocumentor.Test
{
    [TestClass]
    public class InterfaceIntegrationTest
    {
        /// <summary>
        /// The test code.
        /// </summary>
        private const string TestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	interface IInterfaceTester
	{
	}
}";

        /// <summary>
        /// The test fix code.
        /// </summary>
        private const string TestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
    /// <summary>
    /// The interface tester.
    /// </summary>
    interface IInterfaceTester
	{
	}
}";

        //No diagnostics expected to show up
        [DataTestMethod]
        [DataRow("")]       
        public async Task NoDiagnosticTriggered(string test)
        {
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [DataTestMethod]
        [DataRow(TestCode, TestFixCode, InterfaceAnalyzer.DiagnosticId, InterfaceAnalyzer.MessageFormat, 8, 12)]        
        public async Task DiagnosticCodeFixTriggered(string test, string fixtest,string diagnosticId, string message, int line, int column)
        {
            var expected = VerifyCS.Diagnostic(diagnosticId)
                .WithLocation(line, column)
                .WithSeverity(Microsoft.CodeAnalysis.DiagnosticSeverity.Warning)
                .WithMessage(message);
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
