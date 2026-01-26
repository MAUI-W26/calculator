using ExpressionEngine.Ast;
using ExpressionEngine.Errors;
using ExpressionEngine.Lexing;
using ExpressionEngine.Logging;

namespace ExpressionEngine.Evaluation
{
    /// <summary>
    /// Evaluates an AST using recursive tree walking.
    /// </summary>
    public sealed class Evaluator
    {
        private readonly ILogger _logger;

        public Evaluator(ILogger logger)
        {
            _logger = logger;
        }

        public double Evaluate(ExpressionNode node)
        {
            _logger.Trace($"Evaluating node {node.GetType().Name}");

            return node switch
            {
                LiteralExpression lit => lit.Value,
                UnaryExpression unary => EvaluateUnary(unary),
                BinaryExpression binary => EvaluateBinary(binary),
                _ => throw new EvaluationException("Unknown expression node")
            };
        }

        private double EvaluateUnary(UnaryExpression expr)
        {
            _logger.Debug($"Evaluating unary {expr.Operator}");

            double value = Evaluate(expr.Operand);

            return expr.Operator switch
            {
                TokenType.MINUS => -value,
                TokenType.PLUS => value,
                _ => throw new EvaluationException("Invalid unary operator")
            };
        }

        private double EvaluateBinary(BinaryExpression expr)
        {
            _logger.Debug($"Evaluating binary {expr.Operator}");

            double left = Evaluate(expr.Left);
            double right = Evaluate(expr.Right);

            return expr.Operator switch
            {
                TokenType.PLUS => left + right,
                TokenType.MINUS => left - right,
                TokenType.MULTIPLY => left * right,
                TokenType.DIVIDE => right == 0
                    ? throw new EvaluationException("Division by zero")
                    : left / right,
                _ => throw new EvaluationException("Invalid binary operator")
            };
        }
    }
}
