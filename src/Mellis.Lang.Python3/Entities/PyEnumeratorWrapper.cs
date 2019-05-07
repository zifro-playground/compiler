using System;
using System.Collections;
using System.Collections.Generic;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyEnumeratorWrapper : ScriptBaseType, IEnumerator<IScriptType>, IEnumerable<IScriptType>
    {
        public IScriptType SourceType { get; }
        public IEnumerator<IScriptType> Enumerator { get; }

        public PyEnumeratorWrapper(
            IProcessor processor,
            IScriptType sourceType,
            IEnumerator<IScriptType> enumerator,
            string name = null)
            : base(processor, name)
        {
            SourceType = sourceType;
            Enumerator = enumerator;
        }

        public override IScriptType Copy(string newName)
        {
            return new PyEnumeratorWrapper(Processor, SourceType, Enumerator, newName);
        }

        public override IScriptType GetTypeDef()
        {
            return SourceType.GetTypeDef();
        }

        public override string GetTypeName()
        {
            return Localized_Python3_Entities.Type_Enumerator_Name;
        }

        public override bool IsTruthy()
        {
            return true;
        }

        public override bool TryCoerce(Type type, out object value)
        {
            value = default;
            return false;
        }

        public override string ToString()
        {
            return string.Format(
                Localized_Python3_Entities.Type_Enumerator_ToString,
                SourceType.GetTypeName()
            );
        }

        #region IEnumerable<IScriptType> delegation

        public IEnumerator<IScriptType> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerator<IScriptType> delegation

        void IDisposable.Dispose()
        {
            Enumerator.Dispose();
        }
        
        bool IEnumerator.MoveNext()
        {
            return Enumerator.MoveNext();
        }

        void IEnumerator.Reset()
        {
            Enumerator.Reset();
        }

        IScriptType IEnumerator<IScriptType>.Current => Enumerator.Current;

        object IEnumerator.Current => ((IEnumerator)Enumerator).Current;

        #endregion
    }
}