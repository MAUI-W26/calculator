using System;
using System.Reflection;

namespace ExpressionEngine.Logging
{
    public sealed class Logger : ILogger
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
            if (TryWriteToAndroidLog(level, message))
                return;

            WriteFallback(level, message, color);
        }

        private static bool TryWriteToAndroidLog(string level, string message)
        {
            try
            {
                var logType = Type.GetType("Android.Util.Log, Mono.Android");
                if (logType == null)
                    return false;

                var debugMethod = logType.GetMethod(
                    "Debug",
                    new[] { typeof(string), typeof(string) }
                );

                debugMethod?.Invoke(
                    null,
                    new object[] { "ExpressionEngine", $"[{level}] {message}" }
                );

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void WriteFallback(string level, string message, ConsoleColor color)
        {
            try
            {
                if (HasConsole())
                {
                    var previous = Console.ForegroundColor;
                    Console.ForegroundColor = color;
                    Console.WriteLine($"[{level}] {message}");
                    Console.ForegroundColor = previous;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[{level}] {message}");
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine($"[{level}] {message}");
            }
        }

        private static bool HasConsole()
        {
            try
            {
                return Environment.UserInteractive && !Console.IsOutputRedirected;
            }
            catch
            {
                return false;
            }
        }
    }
}
