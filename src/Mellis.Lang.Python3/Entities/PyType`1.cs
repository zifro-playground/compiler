using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Resources;

namespace Mellis.Lang.Python3.Entities
{
    public class PyType<T> : PyType
    {
        public string ClassName { get; }

        public PyType(
            IProcessor processor,
            string className,
            string name = null)
            : base(processor, name)
        {
            ClassName = className;
        }

        public override IScriptType Copy(string newName)
        {
            return new PyType<T>(Processor, newName);
        }

        public override IScriptType GetTypeDef()
        {
            return new PyType(Processor);
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
    }
}