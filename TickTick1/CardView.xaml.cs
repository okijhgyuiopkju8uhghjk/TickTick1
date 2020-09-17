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
    /// Interaction logic for CardView.xaml
    /// </summary>
    public partial class CardView : Window
    {
        public static int cardIdNo;
        SqlCommand cmd;
        SqlConnection con;
        public CardView()
        {
            InitializeComponent();
            con = new SqlConnection("Server=tcp:srinivasa.database.windows.net,1433;Initial Catalog=FCdb;Persist Security Info=False;User ID=Srinivas;Password=Caustic6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            con.Open();
            MessageBox.Show("db connected");
            this.Closed += new EventHandler(CardView_Closing);

            String sqlSelectQuery = "SELECT [id] FROM [dbo].[FlashCards] WHERE id=(SELECT max(id) FROM [dbo].[FlashCards])";
            cmd = new SqlCommand(sqlSelectQuery, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                cardIdNo = (int)dr["id"];
                String cardId = String.Concat((cardIdNo + 1).ToString());

                CardID.Content = cardId;

            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveWindow();

        }
        void CardView_Closing(object sender, EventArgs e)
        {
            //Put your close code here
            if (MessageBox.Show("should this card be saved?", "before you leave", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SaveWindow();
                con.Close();
                MessageBox.Show("saved and conection closed");

            }
            else
            {
                con.Close();
                MessageBox.Show("not saved and connection closed");

            }
        }
        private void SaveWindow()
        {

            //creating uniq path for the 3 isf files
            string id_string = (cardIdNo + 1).ToString();
            string cpath = String.Concat(@"Z:\", id_string, @"_c_Ink.isf");
            string qpath = String.Concat(@"Z:\", id_string, @"_q_Ink.isf");
            string apath = String.Concat(@"Z:\", id_string, @"_a_Ink.isf");

            // Delete the file if it exists.
            if (File.Exists(cpath)) { File.Delete(cpath); }
            if (File.Exists(qpath)) { File.Delete(qpath); }
            if (File.Exists(apath)) { File.Delete(apath); }

            //Create the file.

            using (FileStream fs = File.Create(cpath)) { InkCanvas_c.Strokes.Save(fs); }
            using (FileStream fs = File.Create(qpath)) { InkCanvas_q.Strokes.Save(fs); }
            using (FileStream fs = File.Create(apath)) { InkCanvas_a.Strokes.Save(fs); }



            SqlCommand cmd = new SqlCommand("insert into FlashCards(setNo,context, question, answer, tag, q_fileLocation, a_fileLocation , c_fileLocation) values ('" + 1 + "','" + context_text.Text + "','" + question_text.Text + "','" + answer_text.Text + "','" + tag_text.Text + "','" + qpath + "','" + apath + "','" + cpath + "')", con);


            int i = cmd.ExecuteNonQuery();
            if (i != 0)
            {
                MessageBox.Show("Saved to dbTable");
            }
            else
            {
                MessageBox.Show("Error");
            }

        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            String sqlSelectQuery = "SELECT [id] FROM [dbo].[FlashCards] WHERE id=(SELECT max(id) FROM [dbo].[FlashCards])";
            cmd = new SqlCommand(sqlSelectQuery, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                cardIdNo = (int)dr["id"];
                String cardId = String.Concat((cardIdNo + 1).ToString());

                //cleaeing the all 3 ink canvas

                InkCanvas_c.Strokes.Clear();
                InkCanvas_q.Strokes.Clear();
                InkCanvas_a.Strokes.Clear();

                //clearing the contens of text boxs

                context_text.Text = String.Empty;
                question_text.Text = String.Empty;
                answer_text.Text = String.Empty;

                CardID.Content = cardId;

            }

        }
    }
}
