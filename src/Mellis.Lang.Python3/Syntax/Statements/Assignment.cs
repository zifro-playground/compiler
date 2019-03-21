using Mellis.Core.Entities;
using Mellis.Core.Exceptions;
using Mellis.Lang.Base.Resources;
using Mellis.Lang.Python3.Instructions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax.Literals;

namespace Mellis.Lang.Python3.Syntax.Statements
{
    public class Assignment : Statement
    {
        public ExpressionNode LeftOperand { get; }
        public ExpressionNode RightOperand { get; }

        public Assignment(
            SourceReference source,
            ExpressionNode leftOperand,
            ExpressionNode rightOperand)
            : base(source)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        public override void Compile(PyCompiler compiler)
        {
            string literalName;

            switch (LeftOperand)
            {
            case Identifier id:
                RightOperand.Compile(compiler);
                compiler.Push(new VarSet(Source, id.Name));
                return;

            case LiteralBoolean b:
                throw new SyntaxException(LeftOperand.Source,
                    nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Boolean),
                    Localized_Python3_Parser.Ex_Syntax_Assign_Boolean,
                    b.Value
                        ? Localized_Base_Entities.Type_Boolean_True
                        : Localized_Base_Entities.Type_Boolean_False
                );

            case LiteralInteger i:
                literalName = i.GetTypeName();
                goto Literal;
            case LiteralDouble d:
                literalName = d.GetTypeName();
                goto Literal;
            case LiteralString s:
                literalName = s.GetTypeName();
                goto Literal;

            default:
                throw new SyntaxException(LeftOperand.Source,
                    nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Expression),
                    Localized_Python3_Parser.Ex_Syntax_Assign_Expression
                );
            }

            Literal:
            throw new SyntaxException(LeftOperand.Source,
                nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                Localized_Python3_Parser.Ex_Syntax_Assign_Literal,
                literalName
            );
        }
    }
}