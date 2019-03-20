using System.Collections;
using System.Collections.Generic;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Functions;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Entities.Functions
{
    [TestClass]
    public class IterTests : BaseClrFunctionTester<Iter>
    {
        protected override Iter CreateFunction()
        {
            return new Iter();
        }

        [TestMethod]
        public void GetsIEnumerator()
        {
            // Arrange
            var func = CreateInitializedFunction();

            var enumMock = new Mock<IEnumerator<IScriptType>>();
            enumMock.As<IScriptType>();

            var argMock = new Mock<IScriptType>();
            argMock.As<IEnumerable<IScriptType>>()
                .Setup(o => o.GetEnumerator())
                .Returns(enumMock.Object);

            // Act
            var result = func.Invoke(argMock.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerator<IScriptType>));
            Assert.AreSame(enumMock.Object, result);
        }

        [TestMethod]
        public void WrapsIEnumerator()
        {
            // Arrange
            var func = CreateInitializedFunction();

            var enumMock = new Mock<IEnumerator<IScriptType>>();
            //enumMock.As<IScriptType>();

            var argMock = new Mock<IScriptType>();
            argMock.As<IEnumerable<IScriptType>>()
                .Setup(o => o.GetEnumerator())
                .Returns(enumMock.Object);

            // Act
            var result = func.Invoke(argMock.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(PyEnumeratorWrapper));
            var wrapper = (PyEnumeratorWrapper)result;
            Assert.AreSame(enumMock.Object, wrapper.Enumerator);
        }

        [TestMethod]
        public void ThrowsOnNoArguments()
        {
            // Arrange
            var func = CreateInitializedFunction();

            // Act
            Assert.That.ThrowsTooFewArguments(func, minimum: 1);
        }

        [TestMethod]
        public void ThrowsOnTooManyArguments()
        {
            // Arrange
            var func = CreateInitializedFunction();

            // Act
            Assert.That.ThrowsTooManyArguments(func, maximum: 2);
        }

        [TestMethod]
        public void ThrowsOnTwoArguments_NotYetImplemented()
        {
            // Arrange
            var func = CreateInitializedFunction();

            // Act
            var ex = Assert.That.Throws(func, new IScriptType[2]);

            // Assert
            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Entities.Builtin_Iter_Arg2_NotYetImplemented));
        }
    }
}