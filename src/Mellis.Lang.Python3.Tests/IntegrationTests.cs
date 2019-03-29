using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Sharpen;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Entities;
using Mellis.Lang.Python3.Entities.Classes;
using Mellis.Lang.Python3.VM;
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
            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();

            // Assert
            var variable = processor.GetVariable("myInt");
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
            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();

            // Assert
            var variable = processor.GetVariable("myString");
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
            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            processor.WalkLine();

            // Assert
            var x = processor.GetVariable("x");
            Assert.IsInstanceOfType(x, typeof(PyInteger));
            Assert.AreEqual(88, ((PyInteger)x).Value);

            var y = processor.GetVariable("y");
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
            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            processor.WalkLine();

            // Assert
            var variable = processor.GetVariable("a");
            Assert.IsInstanceOfType(variable, typeof(PyInteger));
            Assert.AreEqual(64, ((PyInteger)variable).Value);

            var y = processor.GetVariable("b");
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
            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();

            // Assert
            var variable = processor.GetVariable("myMath");
            Assert.IsInstanceOfType(variable, typeof(PyInteger));
            Assert.AreEqual(1024+999, ((PyInteger)variable).Value);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessLogicAndTest()
        {
            // Arrange
            const string code = "a = 1024 and 999\n" +
                                "b = '' and 42";
            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            processor.WalkLine();

            // Assert
            var a = processor.GetVariable("a");
            Assert.That.ScriptTypeEqual(999, a);

            var b = processor.GetVariable("b");
            Assert.That.ScriptTypeEqual("", b);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessLogicOrTest()
        {
            // Arrange
            const string code = "a = 1024 or 999\n" +
                                "b = '' or 42";
            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            processor.WalkLine();

            // Assert
            var a = processor.GetVariable("a");
            Assert.That.ScriptTypeEqual(1024, a);

            var b = processor.GetVariable("b");
            Assert.That.ScriptTypeEqual(42, b);

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
            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine();
            processor.WalkLine();
            processor.WalkLine();

            // Assert
            var variable = processor.GetVariable("val");
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
            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            processor.WalkInstruction(); // to enter first op
            processor.WalkLine(); // push->val
            processor.WalkLine(); // jumpif->@4

            // Assert
            var variable = processor.GetVariable("val");
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

            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            do
            {
                processor.WalkLine();
            } while (processor.State == ProcessState.Running);

            // Assert
            var x = processor.GetVariable("x");
            Assert.IsInstanceOfType(x, typeof(PyInteger));
            Assert.AreEqual(35, ((PyInteger)x).Value);

            var y = processor.GetVariable("y");
            Assert.IsInstanceOfType(y, typeof(PyString));
            Assert.AreEqual("inge print än", ((PyString)y).Value);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessP5Test()
        {
            /*
                x = "NaN"
                y = x * int("5")
                z = y + " " + str(float(x))

                a = type(z)
                b = a(float(1))

             */

            // Arrange
            const string code = "x = 'NaN'\n" +
                                "y = x * int('5')\n" +
                                "z = y + ' ' + str(float(x))\n" +
                                "a = type(z)\n" +
                                "b = a(float(1))";

            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            do
            {
                processor.WalkLine();
            } while (processor.State == ProcessState.Running);

            // Assert
            var x = processor.GetVariable("x");
            Assert.That.ScriptTypeEqual("NaN", x);

            var y = processor.GetVariable("y");
            Assert.That.ScriptTypeEqual("NaNNaNNaNNaNNaN", y);

            string nan = Localized_Base_Entities.Type_Double_NaN;
            var z = processor.GetVariable("z");
            Assert.That.ScriptTypeEqual("NaNNaNNaNNaNNaN " + nan, z);

            var a = processor.GetVariable("a");
            Assert.IsInstanceOfType(a, typeof(PyStringType));

            var b = processor.GetVariable("b");
            Assert.That.ScriptTypeEqual("1.0", b);

            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }
        [TestMethod]
        public void ProcessP7Test()
        {
            /*
                x = 1
                for i in range(1, 10):
                    x = x * i

             */

            // Arrange
            const string code = "x = 1\n" +
                                "for i in range(1, 10):\n" +
                                "    x = x * i";

            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            do
            {
                processor.WalkLine();
            } while (processor.State == ProcessState.Running);

            // Assert
            var x = processor.GetVariable("x");
            Assert.That.ScriptTypeEqual(362880, x);
            
            Assert.AreEqual(ProcessState.Ended, processor.State);
            errorCatcher.AssertNoErrors();
        }

        [TestMethod]
        public void ProcessCallClrTest()
        {
            // Arrange
            const string code = "foo()";
            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

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

        [TestMethod]
        public void ProcessP8Test()
        {
            /*
                
                dt = 7.23
                t = 0.01
                sum = 0.01

                while t < 100:
                    sum = sum + t
                    dt = dt * 1.01
                    t = t + dt


             */

            // Arrange
            const string code = "dt = 7.23\n" +
                                "t = 0.01\n" +
                                "sum = 0.01\n" +
                                "\n" +
                                "while t < 100:\n" +
                                "    sum = sum + t\n" +
                                "    dt = dt * 1.01\n" +
                                "    t = t + dt";

            var processor = (PyProcessor)new PyCompiler().Compile(code, errorCatcher);

            // Act
            do
            {
                processor.WalkLine();
            } while (processor.State == ProcessState.Running);

            // Assert
            var t = processor.GetVariable("t");
            var tDouble = (PyDouble)t;
            Assert.IsTrue(tDouble.Value >= 100);

            var sum = processor.GetVariable("sum");
            var sumDouble = (PyDouble)sum;
            Assert.AreEqual(591, (int)sumDouble.Value);

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