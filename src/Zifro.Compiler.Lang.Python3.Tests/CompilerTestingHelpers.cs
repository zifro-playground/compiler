using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Lang.Python3.Instructions;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Tests
{
    public static class CompilerTestingHelpers
    {
        public static void IsPushLiteralOpCode<TValue>(this Assert assert, int expectedValue, PyCompiler compiler, int index)
        {
            if (index >= compiler.Count)
                throw new AssertFailedException($"Expected IOpCode at index {index} but compiler only contains {compiler.Count} op codes.");

            IOpCode opCode = compiler[index];
            Assert.IsInstanceOfType(opCode, typeof(PushLiteral<TValue>));
            var pushLiteral = (PushLiteral<TValue>) opCode;
            Assert.AreEqual(expectedValue, pushLiteral.Literal.Value, $"Value not matched for Literal<{typeof(TValue).Name}>");
        }

        public static void IsBinaryOpCode(this Assert assert, OperatorCode expectedCode, PyCompiler compiler, int index)
        {
            if (index >= compiler.Count)
                throw new AssertFailedException($"Expected IOpCode at index {index} but compiler only contains {compiler.Count} op codes.");

            IOpCode opCode = compiler[index];
            Assert.IsInstanceOfType(opCode, typeof(BasicOperator));
            var binOpCode = (BasicOperator) opCode;
            Assert.AreEqual(expectedCode, binOpCode.Code);
        }
    }
}