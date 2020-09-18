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
using System.Data;

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

        public int[] cardSet;
        String sqlSelectQuery;
        public String aText;
        public String qText;
        public String cText;
        public String apath;
        public String cpath;
        public String qpath;
        public int randomCardid;
        public List<int> list;

        //savewindow(),newcardcardview(),clearcardview()

        public CardView()
        {
            InitializeComponent();
            con = new SqlConnection("Server=tcp:srinivasa.database.windows.net,1433;Initial Catalog=FCdb;Persist Security Info=False;User ID=Srinivas;Password=Caustic6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            if (con != null && con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            MessageBox.Show("db connected");
            this.Closed += new EventHandler(CardView_Closing);
            
            if (Int32.Parse(GlobalVariables.SetNo) == 0) //for add card
            {
                // show/hide relavent buttons
                ShowAnswer.Visibility = Visibility.Hidden;
                ShowNext.Visibility = Visibility.Hidden;

                Promote.Visibility = Visibility.Hidden;
                Demote.Visibility = Visibility.Hidden;

                Save.Visibility = Visibility.Visible;
                AddNext.Visibility = Visibility.Visible;

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
            else
            {
                //for study mode

                //Show hide relavent buttons
                ShowAnswer.Visibility = Visibility.Visible;
                ShowNext.Visibility = Visibility.Hidden;

                Promote.Visibility = Visibility.Visible;
                Demote.Visibility = Visibility.Visible;

                Save.Visibility = Visibility.Hidden;
                AddNext.Visibility = Visibility.Hidden;

                //ask db to give all id no with set no what ever it is that is ckicked
                sqlSelectQuery = "SELECT[id] FROM[dbo].[FlashCards] WHERE[setNo] = " + GlobalVariables.SetNo;
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd = new SqlCommand(sqlSelectQuery, con);
                SqlDataReader dr2 = cmd.ExecuteReader();

                //capture all the card id of the corresponding study set to into an list caled list
                list = (from IDataRecord r in dr2 select (int)r["id"]).ToList();
                dr2.Close();
                //display list in window
                List.Content = string.Join(",", list.ToArray());

                //select random integer between 0 and length of the list get the cardid at that position
                Random rnd = new Random();
                randomCardid = list[rnd.Next(list.Count)];
                //------------
                NewCardCardView(randomCardid);

                //remove that id from the list when next is selected and repat the entire process

                // there should be a lable showing the number of cards remaining
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveWindow();

        }
        void CardView_Closing(object sender, EventArgs e)
        {
            if (Int32.Parse(GlobalVariables.SetNo) == 0)
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
        private void AddNext_Click(object sender, RoutedEventArgs e)
        {
            String sqlSelectQuery = "SELECT [id] FROM [dbo].[FlashCards] WHERE id=(SELECT max(id) FROM [dbo].[FlashCards])";
            cmd = new SqlCommand(sqlSelectQuery, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                cardIdNo = (int)dr["id"];
                String cardId = String.Concat((cardIdNo + 1).ToString());

                ClearCardView();

                CardID.Content = cardId;

            }

        }

        private void ShowAnswer_Click(object sender, RoutedEventArgs e)
        {
            // on clicking answer button show answer
            answer_text.Text = aText;
            FileStream fs = new FileStream(apath, FileMode.Open, (FileAccess)FileShare.ReadWrite);
            InkCanvas_a.Strokes = new StrokeCollection(fs);
            fs.Close();
        }

        private void Promote_Click(object sender, RoutedEventArgs e)
        {
            //increase set no by 1 if less than 7
            if (Int32.Parse(GlobalVariables.SetNo) < 7)
            {
                String sqlUpdateQuery = $"UPDATE FlashCards SET setNo = {(Int32.Parse(GlobalVariables.SetNo)) + 1}, tag = '{tag_text.Text}' WHERE id = {randomCardid};";
                cmd = new SqlCommand(sqlUpdateQuery, con);
                
            }
            ClearCardView();
            //remove this id from list
            list.Remove(randomCardid);

            //if longht of list not 0 show next else show msg box set over
            if (list.Count != 0) 
            {
                Random rnd = new Random();
                randomCardid = list[rnd.Next(list.Count)];
                NewCardCardView(randomCardid);
            } else 
            {
                MessageBox.Show($"{GlobalVariables.SetNo} set study session is over", "Note");
            }
            //update new list
            List.Content = string.Join(",", list.ToArray());

        }
        private void Demote_Click(object sender, RoutedEventArgs e)
        {
            //decrease set no by 1 if more than 1
            if (Int32.Parse(GlobalVariables.SetNo) > 1)
            {
                String sqlUpdateQuery = $"UPDATE FlashCards SET setNo = {(Int32.Parse(GlobalVariables.SetNo)) - 1}, tag = '{tag_text.Text}' WHERE id = {randomCardid};";
                cmd = new SqlCommand(sqlUpdateQuery, con);

            }

            ClearCardView();
            //remove this id from list
            list.Remove(randomCardid);

            //if longht of list not 0 show next else show msg box set over
            if (list.Count != 0)
            {
                Random rnd = new Random();
                randomCardid = list[rnd.Next(list.Count)];
                NewCardCardView(randomCardid);
            }
            else
            {
                MessageBox.Show($"{GlobalVariables.SetNo} set study session is over", "Note");
            }
            //update new list
            List.Content = string.Join(",", list.ToArray());

        }
        public void ClearCardView()
        {
            //cleaeing the all 3 ink canvas

            InkCanvas_c.Strokes.Clear();
            InkCanvas_q.Strokes.Clear();
            InkCanvas_a.Strokes.Clear();

            //clearing the contens of text boxs

            context_text.Text = String.Empty;
            question_text.Text = String.Empty;
            answer_text.Text = String.Empty;
            tag_text.Text = String.Empty;

            //clear card id
            CardID.Content = "";

        }
        public void NewCardCardView(int id)
        {
            //------

            //ask the database for everything about that id
            if (con != null && con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            
            String sqlSelectQuery2 = $"SELECT [id],[setNo],[context],[question],[answer],[tag],[q_fileLocation],[a_fileLocation],[c_fileLocation] FROM[dbo].[FlashCards] WHERE[id] = {id};";

            SqlCommand cmd2 = new SqlCommand(sqlSelectQuery2, con);
            SqlDataReader dr = cmd2.ExecuteReader();
            //display the c and q  on the window

            if (dr.Read())
            {
                //display card id ,set no, context text, question text
                CardID.Content = dr["id"].ToString();

                Set_No.Content = $"Set No:{dr["setNo"]}";
                context_text.Text = dr["context"].ToString();
                question_text.Text = dr["question"].ToString();
                aText = dr["answer"].ToString();

                //display ink on c and q 
                qpath = dr["q_fileLocation"].ToString();
                cpath = dr["q_fileLocation"].ToString();
                apath = dr["q_fileLocation"].ToString();

                FileStream fs = new FileStream(cpath, FileMode.Open, (FileAccess)FileShare.ReadWrite);
                InkCanvas_c.Strokes = new StrokeCollection(fs);
                fs.Close();
                FileStream fs2 = new FileStream(qpath, FileMode.Open, (FileAccess)FileShare.ReadWrite);
                InkCanvas_q.Strokes = new StrokeCollection(fs2);
                fs2.Close();
            }
            dr.Close();
            con.Close();
            
            //---------------------------------

        }

        
    }
}
