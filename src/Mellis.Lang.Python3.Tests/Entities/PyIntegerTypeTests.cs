using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.VM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests.Entities
{
    [TestClass]
    public class PyIntegerTypeTests : BaseEntityTypeTester<PyIntegerType, PyInteger>
    {
        protected override string ExpectedClassName => Localized_Base_Entities.Type_Int_Name;

        protected override PyIntegerType CreateEntity(PyProcessor processor)
        {
            return new PyIntegerType(processor);
        }

        [TestMethod]
        public void CtorEmptyArgsTest()
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            var result = entity.Invoke(new IScriptType[0]);

            // Assert
            Assert.That.ScriptTypeEqual(expectedInt: 0, actual: result);
        }

        [TestMethod]
        public void CtorTooManyArgs()
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke(new IScriptType[3]);
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeTooManyArgumentsException>((Action) Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Invoke_TooManyArguments),
                /* func name */ ExpectedClassName,
                /* maximum */ 2,
                /* actual */ 3);
        }

        [DataTestMethod]
        [DataRow(1.0d, 1)]
        [DataRow(1.5d, 1)]
        [DataRow(1.9d, 1)]
        [DataRow(-1.0d, -1)]
        [DataRow(-1.5d, -1)]
        [DataRow(-1.9d, -1)]
        public void CtorConvertDoubleTruncate(double input, int expected)
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            IScriptType result = entity.Invoke(new IScriptType[] {new PyDouble(entity.Processor, input)});

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(double.NaN,
            nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_NaN))]
        [DataRow(double.PositiveInfinity,
            nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_PosInf))]
        [DataRow(double.NegativeInfinity,
            nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_NegInf))]
        public void CtorInvalidArg1Double(double input, string expectedError)
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke(new IScriptType[] { new PyDouble(entity.Processor, input) });
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeException>((Action) Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex, expectedError);
        }

        [DataTestMethod]
        [DataRow("123", 123)]
        [DataRow("  123  ", 123, DisplayName = "\"  123  \", 123 (with padding)")]
        public void CtorConvertStringDefaultBase(string input, int expected)
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            IScriptType result = entity.Invoke(new IScriptType[] {new PyString(entity.Processor, input)});

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("123", 10, 123)]
        [DataRow("10", 2, 2)]
        [DataRow("10", 36, 36)]
        [DataRow("Z", 36, 35)] // Uppercase
        [DataRow("y", 36, 34)] // Lowercase
        public void CtorConvertStringWithBaseArg(string input, int numBase, int expected)
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            IScriptType result = entity.Invoke(new IScriptType[]
            {
                new PyString(entity.Processor, input),
                new PyInteger(entity.Processor, numBase)
            });

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("123", 123)]
        [DataRow("0b10", 2)]
        [DataRow("0o10", 8)]
        [DataRow("0x10", 16)]
        public void CtorConvertStringWithBase0(string input, int expected)
        {
            // Arrange
            var entity = CreateEntity();

            // Act
            IScriptType result = entity.Invoke(new IScriptType[]
            {
                new PyString(entity.Processor, input),
                new PyInteger(entity.Processor, 0)
            });

            // Assert
            Assert.That.ScriptTypeEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("0x10", "'0x10'")]
        [DataRow("inf", "'inf'")]
        [DataRow("'", "\"'\"")]
        [DataRow("\n", "'\\n'")]
        public void CtorInvalidArg1String(string input, string expectedFormatArg)
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
                nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_InvalidString),
                expectedFormatArg
            );
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("             ")]
        [DataRow("\n\r \t\t \n")]
        public void CtorEmptyString(string input)
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
                nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_EmptyString)
            );
        }

        [TestMethod]
        public void CtorInvalidArg1Type()
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
                nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg1_Type),
                unexpected.GetTypeName()
            );
        }

        [TestMethod]
        public void CtorInvalidArg2Type()
        {
            // Arrange
            var entity = CreateEntity();
            var unexpected = new PyNone(entity.Processor);

            void Action()
            {
                entity.Invoke(new IScriptType[] {new PyInteger(entity.Processor, 0), unexpected,});
            }

            // Act
            var ex = Assert.ThrowsException<RuntimeException>((Action) Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Ex_IntegerType_Ctor_Arg2_Type),
                unexpected.GetTypeName()
            );
        }
    }
}