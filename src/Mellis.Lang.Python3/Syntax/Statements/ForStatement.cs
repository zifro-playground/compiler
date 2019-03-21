using Mellis.Core.Entities;
using Mellis.Lang.Python3.Instructions;

namespace Mellis.Lang.Python3.Syntax.Statements
{
    public class ForStatement : Statement
    {
        public ExpressionNode Operand { get; }
        public ExpressionNode Iterator { get; }
        public Statement Suite { get; }

        public ForStatement(
            SourceReference source,
            ExpressionNode operand,
            ExpressionNode iterator,
            Statement suite) : base(source)
        {
            Operand = operand;
            Iterator = iterator;
            Suite = suite;
        }

        public override void Compile(PyCompiler compiler)
        {
            VarSet operandOpCode = Assignment.GetOpCodeForAssignmentOrThrow(Operand.Source, Operand);

            Iterator.Compile(compiler);

            compiler.Push(new ForEachEnter(Source));

            var jumpToNext = new Jump(Source);
            compiler.Push(jumpToNext);
            int jumpTargetToAssign = compiler.GetJumpTargetForNext();

            compiler.Push(operandOpCode);

            Suite.Compile(compiler);

            jumpToNext.Target = compiler.GetJumpTargetForNext();

            compiler.Push(
                new ForEachNext(Iterator.Source, jumpTargetToAssign)
            );

            compiler.Push(new ForEachExit(Iterator.Source));
        }
    }
}