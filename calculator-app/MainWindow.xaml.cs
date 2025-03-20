using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

            // Load settings
            DigitGroupingMenuItem.IsChecked = _settings.DigitGroupingEnabled;

            // Add key event handlers
            KeyDown += MainWindow_KeyDown;
        }
        private void DigitGrouping_Changed(object sender, RoutedEventArgs e)
        {
            _settings.DigitGroupingEnabled = DigitGroupingMenuItem.IsChecked;
            _settings.Save();
            FormatDisplayIfNumber();
        }

        private void FormatDisplayIfNumber()
        {
            // Check if the display contains a valid number
            if (double.TryParse(DisplayText.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                if (_settings.DigitGroupingEnabled)
                {
                    // Only apply grouping to the integer part
                    string text = DisplayText.Text;
                    bool hasDecimalPoint = text.Contains('.');

                    if (hasDecimalPoint)
                    {
                        string[] parts = text.Split('.');
                        // Format only the integer part with grouping
                        string integerPart = double.Parse(parts[0], CultureInfo.InvariantCulture)
                            .ToString("N0", CultureInfo.CurrentCulture);
                        // Keep decimal part as is
                        DisplayText.Text = integerPart + "." + parts[1];
                    }
                    else
                    {
                        // No decimal, safe to format the whole number
                        DisplayText.Text = value.ToString("N0", CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    // Ensure we have plain number without grouping
                    DisplayText.Text = value.ToString(CultureInfo.InvariantCulture);
                }
            }
            // If not a valid number, leave as is
        }

        private void UpdateDisplay()
        {
            // Don't format if the text isn't a valid number
            if (!double.TryParse(DisplayText.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                return;

            if (_settings.DigitGroupingEnabled)
            {
                // Use the current culture's number format with digit grouping
                DisplayText.Text = value.ToString("N", CultureInfo.CurrentCulture);
            }
            else
            {
                // Use invariant culture without digit grouping
                DisplayText.Text = value.ToString(CultureInfo.InvariantCulture);
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
            DisplayText.Text = _calculator.InputNumber(number, DisplayText.Text);

            // Only apply formatting if digit grouping is enabled and not a decimal point
            if (_settings.DigitGroupingEnabled && number != ".")
            {
                FormatDisplayIfNumber();
            }

        }

        private void HandleOperatorInput(string operatorSymbol)
        {
            if (operatorSymbol == "=")
            {
                DisplayText.Text = _calculator.CalculateResult(DisplayText.Text);

            }
            else
            {
                DisplayText.Text = _calculator.InputOperator(operatorSymbol, DisplayText.Text);
            }
            // Format the result if digit grouping is enabled
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }

        }


        private void HandleClear()
        {
            DisplayText.Text = _calculator.Clear();
        }

        private void HandleBackspace()
        {
            DisplayText.Text = _calculator.Backspace(DisplayText.Text);
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string number = button.Content.ToString();
                DisplayText.Text = _calculator.InputNumber(number, DisplayText.Text);

                // Only apply formatting if digit grouping is enabled and not a decimal point
                if (_settings.DigitGroupingEnabled && number != ".")
                {
                    FormatDisplayIfNumber();
                }
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
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
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }

        }

        private void ToggleSign_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.ToggleSign(DisplayText.Text);
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.Clear();
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.ClearEntry();
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.Backspace(DisplayText.Text);
        }

        private void Invert_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.Invert(DisplayText.Text);
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.Square(DisplayText.Text);
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        private void SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.SquareRoot(DisplayText.Text);
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        private void Percentage_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.Percentage(DisplayText.Text);
            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }

        // Memory operations
        private void MemoryClear_Click(object sender, RoutedEventArgs e)
        {
            _calculator.MemoryClear();
        }

        private void MemoryAdd_Click(object sender, RoutedEventArgs e)
        {
            _calculator.MemoryAdd(DisplayText.Text);
        }

        private void MemorySubtract_Click(object sender, RoutedEventArgs e)
        {
            _calculator.MemorySubtract(DisplayText.Text);
        }

        private void MemoryStore_Click(object sender, RoutedEventArgs e)
        {
            _calculator.MemoryStore(DisplayText.Text);
        }

        private void MemoryRecall_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.MemoryRecall();
        }

        private void MemoryList_Click(object sender, RoutedEventArgs e)
        {
            ShowMemoryStackWindow();
        }

        private void ShowMemoryStackWindow()
        {
            var memoryStack = _calculator.GetMemoryStack();
            if (memoryStack.Count == 0)
            {
                MessageBox.Show("Memory is empty.", "Memory", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Create a simple window to display the memory stack
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

            foreach (var value in memoryStack)
            {
                var item = new ListBoxItem
                {
                    Content = value.ToString(CultureInfo.InvariantCulture),
                    Padding = new Thickness(5)
                };
                listBox.Items.Add(item);
            }

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
            DisplayText.Text = _calculator.CalculateResult(DisplayText.Text);

            if (_settings.DigitGroupingEnabled)
            {
                FormatDisplayIfNumber();
            }
        }


        // CUT,COPY,PASTE OPERATIONS
        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            Copy_Click(sender, e);
            DisplayText.Text = "0";
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(DisplayText.Text);
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string clipboardText = Clipboard.GetText();

                // Validate if the text is a number
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
                // Ignore paste errors
            }
        }
    }

}
