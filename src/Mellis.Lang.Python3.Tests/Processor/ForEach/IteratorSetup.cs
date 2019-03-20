using System.Collections.Generic;
using Mellis.Core.Interfaces;
using Moq;

namespace Mellis.Lang.Python3.Tests.Processor.ForEach
{
    public class IteratorSetup
    {
        public readonly Mock<IScriptType> ValueMock;
        public readonly Mock<IEnumerable<IScriptType>> ValueEnumerableMock;
        public readonly Mock<IEnumerator<IScriptType>> EnumeratorMock;

        public IteratorSetup()
        {
            EnumeratorMock = new Mock<IEnumerator<IScriptType>>();

            ValueMock = new Mock<IScriptType>();

            ValueEnumerableMock = ValueMock.As<IEnumerable<IScriptType>>();
        }

        public void SetupGetEnumerator()
        {
            ValueEnumerableMock.Setup(o => o.GetEnumerator())
                .Returns(EnumeratorMock.Object).Verifiable();
        }

        public void SetupEnumeratorIsIScriptType()
        {
            EnumeratorMock.As<IScriptType>();
        }

        public void VerifyAll()
        {
            ValueMock.Verify();
            EnumeratorMock.Verify();
            ValueEnumerableMock.Verify();
        }
    }
}