using System;
using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.Syntax.Literals;
using Mellis.Lang.Python3.Syntax.Statements;
using Mellis.Lang.Python3.Tests.TestingOps;
using Mellis.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Python3.Tests.Compiler.Statements
{
    [TestClass]
    public class ForStatementTests
    {
        private static PyCompiler ArrangeAndAct(
            out NopOp iterOp,
            out NopOp suiteOp)
        {
            return ArrangeAndAct(CompilerSettings.DefaultSettings,
                out iterOp,
                out suiteOp);
        }

        private static PyCompiler ArrangeAndAct(
            CompilerSettings settings,
            out NopOp iterOp,
            out NopOp suiteOp)
        {
            // Arrange
            var compiler = new PyCompiler {Settings = settings};
            var id = new Identifier(SourceReference.ClrSource, "fool");

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> iterMock,
                out iterOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out suiteOp);

            var stmt = new ForStatement(SourceReference.ClrSource,
                id, iterMock.Object, suiteMock.Object
            );

            // Act
            stmt.Compile(compiler);

            return compiler;
        }

        private static void _ThrowAssignLiteralIntegerTest<TLiteral>(
            SourceReference source,
            Mock<TLiteral> literalMock,
            string errorLocalizedKey,
            params object[] errorFormatArgs)
            where TLiteral : ExpressionNode
        {
            // Arrange
            var compiler = new PyCompiler();

            compiler.CreateAndSetup(
                out Mock<ExpressionNode> iterMock,
                out var iterOp);

            compiler.CreateAndSetup(
                out Mock<Statement> suiteMock,
                out var suiteOp);

            var stmt = new ForStatement(SourceReference.ClrSource,
                literalMock.Object, iterMock.Object, suiteMock.Object
            );

            void Action()
            {
                stmt.Compile(compiler);
            }

            // Act
            var ex = Assert.ThrowsException<SyntaxException>((Action)Action);

            // Assert
            Assert.That.ErrorSyntaxFormatArgsEqual(ex,
                errorLocalizedKey,
                source,
                errorFormatArgs
            );

            literalMock.Verify(o => o.Compile(compiler), Times.Never);
            iterMock.Verify(o => o.Compile(compiler), Times.Never);
            suiteMock.Verify(o => o.Compile(compiler), Times.Never);
        }

        [TestMethod]
        public void ForLoopAssignIdentifier()
        {
            var compiler = ArrangeAndAct(out var iterOp, out var suiteOp);

            // Assert

            /* 
             0 nop "iter"
             1 iter->prep
             2 jmp->@5 (iter->next)
             3 set->fool
             4 nop "suite"
             5 iter->next->@3 (set->foo)
             6 iter->end
             */

            Assert.That.IsExpectedOpCode(compiler, 0, iterOp);
            Assert.That.IsOpCode<ForEachEnter>(compiler, 1);
            var jumpToNext = Assert.That.IsOpCode<Jump>(compiler, 2);
            var varSet = Assert.That.IsOpCode<VarSet>(compiler, 3);
            Assert.That.IsExpectedOpCode(compiler, 4, suiteOp);
            var iterNext = Assert.That.IsOpCode<ForEachNext>(compiler, 5);
            Assert.That.IsOpCode<ForEachExit>(compiler, 6);

            Assert.AreSame("fool", varSet.Identifier);
            Assert.AreEqual(jumpToNext.Target, 5);
            Assert.AreEqual(iterNext.JumpTarget, 3);
            Assert.AreEqual(7, compiler.Count, "Too many op codes.");
        }

        [TestMethod]
        public void BreakOn_LoopEnter_Test()
        {
            var compiler = ArrangeAndAct(
                new CompilerSettings {
                    BreakOn = BreakCause.LoopEnter
                },
                out var iterOp,
                out var suiteOp
            );

            // Assert

            /*
             0 break->LoopEnter
             1 nop "iter"
             2 iter->prep
             3 jmp->@5 (iter->next)
             4 set->fool
             5 nop "suite"
             6 iter->next->@3 (set->foo)
             7 iter->end
             */

            var breakpoint = Assert.That.IsOpCode<Breakpoint>(compiler, 0);
            Assert.That.IsExpectedOpCode(compiler, 1, iterOp);
            Assert.That.IsOpCode<ForEachEnter>(compiler, 2);
            Assert.That.IsOpCode<Jump>(compiler, 3);
            Assert.That.IsOpCode<VarSet>(compiler, 4);
            Assert.That.IsExpectedOpCode(compiler, 5, suiteOp);
            Assert.That.IsOpCode<ForEachNext>(compiler, 6);
            Assert.That.IsOpCode<ForEachExit>(compiler, 7);

            Assert.AreEqual(BreakCause.LoopEnter, breakpoint.BreakCause);
            Assert.AreEqual(8, compiler.Count, "Too many op codes.");
        }

        [TestMethod]
        public void BreakOn_LoopBlockEnd_Test()
        {
            var compiler = ArrangeAndAct(
                new CompilerSettings {
                    BreakOn = BreakCause.LoopBlockEnd
                },
                out var iterOp,
                out var suiteOp
            );

            // Assert

            /* 
             0 nop "iter"
             1 iter->prep
             2 jmp->@5 (iter->next)
             3 set->fool
             4 nop "suite"
             5 break->LoopBlockEnd
             6 iter->next->@3 (set->foo)
             7 iter->end
             */

            Assert.That.IsExpectedOpCode(compiler, 0, iterOp);
            Assert.That.IsOpCode<ForEachEnter>(compiler, 1);
            Assert.That.IsOpCode<Jump>(compiler, 2);
            Assert.That.IsOpCode<VarSet>(compiler, 3);
            Assert.That.IsExpectedOpCode(compiler, 4, suiteOp);
            var breakpoint = Assert.That.IsOpCode<Breakpoint>(compiler, 5);
            Assert.That.IsOpCode<ForEachNext>(compiler, 6);
            Assert.That.IsOpCode<ForEachExit>(compiler, 7);

            Assert.AreEqual(BreakCause.LoopBlockEnd, breakpoint.BreakCause);
            Assert.AreEqual(8, compiler.Count, "Too many op codes.");
        }

        [TestMethod]
        public void BreakOn_LoopBoth_Test()
        {
            var compiler = ArrangeAndAct(
                new CompilerSettings {
                    BreakOn = BreakCause.LoopEnter |
                              BreakCause.LoopBlockEnd
                },
                out var iterOp,
                out var suiteOp
            );

            // Assert

            /*
             0 break->LoopEnter
             1 nop "iter"
             2 iter->prep
             3 jmp->@5 (iter->next)
             4 set->fool
             5 nop "suite"
             6 break->LoopBlockEnd
             7 iter->next->@3 (set->foo)
             8 iter->end
             */

            var breakpoint1 = Assert.That.IsOpCode<Breakpoint>(compiler, 0);
            Assert.That.IsExpectedOpCode(compiler, 1, iterOp);
            Assert.That.IsOpCode<ForEachEnter>(compiler, 2);
            Assert.That.IsOpCode<Jump>(compiler, 3);
            Assert.That.IsOpCode<VarSet>(compiler, 4);
            Assert.That.IsExpectedOpCode(compiler, 5, suiteOp);
            var breakpoint2 = Assert.That.IsOpCode<Breakpoint>(compiler, 6);
            Assert.That.IsOpCode<ForEachNext>(compiler, 7);
            Assert.That.IsOpCode<ForEachExit>(compiler, 8);

            Assert.AreEqual(BreakCause.LoopEnter, breakpoint1.BreakCause);
            Assert.AreEqual(BreakCause.LoopBlockEnd, breakpoint2.BreakCause);
            Assert.AreEqual(9, compiler.Count, "Too many op codes.");
        }

        [TestMethod]
        public void ThrowAssignLiteralIntegerTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralInteger>(source, 5);

            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                literalMock.Object.GetTypeName());
        }

        [TestMethod]
        public void ThrowAssignLiteralDoubleTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralDouble>(source, 10d);

            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                literalMock.Object.GetTypeName());
        }

        [TestMethod]
        public void ThrowAssignLiteralStringTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralString>(source, "foo");

            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                literalMock.Object.GetTypeName());
        }

        [TestMethod]
        public void ThrowAssignTrueTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralBoolean>(source, true);
            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Boolean),
                Localized_Base_Entities.Type_Boolean_True
            );
        }

        [TestMethod]
        public void ThrowAssignFalseTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralBoolean>(source, false);
            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Boolean),
                Localized_Base_Entities.Type_Boolean_False
            );
        }

        [TestMethod]
        public void ThrowAssignExpression()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<ExpressionNode>(source);

            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Expression)
            );
        }

        [TestMethod]
        public void ThrowAssignNoneTest()
        {
            var source = new SourceReference(3, 5, 5, 1);
            var literalMock = new Mock<LiteralNone>(source);

            _ThrowAssignLiteralIntegerTest(
                source,
                literalMock,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_None)
            );
        }
    }
}