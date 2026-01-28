using ExpressionEngine;
using ExpressionEngine.Errors;

namespace Calculator;

public partial class MainPage : ContentPage
{
    private readonly ExpressionEngine.ExpressionEngine _engine =
        new ExpressionEngine.ExpressionEngine(new ExpressionEngine.Logging.Logger());

    private string _currentInput = "0";
    private string _expression = "";
    private bool _justEvaluated = false;
    private bool _awaitingNextNumber = false;

    public MainPage()
    {
        InitializeComponent();
        UpdateDisplay();
    }

    void OnButtonClicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
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
                    else if (!Unwrap(_currentInput).Contains("."))
                    {
                        _currentInput += ".";
                    }
                    break;

                default:
                    AppendDigit(t);
                    break;
            }
        }
        catch
        {
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

        _currentInput = raw == "0"
            ? digit
            : raw + digit;
    }

    private void CommitOperator(string op)
    {
        if (_justEvaluated)
            _justEvaluated = false;

        _expression += _currentInput + NormalizeOperator(op);
        _currentInput = "";
        _awaitingNextNumber = true;
    }

    private void EvaluateExpression()
    {
        if (string.IsNullOrWhiteSpace(_expression))
            return;

        var fullExpression = _expression + _currentInput;
        var result = _engine.Evaluate(fullExpression);

        History.Text = fullExpression;
        _currentInput = result.ToString();

        _expression = "";
        _justEvaluated = true;
        _awaitingNextNumber = false;
    }

    //========================================================= Unary ops =========================================================//

    private void ToggleSign()
    {
        if (_currentInput == "0")
            return;

        if (_currentInput.StartsWith("(-") && _currentInput.EndsWith(")"))
        {
            _currentInput = _currentInput[2..^1];
            PushUnaryHistory(_currentInput);
        }
        else
        {
            var raw = Unwrap(_currentInput);
            _currentInput = $"(-{raw})";
            PushUnaryHistory(_currentInput);
        }
    }

    private void ApplyPercent()
    {
        var raw = Unwrap(_currentInput);
        PushUnaryHistory($"{raw}%");
        _currentInput = (double.Parse(raw) / 100).ToString();
        _justEvaluated = true;
    }

    private void ApplyInverse()
    {
        var raw = Unwrap(_currentInput);
        if (raw == "0")
            throw new DivideByZeroException();

        PushUnaryHistory($"1/({raw})");
        _currentInput = (1 / double.Parse(raw)).ToString();
        _justEvaluated = true;
    }

    private void PushUnaryHistory(string notation)
    {
        History.Text = _expression + notation;
    }

    //========================================================= Utils =========================================================//

    private static string Unwrap(string value)
    {
        if (value.StartsWith("(-") && value.EndsWith(")"))
            return value[2..^1];

        return value;
    }

    private static string NormalizeOperator(string op) =>
        op switch
        {
            "−" => "-",
            "×" => "*",
            "÷" => "/",
            _ => op
        };

    private void Reset(bool keepDisplay = false)
    {
        _currentInput = "0";
        _expression = "";
        _justEvaluated = false;
        _awaitingNextNumber = false;
        History.Text = "";

        if (!keepDisplay)
            UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        Display.Text = _expression + _currentInput;
    }
}
