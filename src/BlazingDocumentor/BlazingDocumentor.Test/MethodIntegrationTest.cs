using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = BlazingDocumentor.Test.CSharpCodeFixVerifier<
    BlazingDocumentor.MethodAnalyzer,
    BlazingDocumentor.MethodCodeFixProvider>;

namespace BlazingDocumentor.Test
{
    [TestClass]
    public class MethodIntegrationTest
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
	public class MethodTester
	{
		/// <inheritdoc/>
		public void ShowBasicMethodTester()
		{
		}
	}
}";

        /// <summary>
        /// The basic test code.
        /// </summary>
        private const string BasicTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public void ShowBasicMethodTester()
		{
		}
	}
}";

        /// <summary>
        /// The basic test fix code.
        /// </summary>
        private const string BasicTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the basic method tester.
        /// </summary>
        public void ShowBasicMethodTester()
		{
		}
	}
}";

        /// <summary>
        /// The method with parameter test code.
        /// </summary>
        private const string MethodWithParameterTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public void ShowMethodWithParameterTester(string param1, int param2, bool param3)
		{
		}
	}
}";
        /// <summary>
        /// The method with parameter test fix code.
        /// </summary>
        private const string MethodWithParameterTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the method with parameter tester.
        /// </summary>
        /// <param name=""param1"">The param1.</param>
        /// <param name=""param2"">The param2.</param>
        /// <param name=""param3"">If true, param3.</param>
        public void ShowMethodWithParameterTester(string param1, int param2, bool param3)
		{
		}
	}
}";

        /// <summary>
        /// The method with parameter test code.
        /// </summary>
        private const string MethodWithBooleanParameterTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public void ShowMethodWithBooleanParameterTester(bool isRed, bool? isAssociatedWithAllProduct)
		{
		}
	}
}";
        /// <summary>
        /// The method with parameter test fix code.
        /// </summary>
        private const string MethodWithBooleanParameterTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the method with boolean parameter tester.
        /// </summary>
        /// <param name=""isRed"">If true, is red.</param>
        /// <param name=""isAssociatedWithAllProduct"">If true, is associated with all product.</param>
        public void ShowMethodWithBooleanParameterTester(bool isRed, bool? isAssociatedWithAllProduct)
		{
		}
	}
}";

        /// <summary>
        /// The method with parameter test code.
        /// </summary>
        private const string MethodWithNullableStructParameterTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public void Show(string param1, int param2, bool param3)
		{
		}
	}
}";

        /// <summary>
        /// The method with parameter test fix code.
        /// </summary>
        private const string MethodWithNullableStructParameterTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the.
        /// </summary>
        /// <param name=""param1"">The param1.</param>
        /// <param name=""param2"">The param2.</param>
        /// <param name=""param3"">If true, param3.</param>
        public void Show(string param1, int param2, bool param3)
		{
		}
	}
}";

        /// <summary>
        /// The method with return test code.
        /// </summary>
        private const string MethodWithReturnTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public MethodTester ShowMethodWithReturnTester()
		{
			return null;
		}
	}
}";

        /// <summary>
        /// The method with return test fix code.
        /// </summary>
        private const string MethodWithReturnTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the method with return tester.
        /// </summary>
        /// <returns>A MethodTester.</returns>
        public MethodTester ShowMethodWithReturnTester()
		{
			return null;
		}
	}
}";

        /// <summary>
        /// The method with string return test code.
        /// </summary>
        private const string MethodWithStringReturnTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public string ShowMethodWithStringReturnTester()
		{
			return null;
		}
	}
}";

        /// <summary>
        /// The method with string return test fix code.
        /// </summary>
        private const string MethodWithStringReturnTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the method with string return tester.
        /// </summary>
        /// <returns>A string.</returns>
        public string ShowMethodWithStringReturnTester()
		{
			return null;
		}
	}
}";

        /// <summary>
        /// The method with object return test code.
        /// </summary>
        private const string MethodWithObjectReturnTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public object ShowMethodWithObjectReturnTester()
		{
			return null;
		}
	}
}";

        /// <summary>
        /// The method with object return test fix code.
        /// </summary>
        private const string MethodWithObjectReturnTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the method with object return tester.
        /// </summary>
        /// <returns>An object.</returns>
        public object ShowMethodWithObjectReturnTester()
		{
			return null;
		}
	}
}";

        /// <summary>
        /// The method with int return test code.
        /// </summary>
        private const string MethodWithIntReturnTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public int ShowMethodWithIntReturnTester()
		{
			return 12;
		}
	}
}";

        /// <summary>
        /// The method with int return test fix code.
        /// </summary>
        private const string MethodWithIntReturnTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the method with int return tester.
        /// </summary>
        /// <returns>An int.</returns>
        public int ShowMethodWithIntReturnTester()
		{
			return 12;
		}
	}
}";
        /// <summary>
        /// The method with list int return test code.
        /// </summary>
        private const string MethodWithListIntReturnTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public List<int> ShowMethodWithListIntReturnTester()
		{
			return null;
		}
	}
}";
        /// <summary>
        /// The method with list int return test code.
        /// </summary>
        private const string PrivateMethodWithListIntReturnTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		private List<int> ShowMethodWithListIntReturnTester()
		{
			return null;
		}
	}
}";

        /// <summary>
        /// The method with list int return test fix code.
        /// </summary>
        private const string MethodWithListIntReturnTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the method with list int return tester.
        /// </summary>
        /// <returns>A list of int.</returns>
        public List<int> ShowMethodWithListIntReturnTester()
		{
			return null;
		}
	}
}";

        /// <summary>
        /// The method with list list int return test code.
        /// </summary>
        private const string MethodWithListListIntReturnTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public List<List<int>> ShowMethodWithListListIntReturnTester()
		{
			return null;
		}
	}
}";

        /// <summary>
        /// The method with list list int return test fix code.
        /// </summary>
        private const string MethodWithListListIntReturnTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the method with list list int return tester.
        /// </summary>
        /// <returns>A list of List.</returns>
        public List<List<int>> ShowMethodWithListListIntReturnTester()
		{
			return null;
		}
	}
}";

        /// <summary>
        /// The method with list qualified name return test code.
        /// </summary>
        private const string MethodWithListQualifiedNameReturnTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
		public List<ConsoleApp4.MethodTester> ShowMethodWithListQualifiedNameReturnTester()
		{
			return null;
		}
	}
}";
        /// <summary>
        /// The method with list qualified name return test code.
        /// </summary>
        private const string PrivateClassTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class MethodTester
	{
		public List<ConsoleApp4.MethodTester> ShowMethodWithListQualifiedNameReturnTester()
		{
			return null;
		}
	}
}";
        /// <summary>
        /// The method with list qualified name return test fix code.
        /// </summary>
        private const string MethodWithListQualifiedNameReturnTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class MethodTester
	{
        /// <summary>
        /// Shows the method with list qualified name return tester.
        /// </summary>
        /// <returns>A list of ConsoleApp4.MethodTester.</returns>
        public List<ConsoleApp4.MethodTester> ShowMethodWithListQualifiedNameReturnTester()
		{
			return null;
		}
	}
}";
        //No diagnostics expected to show up
        [DataTestMethod]
        [DataRow("")]
        [DataRow(PrivateMethodWithListIntReturnTestCode)]
        [DataRow(InheritDocTestCode)]
        [DataRow(PrivateClassTestCode)] 
        public async Task NoDiagnosticTriggered(string test)
        {
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [DataTestMethod]
        //[DataRow(BasicTestCode, BasicTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 15)]
        //[DataRow(MethodWithParameterTestCode, MethodWithParameterTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 15)]
        //[DataRow(MethodWithBooleanParameterTestCode, MethodWithBooleanParameterTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 15)]
        //[DataRow(MethodWithNullableStructParameterTestCode, MethodWithNullableStructParameterTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 15)]
        //[DataRow(MethodWithReturnTestCode, MethodWithReturnTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 23)]
        //[DataRow(MethodWithStringReturnTestCode, MethodWithStringReturnTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 17)]
        //[DataRow(MethodWithObjectReturnTestCode, MethodWithObjectReturnTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 17)]
        [DataRow(MethodWithIntReturnTestCode, MethodWithIntReturnTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 14)]
        //[DataRow(MethodWithListIntReturnTestCode, MethodWithListIntReturnTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 20)]
        //[DataRow(MethodWithListListIntReturnTestCode, MethodWithListListIntReturnTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 26)]
        //[DataRow(MethodWithListQualifiedNameReturnTestCode, MethodWithListQualifiedNameReturnTestFixCode, MethodAnalyzer.DiagnosticId, MethodAnalyzer.MessageFormat, 10, 41)]
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
