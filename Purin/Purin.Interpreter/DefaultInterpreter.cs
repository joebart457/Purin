using Purin.Interpreter.Interfaces;
using Purin.Interpreter.Models;
using Purin.Interpreter.Models.Exceptions;
using Purin.Parser.Interfaces;
using Purin.Parser.Models.Enums;
using Purin.Parser.Models.Expressions;
using Purin.Parser.Models.Statements;
using Purin.Runtime;
using Purin.Runtime.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Environment = Purin.Runtime.Models.Environment;

namespace Purin.Interpreter
{
    public class DefaultInterpreter: IInterpreter
    {
        private RuntimeContext _context;
        private BaseExpression? _entry = null;

        public Environment Environment
        {
            get { return _context.Environment; }
            set { _context.Environment = value; }
        }
        public DefaultInterpreter(RuntimeContext? context = null)
        {
            _context = context?? new RuntimeContext();
        }

        public ISubRoutine? GetEntry()
        {
            if (_entry == null) return null;
            var entry = Accept(_entry);
            if (entry is ISubRoutine subRoutine) return subRoutine;
            else {
                throw new PurinRuntimeException("unable to convert entry object to type ISubRoutine");
            }
        }

        public object? Accept(BaseExpression expression)
        {
            return expression.Visit(this);
        }

        public object? Accept(ExprNullableSwitch nullableSwitch)
        {
            return Accept(nullableSwitch.Lhs) ?? Accept(nullableSwitch.Rhs);
        }

        public object? Accept(ExprUnary exprUnary)
        {
            throw new NotImplementedException();
        }

        public object? Accept(ExprAssignment exprAssignment)
        {
            var lhs = Accept(exprAssignment.Lhs);
            var value = Accept(exprAssignment.Value);

            if (lhs is ExprIdentifier identifier)
            {
                _context.Environment.Set(identifier.Symbol, value);
                return value;
            }
            if (lhs is ExprGet get)
            {
                if (get.RetrievalType == RetrievalType.Property)
                {
                    var target = Accept(get.Lhs);
                    if (target == null) throw new PurinRuntimeException($"unable to assign property {get.Name} of null value");
                    var property = target.GetType().GetProperty(get.Name);
                    if (property == null) throw new PurinRuntimeException($"property {get.Name} does not exist on object {target}");
                    property.SetValue(target, value);
                    return value;
                }
                else if (get.RetrievalType == RetrievalType.Field)
                {
                    var target = Accept(get.Lhs);
                    if (target == null) throw new PurinRuntimeException($"unable to assign field {get.Name} of null value");
                    var field = target.GetType().GetField(get.Name);
                    if (field == null) throw new PurinRuntimeException($"field {get.Name} does not exist on object {target}");
                    field.SetValue(target, value);
                    return value;
                }
                else throw new PurinRuntimeException($"invalid assignment target");
            }
            throw new PurinRuntimeException($"assignment not supported for expression {exprAssignment.Lhs.Label}");
        }

        public object? Accept(ExprBinary exprBinary)
        {
            throw new NotImplementedException();
        }

        public object? Accept(ExprInitializer exprInitializer)
        {
            var type = Accept(exprInitializer.Type);
            if (type is Type ty)
            {
                var args = exprInitializer.Arguments.Select(arg => Accept(arg)).ToArray();
                var ctor = FindConstructorHelper(ty, args);
                if (ctor == null) throw new PurinRuntimeException($"unable to resolve ctor of type {ty.Name}");
                return ctor.Invoke(args);
            }
            else throw new PurinRuntimeException($"expected typeinfo but got {type?.GetType().Name}");
        }

        public object? Accept(ExprCall call)
        {
            var args = call.Arguments.Select(x => Accept(x)).ToArray();
            if (call.Callee is ExprGet get && get.RetrievalType == Parser.Models.Enums.RetrievalType.Method)
            {
                var lhs = Accept(get.Lhs);
                if (lhs == null) throw new PurinRuntimeException("unable to perform call on null object");
                var methodInfo = FindMethodHelper(get.Name, lhs.GetType(), args);
                if (methodInfo == null) throw new PurinRuntimeException($"method {get.Name} not found for type {lhs.GetType().Name}");
                if (methodInfo.IsStatic) return methodInfo.Invoke(lhs.GetType(), args);
                return methodInfo.Invoke(lhs, args);
            }

            var callee = Accept(call.Callee);
            if (callee == null) throw new PurinRuntimeException("unable to perform call on null object");
            if (callee is MethodInfo method)
            {
                if (method.IsStatic) return method.Invoke(callee.GetType(), args);
                return method.Invoke(callee, args);
            }
            else throw new PurinRuntimeException($"expected methodinfo or ICallable but got object of type {callee.GetType()}");
        }

        private MethodInfo? FindMethodHelper(string methodName, Type type, object?[] args)
        {
            try
            {
                return type.GetMethod(methodName);
            }
            catch (AmbiguousMatchException)
            {
                var types = new Type[args.Length];

                return type.GetMethod(methodName, args.Select(arg => arg?.GetType() ?? throw new PurinRuntimeException($"call to {methodName} on type {type.Name} is ambiguous")).ToArray());
            }          
        }

        private ConstructorInfo? FindConstructorHelper(Type type, object?[] args)
        {
            var types = new Type[args.Length];

            return type.GetConstructor(args.Select(arg => arg?.GetType() ?? throw new PurinRuntimeException($"call to ctor of type {type.Name} is ambiguous")).ToArray());
        }

