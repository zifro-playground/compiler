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
            var op = GetOpCodeForAssignmentOrThrow(Source, LeftOperand);

            RightOperand.Compile(compiler);
            compiler.Push(op);
        }

        public static VarSet GetOpCodeForAssignmentOrThrow(SourceReference source, ExpressionNode leftOperand)
        {
            switch (leftOperand)
            {
            case Identifier id:
                return new VarSet(source, id.Name);

            case LiteralBoolean b:
                throw new SyntaxException(leftOperand.Source,
                    nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Boolean),
                    Localized_Python3_Parser.Ex_Syntax_Assign_Boolean,
                    b.Value
                        ? Localized_Base_Entities.Type_Boolean_True
                        : Localized_Base_Entities.Type_Boolean_False
                );

            case LiteralNone _:
                throw new SyntaxException(leftOperand.Source,
                    nameof(Localized_Python3_Parser.Ex_Syntax_Assign_None),
                    Localized_Python3_Parser.Ex_Syntax_Assign_None
                );

            case Literal lit:
                throw new SyntaxException(leftOperand.Source,
                    nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Literal),
                    Localized_Python3_Parser.Ex_Syntax_Assign_Literal,
                    lit.GetTypeName()
                );

            default:
                throw new SyntaxException(leftOperand.Source,
                    nameof(Localized_Python3_Parser.Ex_Syntax_Assign_Expression),
                    Localized_Python3_Parser.Ex_Syntax_Assign_Expression
                );
            }
        }
    }
}