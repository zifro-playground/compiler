using System;
using System.Linq;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Base.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Mellis.Lang.Base.Tests
{
    public class BaseTestClass
    {
        protected Mock<IScriptTypeFactory> factoryMock;
        protected Mock<IProcessor> processorMock;

        [TestInitialize]
        public void TestInitialize()
        {
            processorMock = new Mock<IProcessor>();
            factoryMock = new Mock<IScriptTypeFactory>();

            processorMock.SetupGet(o => o.Factory).Returns(factoryMock.Object);
            factoryMock.Setup(o => o.Create(It.IsAny<int>()))
                .Returns<int>(GetInteger);
            factoryMock.Setup(o => o.Create(It.IsAny<double>()))
                .Returns<double>(GetDouble);
            factoryMock.Setup(o => o.Create(It.IsAny<string>()))
                .Returns<string>(GetString);
            factoryMock.Setup(o => o.Create(It.IsAny<char>()))
                .Returns<char>(c => GetString(c.ToString()));

            factoryMock.SetupGet(o => o.True)
                .Returns(GetBoolean(true));
            factoryMock.SetupGet(o => o.False)
                .Returns(GetBoolean(false));
        }

        protected ScriptInteger GetInteger(int value)
        {
            return GetValue<ScriptInteger, int>(value);
        }

        protected ScriptDouble GetDouble(double value)
        {
            return GetValue<ScriptDouble, double>(value);
        }

        protected ScriptString GetString(string value)
        {
            return GetValue<ScriptString, string>(value);
        }

        protected ScriptBoolean GetBoolean(bool value)
        {
            return GetValue<ScriptBoolean, bool>(value);
        }

        protected IScriptType GetValue(object value)
        {
            switch (value)
            {
            case int i:
                return GetInteger(i);
            case double d:
                return GetDouble(d);
            case string s:
                return GetString(s);
            case bool b:
                return GetBoolean(b);
            default:
                throw new NotSupportedException();
            }
        }

        protected void AssertArithmeticResult<T>(
            IScriptType resultBase,
            IScriptType lhs,
            IScriptType rhs,
            object expected)
            where T : IScriptType
        {
            Assert.IsNotNull(resultBase);
            Assert.IsInstanceOfType(resultBase, typeof(T));
            var result = (T)resultBase;
            Assert.IsNotNull(result);
            Assert.AreNotSame(lhs, result);
            Assert.AreNotSame(rhs, result);

            AssertAreEqual(expected, result);

            processorMock.VerifyGet(o => o.Factory, Times.Once);
            processorMock.VerifyNoOtherCalls();

            switch (expected)
            {
            case int i:
                factoryMock.Verify(o => o.Create(i),
                    Times.Once);
                break;

            case double d:
                factoryMock.Verify(o => o.Create(It.Is<double>(v => Math.Abs(v - d) < 1e-10)),
                    Times.Once);
                break;

            case string s:
                factoryMock.Verify(o => o.Create(It.Is<string>(v => v == s)),
                    Times.Exactly(s.Length == 1 ? 0 : 1));
                factoryMock.Verify(o => o.Create(It.Is<char>(v => v.ToString() == s)),
                    Times.Exactly(s.Length == 1 ? 1 : 0));
                break;

            case bool b when b:
                factoryMock.VerifyGet(o => o.True, Times.Once);
                break;

            case bool b when !b:
                factoryMock.VerifyGet(o => o.False, Times.Once);
                break;
            }

            factoryMock.VerifyNoOtherCalls();
        }

        protected void AssertThrow(Action action, string localizationKey, object[] expectedFormatArgs)
        {
            var ex = Assert.ThrowsException<RuntimeException>(action);
            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.LocalizeKey, localizationKey);
            CollectionAssert.AreEqual(ex.FormatArgs, expectedFormatArgs);
            processorMock.VerifyNoOtherCalls();
            factoryMock.VerifyNoOtherCalls();
        }

        protected void AssertAreEqual(object expected, IScriptType actual)
        {
            switch (actual)
            {
            case ScriptDouble d when expected is double e:
                Assert.AreEqual(e, d.Value, 1e-10);
                break;
            case ScriptInteger i:
                Assert.AreEqual(expected, i.Value);
                break;
            case ScriptString s:
                Assert.AreEqual(expected, s.Value);
                break;
            case ScriptBoolean s:
                Assert.AreEqual(expected, s.Value);
                break;
            default:
                throw new NotSupportedException();
            }
        }

        private TScriptType GetValue<TScriptType, TValue>(TValue value)
            where TScriptType : class, IScriptType
        {
            return new Mock<TScriptType>(processorMock.Object, value) {CallBase = true}.Object;
        }
    }
}