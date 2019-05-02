using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Statements
{
    public class WhileStatement : Statement
    {
        public ExpressionNode Condition { get; }
        public Statement Suite { get; }

        public WhileStatement(SourceReference source,
            ExpressionNode condition,
            Statement suite)
            : base(source)
        {
            Condition = condition;
            Suite = suite;
        }

        public override void Compile(PyCompiler compiler)
        {
            if (compiler.Settings.BreakOn.HasFlag(BreakCause.LoopEnter))
            {
                compiler.Push(new Breakpoint(Source, BreakCause.LoopEnter));
            }

            var jumpToCondition = new Jump(Source);
            compiler.Push(jumpToCondition);

            int jumpToSuitePos = compiler.GetJumpTargetForNext();
            Suite.Compile(compiler);

            jumpToCondition.Target = compiler.GetJumpTargetForNext();
            Condition.Compile(compiler);

            if (compiler.Settings.BreakOn.HasFlag(BreakCause.LoopBlockEnd))
            {
                compiler.Push(new Breakpoint(Source, BreakCause.LoopBlockEnd));
            }

            compiler.Push(new JumpIfTrue(Condition.Source, jumpToSuitePos));
        }
    }
}