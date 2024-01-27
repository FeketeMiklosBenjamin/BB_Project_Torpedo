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

namespace Torpedo_Project
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

        private void keyPress_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                homeWindow oldal = new homeWindow();
                oldal.Owner = this.Owner;
                oldal.Show();
                this.Close();
            }
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            homeWindow oldal = new homeWindow();
            oldal.Owner = this.Owner;
            oldal.Show();
            this.Close();
        }
    }
}