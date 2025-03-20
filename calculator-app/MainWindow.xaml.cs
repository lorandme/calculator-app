using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;

namespace calculator_app
{
    public partial class MainWindow : Window
    {
        private CalculatorLogic _calculator = new CalculatorLogic();
        private CalculatorSettings _settings = CalculatorSettings.Load();

        public MainWindow()
        {
            InitializeComponent();

            DigitGroupingMenuItem.IsChecked = _settings.DigitGroupingEnabled;

            // adauga handler-e pentru evenimente de la tastatura
            KeyDown += MainWindow_KeyDown;
        }

        private void DigitGrouping_Changed(object sender, RoutedEventArgs e)
        {
            // actualizeaza setarea pentru gruparea cifrelor si salveaza
            _settings.DigitGroupingEnabled = DigitGroupingMenuItem.IsChecked;
            _settings.Save();
            FormatDisplayIfNumber();
        }

        private void FormatDisplayIfNumber()
        {
            // verifica daca textul din display este un numar valid
            if (double.TryParse(DisplayText.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                if (_settings.DigitGroupingEnabled)
                {
                    // aplica gruparea cifrelor doar pentru partea intreaga
                    string text = DisplayText.Text;
                    bool hasDecimalPoint = text.Contains('.');

                    if (hasDecimalPoint)
                    {
                        string[] parts = text.Split('.');
                        // formateaza partea intreaga cu gruparea cifrelor
                        string integerPart = double.Parse(parts[0], CultureInfo.InvariantCulture)
                            .ToString("N0", CultureInfo.CurrentCulture);
                        // pastreaza partea zecimala nemodificata
                        DisplayText.Text = integerPart + "." + parts[1];
                    }
                    else
                    {
                        // daca nu exista punct zecimal, formateaza intregul numar
                        DisplayText.Text = value.ToString("N0", CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    // asigura ca numarul este afisat fara gruparea cifrelor
                    DisplayText.Text = value.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        
        private void About_Click(object sender, RoutedEventArgs e)
        {
            string name = "Menyhárt Loránd";
            string group = "10LF233";

            MessageBox.Show($"Calculator\n\nCreated by: {name},\n{group}",
                            "About",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            bool handled = true;

            switch (e.Key)
            {
                case Key.NumPad0:
                case Key.D0:
                    HandleNumberInput("0");
                    break;
                case Key.NumPad1:
                case Key.D1:
                    HandleNumberInput("1");
                    break;
                case Key.NumPad2:
                case Key.D2:
                    HandleNumberInput("2");
                    break;
                case Key.NumPad3:
                case Key.D3:
                    HandleNumberInput("3");
                    break;
                case Key.NumPad4:
                case Key.D4:
                    HandleNumberInput("4");
                    break;
                case Key.NumPad5:
                case Key.D5:
                    HandleNumberInput("5");
                    break;
                case Key.NumPad6:
                case Key.D6:
                    HandleNumberInput("6");
                    break;
                case Key.NumPad7:
                case Key.D7:
                    HandleNumberInput("7");
                    break;
                case Key.NumPad8:
                case Key.D8:
                    HandleNumberInput("8");
                    break;
                case Key.NumPad9:
                case Key.D9:
                    HandleNumberInput("9");
                    break;
                case Key.Decimal:
                case Key.OemPeriod:
                    HandleNumberInput(".");
                    break;
                case Key.Add:
                case Key.OemPlus:
                    HandleOperatorInput("+");
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    HandleOperatorInput("-");
                    break;
                case Key.Multiply:
                    HandleOperatorInput("*");
                    break;
                case Key.Divide:
                case Key.OemQuestion:
                    HandleOperatorInput("/");
                    break;
                case Key.Enter:
                    HandleOperatorInput("=");
                    break;
                case Key.Escape:
                    HandleClear();
                    break;
                case Key.Back:
                    HandleBackspace();
                    break;
                default:
                    handled = false;
                    break;
            }

            e.Handled = handled;
        }

        private void HandleNumberInput(string number)
        {
            // actualizeaza display-ul cu numarul introdus
            DisplayText.Text = _calculator.InputNumber(number, DisplayText.Text);

            // aplica formatarea doar daca gruparea cifrelor este activata si nu este un punct zecimal
            if (_settings.DigitGroupingEnabled && number != ".")
            {
                FormatDisplayIfNumber();
            }
        }

        private void HandleOperatorInput(string operatorSymbol)
        {
            if (operatorSymbol == "=")
            {
                // calculeaza si afiseaza rezultatul
                DisplayText.Text = _calculator.CalculateResult(DisplayText.Text);
            }
            else
            {
                // actualizeaza display-ul cu operatorul introdus
                DisplayText.Text = _calculator.InputOperator(operatorSymbol, DisplayText.Text);
            }

            // formateaza rezultatul daca gruparea cifrelor este activata
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        private void HandleClear()
        {
            // sterge totul de pe display
            DisplayText.Text = _calculator.Clear();
        }

        private void HandleBackspace()
        {
            // sterge ultima cifra de pe display
            DisplayText.Text = _calculator.Backspace(DisplayText.Text);
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            // gestioneaza click-ul pe butoanele numerice
            if (sender is Button button)
            {
                string number = button.Content.ToString();
                DisplayText.Text = _calculator.InputNumber(number, DisplayText.Text);

                // aplica formatarea doar daca gruparea cifrelor este activata si nu este un punct zecimal
                if (_settings.DigitGroupingEnabled && number != ".")
                {
                    FormatDisplayIfNumber();
                }
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            // gestioneaza click-ul pe butoanele de operatori
            if (sender is Button button)
            {
                string op = button.Content.ToString();
                if (op == "=")
                {
                    DisplayText.Text = _calculator.CalculateResult(DisplayText.Text);
                }
                else
                {
                    DisplayText.Text = _calculator.InputOperator(op, DisplayText.Text);
                }
            }

            // formateaza rezultatul daca gruparea cifrelor este activata
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        private void ToggleSign_Click(object sender, RoutedEventArgs e)
        {
            // schimba semnul numarului afisat
            DisplayText.Text = _calculator.ToggleSign(DisplayText.Text);
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            // sterge totul de pe display
            DisplayText.Text = _calculator.Clear();
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            // sterge ultima intrare de pe display
            DisplayText.Text = _calculator.ClearEntry();
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            // sterge ultima cifra de pe display
            DisplayText.Text = _calculator.Backspace(DisplayText.Text);
        }

        private void Invert_Click(object sender, RoutedEventArgs e)
        {
            // inverseaza numarul afisat (1/x)
            DisplayText.Text = _calculator.Invert(DisplayText.Text);
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            // calculeaza patratul numarului afisat
            DisplayText.Text = _calculator.Square(DisplayText.Text);
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        private void SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            // calculeaza radacina patrata a numarului afisat
            DisplayText.Text = _calculator.SquareRoot(DisplayText.Text);
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        private void Percentage_Click(object sender, RoutedEventArgs e)
        {
            // calculeaza procentul numarului afisat
            DisplayText.Text = _calculator.Percentage(DisplayText.Text);
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        // operatii cu memoria
        private void MemoryClear_Click(object sender, RoutedEventArgs e)
        {
            // sterge toate valorile din memorie
            _calculator.MemoryClear();
        }

        private void MemoryAdd_Click(object sender, RoutedEventArgs e)
        {
            // adauga valoarea curenta la memoria calculatorului
            _calculator.MemoryAdd(DisplayText.Text);
        }

        private void MemorySubtract_Click(object sender, RoutedEventArgs e)
        {
            // scade valoarea curenta din memoria calculatorului
            _calculator.MemorySubtract(DisplayText.Text);
        }

        private void MemoryStore_Click(object sender, RoutedEventArgs e)
        {
            // salveaza valoarea curenta in memorie
            _calculator.MemoryStore(DisplayText.Text);
        }

        private void MemoryRecall_Click(object sender, RoutedEventArgs e)
        {
            // recupereaza valoarea din memorie si o afiseaza
            DisplayText.Text = _calculator.MemoryRecall();
        }

        private void MemoryList_Click(object sender, RoutedEventArgs e)
        {
            // afiseaza lista cu valorile din memorie
            ShowMemoryStackWindow();
        }

        private void ShowMemoryStackWindow()
        {
            // obtine stiva de memorie
            var memoryStack = _calculator.GetMemoryStack();
            if (memoryStack.Count == 0)
            {
                // daca memoria este goala, afiseaza un mesaj
                MessageBox.Show("Memory is empty.", "Memory", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // creeaza o fereastra simpla pentru afisarea stivei de memorie
            var memoryWindow = new Window
            {
                Title = "Memory Stack",
                Width = 200,
                Height = 300,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            var stackPanel = new StackPanel();
            var listBox = new ListBox();

            // adauga fiecare valoare din memorie in lista
            foreach (var value in memoryStack)
            {
                var item = new ListBoxItem
                {
                    Content = value.ToString(CultureInfo.InvariantCulture), // afiseaza valoarea fara gruparea cifrelor
                    Padding = new Thickness(5) // adauga un spatiu intre elemente
                };
                listBox.Items.Add(item);
            }

            // gestioneaza selectia unei valori din lista
            listBox.SelectionChanged += (s, e) =>
            {
                if (listBox.SelectedItem is ListBoxItem selectedItem)
                {
                    DisplayText.Text = selectedItem.Content.ToString();
                    memoryWindow.Close();
                }
            };

            stackPanel.Children.Add(listBox);
            memoryWindow.Content = stackPanel;
            memoryWindow.ShowDialog();
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            // calculeaza si afiseaza rezultatul
            DisplayText.Text = _calculator.CalculateResult(DisplayText.Text);

            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        // operatii de taiere, copiere si lipire
        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            // taie textul din display si il copiaza in clipboard
            Copy_Click(sender, e);
            DisplayText.Text = "0";
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            // copiaza textul din display in clipboard
            Clipboard.SetText(DisplayText.Text);
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // incearca sa lipesti textul din clipboard in display
                string clipboardText = Clipboard.GetText();

                // valideaza daca textul este un numar
                if (double.TryParse(clipboardText, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    DisplayText.Text = clipboardText;
                }

                if (_settings.DigitGroupingEnabled)
                {
                    FormatDisplayIfNumber();
                }
            }
            catch
            {
                // ignora erorile de lipire
            }
        }
    }
}