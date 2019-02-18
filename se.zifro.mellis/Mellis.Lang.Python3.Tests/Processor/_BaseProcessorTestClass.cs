using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Entities;

namespace Mellis.Lang.Python3.Tests.Processor
{
    public abstract class BaseProcessorTestClass
    {
        protected PyProcessor processor;

        protected PyInteger GetInteger(int value)
        {
            return new PyInteger(processor, value);
        }

        protected PyString GetString(string value)
        {
            return new PyString(processor, value);
        }

        protected PyDouble GetDouble(double value)
        {
            return new PyDouble(processor, value);
        }

        protected PyBoolean GetBoolean(bool value)
        {
            return new PyBoolean(processor, value);
        }
    }
}