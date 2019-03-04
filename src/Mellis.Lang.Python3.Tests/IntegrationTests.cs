using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Sharpen;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;
using Moq;

namespace Mellis.Lang.Python3.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private ErrorCatcher errorCatcher;

        [TestInitialize]
        public void TestInitialize()
        {
            errorCatcher = new ErrorCatcher();
        }

        [TestMethod]
        public void ProcessAssignIntegerTest()
        {
            // Arrange
            const string code = "myInt = 10";
            var processor = (VM.PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();

            // Assert
            IScriptType variable = processor.GetVariable("myInt");
            Assert.IsInstanceOfType(variable, typeof(PyInteger));
            Assert.AreEqual(10, ((PyInteger)variable).Value);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessAssignStringTest()
        {
            // Arrange
            const string code = "myString = 'hello world'";
            var processor = (VM.PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();

            // Assert
            IScriptType variable = processor.GetVariable("myString");
            Assert.IsInstanceOfType(variable, typeof(PyString));
            Assert.AreEqual("hello world", ((PyString)variable).Value);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessAssignMultipleTest()
        {
            // Arrange
            const string code = "x = 88\n" +
                                "y = 255";
            var processor = (VM.PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            processor.WalkLine();

            // Assert
            IScriptType x = processor.GetVariable("x");
            Assert.IsInstanceOfType(x, typeof(PyInteger));
            Assert.AreEqual(88, ((PyInteger)x).Value);

            IScriptType y = processor.GetVariable("y");
            Assert.IsInstanceOfType(y, typeof(PyInteger));
            Assert.AreEqual(255, ((PyInteger)y).Value);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessAssignVariableToVariableTest()
        {
            // Arrange
            const string code = "a = 64\n" +
                                "b = a";
            var processor = (VM.PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            processor.WalkLine();

            // Assert
            IScriptType variable = processor.GetVariable("a");
            Assert.IsInstanceOfType(variable, typeof(PyInteger));
            Assert.AreEqual(64, ((PyInteger)variable).Value);

            IScriptType y = processor.GetVariable("b");
            Assert.IsInstanceOfType(y, typeof(PyInteger));
            Assert.AreEqual(64, ((PyInteger)y).Value);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessArithmeticAddTest()
        {
            // Arrange
            const string code = "myMath = 1024 + 999";
            var processor = (VM.PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();

            // Assert
            IScriptType variable = processor.GetVariable("myMath");
            Assert.IsInstanceOfType(variable, typeof(PyInteger));
            Assert.AreEqual(1024+999, ((PyInteger)variable).Value);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessIfTrueTest()
        {
            // Arrange
            const string code = "val = 5\n" +
                                "if True:\n" +
                                "   val = 200";
            var processor = (VM.PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            processor.WalkLine();
            processor.WalkLine();

            // Assert
            IScriptType variable = processor.GetVariable("val");
            Assert.IsInstanceOfType(variable, typeof(PyInteger));
            Assert.AreEqual(200, ((PyInteger)variable).Value);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessIfFalseTest()
        {
            // Arrange
            const string code = "val = 5\n" +
                                "if False:\n" +
                                "   val = 200";
            var processor = (VM.PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine(); // push->val
            processor.WalkLine(); // jumpif->@4

            // Assert
            IScriptType variable = processor.GetVariable("val");
            Assert.IsInstanceOfType(variable, typeof(PyInteger));
            Assert.AreEqual(5, ((PyInteger)variable).Value);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessP3Test()
        {
            /*
             
               y = "hej"
               x = (y + y) * 2
               x = 33
               x = x + 2
               
               if x < 50: 
                   y = "inge print än"
               
             */

            // Arrange
            const string code = "y = \"hej\"\n" +
                                "x = (y + y) * 2\n" +
                                "x = 33\n" +
                                "x = x + 2\n" +
                                "\n" +
                                "if x < 50:\n" +
                                "    y = \"inge print än\"";

            var processor = (VM.PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            do
            {
                processor.WalkLine();
            } while (processor.State == ProcessState.Running);

            // Assert
            IScriptType x = processor.GetVariable("x");
            Assert.IsInstanceOfType(x, typeof(PyInteger));
            Assert.AreEqual(35, ((PyInteger)x).Value);

            IScriptType y = processor.GetVariable("y");
            Assert.IsInstanceOfType(y, typeof(PyString));
            Assert.AreEqual("inge print än", ((PyString)y).Value);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessCallClrTest()
        {
            // Arrange
            const string code = "foo()";
            var processor = (VM.PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            var clrMock = new Mock<IClrFunction>();
            clrMock.SetupGet(o => o.FunctionName).Returns("foo");

            processor.AddBuiltin(clrMock.Object);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine(); // foo()

            // Assert
            clrMock.Verify(o => o.Invoke(It.IsAny<IScriptType[]>()));
            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        private class ErrorCatcher : IParserErrorListener
        {
            private int _syntaxErrors;
            private int _reportAmbiguities;
            private int _reportAttemptingFullContexts;
            private int _reportContextSensitivities;

            public void AssertNoErrors()
            {
                Assert.AreEqual(0, _syntaxErrors, "Received a syntax error.");
                Assert.AreEqual(0, _reportAmbiguities, "Received an ambiguity report.");
                Assert.AreEqual(0, _reportAttemptingFullContexts, "Received a full context attempt report.");
                Assert.AreEqual(0, _reportContextSensitivities, "Received a context sensitivity report.");
            }

            public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
                string msg, RecognitionException e)
            {
                _syntaxErrors++;
            }

            public void ReportAmbiguity(Parser recognizer, DFA dfa, int startIndex, int stopIndex, bool exact, BitSet ambigAlts,
                ATNConfigSet configs)
            {
                _reportAmbiguities++;
            }

            public void ReportAttemptingFullContext(Parser recognizer, DFA dfa, int startIndex, int stopIndex, BitSet conflictingAlts,
                SimulatorState conflictState)
            {
                _reportAttemptingFullContexts++;
            }

            public void ReportContextSensitivity(Parser recognizer, DFA dfa, int startIndex, int stopIndex, int prediction,
                SimulatorState acceptState)
            {
                _reportContextSensitivities++;
            }
        }
    }
}