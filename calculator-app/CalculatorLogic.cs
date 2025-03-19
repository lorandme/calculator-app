using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace calculator_app
{
    public class CalculatorLogic
    {
        private double _currentValue = 0;
        private double _storedValue = 0;
        private string _currentOperator = "";
        private bool _isNewEntry = true;
        private bool _hasDecimal = false;
        private MemoryLogic _memory = new MemoryLogic();

        public MemoryLogic Memory => _memory;

        public string InputNumber(string number, string currentDisplay)
        {
            if (_isNewEntry || currentDisplay == "0")
            {
                _isNewEntry = false;
                return number == "." ? "0." : number;
            }

            if (number == "." && _hasDecimal) return currentDisplay;
            if (number == ".") _hasDecimal = true;

            return currentDisplay + number;
        }

        public string InputOperator(string operatorSymbol, string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out _currentValue))
            {
                if (!string.IsNullOrEmpty(_currentOperator))
                {
                    _storedValue = PerformCalculation(_storedValue, _currentValue, _currentOperator);
                }
                else
                {
                    _storedValue = _currentValue;
                }
            }
            _currentOperator = operatorSymbol;
            _isNewEntry = true;
            _hasDecimal = false;
            return _storedValue.ToString(CultureInfo.InvariantCulture);
        }

        public string CalculateResult(string currentDisplay)
        {
            if (!double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out _currentValue))
                return currentDisplay;

            _storedValue = PerformCalculation(_storedValue, _currentValue, _currentOperator);
            _currentOperator = "";
            _isNewEntry = true;
            _hasDecimal = false;
            return _storedValue.ToString(CultureInfo.InvariantCulture);
        }

        private double PerformCalculation(double left, double right, string operation)
        {
            return operation switch
            {
                "+" => left + right,
                "-" => left - right,
                "*" => left * right,
                "/" => right != 0 ? left / right : double.NaN,
                "=" => right,
                _ => right
            };
        }

        public string ToggleSign(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                return (-value).ToString(CultureInfo.InvariantCulture);
            }
            return currentDisplay;
        }

        public string Clear()
        {
            _currentValue = 0;
            _storedValue = 0;
            _currentOperator = "";
            _isNewEntry = true;
            _hasDecimal = false;
            return "0";
        }

        public string ClearEntry()
        {
            _isNewEntry = true;
            _hasDecimal = false;
            return "0";
        }

        public string Backspace(string currentDisplay)
        {
            if (currentDisplay == "Error" || currentDisplay == "NaN" || currentDisplay == "-NaN")
            {
                return "0";
            }

            if (currentDisplay.Length > 1)
            {
                string newDisplay = currentDisplay.Substring(0, currentDisplay.Length - 1);

                // If we removed the decimal point, update the hasDecimal flag
                if (currentDisplay.EndsWith("."))
                {
                    _hasDecimal = false;
                }

                return newDisplay;
            }

            return "0";
        }

        public string Invert(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) && value != 0)
            {
                return (1 / value).ToString(CultureInfo.InvariantCulture);
            }
            return "Error";
        }

        public string Square(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                return (value * value).ToString(CultureInfo.InvariantCulture);
            }
            return currentDisplay;
        }

        public string SquareRoot(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) && value >= 0)
            {
                return Math.Sqrt(value).ToString(CultureInfo.InvariantCulture);
            }
            return "Error";
        }

        public string Percentage(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                return (value / 100).ToString(CultureInfo.InvariantCulture);
            }
            return currentDisplay;
        }

        // Memory operations
        public string MemoryClear()
        {
            _memory.ClearMemory();
            return "0";
        }

        public string MemoryAdd(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                _memory.AddToMemory(value);
            }
            return currentDisplay;
        }

        public string MemorySubtract(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                _memory.SubtractFromMemory(value);
            }
            return currentDisplay;
        }

        public string MemoryStore(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                _memory.StoreInMemory(value);
            }
            return currentDisplay;
        }

        public string MemoryRecall()
        {
            _isNewEntry = true;
            _hasDecimal = false;
            return _memory.RecallMemory().ToString(CultureInfo.InvariantCulture);
        }

        public List<double> GetMemoryStack()
        {
            return _memory.GetMemoryStack();
        }
    }
}