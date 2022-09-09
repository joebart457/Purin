using Purin.Parser.Models.Expressions;
using Purin.Parser.Models.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.Parser.Interfaces
{
    public interface IInterpreter
    {
        object? Accept(ExprNullableSwitch nullableSwitch);
        object? Accept(ExprUnary exprUnary);
        object? Accept(ExprBinary exprBinary);
        object? Accept(ExprInitializer exprInitializer);
        object? Accept(ExprCall call);
        object? Accept(ExprGet get);
        object? Accept(ExprTypeReference exprTypeReference);
        object? Accept(ExprIdentifier identifier);
        object? Accept(ExprAssignment exprAssignment);
        object? Accept(ExprGroup exprGroup);
        object? Accept(ExprGeneric exprGeneric);
        object? Accept(ExprLiteral literal);
        void Accept(StmtLib load);
        void Accept(StmtVariableDeclaration set);
        void Accept(StmtBlock stmtBlock);
        void Accept(StmtEntry stmtEntry);
        void Accept(StmtUse stmtUse);
        void Accept(StmtProvideLib stmtProvideLib);
        void Accept(StmtExpression expression);
        void Accept(StmtWhile stmtWhile);
        void Accept(StmtIfElse stmtIfElse);
        void Accept(StmtSubRoutine stmtFunction);
        void Accept(StmtBreak stmtBreak);
        void Accept(StmtContinue stmtContinue);
        void Accept(StmtCallSubRoutine stmtCallSubRoutine);
    }
}
