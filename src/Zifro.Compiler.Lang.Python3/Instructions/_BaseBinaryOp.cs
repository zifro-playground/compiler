using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public abstract class BaseBinaryOp : IOpCode
    {
        public SourceReference Source { get; }

        protected BaseBinaryOp(SourceReference source)
        {
            Source = source;
        }

        public void Execute(PyProcessor processor)
        {
            var lhs = processor.PopValue<IScriptType>();
            var rhs = processor.PopValue<IScriptType>();

            IScriptType result = Evaluate(lhs, rhs);

            processor.PushValue(result);
        }

        protected abstract IScriptType Evaluate(IScriptType lhs, IScriptType rhs);
    }
}