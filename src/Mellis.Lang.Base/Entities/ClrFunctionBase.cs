using System;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Resources;

namespace Mellis.Lang.Base.Entities
{
    public abstract class ClrFunctionBase : ScriptTypeBase
    {
        protected ClrFunctionBase(IProcessor processor, string name = null)
            : base(processor, name)
        {
        }

        /// <inheritdoc />
        public override string GetTypeName()
        {
            return Localized_Base_Entities.Type_Function_Name;
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
                void Action() => Invoke(new IScriptType[0]);
                value = (Action) Action;
                return true;
            }

            // Invoke(arg[])
            if (type == typeof(Action<IScriptType[]>))
            {
                void ActionN(IScriptType[] args) => Invoke(args);
                value = (Action<IScriptType[]>) ActionN;
                return true;
            }

            // Invoke(arg1)
            if (type == typeof(Action<IScriptType>))
            {
                void Action1(IScriptType arg1) => Invoke(new[] {arg1});
                value = (Action<IScriptType>) Action1;
                return true;
            }

            // Invoke() => val
            if (type == typeof(Func<IScriptType>))
            {
                IScriptType Func() => Invoke(new IScriptType[0]);
                value = (Func<IScriptType>) Func;
                return true;
            }

            // Invoke(arg[]) => val
            if (type == typeof(Func<IScriptType[], IScriptType>))
            {
                IScriptType FuncN(IScriptType[] args) => Invoke(args);
                value = (Func<IScriptType[], IScriptType>) FuncN;
                return true;
            }

            // Invoke(arg1) => val
            if (type == typeof(Func<IScriptType, IScriptType>))
            {
                IScriptType Func1(IScriptType arg1) => Invoke(new[] {arg1});
                value = (Func<IScriptType, IScriptType>) Func1;
                return true;
            }

            value = default;
            return false;
        }
    }
}