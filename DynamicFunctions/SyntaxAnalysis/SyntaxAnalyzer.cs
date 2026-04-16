using DynamicFunctions.LexicalAnalysis.LexicalTokens;
using DynamicFunctions.SyntaxAnalysis.Exceptions;
using DynamicFunctions.SyntaxAnalysis.SyntaxNodes;

namespace DynamicFunctions.SyntaxAnalysis;

public sealed class SyntaxAnalyzer : ISyntaxAnalyzer
{
    public ISyntaxNode Analyze(IEnumerable<ILexicalToken> tokens)
    {
        var operands = new Stack<ISyntaxNode>();
        var operators = new Stack<OperatorToken>();

        foreach (var token in tokens)
        {
            // TODO: Implement as strategy
            switch (token)
            {
                case EmptyToken:
                    break;

                case NumberToken number:
                    operands.Push(new NumberNode(number));
                    break;

                case VariableToken variable:
                    operands.Push(new VariableNode(variable));
                    break;

                case FunctionToken function:
                    operands.Push(BuildFunctionCallNode(function));
                    break;

                case GroupToken group:
                    operands.Push(Analyze(group.InnerTokens));
                    break;

                case OperatorToken op:
                    while (operators.Count > 0 && ShouldPop(operators.Peek(), op))
                        PopAndBuild(operands, operators);
                    operators.Push(op);
                    break;

                default:
                    throw new InvalidOperationException($"Unexpected token type: {token.GetType().Name}");
            }
        }

        while (operators.Count > 0)
            PopAndBuild(operands, operators);

        if (operands.Count != 1)
            throw new InvalidOperationException("Invalid expression: expected a single root node.");

        return operands.Pop();
    }

    private ISyntaxNode BuildFunctionCallNode(FunctionToken function)
    {
        var arguments = function.Arguments
            .Cast<IGroupToken>()
            .Select(arg => Analyze(arg.InnerTokens))
            .ToList();

        return new FunctionCallNode(function, arguments);
    }

    // Determines whether the operator on top of the stack should be popped
    // before pushing the current operator. Pow is right-associative, so equal
    // precedence on the stack does NOT trigger a pop.
    private static bool ShouldPop(OperatorToken top, OperatorToken current)
    {
        if (current is PowOperatorToken && top is PowOperatorToken)
            return false;

        // Lower Priority number = higher precedence. Pop when top has
        // higher or equal precedence (i.e. its Priority <= current Priority).
        return top.Priority <= current.Priority;
    }

    private static void PopAndBuild(Stack<ISyntaxNode> operands, Stack<OperatorToken> operators)
    {
        if (operands.Count <= 1 || operators.Count == 0)
        {
            throw new UnexpectedTokensSequenceException();
        }
        
        var op = operators.Pop();
        var right = operands.Pop();
        var left = operands.Pop();
        operands.Push(new BinaryOperatorNode(op, left, right));
    }
}
