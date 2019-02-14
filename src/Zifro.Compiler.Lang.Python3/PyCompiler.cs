using System;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Grammar;
using Zifro.Compiler.Lang.Python3.Interfaces;

namespace Zifro.Compiler.Lang.Python3
{
    public class PyCompiler : ICompiler, IReadOnlyList<IOpCode>
    {
        private readonly List<IOpCode> _opCodes;

        public PyCompiler()
        {
            _opCodes = new List<IOpCode>(1024);
        }

        public PyCompiler(params IOpCode[] opCodes)
        {
            _opCodes = new List<IOpCode>(opCodes);
        }

        public IProcessor Compile(string code)
        {
            throw new System.NotImplementedException();
        }

        public void Push(IOpCode opCode)
        {
            if (opCode is null)
                throw new ArgumentNullException(nameof(opCode));

            _opCodes.Add(opCode);
        }

        public void PushRange(IEnumerable<IOpCode> opCodes)
        {
            if (opCodes is null)
                throw new ArgumentNullException(nameof(opCodes));

            foreach (IOpCode opCode in opCodes)
                Push(opCode);
        }

        public IEnumerator<IOpCode> GetEnumerator()
        {
            return _opCodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _opCodes.Count;

        public IOpCode this[int index] => _opCodes[index];
    }
}