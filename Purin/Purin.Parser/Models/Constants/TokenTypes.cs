using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Models.Constants
{
    public static class TokenTypes
    {
        // Tokenizer required types
        public const string StringEnclosing = "StringEnclosing";
        public const string StringCatalyst = "StringCatalyst";
        public const string EOLComment = "EOLComment";
        public const string MLCommentStart = "MLCommentStart";
        public const string MLCommentEnd = "MLCommentEnd";
        public const string EOF = "EOF";
        public const string TTString = "TTString";
        public const string TTWord = "TTWord";
        public const string TTInteger = "TTInteger";
        public const string TTUnsignedInteger = "TTUnsignedInteger";
        public const string TTFloat = "TTFloat";
        public const string TTDouble = "TTDouble";
        public const string WhiteSpaceSpace = "WhiteSpaceSpace";
        public const string WhiteSpaceTab = "WhiteSpaceTab";
        public const string WhiteSpaceCR = "WhiteSpaceCR";
        public const string WhiteSpaceLF = "WhiteSpaceLF";

        // Custom Types

        public const string LiteralFalse = "LiteralFalse";
        public const string LiteralTrue = "LiteralTrue";
        public const string LiteralNull = "LiteralNull";
        public const string Minus = "Minus";
        public const string Plus = "Plus";
        public const string ForwardSlash = "ForwardSlash";
        public const string Asterisk = "Asterisk";

        public const string Dot = "Dot";
        public const string Comma = "Comma";
        public const string DotDollar = "DotDollar";
        public const string SemiColon = "SemiColon";
        public const string Colon = "Colon";
        public const string DoubleColon = "DoubleColon";
        public const string DoubleQuestionMark = "DoubleQuestionMark";
        public const string DoubleDot = "DoubleDot";
        public const string LParen = "LParen";
        public const string RParen = "RParen";
        public const string LBracket = "LBracket";
        public const string RBracket = "RBracket";
        public const string LCurly = "LCurly";
        public const string RCurly = "RCurly";
        public const string Equal = "Equal";
        public const string DoubleEqual = "DoubleEqual";
        public const string NotEqual = "NotEqual";
        public const string GreaterThan = "GreaterThan";
        public const string GreaterThanEqual = "GreaterThanEqual";
        public const string LessThan = "LessThan";
        public const string LessThanEqual = "LessThanEqual";
        public const string DoubleAmpersand = "DoubleAmpersand";
        public const string DoublePipe = "DoublePipe";
        public const string Not = "Not";

        public const string If = "If";
        public const string Else = "Else";
        public const string While = "While";
        public const string Var = "Var";
        public const string Return = "Return";
        public const string Continue = "Continue";
        public const string Break = "Break";
        public const string Sub = "Sub";

        public const string TypeRef = "TypeRef";

        public const string ProvideLib = "ProvideLib";
        public const string Lib = "Lib";
        public const string Use = "Use";
        public const string Fn = "Fn";
        public const string Entry = "Entry";
        public const string End = "End";
        public const string DotSub = "DotSub";
    }
}
