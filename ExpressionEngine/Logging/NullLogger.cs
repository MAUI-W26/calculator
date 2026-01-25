namespace ExpressionEngine.Logging
{
    /// <summary>
    /// Logger implementation that performs no logging.
    /// Used as default to avoid null checks.
    /// </summary>
    public sealed class NullLogger : ILogger
    {
        public void Trace(string message) { }
        public void Debug(string message) { }
        public void Info(string message) { }
        public void Error(string message) { }
    }
}
