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
using System.Data.SqlClient;

namespace TickTick1
{
    /// <summary>
    /// Interaction logic for AddCard.xaml
    /// </summary>
    public partial class AddCard : Window
    {
        public AddCard()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            CardSet objcardSet = new CardSet();
            //this.Visibility = Visibility.Hidden; //hiding the current window
            objcardSet.Show();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-B1UH9E1\\SQLEXPRESS;Initial Catalog=MyCards;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Card_Table_1(setNo,context, question, answer, tag, q_fileLocation, a_fileLocation , c_fileLocation) values ('" + setNo_text.Text + "','" + context_text.Text + "','" + question_text.Text + "','" + answer_text.Text + "','" + tag_text.Text + "','" + fileLocation_q_text.Text + "','" + fileLocation_a_text.Text + "','" + fileLocation_c_text.Text +"')",con);
            
            int i = cmd.ExecuteNonQuery();
            if(i != 0) 
            {
                MessageBox.Show("Saved");
            }
            else
            {
                MessageBox.Show("Error");
            }
            con.Close();
        }
    }
}
