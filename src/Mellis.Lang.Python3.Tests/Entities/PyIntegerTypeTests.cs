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
            Assert.That.ScriptTypeEqual(expected: 0, actual: result);
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
            var ex = Assert.ThrowsException<RuntimeTooManyArgumentsException>((Action)Action);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Invoke_TooManyArguments),
                /* func name */ ExpectedClassName,
                /* maximum */ 2,
                /* actual */ 3);
        }

        [DataTestMethod]
        [DataRow("123", 123)]
        [DataRow("  123  ", 123, DisplayName = "\"  123  \", 123 (with padding)")]
        public void CtorConvertStringDefaultBase(string input, int expected)
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke(new IScriptType[] {new PyString(entity.Processor, input) });
            }

            // Act
            // TODO: Implement this in PyIntegerType
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>((Action)Action);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, SourceReference.ClrSource);
        }

        [DataTestMethod]
        [DataRow("123", 10, 123)]
        [DataRow("10", 2, 2)]
        [DataRow("10", 36, 36)]
        public void CtorConvertStringWithBaseArg(string input, int numBase, int expected)
        {
            // Arrange
            var entity = CreateEntity();

            void Action()
            {
                entity.Invoke(new IScriptType[]
                {
                    new PyString(entity.Processor, input),
                    new PyInteger(entity.Processor, numBase)
                });
            }

            // Act
            // TODO: Implement this in PyIntegerType
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>((Action)Action);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, SourceReference.ClrSource);
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

            void Action()
            {
                entity.Invoke(new IScriptType[]
                {
                    new PyString(entity.Processor, input),
                    new PyInteger(entity.Processor, 0)
                });
            }

            // Act
            // TODO: Implement this in PyIntegerType
            var ex = Assert.ThrowsException<SyntaxNotYetImplementedException>((Action)Action);

            // Assert
            Assert.That.ErrorNotYetImplFormatArgs(ex, SourceReference.ClrSource);
        }
    }
}