using Purin.Parser.Models;
using Purin.Parser.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Builders
{
    public static class DefaultTokenizerBuilder
    {
        public static Tokenizer Build(List<TokenizerRule>? additionalRules = null)
        {

            TokenizerSettings settings = new TokenizerSettings
            {
                StringCatalystExcluded = ") ;",
                StringCatalystEscapable = ") \\;",
                WordIncluded = "",
                CatchAllType = "",
                SkipWhiteSpace = true,
                IgnoreCase = true
            };

            List<TokenizerRule> rules = new List<TokenizerRule>();
            rules.Add(new TokenizerRule(TokenTypes.StringEnclosing, "\""));
            rules.Add(new TokenizerRule(TokenTypes.TTString, "`", null, true, "'"));
            rules.Add(new TokenizerRule(TokenTypes.StringCatalyst, "$"));
            rules.Add(new TokenizerRule(TokenTypes.EOLComment, "---"));
            rules.Add(new TokenizerRule(TokenTypes.EOLComment, "//"));
            rules.Add(new TokenizerRule(TokenTypes.MLCommentStart, "/*"));
            rules.Add(new TokenizerRule(TokenTypes.MLCommentEnd, "*/"));
            rules.Add(new TokenizerRule(TokenTypes.EOF, "EOF"));

            rules.Add(new TokenizerRule(TokenTypes.LiteralFalse, "false"));
            rules.Add(new TokenizerRule(TokenTypes.LiteralTrue, "true"));
            rules.Add(new TokenizerRule(TokenTypes.LiteralNull, "null"));
            rules.Add(new TokenizerRule(TokenTypes.Minus, "-"));
            rules.Add(new TokenizerRule(TokenTypes.Plus, "+"));
            rules.Add(new TokenizerRule(TokenTypes.ForwardSlash, "/"));
            rules.Add(new TokenizerRule(TokenTypes.Asterisk, "*"));
            rules.Add(new TokenizerRule(TokenTypes.Dot, "."));
            rules.Add(new TokenizerRule(TokenTypes.Comma, ","));
            rules.Add(new TokenizerRule(TokenTypes.DotDollar, ".$"));
            rules.Add(new TokenizerRule(TokenTypes.SemiColon, ";"));
            rules.Add(new TokenizerRule(TokenTypes.Colon, ":"));
            rules.Add(new TokenizerRule(TokenTypes.DoubleColon, "::"));
            rules.Add(new TokenizerRule(TokenTypes.DoubleQuestionMark, "??"));
            rules.Add(new TokenizerRule(TokenTypes.DoubleDot, ".."));
            rules.Add(new TokenizerRule(TokenTypes.LParen, "("));
            rules.Add(new TokenizerRule(TokenTypes.RParen, ")"));
            rules.Add(new TokenizerRule(TokenTypes.LBracket, "["));
            rules.Add(new TokenizerRule(TokenTypes.RBracket, "]"));
            rules.Add(new TokenizerRule(TokenTypes.LCurly, "{"));
            rules.Add(new TokenizerRule(TokenTypes.RCurly, "}"));
            rules.Add(new TokenizerRule(TokenTypes.Equal, "="));
            rules.Add(new TokenizerRule(TokenTypes.DoubleEqual, "=="));
            rules.Add(new TokenizerRule(TokenTypes.NotEqual, "!="));
            rules.Add(new TokenizerRule(TokenTypes.GreaterThan, ">"));
            rules.Add(new TokenizerRule(TokenTypes.GreaterThanEqual, ">="));
            rules.Add(new TokenizerRule(TokenTypes.LessThan, "<"));
            rules.Add(new TokenizerRule(TokenTypes.LessThanEqual, "<="));
            rules.Add(new TokenizerRule(TokenTypes.DoubleAmpersand, "&&"));
            rules.Add(new TokenizerRule(TokenTypes.DoublePipe, "||"));
            rules.Add(new TokenizerRule(TokenTypes.Not, "!"));
            rules.Add(new TokenizerRule(TokenTypes.If, "if"));
            rules.Add(new TokenizerRule(TokenTypes.Else, "else"));
            rules.Add(new TokenizerRule(TokenTypes.While, "while"));
            rules.Add(new TokenizerRule(TokenTypes.Var, "var"));
            rules.Add(new TokenizerRule(TokenTypes.Continue, "continue"));
            rules.Add(new TokenizerRule(TokenTypes.Break, "break"));
            rules.Add(new TokenizerRule(TokenTypes.Sub, "sub:"));
            rules.Add(new TokenizerRule(TokenTypes.TypeRef, "cstype(", null, true, ")"));
            rules.Add(new TokenizerRule(TokenTypes.ProvideLib, ".providelib"));
            rules.Add(new TokenizerRule(TokenTypes.Lib, ".lib"));
            rules.Add(new TokenizerRule(TokenTypes.Use, ".use"));
            rules.Add(new TokenizerRule(TokenTypes.Entry, ".entry"));
            rules.Add(new TokenizerRule(TokenTypes.End, ".end"));
            rules.Add(new TokenizerRule(TokenTypes.DotSub, ".sub"));
            rules.Add(new TokenizerRule(TokenTypes.New, "new"));
            rules.Add(new TokenizerRule(TokenTypes.DoubleLBracket, "[["));
            rules.Add(new TokenizerRule(TokenTypes.DoubleRBracket, "]]"));

            rules.AddRange(additionalRules ?? Enumerable.Empty<TokenizerRule>());

            return new Tokenizer(rules, settings);
        }
    }
}
