using System.Collections.Generic;
using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Lang.Python3
{
    public class PyScriptTypeFactory : IScriptTypeFactory
    {
        public IScriptType Null { get; }
        public IScriptType True { get; }
        public IScriptType False { get; }

        public IScriptType Create(int value)
        {
            throw new System.NotImplementedException();
        }

        public IScriptType Create(long value)
        {
            throw new System.NotImplementedException();
        }

        public IScriptType Create(short value)
        {
            throw new System.NotImplementedException();
        }

        public IScriptType Create(byte value)
        {
            throw new System.NotImplementedException();
        }

        public IScriptType Create(char value)
        {
            throw new System.NotImplementedException();
        }

        public IScriptType Create(string value)
        {
            throw new System.NotImplementedException();
        }

        public IScriptType Create(bool value)
        {
            throw new System.NotImplementedException();
        }

        public IScriptType Create(IList<IScriptType> value)
        {
            throw new System.NotImplementedException();
        }

        public IScriptType Create(IDictionary<IScriptType, IScriptType> value)
        {
            throw new System.NotImplementedException();
        }

        public IScriptType Create(IFunction value)
        {
            throw new System.NotImplementedException();
        }
    }
}