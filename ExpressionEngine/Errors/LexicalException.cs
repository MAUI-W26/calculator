namespace ExpressionEngine.Errors
{
    public sealed class LexicalException : Exception
    {
        public LexicalException(string message, int position)
            : base($"{message} at position {position}") { }
    }
}
