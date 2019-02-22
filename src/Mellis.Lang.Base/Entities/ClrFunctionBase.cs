using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ClrFunctionBase : ScriptTypeBase
    {
        public IClrFunction Definition { get; }

        protected ClrFunctionBase(IProcessor processor, IClrFunction definition, string name = null)
            : base(processor, name)
        {
            Definition = definition;
        }

        /// <inheritdoc />
        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_ClrFunction_Name;
        }

        /// <inheritdoc />
        public override bool IsTruthy()
        {
            return true;
        }

        /// <inheritdoc />
        public override bool TryConvert(Type type, out object value)
        {
            // Invoke()
            if (type == typeof(Action))
            {
                void Action() => Definition.Invoke(new IScriptType[0]);
                value = (Action) Action;
                return true;
            }

            // Invoke(arg[])
            if (type == typeof(Action<IScriptType[]>))
            {
                void ActionN(IScriptType[] args) => Definition.Invoke(args);
                value = (Action<IScriptType[]>) ActionN;
                return true;
            }

            // Invoke(arg0)
            if (type == typeof(Action<IScriptType>))
            {
                void Action1(IScriptType arg0) => Definition.Invoke(new[] {arg0});
                value = (Action<IScriptType>) Action1;
                return true;
            }

            // Invoke() => val
            if (type == typeof(Func<IScriptType>))
            {
                IScriptType Func() => Definition.Invoke(new IScriptType[0]);
                value = (Func<IScriptType>) Func;
                return true;
            }

            // Invoke(arg[]) => val
            if (type == typeof(Func<IScriptType[], IScriptType>))
            {
                IScriptType FuncN(IScriptType[] args) => Definition.Invoke(args);
                value = (Func<IScriptType[], IScriptType>) FuncN;
                return true;
            }

            // Invoke(arg0) => val
            if (type == typeof(Func<IScriptType, IScriptType>))
            {
                IScriptType Func1(IScriptType arg0) => Definition.Invoke(new[] {arg0});
                value = (Func<IScriptType, IScriptType>) Func1;
                return true;
            }

            value = default;
            return false;
        }

        public override string ToString()
        {
            return string.Format(
                format: Localized_Base_Entities.Type_ClrFunction_ToString,
                arg0: Name
            );
        }
    }
}