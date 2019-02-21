using Mellis.Core.Interfaces;

namespace Mellis.Tools.Tests.AutoObject
{
    public class TestingDerivedClass : TestingClass
    {
        public TestingDerivedClass(IProcessor processor, string name = null) : base(processor, name)
        {
        }
    }
}