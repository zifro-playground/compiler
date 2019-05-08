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
            AssertArithmeticResult<ScriptString>(result, a, b, "foobar");
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
            AssertArithmeticResult<ScriptString>(result, a, b, "");
        }

        [TestMethod]
        public void IndexGetInteger()
        {
            // Arrange
            var a = GetScriptString("foo");
            var b = GetScriptInteger(0);

            // Act
            var result = a.GetIndex(b);

            // Assert
            AssertArithmeticResult<ScriptString>(result, a, b, "f");
        }

        [TestMethod]
        public void IndexGetIntegerOutOfRange()
        {
            // Arrange
            var a = GetScriptString("foo");
            var b = GetScriptInteger(10);
            object[] expectedFormatArgs = {a.Value, a.Value.Length, b.Value};

            void Action()
            {
                a.GetIndex(b);
            }

            // Act + Assert
            AssertThrow(Action,
                nameof(Localized_Base_Entities.Ex_String_IndexGet_OutOfRange),
                expectedFormatArgs);
        }

        [TestMethod]
        public void IndexGetInvalid()
        {
            // Arrange
            var a = GetScriptString("foo");
            var b = GetScriptString("a");
            object[] expectedFormatArgs = {a.Value, a.Value.Length, b.GetTypeName()};

            void Action()
            {
                a.GetIndex(b);
            }

            // Act + Assert
            AssertThrow(Action,
                nameof(Localized_Base_Entities.Ex_String_IndexGet_InvalidType),
                expectedFormatArgs);
        }

        public override void IndexGet_NotImplemented()
        {
            // disabled
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