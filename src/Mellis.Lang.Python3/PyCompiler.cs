using System;
using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using Mellis.Core.Entities;
using Mellis.Core.Interfaces;
using Mellis.Lang.Python3.Extensions;
using Mellis.Lang.Python3.Grammar;
using Mellis.Lang.Python3.Interfaces;
using Mellis.Lang.Python3.Syntax;
using Mellis.Lang.Python3.VM;

namespace Mellis.Lang.Python3
{
    public class PyCompiler : ICompiler, IReadOnlyList<IOpCode>
    {
        private readonly List<IOpCode> _opCodes;

        public PyCompiler()
        {
            _opCodes = new List<IOpCode>(1024);
        }

        public PyCompiler(params IOpCode[] opCodes)
        {
            _opCodes = new List<IOpCode>(opCodes);
        }

        public CompilerSettings Settings { get; set; }

        public IProcessor Compile(string code)
        {
            return Compile(code, null);
        }

        internal IProcessor Compile(string code, IParserErrorListener errorListener)
        {
            var inputStream = new AntlrInputStream(code + "\n");

            var lexer = new Python3Lexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new Python3Parser(tokenStream);
            if (errorListener != null)
            {
                parser.AddErrorListener(errorListener);
            }

            var visitor = new SyntaxConstructor();

            SyntaxNode result = visitor.VisitFile_input(parser.file_input());

            if (result is null)
            {
                return new PyProcessor(Settings);
            }

            Statement statement = result.AsTypeOrThrow<Statement>();
            statement.Compile(this);

            return new PyProcessor(Settings, _opCodes.ToArray());
        }

        public void Push(IOpCode opCode)
        {
            if (opCode is null)
            {
                throw new ArgumentNullException(nameof(opCode));
            }

            _opCodes.Add(opCode);
        }

        public void PushRange(IEnumerable<IOpCode> opCodes)
        {
            if (opCodes is null)
            {
                throw new ArgumentNullException(nameof(opCodes));
            }

            foreach (IOpCode opCode in opCodes)
            {
                Push(opCode);
            }
        }

        /// <summary>
        /// Get jump destination that targets the next op-code to be pushed.
        /// </summary>
        public int GetJumpTargetForNext()
        {
            return Count;
        }

        /// <summary>
        /// Get jump destination that targets a relative jump,
        /// where 0 targets the most recent pushed op-code.
        /// </summary>
        public int GetJumpTargetRelative(int offset)
        {
            return Count - 1 + offset;
        }

        /// <summary>
        /// Get jump destination that targets the most recent pushed op-code.
        /// </summary>
        public int GetJumpTargetForPrevious()
        {
            return Count - 1;
        }

        public IEnumerator<IOpCode> GetEnumerator()
        {
            return _opCodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _opCodes.Count;

        public IOpCode this[int index] => _opCodes[index];
    }
}