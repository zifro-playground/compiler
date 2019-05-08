using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
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
            Assert.IsInstanceOfType(actual, typeof(ScriptInteger), $"Expected integer variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expectedInt, ((ScriptInteger)actual).Value);
        }

        public static void ScriptTypeEqual(this Assert assert, double expectedDouble, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected double variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(ScriptDouble), $"Expected double variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expectedDouble, ((ScriptDouble)actual).Value);
        }

        public static void ScriptTypeEqual(this Assert assert, string expectedString, IScriptType actual, string message = null)
        {
            Assert.IsNotNull(actual, $"Expected string variable, got null.\n{message}");
            Assert.IsInstanceOfType(actual, typeof(ScriptString), $"Expected string variable, got {actual.GetType().Name}.\n{message}");
            Assert.AreEqual(expectedString, ((ScriptString)actual).Value, message);
        }

        public static void ScriptTypeEqual(this Assert assert, bool expectedBool, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected boolean variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(ScriptBoolean), $"Expected boolean variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expectedBool, ((ScriptBoolean)actual).Value);
        }

        public static void ScriptTypeEqual(this Assert assert, IClrFunction expectedClrFunction, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected CLR function variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(IClrFunction), $"Expected CLR function variable, got {actual.GetType().Name}.");
            var actualDef = (ScriptEmbeddedClrFunction)actual;
            Assert.AreSame(expectedClrFunction, actualDef.Definition,
                $"CLR function definition was not same as expected.\n" +
                $"Expected: {expectedClrFunction.FunctionName ?? "null"}\n" +
                $"Actual: {actualDef.FunctionName ?? "null"}");
        }

        public static void ScriptTypeEqual(this Assert assert, (int from, int to, int step) expectedRange, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected range variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(PyRange), $"Expected range variable, got {actual.GetType().Name}.");

            var range = (PyRange)actual;
            (int from, int to, int step) = expectedRange;

            string expectedString = $"range({from}, {to}, {step})";
            string actualString = $"range({range.RangeFrom}, {range.RangeTo}, {range.RangeStep})";
            string errorMessage = $"Expected:{expectedRange}. Actual:{actualString}";

            Assert.AreEqual(from, range.RangeFrom, errorMessage);
            Assert.AreEqual(to, range.RangeTo, errorMessage);
            Assert.AreEqual(step, range.RangeStep, errorMessage);
        }
    }
}