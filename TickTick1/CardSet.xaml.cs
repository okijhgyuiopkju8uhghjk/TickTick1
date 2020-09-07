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

namespace TickTick1
{
    /// <summary>
    /// Interaction logic for CardSet.xaml
    /// </summary>
    public partial class CardSet : Window
    {
        public CardSet()
        {
            InitializeComponent();
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            MainWindow objmainwindow = new MainWindow();
            //this.Visibility = Visibility.Hidden; //hiding the current window
            objmainwindow.Show();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            AddCard objaddcard = new AddCard();
            objaddcard.Show();
        }
    }
}
