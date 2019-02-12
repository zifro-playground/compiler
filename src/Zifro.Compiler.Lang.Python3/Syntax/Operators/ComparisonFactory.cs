﻿using System.ComponentModel;
using Zifro.Compiler.Core.Entities;
using Zifro.Compiler.Lang.Python3.Exceptions;
using Zifro.Compiler.Lang.Python3.Resources;
using Zifro.Compiler.Lang.Python3.Syntax.Operators.Comparisons;

namespace Zifro.Compiler.Lang.Python3.Syntax.Operators
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
                case ComparisonType.LessThanOrEqual:
                case ComparisonType.GreaterThan:
                case ComparisonType.GreaterThanOrEqual:
                case ComparisonType.NotEquals:
                case ComparisonType.NotEqualsABC:
                case ComparisonType.In:
                case ComparisonType.InNot:
                case ComparisonType.Is:
                case ComparisonType.IsNot:
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
            throw new System.NotImplementedException();
        }
    }
}