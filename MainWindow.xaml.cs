using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void TextBoxesInputController(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void SameValueInTextBoxes(object sender, TextChangedEventArgs e)
        {
            TextBox tb = ((TextBox)sender);
            if (tb.Name == "textBox1")
                textBox2.Text = textBox1.Text;
            else
                textBox1.Text = textBox2.Text;  
        }
        private void Grid1_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox[] textBoxes = (Grid1.Children.OfType<TextBox>()).ToArray();
            foreach (TextBox tb in textBoxes)
            {
                tb.PreviewTextInput += TextBoxesInputController;
 
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int width=Int32.Parse(textBox1.Text);
            int height = Int32.Parse(textBox2.Text);
            int winNumber = Int32.Parse(textBox3.Text);
            int cellSize = Int32.Parse(textBox4.Text);
            if (winNumber > width && winNumber > height)
            {
                MessageBox.Show("Game cant be won because win number is bigger than width and height","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            GameWindow gameWindow = new GameWindow(width,height,winNumber,cellSize);
            gameWindow.ShowDialog();
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            int width = Int32.Parse(textBox1.Text);
            textBox2.Text = width.ToString();
            textBox1.TextChanged += SameValueInTextBoxes;
            textBox2.TextChanged += SameValueInTextBoxes;
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            textBox1.TextChanged -= SameValueInTextBoxes;
            textBox2.TextChanged -= SameValueInTextBoxes;
        }
    }
}
