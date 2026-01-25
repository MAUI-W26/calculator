namespace ExpressionEngine.Ast
{
    using Lexing;

    /// <summary>
    /// Represents a binary operation.
    /// </summary>
    public sealed class BinaryExpression : ExpressionNode
    {
        public TokenType Operator { get; }
        public ExpressionNode Left { get; }
        public ExpressionNode Right { get; }

        public BinaryExpression(TokenType op, ExpressionNode left, ExpressionNode right)
        {
            Operator = op;
            Left = left;
            Right = right;
        }
    }
}

