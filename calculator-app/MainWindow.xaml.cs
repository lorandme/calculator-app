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

        public MainWindow()
        {
            InitializeComponent();

            // Add key event handlers
            KeyDown += MainWindow_KeyDown;
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
                HandleNumberInput(button.Content.ToString());
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
        }

        private void ToggleSign_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.ToggleSign(DisplayText.Text);
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
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.Square(DisplayText.Text);
        }

        private void SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.SquareRoot(DisplayText.Text);
        }

        private void Percentage_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.Percentage(DisplayText.Text);
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
            }
            catch
            {
                // Ignore paste errors
            }
        }
    }

}
