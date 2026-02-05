using ExpressionEngine.Errors;
using ExpressionEngine.Logging;
using System.Collections.Generic;
using System.Globalization;

namespace ExpressionEngine.Lexing
{

    /// <summary>
    /// Converts raw input text into a sequence of tokens.
    /// Performs no semantic validation.
    /// </summary>
    public sealed class Lexer
    {
        private readonly string _input;
        private readonly ILogger _logger;
        private int _position;

        public Lexer(string input, ILogger logger)
        {
            _input = input;
            _logger = logger;
            _position = 0;
        }

        public List<Token> Tokenize() // NOTE: Lexeme is the raw input text; value is the parsed numeric representation
        {
            _logger.Info("Lexing started");

            var tokens = new List<Token>();

            while (!IsAtEnd())
            {
                char c = Peek();

                if (char.IsWhiteSpace(c))
                {
                    _logger.Trace($"Skipping whitespace at position {_position}");
                    Advance();
                    continue;
                }

                if (char.IsDigit(c))
                {
                    var token = ReadNumber();
                    _logger.Trace($"Tokenized NUMBER '{token.Lexeme}' at {token.Position}");
                    tokens.Add(token);
                    continue;
                }

                var op = ReadOperator();
                _logger.Trace($"Tokenized {op.Type} '{op.Lexeme}' at {op.Position}");
                tokens.Add(op);
            }

            tokens.Add(new Token(TokenType.EOF, string.Empty, 0, _position));
            _logger.Trace("Tokenized EOF");

            _logger.Info($"Lexing completed ({tokens.Count} tokens)");
            return tokens;
        }

        private Token ReadNumber()
        {
            int start = _position;

            while (!IsAtEnd() && (char.IsDigit(Peek()) || Peek() == '.'))
                Advance();

            string lexeme = _input.Substring(start, _position - start);

            //Note: culture-invariant parsing for decimal point normalization
            if (!double.TryParse(lexeme, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)) 
                throw new LexicalException($"Invalid number '{lexeme}'", start);

            return new Token(TokenType.NUMBER, lexeme, value, start);
        }

        private Token ReadOperator()
        {
            int pos = _position;
            char c = Advance();

            return c switch
            {
                '+' => new Token(TokenType.PLUS, "+", 0, pos),
                '-' => new Token(TokenType.MINUS, "-", 0, pos),
                '*' => new Token(TokenType.MULTIPLY, "*", 0, pos),
                '/' => new Token(TokenType.DIVIDE, "/", 0, pos),
                '(' => new Token(TokenType.LEFT_PAREN, "(", 0, pos),
                ')' => new Token(TokenType.RIGHT_PAREN, ")", 0, pos),
                _ => throw new LexicalException($"Unexpected character '{c}'", pos)
            };
        }

        private bool IsAtEnd() => _position >= _input.Length;
        private char Peek() => _input[_position];
        private char Advance() => _input[_position++];
    }
}
