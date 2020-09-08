using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for StudyWindow.xaml
    /// </summary>
    public partial class StudyWindow : Window
    {
        public StudyWindow()
        {
            InitializeComponent();
        }

        private void ID_TextChanged(object sender, TextChangedEventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-B1UH9E1\\SQLEXPRESS;Initial Catalog=MyCards;Integrated Security=True");
            con.Open();

            SqlCommand cmd = new SqlCommand("Select  question from Card_Table_1 where id = @ID", con);
            

            cmd.Parameters.AddWithValue("@ID", int.Parse(ID_text.Text));
            SqlDataReader da = cmd.ExecuteReader();
            while (da.Read()) 
            {
                Q_text.Text = da.GetValue(0).ToString();
            }
            con.Close();


        }
    }
}
