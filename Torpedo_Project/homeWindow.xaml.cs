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
using System.Windows.Shapes;

namespace Torpedo_Project
{
    /// <summary>
    /// Interaction logic for homeWindow.xaml
    /// </summary>
    public partial class homeWindow : Window
    {
        public homeWindow()
        {
            InitializeComponent();
        }

        private void keyPress_Click(object sender, RoutedEventArgs e)
        {
            var window = GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MainWindow oldal2 = new MainWindow();
                oldal2.Owner = this.Owner;
                oldal2.Show();
                this.Close();
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow oldal2 = new MainWindow();
            oldal2.Owner = this.Owner;
            oldal2.Show();
            this.Close();
        }
    }
}
