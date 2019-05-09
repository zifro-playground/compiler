using System;
using System.Collections;
using System.Collections.Generic;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyEnumeratorProxy : ScriptType, IEnumerator<IScriptType>, IEnumerable<IScriptType>
    {
        public IScriptType SourceType { get; }
        public IEnumerator<IScriptType> Enumerator { get; }

        public PyEnumeratorProxy(
            IProcessor processor,
            IScriptType sourceType,
            IEnumerator<IScriptType> enumerator)
            : base(processor)
        {
            SourceType = sourceType;
            Enumerator = enumerator;
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

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            if (rhs is PyEnumeratorProxy proxy &&
                proxy.Enumerator == Enumerator)
            {
                return Processor.Factory.True;
            }

            return Processor.Factory.False;
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            if (!(rhs is PyEnumeratorProxy proxy) ||
                proxy.Enumerator != Enumerator)
            {
                return Processor.Factory.True;
            }

            return Processor.Factory.False;
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