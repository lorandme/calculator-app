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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            string number = button.Content.ToString();

            if (DisplayText.Text == "0" && number != ".")
            {
                DisplayText.Text = number;
            }
            else if (number == "." && DisplayText.Text.Contains("."))
            {
                return;
            }
            else
            {
                DisplayText.Text += number;
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            string operatorSymbol = button.Content.ToString();

            if (DisplayText.Text.Length == 0 ||
                DisplayText.Text.EndsWith(" ") ||   
                DisplayText.Text.EndsWith("+") ||
                DisplayText.Text.EndsWith("-") ||
                DisplayText.Text.EndsWith("*") ||
                DisplayText.Text.EndsWith("/"))
            {
                return;  
            }

            DisplayText.Text += " " + operatorSymbol + " ";
        }


        private void ToggleSign_Click(object sender, RoutedEventArgs e)
        {
            if (DisplayText.Text == "0" || DisplayText.Text == "") return;

            if (DisplayText.Text.StartsWith("-"))
                DisplayText.Text = DisplayText.Text.Substring(1);
            else
                DisplayText.Text = "-" + DisplayText.Text; 
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = "0";
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            DisplayText.Text = "0";
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (DisplayText.Text.Length > 1)
                DisplayText.Text = DisplayText.Text.Substring(0, DisplayText.Text.Length - 1);
            else
                DisplayText.Text = "0"; 
        }



    }
}