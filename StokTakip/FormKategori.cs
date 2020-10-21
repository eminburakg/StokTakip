using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StokTakip
{
    public partial class FormKategori : Form
    {
        public FormKategori()
        {
            InitializeComponent();
        }
        SqlConnection cnn = new SqlConnection(@"Data source=DESKTOP-87VKS3P\SQLEXPRESS; Initial Catalog=StokTakip;Integrated Security=True");
        bool durum;

        private void KategoriKontrol()
        {
            durum = true; 
            cnn.Open();
            SqlCommand komut = new SqlCommand("select * from tblKategoriBilgi", cnn);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtKategori.Text==read["Kategori"].ToString() || txtKategori.Text=="")
                {
                    durum = false;
                }
                

            }
            cnn.Close();

        }
        private void FormKategori_Load(object sender, EventArgs e)
        {

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            KategoriKontrol();
            if (durum==true)
            {
                cnn.Open();
                SqlCommand command = new SqlCommand("insert into tblKategoriBilgi(Kategori) values('" + txtKategori.Text + "') ", cnn);
                command.ExecuteNonQuery();
                cnn.Close();
                MessageBox.Show("Kategoriye Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle bir kategori var UYARI!");
            }
            txtKategori.Text = "";
        }
            
    }
}
