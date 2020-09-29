using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Data;

namespace TickTick1
{
    /// <summary>
    /// Interaction logic for ViewAll.xaml
    /// </summary>
    public partial class ViewAll : Window
    {
        public ViewAll()
        {
            InitializeComponent();
            DataGridView1.ItemsSource = null;
            string dbq = "select * from FlashCards";
            bindDataGrid(dbq);

            


        }

        private void bindDataGrid(string dbq)
        {

            SqlConnection con = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString
            };

            if (con != null && con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = dbq;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Students");
            da.Fill(dt);

            DataGridView1.ItemsSource = dt.DefaultView;




        }

        private void Search_bn_Click(object sender, RoutedEventArgs e)
        {
            String searchCombo = searchoption.Text;
            string dbquestion = $"select * from FlashCards where {searchCombo} = '{SearchBox.Text}'";
            MessageBox.Show(dbquestion, "bdq is");
            bindDataGrid(dbquestion);
        }

        private void Opentoedit_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.IdToEdit = idToEdit.Text;
            GlobalVariables.SetNo = "8";

            CardView cardView = new CardView();
            cardView.Show();

        }
    }
}
