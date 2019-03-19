using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Interfaces;

namespace Mellis.Lang.Python3.Tests
{
    public static class CompilerTestingHelpers
    {
        public static void IsPushLiteralOpCode<TValue>(this Assert assert, int expectedValue, PyCompiler compiler, int index)
        {
            if (index >= compiler.Count)
            {
                throw new AssertFailedException($"Expected PushLiteral<{typeof(TValue).Name}> op code at index {index} but compiler only contains {compiler.Count} op codes.");
            }

            IOpCode opCode = compiler[index];
            Assert.IsInstanceOfType(opCode, typeof(PushLiteral<TValue>));
            var pushLiteral = (PushLiteral<TValue>) opCode;
            Assert.AreEqual(expectedValue, pushLiteral.Literal.Value, $"Value not matched for Literal<{typeof(TValue).Name}>");
        }

        public static void IsBinaryOpCode(this Assert assert, OperatorCode expectedCode, PyCompiler compiler, int index)
        {
            if (index >= compiler.Count)
            {
                throw new AssertFailedException($"Expected {expectedCode} op code at index {index} but compiler only contains {compiler.Count} op codes.");
            }

            IOpCode opCode = compiler[index];
            Assert.IsInstanceOfType(opCode, typeof(BasicOperator));
            var binOpCode = (BasicOperator) opCode;
            Assert.AreEqual(expectedCode, binOpCode.Code);
        }

        public static TOpCode IsOpCode<TOpCode>(this Assert assert, PyCompiler compiler, int index)
            where TOpCode : IOpCode
        {
            if (index >= compiler.Count)
            {
                throw new AssertFailedException($"Expected {typeof(TOpCode).Name} op code at index {index} but compiler only contains {compiler.Count} op codes.");
            }

            IOpCode opCode = compiler[index];
            Assert.IsInstanceOfType(opCode, typeof(TOpCode), $"Op code at index {index} is wrong type.");
            return (TOpCode) opCode;
        }

        public static void IsExpectedOpCode<TOpCode>(this Assert assert, PyCompiler compiler, int index, TOpCode expected)
            where TOpCode : IOpCode
        {
            var opCode = assert.IsOpCode<TOpCode>(compiler, index);
            Assert.AreSame(expected, opCode, $"Op code at index {index} are not same as expected.");
        }
    }
}