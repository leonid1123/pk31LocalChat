using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace pk31LocalChat
{
    public partial class Form1 : Form
    {
        string userName = "";
        int userID;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           /* string cs = @"server=localhost;userid=pk31;password=123456;database=chat31";
            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "SELECT name FROM users";
            using var cmd = new MySqlCommand(sql, con);

            using MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                listBox2.Items.Add(rdr.GetString(0));
            }
            con.Close();*/
        }

        private void button4_Click(object sender, EventArgs e)
        {   //регистрация пользователя
            //сделать нормальную проверку
            userName = textBox2.Text;
            string cs = @"server=localhost;userid=pk31;password=123456;database=chat31";
            using var con = new MySqlConnection(cs);
            con.Open();

            var sql = "INSERT INTO users(name, online) VALUES(@name, @online)";
            using var cmd = new MySqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@name", userName);
            cmd.Parameters.AddWithValue("@online", true);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            con.Close();

            con.Open();
            string sql1 = "SELECT id FROM users WHERE name = @userName";
            using var cmd1 = new MySqlCommand(sql1, con);
            cmd1.Parameters.AddWithValue("@userName", userName);
            cmd1.Prepare();
            cmd1.ExecuteNonQuery();

            using MySqlDataReader rdr = cmd1.ExecuteReader();

            while (rdr.Read())
            {
                userID = rdr.GetInt32(0);
            }
            Debug.WriteLine(userID);
        }

        private void button1_Click(object sender, EventArgs e)
        {//кнопка отправить сообщение
            string cs = @"server=localhost;userid=pk31;password=123456;database=chat31";
            using var con = new MySqlConnection(cs);
            con.Open();
            var sql = "INSERT INTO msg(idname, dstname, text) VALUES(@idname, @dstname, @text)";
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@idname", userID);
            cmd.Parameters.AddWithValue("@dstname", -1);
            cmd.Parameters.AddWithValue("@text", textBox1.Text);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            //con.Close();
            textBox1.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string cs = @"server=localhost;userid=pk31;password=123456;database=chat31";
            using var con = new MySqlConnection(cs);
            con.Open();
            string sql = "SELECT idname,text,time FROM msg ORDER BY time DESC";
            using var cmd = new MySqlCommand(sql, con);

            using MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                listBox1.Items.Add(rdr.GetString(0)+":"+rdr.GetString(1)+"["+rdr.GetString(2)+"]");
            }
            con.Close();

            con.Open();
            listBox2.Items.Clear();
            string getUsers = "SELECT name FROM users";
            using var usersList = new MySqlCommand(getUsers, con);

            using MySqlDataReader readUsers = usersList.ExecuteReader();

            while (readUsers.Read())
            {
                listBox2.Items.Add(readUsers.GetString(0));
            }
            con.Close();
        }
    }
}
