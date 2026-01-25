namespace ExpressionEngine.Ast
{
    using Lexing;

    /// <summary>
    /// Represents a unary operation.
    /// </summary>
    public sealed class UnaryExpression : ExpressionNode
    {
        public TokenType Operator { get; }
        public ExpressionNode Operand { get; }

        public UnaryExpression(TokenType op, ExpressionNode operand)
        {
            Operator = op;
            Operand = operand;
        }
    }
}
