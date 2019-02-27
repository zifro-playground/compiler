using System.Collections.Generic;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;

namespace Mellis.Lang.Python3
{
    public class PyScriptTypeFactory : IScriptTypeFactory
    {
        private readonly PyProcessor _processor;

        public PyScriptTypeFactory(PyProcessor processor)
        {
            _processor = processor;
            True = new PyBoolean(_processor, true);
            False = new PyBoolean(_processor, false);
            Null = new PyNone(_processor);
        }

        public IScriptType Null { get; }
        public IScriptType True { get; }
        public IScriptType False { get; }

        public IScriptType Create(int value)
        {
            return new PyInteger(_processor, value);
        }

        public IScriptType Create(long value)
        {
            // TODO: introduce BigNumber
            return new PyInteger(_processor, (int) value);
        }

        public IScriptType Create(float value)
        {
            return new PyDouble(_processor, value);
        }

        public IScriptType Create(double value)
        {
            return new PyDouble(_processor, value);
        }

        public IScriptType Create(short value)
        {
            return new PyInteger(_processor, value);
        }

        public IScriptType Create(byte value)
        {
            return new PyInteger(_processor, value);
        }

        public IScriptType Create(char value)
        {
            return new PyString(_processor, value.ToString());
        }

        public IScriptType Create(string value)
        {
            return new PyString(_processor, value);
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

        public IScriptType Create(IClrFunction value)
        {
            value.Processor = _processor;
            return new PyClrFunction(_processor, value);
        }
    }
}