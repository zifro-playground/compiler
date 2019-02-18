namespace Mellis.Lang.Python3.Instructions
{
    public enum OperatorCode
    {
        // Binary operators (lhs op rhs)
        AAdd = 0x0, // a+b
        ASub, // a-b
        AMul, // a*b
        ADiv, // a/b
        AFlr, // a//b
        AMod, // a%b
        APow, // a**b

        BAnd = 0x8, // a&b
        BLsh, // a<<b
        BRsh, // a>>b
        BOr, // a|b
        BXor, // a^b

        CEq = 0x10, // a==b
        CNEq, // a!=b
        CGt, // a>b
        CGtEq, // a>=b
        CLt, // a<b
        CLtEq, // a<=b

        LAnd = 0x18, // a&&b
        LOr, // a||b

        // Unary operators (op rhs)
        ANeg = 0x20, // +a
        APos, // -a
        BNot, // ~a
        LNot, // !a
    }
}