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
using System.Windows.Ink;

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
            SqlConnection con = new SqlConnection("Server=tcp:srinivasa.database.windows.net,1433;Initial Catalog=FCdb;Persist Security Info=False;User ID=Srinivas;Password=Caustic6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            con.Open();
            MessageBox.Show("db connected");

            String sqlSelectQuery = "SELECT [id] FROM [dbo].[FlashCards] WHERE id=(SELECT max(id) FROM [dbo].[FlashCards])";
            SqlCommand cmd = new SqlCommand(sqlSelectQuery, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                int cardIdNo = (int)dr["id"];
                String cardId = String.Concat("Card ID : ",( cardIdNo + 1).ToString());

                CardID.Content = cardId;

            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //storing the file on desktop in "path"
            string path = @"Z:\contextInk.isf";
            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            //Create the file.
            
            using (FileStream fs = File.Create(path))
            {
                contextInkCanvas_c.Strokes.Save(fs);
            }
            

            SqlConnection con = new SqlConnection("Server=tcp:srinivasa.database.windows.net,1433;Initial Catalog=FCdb;Persist Security Info=False;User ID=Srinivas;Password=Caustic6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            con.Open();
            //SqlCommand cmd = new SqlCommand("insert into FlashCards(setNo,context, question, answer, tag) values ('" + 1 + "','" + context_text.Text + "','" + question_text.Text + "','" + answer_text.Text + "','" + tag_text.Text + "')", con);

            SqlCommand cmd = new SqlCommand("insert into FlashCards(setNo,context, question, answer, tag, q_fileLocation, a_fileLocation , c_fileLocation) values ('" + 1 + "','" + context_text.Text + "','" + question_text.Text + "','" + answer_text.Text + "','" + tag_text.Text +"','" + fileLocation_q_text.Text + "','" + fileLocation_a_text.Text + "','" + fileLocation_c_text.Text +"')",con);


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

        private void load_Click(object sender, RoutedEventArgs e)
        {
            string filename = @"Z:\contextInk.isf";
            FileStream fs = new FileStream(filename, FileMode.Open);
            contextInkCanvas_c.Strokes = new StrokeCollection(fs);
            fs.Close();
        }
    }
}
