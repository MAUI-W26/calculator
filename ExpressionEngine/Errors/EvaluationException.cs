namespace ExpressionEngine.Errors
{
    public sealed class EvaluationException : Exception
    {
        public EvaluationException(string message)
            : base(message) { }
    }
}
