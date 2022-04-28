
using Monkey.AST;

namespace Monkey.Evaluation;

public class Evaluator
{
    static Null NULL = new Null { };
    static Boolean TRUE = new Boolean { Value = true };
    static Boolean FALSE = new Boolean { Value = false };

    public IObject Eval(AST.Node? node, Environment env)
    {
        switch (node)
        {
            /* Statements */

            case AST.Program p:
                return EvalProgram(p, env);

            case AST.ExpressionStatement es:
                return Eval(es.Expression, env);

            case AST.ReturnStatement rs:
                var val = Eval(rs.ReturnValue, env);
                if (IsError(val))
                {
                    return val;
                }
                return new ReturnValue { Value = val };
            case AST.LetStatement ls:
                val = Eval(ls.Value, env);
                if (IsError(val))
                {
                    return val;
                }
                return env.Set(ls.Name.Value, val); // correct to return?

            /* Expressions */

            case AST.Identifier id:
                return EvalIdentifier(id, env);

            case AST.IntegerLiteral il:
                return new Integer { Value = il.Value };

            case AST.BooleanLiteral bi:
                return bi.Value ? TRUE : FALSE;

            case AST.PrefixExpression pe:
                var right = Eval(pe.Right, env);
                if (IsError(right))
                {
                    return right;
                }
                return EvalPrefixExpression(pe.Operator, right);

            case AST.InfixExpression ie:
                var left = Eval(ie.Left, env);
                if (IsError(left))
                {
                    return left;
                }
                right = Eval(ie.Right, env);
                if (IsError(right))
                {
                    return right;
                }
                return EvalInfixExpression(ie.Operator, left, right);

            case AST.BlockStatement bs:
                return EvalBlockStatement(bs, env);

            case AST.IfExpression ie:
                return EvalIfExpression(ie, env);


            /* */
            default:
                return NULL;
        }
    }


    private IObject EvalIdentifier(Identifier id, Environment env)
    {
        if (env.TryGet(id.Value, out IObject? value))
        {
            return value!;
        }
        return NewError($"identifier not found: {id.Value}");
    }

    private IObject EvalBlockStatement(BlockStatement block, Environment env)
    {
        IObject result = NULL;

        foreach(var stmt in block.Statements)
        {
            result = Eval(stmt, env);

            if (result is ReturnValue)
            {
                return result;
            }

            if (result is Error)
            {
                return result;
            }
        }

        return result;
    }

    private IObject EvalProgram(AST.Program p, Environment env)
    {
        IObject result = NULL;

        foreach(var stmt in p.Statements)
        {
            result = Eval(stmt, env);

            if (result is ReturnValue)
            {
                return ((ReturnValue)result).Value;
            }

            if (result is Error)
            {
                return result;
            }
        }

        return result;
    }

    private IObject EvalIfExpression(IfExpression expr, Environment env)
    {
        var condition = Eval(expr.Condition, env);

        if (IsError(condition))
        {
            return condition;
        }

        if (IsTruthy(condition))
        {
            return Eval(expr.Consequence, env);
        }
        else if (expr.Alternative != null)
        {
            return Eval(expr.Alternative, env);
        }
        else
        {
            return NULL;
        }
    }

    private IObject EvalInfixExpression(string @operator, IObject left, IObject right)
    {
        switch (left.ObjectType, right.ObjectType)
        {
            case (ObjectType.INTEGER, ObjectType.INTEGER):
                return EvalIntegerInfixExpression(@operator, left, right);

            default:
                if (left.ObjectType != right.ObjectType)
                {
                    return NewError($"type missmatch: {left.ObjectType} {@operator} {right.ObjectType}");
                }

                switch (@operator)
                {
                    case "==":
                        return NativeBoolToBooleanObject(left == right);
                    case "!=":
                        return NativeBoolToBooleanObject(left != right);
                }
                return NewError($"unknown operator: {left.ObjectType} {@operator} {right.ObjectType}");
        }
    }

    private IObject EvalIntegerInfixExpression(string @operator, IObject left, IObject right)
    {
        var leftValue = ((Integer)left).Value;
        var rightValue = ((Integer)right).Value;

        switch (@operator)
        {
            case "+":
                return new Integer { Value = leftValue + rightValue };
            case "-":
                return new Integer { Value = leftValue - rightValue };
            case "*":
                return new Integer { Value = leftValue * rightValue };
            case "/":
                return new Integer { Value = leftValue / rightValue };
            case "<":
                return NativeBoolToBooleanObject(leftValue < rightValue);
            case ">":
                return NativeBoolToBooleanObject(leftValue > rightValue);
            case "==":
                return NativeBoolToBooleanObject(leftValue == rightValue);
            case "!=":
                return NativeBoolToBooleanObject(leftValue != rightValue);
            default:
                return NewError($"unknown operator: {left.ObjectType} {@operator} {right.ObjectType}");
        }
    }

    private IObject EvalPrefixExpression(string @operator, IObject right)
    {
        switch (@operator)
        {
            case "!":
                return EvalBangOperatorExpression(right);
            case "-":
                return EvalMinusPrefixOperatorExpression(right);
            default:
                return NewError($"Unknown operator: {@operator}{right.ObjectType}");
        }
    }

    private IObject EvalMinusPrefixOperatorExpression(IObject right)
    {
        if (right.ObjectType != ObjectType.INTEGER)
        {
            return NewError($"unknown operator: -{right.ObjectType}");
        }

        var value = ((Integer)right).Value;
        return new Integer { Value = value * -1 };
    }

    private IObject EvalBangOperatorExpression(IObject right)
    {
        if (right == TRUE) return FALSE;
        if (right == FALSE) return TRUE;
        if (right == NULL) return TRUE;
        return FALSE;
    }

    private IObject NativeBoolToBooleanObject(bool b)
    {
        return b ? TRUE : FALSE;
    }

    private bool IsTruthy(IObject condition)
    {
        if (condition == NULL) return false;
        if (condition == TRUE) return true;
        if (condition == FALSE) return false;
        return false;
    }

    private Error NewError(string message)
    {
        return new Error
        {
            Message = message
        };
    }

    private bool IsError(IObject o)
    {
        return o != null && o.ObjectType == ObjectType.ERROR;
    }
}