using System.Collections.Generic;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities.Functions;
using Mellis.Lang.Python3.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Entities.Functions
{
    [TestClass]
    public class NextTests : BaseClrFunctionTester<Next>
    {
        protected override Next CreateFunction()
        {
            return new Next();
        }

        protected override int MaximumArguments => 2;
        protected override int MinimumArguments => 1;

        [TestMethod]
        public void GetNextFromIEnumerator()
        {
            // Arrange
            var func = CreateInitializedFunction();

            var argMock = new Mock<IScriptType>();

            var enumMock = argMock.As<IEnumerator<IScriptType>>();

            enumMock.Setup(o => o.MoveNext())
                .Returns(true)
                .Verifiable();

            enumMock.SetupGet(o => o.Current)
                .Returns(Mock.Of<IScriptType>);

            // Act
            func.Invoke(argMock.Object);

            // Assert
            enumMock.Verify(o => o.MoveNext());
        }

        [TestMethod]
        public void ReturnsCurrentWithNoDefault()
        {
            // Arrange
            var func = CreateInitializedFunction();

            var argMock = new Mock<IScriptType>();

            var enumMock = argMock.As<IEnumerator<IScriptType>>();

            enumMock.Setup(o => o.MoveNext())
                .Returns(true)
                .Verifiable();

            var current = Mock.Of<IScriptType>();
            enumMock.SetupGet(o => o.Current)
                .Returns(current);

            // Act
            var result = func.Invoke(argMock.Object);

            // Assert
            Assert.AreSame(current, result);
        }
        
        [TestMethod]
        public void ReturnsCurrentIgnoreDefault()
        {
            // Arrange
            const bool moveNext = true;

            var func = CreateInitializedFunction();

            var arg1Mock = new Mock<IScriptType>();

            var enumMock = arg1Mock.As<IEnumerator<IScriptType>>();
            enumMock.Setup(o => o.MoveNext())
                .Returns(moveNext)
                .Verifiable();

            var current = Mock.Of<IScriptType>();
            enumMock.SetupGet(o => o.Current)
                .Returns(current);

            var arg2Default = Mock.Of<IScriptType>();

            // Act
            var result = func.Invoke(arg1Mock.Object, arg2Default);

            // Assert
            Assert.AreSame(current, result);
        }

        [TestMethod]
        public void ReturnsDefaultIfExhausted()
        {
            // Arrange
            const bool moveNext = false;

            var func = CreateInitializedFunction();

            var arg1Mock = new Mock<IScriptType>();

            var enumMock = arg1Mock.As<IEnumerator<IScriptType>>();
            enumMock.Setup(o => o.MoveNext())
                .Returns(moveNext)
                .Verifiable();

            enumMock.SetupGet(o => o.Current)
                .Returns(Mock.Of<IScriptType>);

            var arg2Default = Mock.Of<IScriptType>();

            // Act
            var result = func.Invoke(arg1Mock.Object, arg2Default);

            // Assert
            Assert.AreSame(arg2Default, result);
            enumMock.VerifyGet(o => o.Current, Times.Never);
        }

        [TestMethod]
        public void ThrowsIfExhaustedWithNoDefault()
        {
            // Arrange
            const bool moveNext = false;

            var func = CreateInitializedFunction();

            var arg1Mock = new Mock<IScriptType>();

            var enumMock = arg1Mock.As<IEnumerator<IScriptType>>();
            enumMock.Setup(o => o.MoveNext())
                .Returns(moveNext)
                .Verifiable();

            // Act
            var ex = Assert.That.Throws(func, arg1Mock.Object);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Builtin_Next_StopIteration)
            );

            enumMock.VerifyGet(o => o.Current, Times.Never);
        }

        [TestMethod]
        public void ThrowsIfNotIEnumerator()
        {
            // Arrange
            var func = CreateInitializedFunction();

            var argMock = new Mock<IScriptType>();
            argMock.Setup(o => o.GetTypeName())
                .Returns("foo");

            // Act
            var ex = Assert.That.Throws(func, argMock.Object);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Builtin_Next_Arg1_NotIterator),
                "foo"
            );
        }
    }
}