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

namespace TickTick1
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

        private void one_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.SetNo = "1";
            CardView objcardview = new CardView();
            objcardview.Show();


        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            CardView objcardview = new CardView();
            objcardview.Show();
        }

        private void cloud_Click(object sender, RoutedEventArgs e)
        {

        }

        private void two_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.SetNo = "2";
            CardView objcardview = new CardView();
            objcardview.Show();

        }

        private void three_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.SetNo = "3";
            CardView objcardview = new CardView();
            objcardview.Show();
        }

        private void four_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.SetNo = "4";
            CardView objcardview = new CardView();
            objcardview.Show();
        }

        private void five_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.SetNo = "5";
            CardView objcardview = new CardView();
            objcardview.Show();
        }

        private void six_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.SetNo = "6";
            CardView objcardview = new CardView();
            objcardview.Show();
        }

        private void seven_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.SetNo = "7";
            CardView objcardview = new CardView();
            objcardview.Show();
        }
    }
}
