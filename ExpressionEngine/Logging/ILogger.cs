namespace ExpressionEngine.Logging
{
    /// <summary>
    /// Minimal logging abstraction for observing the evaluation pipeline.
    /// </summary>
    public interface ILogger
    {
        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Error(string message);
    }
}
