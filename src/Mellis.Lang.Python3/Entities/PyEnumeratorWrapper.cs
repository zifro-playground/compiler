﻿using System;
using System.Collections.Generic;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyEnumeratorWrapper : ScriptTypeBase
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

        public override bool TryConvert(Type type, out object value)
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
    }
}