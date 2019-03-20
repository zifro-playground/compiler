using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;

namespace Mellis.Lang.Python3.Entities
{
    public class PyRange : ScriptTypeBase
    {
        public PyRange(IProcessor processor, string name = null)
            : base(processor, name)
        {
        }

        public override IScriptType Copy(string newName)
        {
            throw new NotImplementedException();
        }

        public override IScriptType GetTypeDef()
        {
            throw new NotImplementedException();
        }

        public override string GetTypeName()
        {
            throw new NotImplementedException();
        }

        public override bool IsTruthy()
        {
            throw new NotImplementedException();
        }

        public override bool TryConvert(Type type, out object value)
        {
            throw new NotImplementedException();
        }
    }
}