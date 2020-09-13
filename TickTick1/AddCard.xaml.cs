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
using System.IO;

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

       

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //storing the file on desktop in "path"
            string path = @"C:\Users\Srinivas\Desktop\contextInk.isf";
            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            //Create the file.
            
            using (FileStream fs = File.Create(path))
            {
                contextInkCanvas.Strokes.Save(fs);
            }
            

            SqlConnection con = new SqlConnection("Server=tcp:srinivasa.database.windows.net,1433;Initial Catalog=FCdb;Persist Security Info=False;User ID=Srinivas;Password=Caustic6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into FlashCards(setNo,context, question, answer, tag, q_fileLocation, a_fileLocation , c_fileLocation) values ('" + setNo_text.Text + "','" + context_text.Text + "','" + question_text.Text + "','" + answer_text.Text + "','" + tag_text.Text + "','" + fileLocation_q_text.Text + "','" + fileLocation_a_text.Text + "','" + fileLocation_c_text.Text +"')",con);
            
            int i = cmd.ExecuteNonQuery();
            if(i != 0) 
            {
                MessageBox.Show("Saved to dbTable");
            }
            else
            {
                MessageBox.Show("Error");
            }
            con.Close();
        }
    }
}
