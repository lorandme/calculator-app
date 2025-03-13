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


namespace calculator_app
{
    public partial class MainWindow : Window
    {
        private CalculatorLogic _calculator = new CalculatorLogic();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                DisplayText.Text = _calculator.InputNumber(button.Content.ToString(), DisplayText.Text);
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                DisplayText.Text = _calculator.InputOperator(button.Content.ToString(), DisplayText.Text);
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
            DisplayText.Text = _calculator.Clear();
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = _calculator.Backspace(DisplayText.Text);
        }
    }
}
