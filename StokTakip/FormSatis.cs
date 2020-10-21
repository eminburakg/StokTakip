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
    public partial class FormSatis : Form
    {
        public FormSatis()
        {
            InitializeComponent();
        }
        SqlConnection cnn = new SqlConnection(@"Data source=DESKTOP-87VKS3P\SQLEXPRESS; Initial Catalog=StokTakip;Integrated Security=True");
        DataSet daset = new DataSet();

        private void SepetListele()
        {
            cnn.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from tblSepet", cnn);
            adtr.Fill(daset, "tblSepet");
            dataGridView1.DataSource = daset.Tables["tblSepet"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;

            cnn.Close();

        }

        private void btnMusteriEkle_Click(object sender, EventArgs e)
        {
            FormMusteriEkle formekle = new FormMusteriEkle();
            formekle.ShowDialog();
        }

        private void btnMusteriListe_Click(object sender, EventArgs e)
        {
            FormMusteriListele listele = new FormMusteriListele();
            listele.ShowDialog();
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            FormUrunEkleme ekle = new FormUrunEkleme();
            ekle.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormKategori kategori = new FormKategori();
            kategori.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormMarka marka = new FormMarka();
            marka.ShowDialog();
        }

        private void btnUrunListe_Click(object sender, EventArgs e)
        {
            FormUrunListele listele = new FormUrunListele();
            listele.ShowDialog();
        }
        private void Hesapla()
        {
            try
            {
                cnn.Open();
                SqlCommand komut = new SqlCommand("select sum(ToplamFiyati) from tblSepet", cnn);
                lblGenelToplam.Text = komut.ExecuteScalar() + " TL";
                cnn.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FormSatis_Load(object sender, EventArgs e)
        {
            SepetListele();

        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {
            if (txtTc.Text == "")
            {
                txtAdSoyad.Text = "";
                txtTelefon.Text = "";

            }
            cnn.Open();
            SqlCommand komut = new SqlCommand("select * from tblMusteri where Tc like '" + txtTc.Text + "'", cnn);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtAdSoyad.Text = read["adsoyad"].ToString();
                txtTelefon.Text = read["telefon"].ToString();

            }
            cnn.Close();
        }

        private void Temizle()
        {
            if (txtBarkodNo.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktar)
                        {
                            item.Text = "";
                        }
                    }

                }
            }
        }
        private void txtBarkod_TextChanged(object sender, EventArgs e)
        {
            Temizle();
            cnn.Open();
            SqlCommand komut = new SqlCommand("select * from tblUrun where BarkodNo like '" + txtBarkodNo.Text + "'", cnn);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtUrunAdi.Text = read["UrunAdi"].ToString();
                txtSatisFiyati.Text = read["SatisFiyati"].ToString();

            }
            cnn.Close();
        }

        private void FormSatis_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Multiply)
            {
                txtMiktar.Text = txtBarkodNo.Text.Substring(txtBarkodNo.Text.Length - 1);
                txtBarkodNo.Text = "";
            }
        }
        bool durum;

        private void BarkodKontrol()
        {
            durum = true;
            cnn.Open();
            SqlCommand komut = new SqlCommand("select * from tblSepet", cnn);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["BarkodNo"].ToString())
                {
                    durum = false;
                }
            }
            cnn.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            BarkodKontrol();
            if (durum == true)
            {
                cnn.Open();
                SqlCommand komut = new SqlCommand("insert into tblSepet(Tc,AdSoyad,Telefon,BarkodNo,UrunAdi,Miktar,SatisFiyati,ToplamFiyati,Tarih) values(@Tc,@AdSoyad,@Telefon,@BarkodNo,@UrunAdi,@Miktar,@SatisFiyati,@ToplamFiyati,@Tarih)", cnn);
                komut.Parameters.AddWithValue("@Tc", txtTc.Text);
                komut.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@BarkodNo", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@UrunAdi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@Miktar", int.Parse(txtMiktar.Text));
                komut.Parameters.AddWithValue("@SatisFiyati", double.Parse(txtSatisFiyati.Text));
                komut.Parameters.AddWithValue("@ToplamFiyati", double.Parse(txtToplamMiktar.Text));
                komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                cnn.Close();
            }
            else
            {
                cnn.Open();
                SqlCommand komut2 = new SqlCommand("update tblSepet Miktar=miktar+'" + int.Parse(txtMiktar.Text) + "' where BarkodNo='" + txtBarkodNo.Text + "'", cnn);
                komut2.ExecuteNonQuery();
                SqlCommand komut3 = new SqlCommand("update tblSepet ToplamFiyati=Miktar*SatisFiyati where BarkodNo='" + txtBarkodNo.Text + "'", cnn);
                komut3.ExecuteNonQuery();
                cnn.Close();
            }
            
            txtMiktar.Text = "1";
            daset.Tables["tblSepet"].Clear();
            SepetListele();
            Hesapla();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktar)
                    {
                        item.Text = "";
                    }
                }

            }
        }

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamMiktar.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void txtSatisFiyati_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamMiktar.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            cnn.Open();
            SqlCommand komut = new SqlCommand("delete from tblSepet where BarkodNo='" + dataGridView1.CurrentRow.Cells["BarkodNo"].Value.ToString()+"'", cnn);
            komut.ExecuteNonQuery();
            cnn.Close();
            
            MessageBox.Show("Ürün Sepetten Çıkarıldı");
            daset.Tables["tblSepet"].Clear();
            
            SepetListele();
            Hesapla();
        }

        private void btnSatisIptal_Click(object sender, EventArgs e)
        {

            cnn.Open();
            SqlCommand komut = new SqlCommand("delete from tblSepet ", cnn);
            komut.ExecuteNonQuery();
            cnn.Close();
            MessageBox.Show("Ürünler Sepetten Çıkarıldı");
            daset.Tables["tblSepet"].Clear();
            
            SepetListele();
            Hesapla();
        }

        private void btnSatisListe_Click(object sender, EventArgs e)
        {
            FormSatisListele listele = new FormSatisListele();
            listele.ShowDialog();
        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                cnn.Open();
                SqlCommand komut = new SqlCommand("insert into tblSatis(Tc,AdSoyad,Telefon,BarkodNo,UrunAdi,Miktar,SatisFiyati,ToplamFiyati,Tarih) values(@Tc,@AdSoyad,@Telefon,@BarkodNo,@UrunAdi,@Miktar,@SatisFiyati,@ToplamFiyati,@Tarih)", cnn);
                komut.Parameters.AddWithValue("@Tc", txtTc.Text);
                komut.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@BarkodNo", dataGridView1.Rows[i].Cells["BarkodNo"].Value.ToString());
                komut.Parameters.AddWithValue("@UrunAdi", dataGridView1.Rows[i].Cells["UrunAdi"].Value.ToString());
                komut.Parameters.AddWithValue("@Miktar", int.Parse(dataGridView1.Rows[i].Cells["Miktar"].Value.ToString()));
                komut.Parameters.AddWithValue("@SatisFiyati", double.Parse(dataGridView1.Rows[i].Cells["SatisFiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@ToplamFiyati", double.Parse(dataGridView1.Rows[i].Cells["ToplamFiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update tblUrun set Miktar=Miktar-'" + int.Parse(dataGridView1.Rows[i].Cells["Miktar"].Value.ToString()) + "' where BarkodNo='" + dataGridView1.Rows[i].Cells["BarkodNo"].Value.ToString() + "'", cnn);
                komut2.ExecuteNonQuery();
                cnn.Close();
                
             
               
            }
            cnn.Open();
            SqlCommand komut3 = new SqlCommand("delete from tblSepet ", cnn);
            komut3.ExecuteNonQuery();
            cnn.Close();
            
            daset.Tables["tblSepet"].Clear();
            SepetListele();
            Hesapla();
        }
    }
}
