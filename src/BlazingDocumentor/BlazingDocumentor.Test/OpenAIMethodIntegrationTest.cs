using BlazingDocumentor.Helper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = BlazingDocumentor.Test.CSharpCodeFixVerifier<
    BlazingDocumentor.MethodAnalyzer,
    BlazingDocumentor.MethodCodeFixProvider>;

namespace BlazingDocumentor.Test
{
    [TestClass]
    public class OpenAIMethodIntegrationTest
    {


        private const string PrivateClassTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class MethodTester
	{
		public decimal ParallelPartitionerPi(int steps)
        {
            decimal sum = 0.0;
            decimal step = 1.0 / (decimal)steps;
            object obj = new object();

            Parallel.ForEach(
                Partitioner.Create(0, steps),
                () => 0.0,
                (range, state, partial) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        decimal x = (i - 0.5) * step;
                        partial += 4.0 / (1.0 + x * x);
                    }

                    return partial;
                },
                partial => { lock (obj) sum += partial; });

            return step * sum;
        }
	}
}";
   

        
        [TestMethod]        
        public async Task OpenAITest()
        {
            var result = await OpenAIDocumentationCommentHelper.GetMethodCommentAsync(PrivateClassTestCode);
            result = await OpenAIDocumentationCommentHelper.GetMethodCommentAsync(PrivateClassTestCode);
        }
    }
}
