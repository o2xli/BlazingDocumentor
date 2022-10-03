using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = BlazingDocumentor.Test.CSharpCodeFixVerifier<
    BlazingDocumentor.ConstructorAnalyzer,
    BlazingDocumentor.ConstructorCodeFixProvider>;

namespace BlazingDocumentor.Test
{
    [TestClass]
    public class ConstrcutorIntegrationTest
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
	public class ConstructorTester
	{
		/// <inheritdoc/>
		public ConstructorTester()
		{
		}
	}
}";

        /// <summary>
        /// The public constructor test code.
        /// </summary>
        private const string PublicConstructorTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class ConstructorTester
	{
		public ConstructorTester()
		{
		}
	}
}";

        /// <summary>
        /// The public contructor test fix code.
        /// </summary>
        private const string PublicContructorTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class ConstructorTester
	{
        /// <summary>
        /// Initializes a new instance of the <see cref=""ConstructorTester""/> class.
        /// </summary>
        public ConstructorTester()
		{
		}
	}
}";

        /// <summary>
        /// The private constructor test code.
        /// </summary>
        private const string PrivateConstructorTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class ConstructorTester
	{
		private ConstructorTester()
		{
		}
	}
}";

        /// <summary>
        /// The private contructor test fix code.
        /// </summary>
        private const string PrivateContructorTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class ConstructorTester
	{
        /// <summary>
        /// Prevents a default instance of the <see cref=""ConstructorTester""/> class from being created.
        /// </summary>
        private ConstructorTester()
		{
		}
	}
}";

        /// <summary>
        /// The public constructor test code.
        /// </summary>
        private const string PublicConstructorWithBooleanParameterTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class ConstructorTester
	{
		public ConstructorTester(bool isRed, bool? isAssociatedWithAllProduct)
		{
		}
	}
}";

        /// <summary>
        /// The public contructor test fix code.
        /// </summary>
        private const string PublicContructorWithBooleanParameterTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class ConstructorTester
	{
        /// <summary>
        /// Initializes a new instance of the <see cref=""ConstructorTester""/> class.
        /// </summary>
        /// <param name=""isRed"">If true, is red.</param>
        /// <param name=""isAssociatedWithAllProduct"">If true, is associated with all product.</param>
        public ConstructorTester(bool isRed, bool? isAssociatedWithAllProduct)
		{
		}
	}
}";
        //No diagnostics expected to show up
        [DataTestMethod]
        [DataRow("")]
        [DataRow(PrivateConstructorTestCode)]
        [DataRow(InheritDocTestCode)]
        public async Task NoDiagnosticTriggered(string test)
        {
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [DataTestMethod]
        [DataRow(PublicConstructorTestCode, PublicContructorTestFixCode, ConstructorAnalyzer.DiagnosticId,ConstructorAnalyzer.MessageFormat, 10, 10)]
        [DataRow(PublicConstructorWithBooleanParameterTestCode, PublicContructorWithBooleanParameterTestFixCode, ConstructorAnalyzer.DiagnosticId, ConstructorAnalyzer.MessageFormat, 10, 10)]
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
