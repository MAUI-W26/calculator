namespace ExpressionEngine.Errors
{
    public sealed class ParseException : Exception
    {
        public ParseException(string message, int position)
            : base($"{message} at position {position}") { }
    }
}
