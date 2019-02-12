using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3.Instructions
{
    public class OpBinOpCode : IOpCode
    {
        public OperatorCode Code { get; }

        public SourceReference Source { get; }

        public OpBinOpCode(SourceReference source, OperatorCode code)
        {
            Source = source;
            Code = code;
        }

        public void Execute(PyProcessor processor)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return Code.ToString().ToLowerInvariant();
        }
    }
}