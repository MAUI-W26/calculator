using System;

namespace ExpressionEngine.Logging
{
    /// <summary>
    /// Simple console-based logger for development and debugging.
    /// </summary>
    public sealed class ConsoleLogger : ILogger
    {
        public void Trace(string message)
            => Write("TRACE", message, ConsoleColor.DarkGray);

        public void Debug(string message)
            => Write("DEBUG", message, ConsoleColor.Gray);

        public void Info(string message)
            => Write("INFO ", message, ConsoleColor.White);

        public void Error(string message)
            => Write("ERROR", message, ConsoleColor.Red);

        private static void Write(string level, string message, ConsoleColor color)
        {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[{level}] {message}");
            Console.ForegroundColor = previous;
        }
    }
}
