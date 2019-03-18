using System;
using System.Text;
using Mellis.Core.Exceptions;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    public class PyStringTests : BaseEntityTester<PyString, string>
    {
        protected override string ExpectedTypeName => Localized_Base_Entities.Type_String_Name;
        protected override Type ExpectedTypeDef => typeof(PyStringType);
        protected override string DefaultValue => string.Empty;

        protected override PyString CreateEntity(PyProcessor processor, string value)
        {
            return new PyString(processor, value, nameof(PyBooleanTests));
        }

        [DataTestMethod]
        [DataRow("", "''")]
        [DataRow("foo", "'foo'")]
        [DataRow("foo\"bar", "'foo\"bar'")]
        [DataRow("foo 'bar", "\"foo 'bar\"")]
        [DataRow("foo\nbar", "'foo\\nbar'")]
        [DataRow("\t\n\v\a\b\r\0", "'\\t\\n\\v\\a\\b\\r\\x00'")]
        public override void ToStringDataTest(string value, string expected)
        {
            base.ToStringDataTest(value, expected);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-10)]
        public void MultiplyBelowOne(int value)
        {
            // Arrange
            var entity = CreateEntity("foo");
            var multiplier = new PyInteger(entity.Processor, value);

            // Act
            var result = entity.ArithmeticMultiply(multiplier);

            // Assert
            Assert.That.ScriptTypeEqual(string.Empty, result);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(3)]
        [DataRow(10)]
        public void MultiplyAboveZero(int value)
        {
            // Arrange
            const string str = "foo";
            var entity = CreateEntity(str);
            var multiplier = new PyInteger(entity.Processor, value);

            var expected = new StringBuilder(value * 3, value * 3);
            for (var i = 0; i < value; i++)
            {
                expected.Append(str);
            }

            // Act
            var result = entity.ArithmeticMultiply(multiplier);

            // Assert
            Assert.That.ScriptTypeEqual(expected.ToString(), result);
        }

        [TestMethod]
        public void MultiplyDouble()
        {
            // Arrange
            var entity = CreateEntity();
            var other = new PyDouble(entity.Processor, 1);

            void Action()
            {
                entity.ArithmeticMultiply(other);
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeException>((Action) Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Base_Entities.Ex_Base_OperatorInvalidType),
                /* type name */ ExpectedTypeName,
                /* other type name */ other.GetTypeName(),
                /* operation */ "*");
        }
    }
}