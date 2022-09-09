using Purin.Parser.Builders;
using Purin.Parser.Helpers;
using Purin.Parser.Models;
using Purin.Parser.Models.Constants;
using Purin.Parser.Models.Enums;
using Purin.Parser.Models.Expressions;
using Purin.Parser.Models.Statements;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser
{
    public class DefaultParser : ParsingHelper
    {

        private NumberFormatInfo DefaultNumberFormat = new NumberFormatInfo { NegativeSign = "-" };
        public Tokenizer? DefaultTokenizer { get; set; }
        public DefaultParser()
        {
        }
        public IEnumerable<BaseStatement> Parse(string text)
        {
            var tokenizer = DefaultTokenizer ?? DefaultTokenizerBuilder.Build();

            var tokens = tokenizer.Tokenize(text);
            init(tokens);
            IList<BaseStatement> statements = new List<BaseStatement>();
            while (!atEnd() && !match(TokenTypes.EOF))
            {
                statements.Add(ParseTopLevelStatement());
            }
            return statements;
        }

        public IEnumerable<BaseStatement> Parse(IEnumerable<Token> tokens)
        {
            init(tokens);
            IList<BaseStatement> statements = new List<BaseStatement>();
            while (!atEnd() && !match(TokenTypes.EOF))
            {
                statements.Add(ParseTopLevelStatement());
            }
            return statements;
        }

        private BaseStatement ParseTopLevelStatement()
        {
            if (match(TokenTypes.ProvideLib))
            {
                return ParseProvideLib();
            }
            if (match(TokenTypes.Lib))
            {
                return ParseLib();
            }
            if (match(TokenTypes.Use))
            {
                return ParseUse();
            }
            if (match(TokenTypes.Entry))
            {
                return ParseEntry();
            }
            if (match(TokenTypes.DotSub))
            {
                return ParseSubRoutine();
            }

            throw new Exception($"only top level statements permitted. illegal token {current()}.");
        }

        private BaseStatement ParseEntry()
        {
            var loc = previous().Loc;
            var target = ParseExpression();
            return new StmtEntry(target, loc);
        }

        private BaseStatement ParseUse()
        {
            var loc = previous().Loc;
            var target = ParseExpression();
            return new StmtUse(target, loc);
        }

        private BaseStatement ParseLib()
        {
            var target = consume(TokenTypes.TTString, "expect target after .lib");
            return new StmtLib(target.Lexeme, target.Loc);
        }

        private BaseStatement ParseProvideLib()
        {
            var target = consume(TokenTypes.TTString, "expect target after .providelib");
            return new StmtProvideLib(target.Lexeme, target.Loc);
        }


        private BaseStatement ParseInnerStatement()
        {
            if (match(TokenTypes.While))
            {
                return ParseWhile();
            }
            if (match(TokenTypes.If))
            {
                return ParseIfElse();
            }
            if (match(TokenTypes.Var))
            {
                return ParseVariableDeclaration();
            }
            if (match(TokenTypes.LCurly))
            {
                return ParseBlock();
            }
            if (match(TokenTypes.Break))
            {
                return ParseBreak();
            }
            if (match(TokenTypes.Continue))
            {
                return ParseContinue();
            }
            if (match(TokenTypes.Sub))
            {
                return ParseCallSubRoutine();
            }
            return ParseExpressionStatement();
        }

        private BaseStatement ParseCallSubRoutine()
        {
            var name = consume(TokenTypes.TTWord, "expect subroutine name");
            var args = new List<BaseExpression>();
            if (!match(TokenTypes.RParen))
            {
                do
                {
                    args.Add(ParseExpression());
                } while (match(TokenTypes.Comma));
            }

            consume(TokenTypes.RParen, "expect enclosing ) in call");
            return new StmtCallSubRoutine(name.Lexeme, args, name.Loc);
        }

        private BaseStatement ParseBreak()
        {
            var token = consume(TokenTypes.SemiColon, "expect ; at end of statement");
            return new StmtBreak(token.Loc);
        }

        private BaseStatement ParseContinue()
        {
            var token = consume(TokenTypes.SemiColon, "expect ; at end of statement"); ;
            return new StmtContinue(token.Loc);
        }

        private BaseStatement ParseSubRoutine()
        {
            Token fnName = consume(TokenTypes.TTWord, "expect subroutine name");
            var parameters = new List<StmtSubRoutine.StmtSubRoutineParameter>();
            while (match(TokenTypes.LBracket))
            {
                var name = consume(TokenTypes.TTWord, "expect parameter name").Lexeme;
                consume(TokenTypes.Colon, "expect :Type in parameter declaration");
                var typeName = ParseExpression();
                BaseExpression? value = null;
                if (match(TokenTypes.Equal))
                {
                    value = ParseExpression();
                }
                consume(TokenTypes.RBracket, "expect enclosing ]");
                parameters.Add(new StmtSubRoutine.StmtSubRoutineParameter(name, typeName, value));
            }
            var statements = new List<BaseStatement>();
            while (!match(TokenTypes.End))
            {
                statements.Add(ParseInnerStatement());
            }
            return new StmtSubRoutine(fnName.Lexeme, parameters, statements, fnName.Loc);
        }
        private BaseStatement ParseIfElse()
        {
            var loc = previous().Loc;
            consume(TokenTypes.LParen, "expect (condition) in if statement");
            var condition = ParseExpression();
            consume(TokenTypes.RParen, "expect enclosing ) after condition in if statement");
            var then = ParseInnerStatement();
            BaseStatement? elseDo = null;
            if (match(TokenTypes.Else))
            {
                elseDo = ParseInnerStatement();
            }
            return new StmtIfElse(condition, then, elseDo, loc);
        }

        private BaseStatement ParseWhile()
        {
            var loc = previous().Loc;
            consume(TokenTypes.LParen, "expect (condition) in while statement");
            var condition = ParseExpression();
            consume(TokenTypes.RParen, "expect enclosing ) after condition in while statement");
            var then = ParseInnerStatement();
     
            return new StmtWhile(condition, then, loc);
        }

        private BaseStatement ParseVariableDeclaration()
        {
            var target = consume(TokenTypes.TTWord, "expect identifier in variable declaration");
            consume(TokenTypes.Equal, "expect = in variable declaration");
            var value = ParseExpression();
            consume(TokenTypes.SemiColon, "expect ; at end of statement");

            return new StmtVariableDeclaration(target.Lexeme, value, target.Loc);
        }

        private BaseStatement ParseBlock()
        {
            var loc = previous().Loc;
            var statements = new List<BaseStatement>();
            while (!match(TokenTypes.RCurly))
            {
                statements.Add(ParseInnerStatement());
            }
            return new StmtBlock(statements, loc);
        }

        private BaseStatement ParseExpressionStatement()
        {
            var loc = current().Loc;
            StmtExpression stmtExpression = new StmtExpression(ParseExpression(), loc);
            consume(TokenTypes.SemiColon, "expect ; at end of statement");

            return stmtExpression;
        }

        private BaseExpression ParseExpression()
        {
            return ParseAssignment();
        }
        private BaseExpression ParseAssignment()
        {
            var expr = ParseBinary();
            if (match(TokenTypes.Equal))
            {
                var value = ParseExpression();
                return new ExprAssignment(expr, value, previous().Loc);
            }
            return expr;
        }

        private BaseExpression ParseBinary()
        {
            return ParseBinaryHelper(new List<string>()
            {
                TokenTypes.DoubleAmpersand,
                TokenTypes.DoublePipe,
                TokenTypes.DoubleEqual,
                TokenTypes.NotEqual,
                TokenTypes.LessThan,
                TokenTypes.LessThanEqual,
                TokenTypes.GreaterThan,
                TokenTypes.GreaterThanEqual,
                TokenTypes.Asterisk,
                TokenTypes.ForwardSlash,
                TokenTypes.Plus,
                TokenTypes.Minus,
            }, 0);
        }

        private BaseExpression ParseBinaryHelper(List<string> operators, int index)
        {
            if (index >= operators.Count) return ParseUnary();
            var expr = ParseBinaryHelper(operators, index++);
            while (match(operators[index]))
            {
                expr = new ExprBinary(previous().Lexeme, expr, ParseBinaryHelper(operators, index), previous().Loc);
            }
            return expr;
        }

        private BaseExpression ParseUnary()
        {
            BaseExpression expr;
            if (match(TokenTypes.Not))
            {
                expr = new ExprUnary(previous().Lexeme, ParseGetOrCall(), previous().Loc);
            } else
            {
                expr = ParseGetOrCall();
            }
            return expr;
        }

        private BaseExpression ParseGetOrCall()
        {
            
            var expr = ParsePrimary();
            while (true)
            {
                if (match(TokenTypes.Dot))
                {
                    expr = new ExprGet(expr, RetrievalType.Property, consume(TokenTypes.TTWord, "expect property name").Lexeme, previous().Loc);
                }
                else if (match(TokenTypes.DotDollar))
                {
                    expr = new ExprGet(expr, RetrievalType.Field, consume(TokenTypes.TTWord, "expect field name").Lexeme, previous().Loc);
                }
                else if (match(TokenTypes.Colon))
                {
                    expr = new ExprGet(expr, RetrievalType.Method, consume(TokenTypes.TTWord, "expect method name").Lexeme, previous().Loc);
                }
                else if (match(TokenTypes.LessThan))
                {
                    var typeArguments = new List<ExprTypeReference>();
                    if (!match(TokenTypes.GreaterThan))
                    {
                        do
                        {
                            var type = consume(TokenTypes.TypeRef, "expect type reference");
                            typeArguments.Add(new ExprTypeReference(type.Lexeme, type.Loc));
                        } while (match(TokenTypes.Comma));
                        consume(TokenTypes.GreaterThan, "expect enclosing >");
                    }

                    expr = new ExprGeneric(expr, typeArguments, previous().Loc);
                }
                if (match(TokenTypes.LParen))
                {
                    
                    var args = new List<BaseExpression>();
                    if (!match(TokenTypes.RParen))
                    {
                        do
                        {
                            args.Add(ParseExpression());
                        } while (match(TokenTypes.Comma));
                    }

                    consume(TokenTypes.RParen, "expect enclosing ) in call");
                    expr = new ExprCall(expr, args, previous().Loc);
                }
                else
                {
                    break;
                }
            }
            return expr;
        }

        private BaseExpression ParsePrimary()
        {
            ExprLiteral exprLiteral = new ExprLiteral(current().Loc);
            if (match(TokenTypes.LiteralFalse))
            {
                exprLiteral.Value = false;
                return exprLiteral;
            }
            if (match(TokenTypes.LiteralTrue))
            {
                exprLiteral.Value = true;
                return exprLiteral;
            }
            if (match(TokenTypes.TTInteger))
            {
                exprLiteral.Value = int.Parse(previous().Lexeme, DefaultNumberFormat);
                return exprLiteral;
            }
            if (match(TokenTypes.TTUnsignedInteger))
            {
                exprLiteral.Value = uint.Parse(previous().Lexeme, DefaultNumberFormat);
                return exprLiteral;
            }
            if (match(TokenTypes.TTFloat))
            {
                exprLiteral.Value = float.Parse(previous().Lexeme, DefaultNumberFormat);
                return exprLiteral;
            }
            if (match(TokenTypes.TTDouble))
            {
                exprLiteral.Value = double.Parse(previous().Lexeme, DefaultNumberFormat);
                return exprLiteral;
            }
            if (match(TokenTypes.TTString))
            {
                exprLiteral.Value = previous().Lexeme;
                return exprLiteral;
            }
            if (match(TokenTypes.LiteralNull))
            {
                exprLiteral.Value = null;
                return exprLiteral;
            }
            if (match(current(), TokenTypes.Minus) &&
                (peekMatch(1, TokenTypes.TTUnsignedInteger)
                || peekMatch(1, TokenTypes.TTInteger)
                || peekMatch(1, TokenTypes.TTFloat)
                || peekMatch(1, TokenTypes.TTDouble)))
            {
                advance();
                if (match(TokenTypes.TTInteger))
                {
                    exprLiteral.Value = int.Parse("-" + previous().Lexeme, DefaultNumberFormat);
                    return exprLiteral;
                }
                if (match(TokenTypes.TTUnsignedInteger))
                {
                    exprLiteral.Value = uint.Parse("-" + previous().Lexeme, DefaultNumberFormat);
                    return exprLiteral;
                }
                if (match(TokenTypes.TTFloat))
                {
                    exprLiteral.Value = float.Parse("-" + previous().Lexeme, DefaultNumberFormat);
                    return exprLiteral;
                }
                if (match(TokenTypes.TTDouble))
                {
                    exprLiteral.Value = double.Parse("-" + previous().Lexeme, DefaultNumberFormat);
                    return exprLiteral;
                }
                throw new Exception($"unexpected token while parsing negative {current()}");
            }
            if (match(TokenTypes.TypeRef))
            {
                return new ExprTypeReference(previous().Lexeme, previous().Loc);
            }
            if (match(TokenTypes.TTWord))
            {
                ExprIdentifier exprIdentifier = new ExprIdentifier(previous().Lexeme, previous().Loc);
                return exprIdentifier;
            }  
            if (match(TokenTypes.LParen))
            {
                var expr = ParseExpression();
                consume(TokenTypes.RParen, "expect enclosing ) in grouping");
                return new ExprGroup(expr, previous().Loc);
            }

            throw new Exception($"unexpected token in primary {current()}");
        }
    }
}
