namespace ExpressionEngine.Docs
{
    /// <summary>
    /// ============================================================
    /// FORMAL GRAMMAR SPECIFICATION — ExpressionEngine
    /// ============================================================
    ///
    /// This document defines the formal grammar and theoretical
    /// foundations of the ExpressionEngine expression language.
    ///
    /// It is intentionally non-executable and serves as:
    /// - A language specification
    /// - A correctness reference for Parser.cs
    /// - A long-term maintenance anchor
    ///
    /// ------------------------------------------------------------
    /// LANGUAGE CLASSIFICATION
    /// ------------------------------------------------------------
    /// - Expression-only language
    /// - Deterministic
    /// - Side-effect free
    /// - Single evaluation scope
    /// - No statements or control flow
    ///
    /// ------------------------------------------------------------
    /// FORMAL GRAMMAR (EBNF)
    /// ------------------------------------------------------------
    ///
    /// expr    → term ((+ | -) term)*
    /// term    → factor ((* | /) factor)*
    /// factor  → NUMBER
    ///         | "(" expr ")"
    ///         | unary
    /// unary   → (+ | -) factor
    ///
    /// ------------------------------------------------------------
    /// GRAMMAR INTENT
    /// ------------------------------------------------------------
    /// - Operator precedence is encoded structurally:
    ///     unary   > multiplicative > additive
    ///
    /// - Binary operators are left-associative:
    ///     8 - 4 - 2  →  (8 - 4) - 2
    ///
    /// - Unary operators bind tighter than binary operators:
    ///     -3 * 2    →  (-3) * 2
    ///
    /// - Parentheses override all precedence rules
    ///
    /// ------------------------------------------------------------
    /// TOKEN ASSUMPTIONS
    /// ------------------------------------------------------------
    /// The grammar assumes the following token categories:
    ///
    /// - NUMBER        : numeric literal (floating point)
    /// - PLUS          : '+'
    /// - MINUS         : '-'
    /// - MULTIPLY      : '*'
    /// - DIVIDE        : '/'
    /// - LEFT_PAREN    : '('
    /// - RIGHT_PAREN   : ')'
    /// - EOF           : end of input marker
    ///
    /// Tokenization is performed prior to parsing and is
    /// context-independent.
    ///
    /// ------------------------------------------------------------
    /// PARSING STRATEGY
    /// ------------------------------------------------------------
    /// - Recursive-descent parsing
    /// - One parsing method per grammar rule
    /// - No backtracking
    /// - Single-token lookahead
    ///
    /// Grammar structure guarantees:
    /// - No left recursion
    /// - Deterministic parse
    /// - Linear-time complexity
    ///
    /// ------------------------------------------------------------
    /// SEMANTIC MODEL
    /// ------------------------------------------------------------
    /// - Parsing produces an Abstract Syntax Tree (AST)
    /// - No evaluation occurs during parsing
    /// - AST nodes represent semantic intent, not syntax
    ///
    /// Evaluation is performed as a separate phase by a
    /// tree-walking interpreter.
    ///
    /// ------------------------------------------------------------
    /// ERROR MODEL
    /// ------------------------------------------------------------
    /// Errors are categorized by pipeline phase:
    ///
    /// - Lexical errors:
    ///     Invalid characters, malformed numbers
    ///
    /// - Syntax errors:
    ///     Unexpected tokens, missing parentheses,
    ///     incomplete expressions
    ///
    /// - Evaluation errors:
    ///     Division by zero, invalid operator semantics
    ///
    /// Each error category is isolated and reported explicitly.
    ///
    /// ------------------------------------------------------------
    /// DESIGN CONSTRAINTS
    /// ------------------------------------------------------------
    /// - Entire input must be consumed by the grammar
    /// - Empty expressions are invalid
    /// - Implicit multiplication is not supported
    /// - Whitespace is insignificant
    ///
    /// ------------------------------------------------------------
    /// EXTENSION GUIDELINES
    /// ------------------------------------------------------------
    /// The grammar is intentionally minimal.
    ///
    /// Future extensions should:
    /// - Add new non-terminals explicitly
    /// - Preserve operator precedence clarity
    /// - Avoid ambiguity
    ///
    /// Example extensions:
    /// - Identifiers and variables
    /// - Function calls: IDENTIFIER "(" expr ")"
    /// - Exponentiation with right associativity
    ///
    /// ------------------------------------------------------------
    /// AUTHORITATIVE NOTE
    /// ------------------------------------------------------------
    /// Parser.cs is considered correct if and only if it
    /// conforms to this specification.
    ///
    /// Any divergence between Parser.cs and this grammar
    /// constitutes a defect.
    ///
    /// ============================================================
    /// END OF SPECIFICATION
    /// ============================================================
    /// </summary>
    internal static class Grammar
    {
    }
}
