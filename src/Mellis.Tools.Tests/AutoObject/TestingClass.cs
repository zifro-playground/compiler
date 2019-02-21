using Mellis.Core.Interfaces;
using Mellis.Tools.AutoObject;

namespace Mellis.Tools.Tests.AutoObject
{
    public class TestingClass : AutoValueBase
    {
        public const string ProtectedPropertyName = nameof(ProtectedProperty);
        public const string PrivatePropertyName = nameof(PrivateProperty);
        public const string PrivateFieldName = nameof(PrivateField);

        [ShowInScript]
        public string PublicProperty { get; set; }

        [ShowInScript]
        protected string ProtectedProperty { get; set; }

        [ShowInScript]
        private string PrivateProperty { get; set; }

        public string PublicPropertyWithoutAttribute { get; set; }

        [ShowInScript]
        private string PrivateField;

        [ShowInScript]
        public string PublicField;

        public string PublicFieldWithoutAttribute;

        public TestingClass WithProtectedProperty(string value)
        {
            ProtectedProperty = value;
            return this;
        }

        public TestingClass WithPrivateProperty(string value)
        {
            PrivateProperty = value;
            return this;
        }

        public TestingClass WithPrivatePropertyField(string value)
        {
            PrivateField = value;
            return this;
        }

        public override IScriptType Copy(string newName)
        {
            return new TestingClass(Processor, newName);
        }

        public override IScriptType GetTypeDef()
        {
            throw new System.NotImplementedException();
        }

        public override string GetTypeName()
        {
            throw new System.NotImplementedException();
        }

        public override IScriptType Invoke(IScriptType[] arguments)
        {
            throw new System.NotImplementedException();
        }

        public TestingClass(IProcessor processor, string name = null) : base(processor, name)
        {
        }
    }
}