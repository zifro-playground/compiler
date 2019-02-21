using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests
{
    public static class ProcessorTestingHelpers
    {
        public static void ScriptTypeEqual(this Assert assert, int expected, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected integer variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(IntegerBase), $"Expected integer variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expected, ((IntegerBase)actual).Value);
        }

        public static void ScriptTypeEqual(this Assert assert, double expected, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected double variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(DoubleBase), $"Expected double variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expected, ((DoubleBase)actual).Value);
        }

        public static void ScriptTypeEqual(this Assert assert, string expected, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected string variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(StringBase), $"Expected string variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expected, ((StringBase)actual).Value);
        }

        public static void ScriptTypeEqual(this Assert assert, bool expected, IScriptType actual)
        {
            Assert.IsNotNull(actual, "Expected boolean variable, got null.");
            Assert.IsInstanceOfType(actual, typeof(BooleanBase), $"Expected boolean variable, got {actual.GetType().Name}.");
            Assert.AreEqual(expected, ((BooleanBase)actual).Value);
        }
    }
}