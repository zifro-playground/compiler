using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ScriptClrFunction : ScriptBaseType, IClrFunction
    {
        public ScriptClrFunction(
            IProcessor processor,
            string functionName)
            : base(processor)
        {
            FunctionName = functionName;
        }

        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_ClrFunction_Name;
        }

        public override bool TryCoerce(Type type, out object value)
        {
            // Invoke()
            if (type == typeof(Action))
            {
                void Action() => Invoke();
                value = (Action)Action;
                return true;
            }

            // Invoke(arg[])
            if (type == typeof(Action<IScriptType[]>))
            {
                void ActionN(IScriptType[] args) => Invoke(args);
                value = (Action<IScriptType[]>)ActionN;
                return true;
            }

            // Invoke(arg0)
            if (type == typeof(Action<IScriptType>))
            {
                void Action1(IScriptType arg0) => Invoke(arg0);
                value = (Action<IScriptType>)Action1;
                return true;
            }

            // Invoke() => val
            if (type == typeof(Func<IScriptType>))
            {
                IScriptType Func() => Invoke();
                value = (Func<IScriptType>)Func;
                return true;
            }

            // Invoke(arg[]) => val
            if (type == typeof(Func<IScriptType[], IScriptType>))
            {
                IScriptType FuncN(IScriptType[] args) => Invoke(args);
                value = (Func<IScriptType[], IScriptType>)FuncN;
                return true;
            }

            // Invoke(arg0) => val
            if (type == typeof(Func<IScriptType, IScriptType>))
            {
                IScriptType Func1(IScriptType arg0) => Invoke(arg0);
                value = (Func<IScriptType, IScriptType>)Func1;
                return true;
            }

            value = default;
            return false;
        }

        IProcessor IEmbeddedType.Processor
        {
            set => Processor = value;
        }

        public string FunctionName { get; }

        public abstract IScriptType Invoke(params IScriptType[] arguments);
    }
}