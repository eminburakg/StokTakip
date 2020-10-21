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
    public partial class FormUrunListele : Form
    {
        public FormUrunListele()
        {
            InitializeComponent();
        }
        SqlConnection cnn = new SqlConnection(@"Data source=DESKTOP-87VKS3P\SQLEXPRESS; Initial Catalog=StokTakip;Integrated Security=True");
        DataSet daset = new DataSet();

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

        private void UrunListele()
        {
            cnn.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from tblUrun", cnn);
            adtr.Fill(daset, "tblUrun");
            dataGridView1.DataSource = daset.Tables["tblUrun"];
            cnn.Close();
        }
        private void FormUrunListele_Load(object sender, EventArgs e)
        {
            UrunListele();
            KategoriGetir();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BarkodNotxt.Text = dataGridView1.CurrentRow.Cells["BarkodNo"].Value.ToString();
            Kategoritxt.Text = dataGridView1.CurrentRow.Cells["Kategori"].Value.ToString();
            Markatxt.Text = dataGridView1.CurrentRow.Cells["Marka"].Value.ToString();
            UrunAditxt.Text = dataGridView1.CurrentRow.Cells["UrunAdi"].Value.ToString();
            Miktartxt.Text = dataGridView1.CurrentRow.Cells["Miktar"].Value.ToString();
            AlisFiyattxt.Text = dataGridView1.CurrentRow.Cells["AlisFiyati"].Value.ToString();
            SatisFiyattxt.Text = dataGridView1.CurrentRow.Cells["SatisFiyati"].Value.ToString();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            cnn.Open();
            SqlCommand komut = new SqlCommand("update tblUrun set UrunAdi=@urunadi,Miktar=@miktar,AlisFiyati=@alisfiyati,SatisFiyati=@satisfiyati where BarkodNo=@barkodno",cnn);
            komut.Parameters.AddWithValue("@barkodno", BarkodNotxt.Text);
            komut.Parameters.AddWithValue("@UrunAdi", UrunAditxt.Text);
            komut.Parameters.AddWithValue("@Miktar", Miktartxt.Text);
            komut.Parameters.AddWithValue("@AlisFiyati", AlisFiyattxt.Text);
            komut.Parameters.AddWithValue("@SatisFiyati", AlisFiyattxt.Text);
            komut.ExecuteNonQuery();
            cnn.Close();
            daset.Tables["tblUrun"].Clear();
            UrunListele();
            MessageBox.Show("Güncelleme yapıldı");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void btnMarkaGuncelle_Click(object sender, EventArgs e)
        {
            if (BarkodNotxt.Text != "")
            {
                cnn.Open();
                SqlCommand komut = new SqlCommand("update tblUrun set Kategori=@kategori,Marka=@marka where BarkodNo=@barkodno", cnn);
                komut.Parameters.AddWithValue("@barkodno", BarkodNotxt.Text);
                komut.Parameters.AddWithValue("@kategori", comboKategori.Text);
                komut.Parameters.AddWithValue("@marka", comboMarka.Text);
                komut.ExecuteNonQuery();
                cnn.Close();
                MessageBox.Show("Güncelleme yapıldı");
                daset.Tables["tblUrun"].Clear();
                UrunListele();
            }
            else
            {
                MessageBox.Show("BarkodNo yazılı değil");
            }
            foreach (Control item in this.Controls)
            {
                if (item is ComboBox)
                {
                    item.Text = "";
                }
            }
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

        private void btnSil_Click(object sender, EventArgs e)
        {
            cnn.Open();
            SqlCommand con = new SqlCommand("delete from tblUrun where BarkodNo='" + dataGridView1.CurrentRow.Cells["BarkodNo"].Value.ToString() + "'", cnn);
            con.ExecuteNonQuery();
            cnn.Close();
            daset.Tables["tblUrun"].Clear();
            UrunListele();
            MessageBox.Show("Kayıt Silindi");
        }

        private void txtBarkodNoAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tbl = new DataTable();
            cnn.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from tblUrun where BarkodNo like '%" + txtBarkodNoAra.Text + "%'", cnn);
            adtr.Fill(tbl);
            dataGridView1.DataSource = tbl;
            cnn.Close();
        }
    }
}
