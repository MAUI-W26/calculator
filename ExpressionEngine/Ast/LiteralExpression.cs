namespace ExpressionEngine.Ast
{
    /// <summary>
    /// Represents a numeric literal.
    /// </summary>
    public sealed class LiteralExpression : ExpressionNode
    {
        public double Value { get; }

        public LiteralExpression(double value)
        {
            Value = value;
        }
    }
}
