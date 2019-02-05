using System.Collections.Generic;
using Zifro.Compiler.Core.Interfaces;
using Zifro.Compiler.Lang.Python3.Entities;

namespace Zifro.Compiler.Lang.Python3
{
    public class PyScriptTypeFactory : IScriptTypeFactory
    {
        private readonly PyProcessor _processor;

        public PyScriptTypeFactory(PyProcessor processor)
        {
            _processor = processor;
            True = new PyBoolean(true, _processor);
            False = new PyBoolean(false, _processor);
        }

        public IScriptType Null { get; }
        public IScriptType True { get; }
        public IScriptType False { get; }

        public IScriptType Create(int value)
        {
            return new PyInteger(value, _processor);
        }

        public IScriptType Create(long value)
        {
            // TODO: introduce BigNumber
            return new PyInteger((int) value, _processor);
        }

        public IScriptType Create(float value)
        {
            return new PyDouble(value, _processor);
        }

        public IScriptType Create(double value)
        {
            return new PyDouble(value, _processor);
        }

        public IScriptType Create(short value)
        {
            return new PyInteger(value, _processor);
        }

        public IScriptType Create(byte value)
        {
            return new PyInteger(value, _processor);
        }

        public IScriptType Create(char value)
        {
            return new PyString(value.ToString(), _processor);
        }

        public IScriptType Create(string value)
        {
            return new PyString(value, _processor);
        }

        public IScriptType Create(bool value)
        {
            return value ? True : False;
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