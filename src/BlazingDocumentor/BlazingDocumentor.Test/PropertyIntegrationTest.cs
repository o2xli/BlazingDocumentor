using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = BlazingDocumentor.Test.CSharpCodeFixVerifier<
    BlazingDocumentor.PropertyAnalyzer,
    BlazingDocumentor.PropertyCodeFixProvider>;

namespace BlazingDocumentor.Test
{
    [TestClass]
    public class PropertyIntegrationTest
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
	public class PropertyTester
	{
		/// <inheritdoc/>
		public string PersonName { get; set; }
	}
}";

    /// <summary>
    /// The property with getter setter test code.
    /// </summary>
    private const string PropertyWithGetterSetterTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
		public string PersonName { get; set; }
	}
}";

    /// <summary>
    /// The property with getter setter test fix code.
    /// </summary>
    private const string PropertyWithGetterSetterTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
        /// <summary>
        /// Gets or sets the person name.
        /// </summary>
        public string PersonName { get; set; }
	}
}";

    /// <summary>
    /// The property only getter test code.
    /// </summary>
    private const string PropertyOnlyGetterTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
		public string PersonName { get; }
	}
}";

    /// <summary>
    /// The property only getter test fix code.
    /// </summary>
    private const string PropertyOnlyGetterTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
        /// <summary>
        /// Gets the person name.
        /// </summary>
        public string PersonName { get; }
	}
}";

    /// <summary>
    /// The property private getter test fix code.
    /// </summary>
    private const string PropertyPrivateGetterTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
        public string PersonName { get; private set; }
	}
}";

    /// <summary>
    /// The property private getter test fix code.
    /// </summary>
    private const string PropertyPrivateGetterTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
        /// <summary>
        /// Gets the person name.
        /// </summary>
        public string PersonName { get; private set; }
	}
}";

    /// <summary>
    /// The property internal getter test fix code.
    /// </summary>
    private const string PropertyInternalGetterTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
        public string PersonName { get; internal set; }
	}
}";

    /// <summary>
    /// The property internal getter test fix code.
    /// </summary>
    private const string PropertyInternalGetterTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
        /// <summary>
        /// Gets the person name.
        /// </summary>
        public string PersonName { get; internal set; }
	}
}";

    /// <summary>
    /// The boolean property test code.
    /// </summary>
    private const string BooleanPropertyTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
		public bool IsTesterStarted { get; set; }
	}
}";

    /// <summary>
    /// The boolean property test fix code.
    /// </summary>
    private const string BooleanPropertyTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
        /// <summary>
        /// Gets or sets a value indicating whether tester is started.
        /// </summary>
        public bool IsTesterStarted { get; set; }
	}
}";

    /// <summary>
    /// The nullable boolean property test code.
    /// </summary>
    private const string NullableBooleanPropertyTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
		public bool? IsTesterStarted { get; set; }
	}
}";

        /// <summary>
        /// The nullable boolean property test code.
        /// </summary>
        private const string PrivateNullableBooleanPropertyTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
		bool? IsTesterStarted { get; set; }
	}
}";

        /// <summary>
        /// The nullable boolean property test fix code.
        /// </summary>
        private const string NullableBooleanPropertyTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
        /// <summary>
        /// Gets or sets a value indicating whether tester is started.
        /// </summary>
        public bool? IsTesterStarted { get; set; }
	}
}";

    /// <summary>
    /// The expression body property test code.
    /// </summary>
    private const string ExpressionBodyPropertyTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
		public string PersonName => ""Person Name"";
	}
}";

        /// <summary>
        /// The expression body property test code.
        /// </summary>
        private const string PrivateClassTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class PropertyTester
	{
		public string PersonName => ""Person Name"";
	}
}";

        /// <summary>
        /// The expression body property test fix code.
        /// </summary>
        private const string ExpressionBodyPropertyTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	public class PropertyTester
	{
        /// <summary>
        /// Gets the person name.
        /// </summary>
        public string PersonName => ""Person Name"";
	}
}";

    //No diagnostics expected to show up
    [DataTestMethod]
        [DataRow("")]
        [DataRow(PrivateNullableBooleanPropertyTestCode)]
        [DataRow(InheritDocTestCode)]
        [DataRow(PrivateClassTestCode)]
        public async Task NoDiagnosticTriggered(string test)
        {
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [DataTestMethod]
        [DataRow(PropertyWithGetterSetterTestCode, PropertyWithGetterSetterTestFixCode, PropertyAnalyzer.DiagnosticId, PropertyAnalyzer.MessageFormat, 10, 17)]
        [DataRow(PropertyOnlyGetterTestCode, PropertyOnlyGetterTestFixCode, PropertyAnalyzer.DiagnosticId, PropertyAnalyzer.MessageFormat, 10, 17)]
        [DataRow(PropertyPrivateGetterTestCode, PropertyPrivateGetterTestFixCode, PropertyAnalyzer.DiagnosticId, PropertyAnalyzer.MessageFormat, 10, 23)]
        [DataRow(PropertyInternalGetterTestCode, PropertyInternalGetterTestFixCode, PropertyAnalyzer.DiagnosticId, PropertyAnalyzer.MessageFormat, 10, 23)]
        [DataRow(BooleanPropertyTestCode, BooleanPropertyTestFixCode, PropertyAnalyzer.DiagnosticId, PropertyAnalyzer.MessageFormat, 10, 15)]
        [DataRow(NullableBooleanPropertyTestCode, NullableBooleanPropertyTestFixCode, PropertyAnalyzer.DiagnosticId, PropertyAnalyzer.MessageFormat, 10, 16)]
        [DataRow(ExpressionBodyPropertyTestCode, ExpressionBodyPropertyTestFixCode, PropertyAnalyzer.DiagnosticId, PropertyAnalyzer.MessageFormat, 10, 17)]
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
