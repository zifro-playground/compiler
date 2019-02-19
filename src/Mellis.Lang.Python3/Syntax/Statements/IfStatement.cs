using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Statements
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
            var jumpToEnd = new Collection<Jump>();

            CompileWithEndLabel(compiler, jumpToEnd);

            int endTarget = compiler.GetJumpTargetForNext();
            foreach (Jump jump in jumpToEnd)
            {
                jump.Target = endTarget;
            }
        }

        private void CompileWithEndLabel(PyCompiler compiler, ICollection<Jump> endJumps)
        {
            Condition.Compile(compiler);

            switch (ElseSuite)
            {
                case IfStatement innerIf:
                {
                    // Else is if statement
                    // Jump to inner-if if false
                    var jumpToElifIfFalse = new JumpIfFalse(Condition.Source);
                    compiler.Push(jumpToElifIfFalse);

                    IfSuite.Compile(compiler);

                    var jumpToEnd = new Jump(IfSuite.Source.LastRow());
                    endJumps.Add(jumpToEnd);

                    compiler.Push(jumpToEnd);

                    jumpToElifIfFalse.Target = compiler.GetJumpTargetForNext();
                    innerIf.CompileWithEndLabel(compiler, endJumps);

                    break;
                }

                case null:
                {
                    // No else, jump to end if false
                    var jumpToEndIfFalse = new JumpIfFalse(Condition.Source);
                    endJumps.Add(jumpToEndIfFalse);
                    compiler.Push(jumpToEndIfFalse);
                    IfSuite.Compile(compiler);
                    break;
                }

                default:
                {
                    // Else is any other statement
                    // Jump to else if false
                    var jumpToElseIfFalse = new JumpIfFalse(Condition.Source);
                    compiler.Push(jumpToElseIfFalse);

                    IfSuite.Compile(compiler);

                    var jumpToEnd = new Jump(IfSuite.Source.LastRow());
                    endJumps.Add(jumpToEnd);
                    compiler.Push(jumpToEnd);

                    jumpToElseIfFalse.Target = compiler.GetJumpTargetForNext();
                    ElseSuite.Compile(compiler);
                    break;
                }
            }
        }
    }
}