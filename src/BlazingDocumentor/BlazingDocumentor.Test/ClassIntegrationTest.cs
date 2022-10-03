using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = BlazingDocumentor.Test.CSharpCodeFixVerifier<
    BlazingDocumentor.ClassAnalyzer,
    BlazingDocumentor.ClassCodeFixProvider>;

namespace BlazingDocumentor.Test
{
    [TestClass]
    public class ClassIntegrationTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task NoDiagnosticTriggered()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task DiagnosticCodeFixTriggered()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp7
{
	public class SuperDuperClassTester
	{
	}
}";

            var fixtest = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp7
{
    /// <summary>
    /// The super duper class tester.
    /// </summary>
    public class SuperDuperClassTester
	{
	}
}";

            var expected = VerifyCS.Diagnostic(ClassAnalyzer.DiagnosticId)
                .WithLocation(8, 15)
                .WithSeverity(Microsoft.CodeAnalysis.DiagnosticSeverity.Warning)
                .WithMessage(ClassAnalyzer.MessageFormat);
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
