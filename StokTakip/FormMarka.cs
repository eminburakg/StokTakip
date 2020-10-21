using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StokTakip
{
    public partial class FormMarka : Form
    {
        public FormMarka()
        {
            InitializeComponent();
        }
        SqlConnection cnn = new SqlConnection(@"Data source=DESKTOP-87VKS3P\SQLEXPRESS; Initial Catalog=StokTakip;Integrated Security=True");
        bool durum;
        private void MarkaKontrol()
        {
            durum = true;
            cnn.Open();
            SqlCommand komut = new SqlCommand("select * from tblMarkaBilgi", cnn);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (comboBox1.Text==read["Kategori"].ToString() && textBox1.Text == read["Marka"].ToString() || comboBox1.Text == "" || textBox1.Text == "")
                {
                    durum = false;
                }

                
            }
            cnn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MarkaKontrol();
            if (durum == true)
            {
                cnn.Open();
                SqlCommand command = new SqlCommand("insert into tblMarkaBilgi(Kategori,Marka) values('" + comboBox1.Text + "','" + textBox1.Text + "' )", cnn);
                command.ExecuteNonQuery();
                cnn.Close();
                textBox1.Text = "";
                comboBox1.Text = "";
                MessageBox.Show("Marka Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle bir kategori ve marka var UYARI!");
            }
            textBox1.Text = "";
            comboBox1.Text = "";
            
        }

        private void KategoriGetir()
        {
            cnn.Open();
            SqlCommand komut = new SqlCommand("select * from tblKategoriBilgi", cnn);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboBox1.Items.Add(read["Kategori"].ToString());
            }
            cnn.Close();
        }
        private void FormMarka_Load(object sender, EventArgs e)
        {
            KategoriGetir();
        }
    }
}
