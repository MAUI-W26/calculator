using ExpressionEngine;
using ExpressionEngine.Errors;

namespace Calculator;

public partial class MainPage : ContentPage
{
    private readonly ExpressionEngine.Logging.Logger _logger;
    private readonly ExpressionEngine.ExpressionEngine _engine;

    private string _currentInput = "0";
    private string _expression = "";
    private bool _justEvaluated = false;
    private bool _awaitingNextNumber = false;

    public MainPage()
    {
        InitializeComponent();

        _logger = new ExpressionEngine.Logging.Logger();
        _engine = new ExpressionEngine.ExpressionEngine(_logger);

        UpdateDisplay();
    }

    void OnButtonClicked(object sender, EventArgs e) // INFO: this is the only event handler for all buttons
    {
        if (sender is not Button) // safe guard -- very unlikely to happen
            return;

        var btn = (Button)sender;// downcast the sender to a Button
        var t = btn.Text;

        try
        {
            switch (t)
            {
                case "AC":
                    Reset();
                    break;

                case "±":
                    ToggleSign();
                    break;

                case "%":
                    ApplyPercent();
                    break;

                case "1/x":
                    ApplyInverse();
                    break;

                case "+":
                case "−":
                case "×":
                case "÷":
                    CommitOperator(t);
                    break;

                case "=":
                    EvaluateExpression();
                    break;

                case ".":
                    if (_awaitingNextNumber)
                    {
                        _currentInput = "0.";
                        _awaitingNextNumber = false;
                    }
                    else if (!Unwrap(_currentInput).Contains(".")) // prevent multiple decimals
                    {
                        _currentInput += ".";
                    }
                    break;

                default:
                    AppendDigit(t);
                    break;
            }
        }
        catch (LexicalException ex)
        {
            _logger.Error(ex.ToString());
            Display.Text = "Error";
            Reset(keepDisplay: true);
            return;
        }
        catch (ParseException ex)
        {
            _logger.Error(ex.ToString());
            Display.Text = "Error";
            Reset(keepDisplay: true);
            return;
        }
        catch (EvaluationException ex)
        {
            _logger.Error(ex.ToString());
            Display.Text = "Error";
            Reset(keepDisplay: true);
            return;
        }
        catch (Exception ex)
        {
            // truly unexpected bug
            _logger.Error(ex.ToString());
            Display.Text = "Error";
            Reset(keepDisplay: true);
            return;
        }

        UpdateDisplay();
    }

    //========================================================= Core =========================================================//

    private void AppendDigit(string digit)
    {
        if (_justEvaluated)
        {
            _expression = "";
            _currentInput = digit;
            _justEvaluated = false;
            return;
        }

        if (_awaitingNextNumber)
        {
            _currentInput = digit;
            _awaitingNextNumber = false;
            return;
        }

        var raw = Unwrap(_currentInput);

        _currentInput = raw == "0" // leading zero case - replace or concatenate
            ? digit
            : raw + digit;
    }

    private void CommitOperator(string op) // tells that the current number is finished and appends the operator
    {
        if (_justEvaluated)
            _justEvaluated = false;

        _expression += _currentInput + NormalizeOperator(op);
        _currentInput = "";
        _awaitingNextNumber = true;
    }

    private void EvaluateExpression()
    {
        if (string.IsNullOrWhiteSpace(_expression)) // safety check
            return;

        var fullExpression = _expression + _currentInput;
        var result = _engine.Evaluate(fullExpression);

        History.Text = fullExpression;
        _currentInput = NormalizeNumber(result); // ensure consistent internal representation

        _expression = "";
        _justEvaluated = true;
        _awaitingNextNumber = false;
    }

    //========================================================= Unary ops =========================================================//

    private void ToggleSign() // toggles the sign of the current input eg: 1 -> (-1), (-1) -> 1
    {
        if (_currentInput == "0")
            return;

        if (_currentInput.StartsWith("(-") && _currentInput.EndsWith(")"))
        {
            _currentInput = _currentInput[2..^1]; // between the parentheses (second char to second last char)
        }
        else // apply negative sign with parentheses
        {
            _currentInput = $"(-{_currentInput})";
        }

        PushUnaryHistory(_currentInput);
    }

    private void ApplyPercent()
    {
        var raw = Unwrap(_currentInput);
        PushUnaryHistory($"{raw}%");
        _currentInput = NormalizeNumber(double.Parse(raw) / 100);
        _justEvaluated = true;
    }

    private void ApplyInverse()
    {
        var raw = Unwrap(_currentInput);
        if (raw == "0")
            throw new DivideByZeroException();

        PushUnaryHistory($"1/({raw})");
        _currentInput = NormalizeNumber(1 / double.Parse(raw));
        _justEvaluated = true;
    }

    private void PushUnaryHistory(string notation)
    {
        History.Text = _expression + notation;
    }

    //========================================================= Utils =========================================================//

    private static string Unwrap(string value) // parentheses used to denote negative numbers
    {
        if (value.StartsWith("(-") && value.EndsWith(")"))
            return value[2..^1]; // between the parentheses (second char to second last char)

        return value;
    }

    private static string NormalizeOperator(string op) =>
        op switch // convert to standard operators
        {
            "−" => "-",
            "×" => "*",
            "÷" => "/",
            _ => op
        };

    private static string NormalizeNumber(double value) // ensures consistent internal representation of negative numbers
    {
        return value < 0
            ? $"({value})"
            : value.ToString();
    }

    private void Reset(bool keepDisplay = false) // default to resetting everything
    {
        _currentInput = "0";
        _expression = "";
        _justEvaluated = false;
        _awaitingNextNumber = false;
        History.Text = "";

        if (!keepDisplay)
            UpdateDisplay();
    }

    private void UpdateDisplay() // concatenate expression and current input for display
    {
        Display.Text = _expression + _currentInput;
    }
}
