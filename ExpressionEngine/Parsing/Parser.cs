namespace ExpressionEngine.Parsing
{
    using Ast;
    using Errors;
    using Logging;
    using Lexing;
    using System.Collections.Generic;

    /// <summary>
    /// Recursive-descent parser that builds an AST from tokens.
    /// Implements operator precedence through grammar structure.
    /// </summary>
    public sealed class Parser
    {
        private readonly List<Token> _tokens;
        private readonly ILogger _logger;
        private int _current;

        public Parser(List<Token> tokens, ILogger logger)
        {
            _tokens = tokens;
            _logger = logger;
        }

        public ExpressionNode Parse()
        {
            _logger.Info("Parsing started");

            var expression = ParseExpression();
            Expect(TokenType.EOF);

            _logger.Info("Parsing completed");
            return expression;
        }

        // expr → term ((+ | -) term)*
        private ExpressionNode ParseExpression()
        {
            _logger.Trace("Enter <expr>");

            var node = ParseTerm();

            while (Match(TokenType.PLUS, TokenType.MINUS))
            {
                var op = Previous();
                _logger.Debug($"Parsed additive operator {op.Type}");

                var right = ParseTerm();
                node = new BinaryExpression(op.Type, node, right);
            }

            _logger.Trace("Exit <expr>");
            return node;
        }

        // term → factor ((* | /) factor)*
        private ExpressionNode ParseTerm()
        {
            _logger.Trace("Enter <term>");

            var node = ParseFactor();

            while (Match(TokenType.MULTIPLY, TokenType.DIVIDE))
            {
                var op = Previous();
                _logger.Debug($"Parsed multiplicative operator {op.Type}");

                var right = ParseFactor();
                node = new BinaryExpression(op.Type, node, right);
            }

            _logger.Trace("Exit <term>");
            return node;
        }

        // factor → NUMBER | "(" expr ")" | unary
        private ExpressionNode ParseFactor()
        {
            _logger.Trace("Enter <factor>");

            if (Match(TokenType.NUMBER))
            {
                var value = Previous().NumericValue;
                _logger.Debug($"Parsed literal {value}");
                return new LiteralExpression(value);
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                _logger.Debug("Parsed '(' starting sub-expression");

                var expr = ParseExpression();
                Expect(TokenType.RIGHT_PAREN);

                _logger.Debug("Parsed ')' closing sub-expression");
                return expr;
            }

            return ParseUnary();
        }

        // unary → (+ | -) factor
        private ExpressionNode ParseUnary()
        {
            if (Match(TokenType.PLUS, TokenType.MINUS))
            {
                var op = Previous();
                _logger.Debug($"Parsed unary operator {op.Type}");

                var operand = ParseUnary();
                return new UnaryExpression(op.Type, operand);
            }

            throw new ParseException("Expected expression", Peek().Position);
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private void Expect(TokenType type)
        {
            if (!Check(type))
                throw new ParseException($"Expected {type}", Peek().Position);

            Advance();
        }

        private bool Check(TokenType type) => Peek().Type == type;
        private Token Advance() => _tokens[_current++];
        private Token Peek() => _tokens[_current];
        private Token Previous() => _tokens[_current - 1];
    }
}
