using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zifro.Compiler.Lang.Python3.Instructions;
using Zifro.Compiler.Lang.Python3.Interfaces;
using Zifro.Compiler.Lang.Python3.Syntax;

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

        public static void IsBinaryOpCode<TOpType>(this Assert assert, PyCompiler compiler, int index)
            where TOpType : BaseBinaryOp
        {
            assert.IsBinaryOpCode(typeof(TOpType), compiler, index);
        }

        public static void IsBinaryOpCode(this Assert assert, Type expectedType, PyCompiler compiler, int index)
        {
            if (index >= compiler.Count)
                throw new AssertFailedException($"Expected IOpCode at index {index} but compiler only contains {compiler.Count} op codes.");

            IOpCode opCode = compiler[index];
            Assert.IsInstanceOfType(opCode, expectedType);
        }
    }
}