        public object? Accept(ExprGet get)
        {
            var lhs = Accept(get.Lhs);
            if (lhs == null) throw new PurinRuntimeException($"unable to access {get.Name} of null");
            var type = lhs.GetType();
            if (lhs is Type ty) type = ty;
            if (get.RetrievalType == Parser.Models.Enums.RetrievalType.Property)
            {
                var property = type.GetProperty(get.Name);
                if (property == null) throw new PurinRuntimeException($"unable to retrieve property {get.Name} of object {lhs}");
                return property.GetValue(lhs);
            }
            if (get.RetrievalType == Parser.Models.Enums.RetrievalType.Field)
            {
                var field = type.GetField(get.Name);
                if (field == null) throw new PurinRuntimeException($"unable to retrieve field {get.Name} of object {lhs}");
                return field.GetValue(lhs);
            }
            // This will also be handled separately in Accept(ExprCall)
            if (get.RetrievalType == Parser.Models.Enums.RetrievalType.Method)
            {
                var member = type.GetMethod(get.Name);
                if (member == null) throw new PurinRuntimeException($"object {lhs} has no defined method {get.Name}");
                return member;
            }

            throw new PurinRuntimeException("unexpected error has occurred in the runtime");
        }

        public object? Accept(ExprTypeReference exprTypeReference)
        {
            return _context.GetType(exprTypeReference.Path);
        }

        public object? Accept(ExprIdentifier identifier)
        {
            return _context.Environment.Get(identifier.Symbol);
        }

        public object? Accept(ExprGroup exprGroup)
        {
            return Accept(exprGroup.Expression);
        }

        public object? Accept(ExprGeneric exprGeneric)
        {
            var lhs = Accept(exprGeneric.Lhs);
            var types = exprGeneric.TypeArguments.Select(arg =>
            {
                var val = Accept(arg);
                if (val is Type t) return t;
                throw new PurinRuntimeException("unable to resolve type argument to type in generic");
            });
            if (lhs == null) throw new PurinRuntimeException($"unable to genericize null value");
            if (lhs is Type ty)
            {
                return ty.MakeGenericType(types.ToArray());
            }
            if (lhs is MethodInfo methodInfo)
            {
                return methodInfo.MakeGenericMethod(types.ToArray());
            }
            throw new PurinRuntimeException($"generics only allowed for types and methods");
        }

        public object? Accept(ExprLiteral literal)
        {
            return literal.Value;
        }

        public void Accept(BaseStatement statement)
        {
           statement.Visit(this);
        }

        public void Accept(StmtLib load)
        {
            _context.RegisterAssembly(load.Path);
        }

        public void Accept(StmtVariableDeclaration stmtVariableDeclaration)
        {
            var value = Accept(stmtVariableDeclaration.Value);
            _context.Environment.Define(stmtVariableDeclaration.Target, value);
        }

        public void Accept(StmtBlock stmtBlock)
        {
            var previous = _context.Environment;
            _context.Environment = new Environment(previous);
            Exception? err = null;
            try
            {
                foreach(var stmt in stmtBlock.Statements)
                {
                    Accept(stmt);
                }
            }
            catch (Exception ex)
            {
                err = ex;
            }
            _context.Environment = previous;
            if (err != null) throw err;
        }

        public void Accept(StmtEntry stmtEntry)
        {
            _entry = stmtEntry.Target;
        }

        public void Accept(StmtUse stmtUse)
        {
            // Used by the compiler,
            // does nothing in runtime
        }

        public void Accept(StmtProvideLib stmtProvideLib)
        {
            // Used by the compiler,
            // does nothing in runtime
        }

        public void Accept(StmtExpression stmtExpression)
        {
            Accept(stmtExpression.Expression);
        }

        public void Accept(StmtWhile stmtWhile)
        {
            while (true)
            {
                var condition = Accept(stmtWhile.Condition);
                if (condition is bool c)
                {
                    if (c) {
                        try
                        {
                            Accept(stmtWhile.Then);
                        }
                        catch (BreakException)
                        {
                            break;
                        }
                        catch (ContinueException)
                        {
                            continue;
                        }
                    } 
                    else break;
                }
                else throw new PurinRuntimeException("expect condition to evaluate to boolean value");
            }
        }

        public void Accept(StmtIfElse stmtIfElse)
        {
            var condition = Accept(stmtIfElse.Condition);
            if (condition is bool c)
            {
                if (c) Accept(stmtIfElse.Then);
                else if (stmtIfElse.Else != null) Accept(stmtIfElse.Else);
            }
            else throw new PurinRuntimeException("expect condition to evaluate to boolean value");
        }

        public void Accept(StmtSubRoutine stmtSubroutine)
        {
            var parameters = stmtSubroutine.Parameters.Select(param =>
            {
                var tyVal = Accept(param.TypeName);
                if (tyVal is Type type)
                {
                    object? value = null;
                    if (param.Value != null) value = Accept(param.Value);
                    return new SubRoutine.Parameter(type, param.Name, param.Value != null,  value);
                }
                throw new PurinRuntimeException($"expected parameter type but got {tyVal?.GetType()}"); 
            });
            _context.Environment.Define(stmtSubroutine.Name, new SubRoutine(stmtSubroutine.Name, parameters.ToList(), stmtSubroutine.Statements));
        }

        public void Accept(StmtBreak stmtBreak)
        {
            throw new BreakException();
        }

        public void Accept(StmtContinue stmtContinue)
        {
            throw new ContinueException();
        }

        public void Accept(StmtCallSubRoutine stmtCallSubRoutine)
        {
            var value = _context.Environment.Get(stmtCallSubRoutine.Callee);
            if (value is ISubRoutine sub)
            {
                var args = stmtCallSubRoutine.Arguments.Select(arg => Accept(arg)).ToArray();
                sub.Call(this, args);
                return;
            }
            throw new PurinRuntimeException($"expect ISubRoutine but got object {value}");
        }
    }
}
