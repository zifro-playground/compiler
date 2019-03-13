using System;
using System.Linq;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyType<T> : PyClrFunction
    {
        public string ClassName { get; }
        public Construct Constructor { get; }

        public delegate IScriptType Construct(IProcessor processor, IScriptType[] arguments);

        public PyType(
            IProcessor processor,
            string className,
            Construct constructor,
            string name = null)
            : base(processor,
                new TypeConstructorFunction(processor, className, constructor),
                name)
        {
            ClassName = className;
            Constructor = constructor;
        }

        public override IScriptType Copy(string newName)
        {
            return new PyType<T>(Processor, ClassName, Constructor, newName);
        }

        public override IScriptType GetTypeDef()
        {
            return new PyType(Processor);
        }

        public override string GetTypeName()
        {
            return Localized_Python3_Entities.Type_Type_Name;
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
            return string.Format(Localized_Python3_Entities.Type_Type_ToString,
                /* {0} */ ClassName
            );
        }

        #region Comparison implementations

        public override IScriptType CompareEqual(IScriptType rhs)
        {
            return Processor.Factory.Create(rhs is PyType<T>);
        }

        public override IScriptType CompareNotEqual(IScriptType rhs)
        {
            return Processor.Factory.Create(!(rhs is PyType<T>));
        }

        #endregion

        private class TypeConstructorFunction : IClrFunction
        {
            public IProcessor Processor { private get; set; }
            public string FunctionName { get; }
            private readonly Construct _constructor;

            public TypeConstructorFunction(
                IProcessor processor,
                string className,
                Construct constructor)
            {
                Processor = processor;
                FunctionName = className;
                _constructor = constructor;
            }

            public IScriptType Invoke(IScriptType[] arguments)
            {
                return _constructor(Processor, arguments);
            }
        }
    }
}