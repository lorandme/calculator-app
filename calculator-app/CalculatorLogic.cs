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

        // valoarea stocata pentru operatiile succesive
        private double _storedValue = 0;

        private string _currentOperator = "";

        private bool _isNewEntry = true;

        private bool _hasDecimal = false;

        // obiect pentru gestionarea functiilor de memorie
        private MemoryLogic _memory = new MemoryLogic();

        // getter pentru memoria calculatorului
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
                    // daca exista deja un operator activ, efectueaza calculul anterior
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

        // metoda pentru calcularea rezultatului pe baza valorilor introduse si a operatorului
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

        // metoda privata pentru efectuarea calculului pe baza operatorului
        private double PerformCalculation(double left, double right, string operation)
        {
            return operation switch
            {
                "+" => left + right,
                "-" => left - right,
                "*" => left * right,
                "/" => right != 0 ? left / right : double.NaN, // evita impartirea la zero
                "=" => right,
                _ => right
            };
        }

        // metoda pentru schimbarea semnului numarului curent
        public string ToggleSign(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                return (-value).ToString(CultureInfo.InvariantCulture);
            }
            return currentDisplay;
        }

        // metoda pentru resetarea completa a calculatorului
        public string Clear()
        {
            _currentValue = 0;
            _storedValue = 0;
            _currentOperator = "";
            _isNewEntry = true;
            _hasDecimal = false;
            return "0";
        }

        // metoda pentru resetarea doar a ultimei valori introduse
        public string ClearEntry()
        {
            _isNewEntry = true;
            _hasDecimal = false;
            return "0";
        }

        // metoda pentru stergerea ultimului caracter din display
        public string Backspace(string currentDisplay)
        {
            if (currentDisplay == "Error" || currentDisplay == "NaN" || currentDisplay == "-NaN")
            {
                return "0";
            }

            if (currentDisplay.Length > 1)
            {
                string newDisplay = currentDisplay.Substring(0, currentDisplay.Length - 1);

                // daca ultimul caracter sters a fost punctul zecimal, reseteaza flag-ul
                if (currentDisplay.EndsWith("."))
                {
                    _hasDecimal = false;
                }

                return newDisplay;
            }

            return "0";
        }

        // metoda pentru inversarea numarului (1 / numar)
        public string Invert(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) && value != 0)
            {
                return (1 / value).ToString(CultureInfo.InvariantCulture);
            }
            return "Error"; // daca valoarea este 0, returneaza eroare
        }

        // metoda pentru ridicarea la patrat
        public string Square(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                return (value * value).ToString(CultureInfo.InvariantCulture);
            }
            return currentDisplay;
        }

        // metoda pentru calcularea radicalului unui numar
        public string SquareRoot(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) && value >= 0)
            {
                return Math.Sqrt(value).ToString(CultureInfo.InvariantCulture);
            }
            return "Error"; // daca valoarea este negativa, returneaza eroare
        }

        // metoda pentru calcularea procentului (imparte la 100)
        public string Percentage(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                return (value / 100).ToString(CultureInfo.InvariantCulture);
            }
            return currentDisplay;
        }

        // operatii pentru memorie

        // metoda pentru stergerea memoriei
        public string MemoryClear()
        {
            _memory.ClearMemory();
            return "0";
        }

        // metoda pentru adaugarea unei valori in memorie
        public string MemoryAdd(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                _memory.AddToMemory(value);
            }
            return currentDisplay;
        }

        // metoda pentru scaderea unei valori din memorie
        public string MemorySubtract(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                _memory.SubtractFromMemory(value);
            }
            return currentDisplay;
        }

        // metoda pentru salvarea unei valori in memorie
        public string MemoryStore(string currentDisplay)
        {
            if (double.TryParse(currentDisplay, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                _memory.StoreInMemory(value);
            }
            return currentDisplay;
        }

        // metoda pentru a obtine valoarea salvata in memorie
        public string MemoryRecall()
        {
            _isNewEntry = true;
            _hasDecimal = false;
            return _memory.RecallMemory().ToString(CultureInfo.InvariantCulture);
        }

        // metoda pentru obtinerea istoricului memoriei
        public List<double> GetMemoryStack()
        {
            return _memory.GetMemoryStack();
        }
    }
}
