using Mellis.Core.Interfaces;
using Mellis.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Tests.Entities
{
    [TestClass]
    public class ScriptStringTests : ScriptTypeBaseTestClass
    {
        [TestMethod]
        public void AdditionTest()
        {
            // Arrange
            var a = GetScriptString("foo");
            var b = GetScriptString("bar");

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<IScriptString>(result, a, b, "foobar");
        }

        [TestMethod]
        public void AdditionEmptyTest()
        {
            // Arrange
            var a = GetScriptString("");
            var b = GetScriptString("");

            // Act
            var result = a.ArithmeticAdd(b);

            // Assert
            AssertArithmeticResult<IScriptString>(result, a, b, "");
        }

        protected override IScriptType GetBasicOperand()
        {
            return GetScriptString("foo");
        }

        protected override IScriptType GetBasicOtherOperandInvalidType()
        {
            return GetScriptNull();
        }
    }
}