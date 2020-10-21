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
    public partial class FormUrunEkleme : Form
    {
        public FormUrunEkleme()
        {
            InitializeComponent();
        }
        SqlConnection cnn = new SqlConnection(@"Data source=DESKTOP-87VKS3P\SQLEXPRESS; Initial Catalog=StokTakip;Integrated Security=True");
        bool durum;
        private void BarkodKontrol()
        {
            durum = true;
            cnn.Open();
            SqlCommand komut = new SqlCommand("select * from tblUrun", cnn);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text==read["BarkodNo"].ToString() || txtBarkodNo.Text=="")
                {
                    durum = false;
                }

                
            }
            cnn.Close();
        }

        private void KategoriGetir()
        {
            cnn.Open();
            SqlCommand komut = new SqlCommand("select * from tblKategoriBilgi", cnn);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboKategori.Items.Add(read["Kategori"].ToString());
            }
            cnn.Close();
        }

        private void FormUrunEkleme_Load(object sender, EventArgs e)
        {
            KategoriGetir();
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMarka.Items.Clear();
            comboMarka.Text = "";
            cnn.Open();
            SqlCommand komut = new SqlCommand("select * from tblMarkaBilgi where Kategori = '" + comboKategori.SelectedItem + "'", cnn);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboMarka.Items.Add(read["Marka"].ToString());
            }
            cnn.Close();
        }

        private void btnYeniUrunEkle_Click(object sender, EventArgs e)
        {
            BarkodKontrol();
            if (durum == true)
            {
                cnn.Open();
                SqlCommand komut = new SqlCommand("insert into tblUrun(BarkodNo,Kategori,Marka,UrunAdi,Miktar,AlisFiyati,SatisFiyati,Tarih) values(@BarkodNo,@Kategori,@Marka,@UrunAdi,@Miktar,@AlisFiyati,@SatisFiyati,@Tarih) ", cnn);
                komut.Parameters.AddWithValue("@BarkodNo", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@Kategori", comboKategori.Text);
                komut.Parameters.AddWithValue("@Marka", comboMarka.Text);
                komut.Parameters.AddWithValue("@UrunAdi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@Miktar", txtMiktar.Text);
                komut.Parameters.AddWithValue("@AlisFiyati", txtAlisFiyat.Text);
                komut.Parameters.AddWithValue("@SatisFiyati", txtSatisFiyat.Text);
                komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString());


                komut.ExecuteNonQuery();
                cnn.Close();
                MessageBox.Show("Ürün Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle bir Barkod Numarası var UYARI !");
            }
            comboMarka.Items.Clear(); 
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
                if (item is ComboBox)
                {
                    item.Text = "";
                }
            }
        }

        private void BarkodNotxt_TextChanged(object sender, EventArgs e)
        {
            if (BarkodNotxt.Text=="")
            {
                lblMiktar.Text = "";
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }


            cnn.Open();
            SqlCommand komut = new SqlCommand("select * from tblUrun where BarkodNo like '"+BarkodNotxt.Text+"'", cnn);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                Kategoritxt.Text = read["Kategori"].ToString();
                Markatxt.Text = read["Marka"].ToString();
                UrunAditxt.Text = read["UrunAdi"].ToString();
                lblMiktar.Text = read["Miktar"].ToString();
                AlisFiyattxt.Text = read["AlisFiyati"].ToString();
                SatisFiyattxt.Text = read["SatisFiyati"].ToString();
            }

            cnn.Close();
        }

        private void btnVarOlanaEkle_Click(object sender, EventArgs e)
        {
            cnn.Open();
            SqlCommand komut = new SqlCommand("update tblUrun set Miktar=Miktar+'"+int.Parse(Miktartxt.Text)+"' where BarkodNo='"+BarkodNotxt.Text+"'", cnn);
            komut.ExecuteNonQuery();
            cnn.Close();

            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
            MessageBox.Show("Var Olan Ürüne Ekleme Yapıldı");
        }
    }
}
