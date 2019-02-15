using System.Collections.Generic;
using System.Linq;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Instructions;

namespace Zifro.Compiler.Lang.Python3.Syntax.Statements
{
    public class IfStatement : Statement
    {
        public IfStatement(SourceReference source,
            ExpressionNode condition,
            Statement ifSuite,
            Statement elseSuite) : base(source)
        {
            Condition = condition;
            IfSuite = ifSuite;
            ElseSuite = elseSuite;
        }

        public IfStatement(SourceReference source,
            ExpressionNode condition,
            Statement ifSuite)
            : this(source, condition, ifSuite, null)
        {
        }

        public ExpressionNode Condition { get; }

        public Statement IfSuite { get; }

        public Statement ElseSuite { get; }

        public override void Compile(PyCompiler compiler)
        {
            var endLabel = new Label(Source.LastRow());

            CompileWithEndLabel(compiler, endLabel);

            compiler.Push(endLabel);
        }

        private void CompileWithEndLabel(PyCompiler compiler, Label endLabel)
        {
            Condition.Compile(compiler);

            switch (ElseSuite)
            {
                case IfStatement innerIf:
                {
                    // Else is if statement
                    // Jump to inner-if if false
                    var elifLabel = new Label(innerIf.Source);
                    var jumpToElifIfFalse = new JumpIfFalse(Condition.Source, elifLabel);
                    compiler.Push(jumpToElifIfFalse);

                    IfSuite.Compile(compiler);

                    var jumpToEnd = new Jump(IfSuite.Source.LastRow(), endLabel);
                    compiler.Push(jumpToEnd);
                    compiler.Push(elifLabel);

                    innerIf.CompileWithEndLabel(compiler, endLabel);

                        if (innerIf.ElseSuite != null)
                        {
                        }
                        break;
                }

                case null:
                {
                    // No else, jump to end if false
                    var jumpToEndIfFalse = new JumpIfFalse(Condition.Source, endLabel);
                    compiler.Push(jumpToEndIfFalse);
                    IfSuite.Compile(compiler);
                    break;
                }

                default:
                {
                    // Else is any other statement
                    // Jump to else if false
                    var elseLabel = new Label(ElseSuite.Source);
                    var jumpToElseIfFalse = new JumpIfFalse(Condition.Source, elseLabel);
                    compiler.Push(jumpToElseIfFalse);

                    IfSuite.Compile(compiler);

                    var jumpToEnd = new Jump(IfSuite.Source.LastRow(), endLabel);
                    compiler.Push(jumpToEnd);
                    compiler.Push(elseLabel);

                    ElseSuite.Compile(compiler);
                    break;
                }
            }
        }
    }
}