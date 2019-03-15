using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests
{
    public static class ProcessorTestingHelpers
    {
        public static void PushMockValue(this PyProcessor processor)
        {
            processor.PushValue(Mock.Of<IScriptType>());
        }

        public static void ScriptTypeEqual(this Assert assert, int expectedInt, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected integer variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(IntegerBase), $"Expected integer variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expectedInt, ((IntegerBase)actual).Value);
        }

        public static void ScriptTypeEqual(this Assert assert, double expectedDouble, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected double variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(DoubleBase), $"Expected double variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expectedDouble, ((DoubleBase)actual).Value);
        }

        public static void ScriptTypeEqual(this Assert assert, string expectedString, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected string variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(StringBase), $"Expected string variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expectedString, ((StringBase)actual).Value);
        }

        public static void ScriptTypeEqual(this Assert assert, bool expectedBool, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected boolean variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(BooleanBase), $"Expected boolean variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expectedBool, ((BooleanBase)actual).Value);
        }

        public static void ScriptTypeEqual(this Assert assert, IClrFunction expectedClrFunction, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected CLR function variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(EmbeddedClrFunctionBase), $"Expected CLR function variable, got {actual.GetType().Name}.");
            IClrFunction actualDef = ((EmbeddedClrFunctionBase)actual).Definition;
            Assert.AreSame(expectedClrFunction, actualDef, $"CLR function definition was not same as expected.\nExpected: {expectedClrFunction.FunctionName ?? "null"}\nActual: {actualDef.FunctionName ?? "null"}");
        }
    }
}