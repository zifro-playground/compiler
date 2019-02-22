using System.ComponentModel;
using Mellis.Core.Entities;
using Mellis.Lang.Python3.Exceptions;
using Mellis.Lang.Python3.Resources;
using Mellis.Lang.Python3.Syntax.Operators.Comparisons;

namespace Mellis.Lang.Python3.Syntax.Operators
{
    public class ComparisonFactory : SyntaxNode
    {
        public ComparisonType Type { get; }
        public string Keyword => GetKeyword(Type);

        public ComparisonFactory(ComparisonType type)
            : base(SourceReference.ClrSource)
        {
            Type = type;
        }

        public Comparison Create(ExpressionNode leftOperand, ExpressionNode rightOperand)
        {
            switch (Type)
            {
                case ComparisonType.Equals:
                    return new CompareEquals(leftOperand, rightOperand);
                case ComparisonType.LessThan:
                    return new CompareLessThan(leftOperand, rightOperand);
                case ComparisonType.LessThanOrEqual:
                    return new CompareLessThanOrEqual(leftOperand, rightOperand);
                case ComparisonType.GreaterThan:
                    return new CompareGreaterThan(leftOperand, rightOperand);
                case ComparisonType.GreaterThanOrEqual:
                    return new CompareGreaterThanOrEqual(leftOperand, rightOperand);
                case ComparisonType.NotEquals:
                    return new CompareNotEquals(leftOperand, rightOperand);
                case ComparisonType.In:
                    return new CompareIn(leftOperand, rightOperand);
                case ComparisonType.InNot:
                    return new CompareInNot(leftOperand, rightOperand);
                case ComparisonType.Is:
                    return new CompareIs(leftOperand, rightOperand);
                case ComparisonType.IsNot:
                    return new CompareIsNot(leftOperand, rightOperand);
                case ComparisonType.NotEqualsABC:
                    throw new SyntaxNotYetImplementedExceptionKeyword(
                        source: Source,
                        keyword: Keyword);
                default:
                    throw new InvalidEnumArgumentException(nameof(Type), (int) Type, typeof(ComparisonType));
            }
        }

        /// <summary>
        /// Used in errors, such as the keyworded NotYetImplemented <see cref="Localized_Python3_Parser.Ex_Syntax_NotYetImplemented_Keyword"/>
        /// </summary>
        public static string GetKeyword(ComparisonType type)
        {
            switch (type)
            {
                case ComparisonType.LessThan:
                    return "<";
                case ComparisonType.LessThanOrEqual:
                    return "<=";
                case ComparisonType.GreaterThan:
                    return ">";
                case ComparisonType.GreaterThanOrEqual:
                    return ">=";
                case ComparisonType.Equals:
                    return "==";
                case ComparisonType.NotEquals:
                    return "!=";
                case ComparisonType.NotEqualsABC:
                    return "<>";
                case ComparisonType.In:
                    return "in";
                case ComparisonType.InNot:
                    return "not in";
                case ComparisonType.Is:
                    return "is";
                case ComparisonType.IsNot:
                    return "is not";
                default:
                    throw new InvalidEnumArgumentException(nameof(type), (int) type, typeof(ComparisonType));
            }
        }

        public override void Compile(PyCompiler compiler)
        {
            throw new SyntaxUncompilableException(Source, typeof(ComparisonFactory));
        }
    }
}