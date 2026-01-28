using ExpressionEngine;
using ExpressionEngine.Errors;

namespace Calculator;

public partial class MainPage : ContentPage
{
    private readonly ExpressionEngine.ExpressionEngine _engine = new();

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
                    _currentInput = (double.Parse(_currentInput) / 100).ToString();
                    break;

                case "1/x":
                    if (_currentInput == "0")
                        throw new DivideByZeroException();

                    _currentInput = (1 / double.Parse(_currentInput)).ToString();
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
                    else if (!_currentInput.Contains("."))
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

    //========================================================= Helpers =========================================================//

    private void AppendDigit(string digit)
    {
        if (_justEvaluated)
        {
            _expression = "";
            _justEvaluated = false;
        }

        if (_awaitingNextNumber)
        {
            _currentInput = digit;
            _awaitingNextNumber = false;
            return;
        }

        _currentInput = _currentInput == "0"
            ? digit
            : _currentInput + digit;
    }

    private void CommitOperator(string op)
    {
        if (_justEvaluated)
            _justEvaluated = false;

        _expression += _currentInput + NormalizeOperator(op);
        History.Text = _expression;

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

    private void ToggleSign()
    {
        if (_currentInput == "0")
            return;

        _currentInput = _currentInput.StartsWith("-")
            ? _currentInput[1..]
            : "-" + _currentInput;
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
        Display.Text = _currentInput;
    }
}
