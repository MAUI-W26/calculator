using ExpressionEngine.Logging;
using ExpressionEngine.Lexing;
using ExpressionEngine.Parsing;
using ExpressionEngine.Evaluation;

namespace ExpressionEngine
{
    public sealed class ExpressionEngine
    {
        private readonly ILogger _logger;

        public ExpressionEngine(ILogger? logger = null)
        {
            _logger = logger ?? new NullLogger();
        }

        public double Evaluate(string input)
        {
            _logger.Info("Starting expression evaluation");
            _logger.Debug($"Input: {input}");

            var lexer = new Lexer(input, _logger);
            var tokens = lexer.Tokenize();

            var parser = new Parser(tokens, _logger);
            var ast = parser.Parse();

            var evaluator = new Evaluator(_logger);
            var result = evaluator.Evaluate(ast);

            _logger.Info($"Evaluation result: {result}");
            return result;
        }
    }
}
