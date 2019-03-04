using Mellis.Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor
{
    [TestClass]
    public class BuiltinsTests
    {
        [TestMethod]
        public void GetDeclaredBuiltinTest()
        {
            // Arrange
            var processor = new VM.PyProcessor();
            var builtinMock = new Mock<IClrFunction>();
            builtinMock.SetupGet(o => o.FunctionName).Returns("foo");

            processor.AddBuiltin(
                builtinMock.Object
            );

            // Act
            IScriptType result = processor.GetVariable("foo");

            // Assert
            Assert.That.ScriptTypeEqual(builtinMock.Object, result);
        }

        [TestMethod]
        public void DeclareSameNameTwiceOverridesTest()
        {
            // Arrange
            var processor = new VM.PyProcessor();
            var builtinMock1 = new Mock<IClrFunction>();
            builtinMock1.SetupGet(o => o.FunctionName).Returns("foo");
            var builtinMock2 = new Mock<IClrFunction>();
            builtinMock2.SetupGet(o => o.FunctionName).Returns("foo");

            // Act
            processor.AddBuiltin(
                builtinMock1.Object,
                builtinMock2.Object
            );

            // Assert
            IScriptType result = processor.GetVariable("foo");
            Assert.That.ScriptTypeEqual(builtinMock2.Object, result);
        }
    }
}