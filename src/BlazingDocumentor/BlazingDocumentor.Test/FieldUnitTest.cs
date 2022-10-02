using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = BlazingDocumentor.Test.CSharpCodeFixVerifier<
    BlazingDocumentor.FieldAnalyzer,
    BlazingDocumentor.FieldCodeFixProvider>;

namespace BlazingDocumentor.Test
{
    [TestClass]
    public class FieldUnitTest
    {
        /// <summary>
        /// The inherit doc test code.
        /// </summary>
        private const string InheritDocTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class FieldTester
	{
		/// <inheritdoc/>
		public const int ConstFieldTester = 666;

		public FieldTester()
		{
		}
	}
}";

        /// <summary>
        /// The const field test code.
        /// </summary>
        private const string ConstFieldTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class FieldTester
	{
		public const int ConstFieldTester = 666;

		public FieldTester()
		{
		}
	}
}";

        /// <summary>
        /// The const field test code.
        /// </summary>
        private const string PrivateClassestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class FieldTester
	{
		public const int ConstFieldTester = 666;

		public FieldTester()
		{
		}
	}
}";
        /// <summary>
        /// The const field test code.
        /// </summary>
        private const string PrivateConstFieldTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class FieldTester
	{
		const int ConstFieldTester = 666;

		public FieldTester()
		{
		}
	}
}";

        /// <summary>
        /// The const field test fix code.
        /// </summary>
        private const string ConstFieldTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class FieldTester
	{
        /// <summary>
        /// The const field tester.
        /// </summary>
        public const int ConstFieldTester = 666;

		public FieldTester()
		{
		}
	}
}";
        //No diagnostics expected to show up
        [DataTestMethod]
        [DataRow("")]
        [DataRow(PrivateConstFieldTestCode)]
        [DataRow(InheritDocTestCode)]
        [DataRow(PrivateClassestCode)] 
        public async Task NoDiagnosticTriggered(string test)
        {
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [DataTestMethod]
        [DataRow(ConstFieldTestCode, ConstFieldTestFixCode, FieldAnalyzer.DiagnosticId,FieldAnalyzer.MessageFormat, 10, 20)]        
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
