using System;
using System.Diagnostics;
using Mellis.Core.Exceptions;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mellis.Lang.Python3.Tests
{
    public static class FunctionTestingHelpers
    {
        public static RuntimeTooFewArgumentsException ThrowsTooFewArguments<T>(
            this Assert assert,
            T function, int minimum)
            where T : IClrFunction
        {
            Debug.Assert(minimum > 0, "minimum > 0");

            void Action()
            {
                function.Invoke(new IScriptType[minimum - 1]);
            }

            var ex = Assert.ThrowsException<RuntimeTooFewArgumentsException>((Action)Action);

            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Invoke_TooFewArguments),
                function.FunctionName,
                minimum,
                minimum - 1
            );

            return ex;
        }

        public static RuntimeTooManyArgumentsException ThrowsTooManyArguments<T>(
            this Assert assert,
            T function, int maximum)
            where T : IClrFunction
        {
            void Action()
            {
                function.Invoke(new IScriptType[maximum + 1]);
            }

            var ex = Assert.ThrowsException<RuntimeTooManyArgumentsException>((Action)Action);

            Assert.That.ErrorFormatArgsEqual(ex,
                nameof(Localized_Python3_Runtime.Ex_Invoke_TooManyArguments),
                function.FunctionName,
                maximum,
                maximum + 1
            );

            return ex;
        }

        public static RuntimeException Throws<T>(
            this Assert assert,
            T function, params IScriptType[] arguments)
            where T : IClrFunction
        {
            void Action()
            {
                function.Invoke(arguments);
            }

            var ex = Assert.ThrowsException<RuntimeException>((Action)Action);

            return ex;
        }
    }
}