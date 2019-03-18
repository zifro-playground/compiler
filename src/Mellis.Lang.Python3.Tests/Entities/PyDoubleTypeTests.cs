using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    public class PyDoubleTypeTests : BaseEntityTypeTester<PyDoubleType, PyDouble>
    {
        protected override string ExpectedClassName => Localized_Base_Entities.Type_Double_Name;

        protected override PyDoubleType CreateEntity(PyProcessor processor)
        {
            return new PyDoubleType(processor, nameof(PyDoubleTypeTests));
        }

        [TestMethod]
        public void CtorEmptyArgsTest()
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            var result = entity.Invoke(new IScriptType[0]);

            // Assert
            Assert.That.ScriptTypeEqual(expectedDouble: 0d, actual: result);
        }

        [TestMethod]
        public void CtorTooManyArgs()
        {
            // Arrange
            const int numOfArgs = 2;
            const int maxArgs = 1;

            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke(new IScriptType[numOfArgs]);
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeTooManyArgumentsException>((Action) Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Invoke_TooManyArguments),
                /* func name */ ExpectedClassName,
                /* maximum */ maxArgs,
                /* actual */ numOfArgs);
        }

        [DataTestMethod]
        [DataRow("123", 123d)]
        [DataRow("123.0", 123d)]
        [DataRow("  123  ", 123d, DisplayName = "\"  123  \", 123 (with padding)")]
        [DataRow("-452", -452d)]
        [DataRow("+452", +452d)]
        [DataRow("0.00000000001", 0.00000000001d)]
        [DataRow("1e0", 1d)]
        [DataRow("1e10", 1e10d)]
        [DataRow("NaN", double.NaN)]
        [DataRow("-nan", double.NaN)]
        [DataRow("inf", double.PositiveInfinity)]
        [DataRow("infinity", double.PositiveInfinity)]
        [DataRow("-inf", double.NegativeInfinity)]
        [DataRow("-InFiNiTy", double.NegativeInfinity)]
        [DataRow("1e500", double.PositiveInfinity)]
        [DataRow("-1e500", double.NegativeInfinity)]
        public void CtorConvertString(string input, double expected)
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            IScriptType result = entity.Invoke(new IScriptType[] { new PyString(entity.Processor, input) });

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(nameof(Localized_Base_Entities.Type_Double_PosInfinity), double.PositiveInfinity)]
        [DataRow(nameof(Localized_Base_Entities.Type_Double_NegInfinity), double.NegativeInfinity)]
        [DataRow(nameof(Localized_Base_Entities.Type_Double_NaN), double.NaN)]
        public void CtorConvertBaseStringRepresentation(string key, double expected)
        {
            // Arrange
            var entity = CreateEntity();
            string input = Localized_Base_Entities.ResourceManager.GetString(key);

            // Act
            IScriptType result = entity.Invoke(
                new IScriptType[] { new PyString(entity.Processor, input) }
            );

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
        }

        [TestMethod]
        public void CtorConvertInteger()
        {
            // Arrange
            const int input = 10;
            const double expected = 10d;
            var entity = CreateEntity();

            // Act
            IScriptType result = entity.Invoke(new IScriptType[] { new PyInteger(entity.Processor, input), });

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
        }

        [TestMethod]
        public void CtorConvertDouble()
        {
            // Arrange
            const double input = 10d;
            const double expected = 10d;
            var entity = CreateEntity();

            // Act
            IScriptType result = entity.Invoke(new IScriptType[] { new PyDouble(entity.Processor, input), });

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
            Assert.AreNotSame(entity, result);
        }

        [DataTestMethod]
        [DataRow(true, 1d)]
        [DataRow(false, 0d)]
        public void CtorConvertBool(bool input, double expected)
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            IScriptType result = entity.Invoke(new IScriptType[] {new PyBoolean(entity.Processor, input),});

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
        }

        [TestMethod]
        public void CtorInvalidNone()
        {
            // Arrange
            var entity = CreateEntity();
            var unexpected = new PyNone(entity.Processor);

            void Action()
            {
                entity.Invoke(new IScriptType[] {unexpected,});
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeException>((Action) Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Ex_DoubleType_Ctor_Arg1_Type),
                unexpected.GetTypeName()
            );
        }

        [DataTestMethod]
        [DataRow("zero", "'zero'")]
        [DataRow("infinite", "'infinite'")]
        [DataRow("'", "\"'\"")]
        [DataRow("\nx", "'\\nx'")]
        public void CtorInvalidString(string input, string expectedFormatArg)
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke(new IScriptType[] {new PyString(entity.Processor, input),});
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeException>((Action) Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Ex_DoubleType_Ctor_Arg1_InvalidString),
                expectedFormatArg
            );
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("             ")]
        [DataRow("\n\r \t\t \n")]
        public void CtorInvalidEmptyString(string input)
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke(new IScriptType[] { new PyString(entity.Processor, input), });
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeException>((Action)Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Ex_DoubleType_Ctor_Arg1_EmptyString)
            );
        }
    }
}