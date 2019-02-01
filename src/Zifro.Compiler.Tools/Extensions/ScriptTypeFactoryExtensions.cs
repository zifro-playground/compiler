using System;
using System.Collections.Generic;
using System.Text;
using Zifro.Compiler.Core.Interfaces;

namespace Zifro.Compiler.Tools.Extensions
{
    public static class ScriptTypeFactoryExtensions
    {
        public static bool TryCreate<T>(this IScriptTypeFactory factory, T clrValue, out IScriptType scriptTypeValue)
        {
            if (clrValue == null)
            {
                scriptTypeValue = factory.Null;
                return true;
            }

            switch (clrValue)
            {
                case bool v:
                    scriptTypeValue = v ? factory.True : factory.False;
                    return true;

                case int v:
                    scriptTypeValue = factory.Create(v);
                    return true;

                case short v:
                    scriptTypeValue = factory.Create(v);
                    return true;

                case byte v:
                    scriptTypeValue = factory.Create(v);
                    return true;

                case long v:
                    scriptTypeValue = factory.Create(v);
                    return true;

                case char v:
                    scriptTypeValue = factory.Create(v);
                    return true;

                case string v:
                    scriptTypeValue = factory.Create(v);
                    return true;

                case IList<IScriptType> v:
                    scriptTypeValue = factory.Create(v);
                    return true;

                case IDictionary<IScriptType, IScriptType> v:
                    scriptTypeValue = factory.Create(v);
                    return true;

                case IFunction v:
                    scriptTypeValue = factory.Create(v);
                    return true;

                default:
                    scriptTypeValue = default;
                    return false;
            }
        }
    }
}
