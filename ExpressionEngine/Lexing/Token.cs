namespace ExpressionEngine.Lexing
{
    /// <summary>
    /// Represents a single lexical unit produced by the lexer.
    /// </summary>
    public sealed class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public double NumericValue { get; }
        public int Position { get; }

        public Token(TokenType type, string lexeme, double numericValue, int position)
        {
            Type = type;
            Lexeme = lexeme;
            NumericValue = numericValue;
            Position = position;
        }
    }
}

