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
using System.Data;
using System.IO;
using System.Windows.Ink;

namespace TickTick1
{
    /// <summary>
    /// Interaction logic for StudyWindow.xaml
    /// </summary>
    public partial class StudyWindow : Window
    {
        
        public int[] cardSet;
        SqlConnection con;
        String sqlSelectQuery;
        public String aText;
        public String apath;
        SqlCommand cmd;

        public StudyWindow()
        {
            InitializeComponent();

            

            con = new SqlConnection("Server=tcp:srinivasa.database.windows.net,1433;Initial Catalog=FCdb;Persist Security Info=False;User ID=Srinivas;Password=Caustic6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets = True");
            con.Open();
            MessageBox.Show("db connected");
            //ask db to give all id no with set no what ever it is that is ckicked
            sqlSelectQuery = "SELECT[id] FROM[dbo].[FlashCards] WHERE[setNo] = "+ GlobalVariables.SetNo;
            cmd = new SqlCommand(sqlSelectQuery, con);
            SqlDataReader dr2 = cmd.ExecuteReader();

            //capture all the card id of the corresponding study set to into an list caled list
            List<int> list = (from IDataRecord r in dr2 select (int)r["id"]).ToList();
            MessageBox.Show(list.Count.ToString());
            dr2.Close();

            //select random integer between 0 and length of the list get the cardid at that position
            Random rnd = new Random();
            int randomCardid = list[rnd.Next(list.Count)];


            //ask the database for everything about that id
            String sqlSelectQuery2 = $"SELECT [id],[setNo],[context],[question],[answer],[tag],[q_fileLocation],[a_fileLocation],[c_fileLocation] FROM[dbo].[FlashCards] WHERE[id] = {randomCardid};"; 
    
            

            SqlCommand cmd2 = new SqlCommand(sqlSelectQuery2, con);
            SqlDataReader dr = cmd2.ExecuteReader();
            //display the c and q  on the window

            if (dr.Read())
            {
                MessageBox.Show(dr["context"].ToString(), "reached if");
                String cText = dr["context"].ToString();
                String qText = dr["question"].ToString();
                aText = dr["answer"].ToString();


                String qpath = dr["q_fileLocation"].ToString();
                String cpath = dr["q_fileLocation"].ToString();
                apath = dr["q_fileLocation"].ToString();
                String cardId = dr["id"].ToString();


                //display card id ,set no, context text, question text
                CardID.Content = cardId;
                Set_No.Content = GlobalVariables.SetNo;
                context_text.Text = cText;
                question_text.Text = qText;


                //display ink on c and q 

                FileStream fs = new FileStream(cpath, FileMode.Open, (FileAccess)FileShare.ReadWrite);
                InkCanvas_c.Strokes = new StrokeCollection(fs);
                fs.Close();
                FileStream fs2 = new FileStream(qpath, FileMode.Open, (FileAccess)FileShare.ReadWrite);
                InkCanvas_q.Strokes = new StrokeCollection(fs2);
                fs2.Close();
            }
            dr.Close();
            con.Close();

            //remove that id from the list when next is selected and repat the entire process

            // there should be a lable showing the number of cards remaining

        }
        private void ShowAnswer_Click(object sender, RoutedEventArgs e)
        {
            // on clicking answer button show answer
            answer_text.Text = aText;
            FileStream fs = new FileStream(apath, FileMode.Open, (FileAccess)FileShare.ReadWrite);
            InkCanvas_a.Strokes = new StrokeCollection(fs);
            fs.Close();
;

        }
    }
}
