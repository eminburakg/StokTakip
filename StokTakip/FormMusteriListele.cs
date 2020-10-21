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
    public partial class FormMusteriListele : Form
    {
        public FormMusteriListele()
        {
            InitializeComponent();
        }
        SqlConnection cnn = new SqlConnection(@"Data source=DESKTOP-87VKS3P\SQLEXPRESS; Initial Catalog=StokTakip;Integrated Security=True");
        DataSet daset = new DataSet();
        
        public void Kayit_Goster()
        {
            cnn.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from tblMusteri", cnn);
            adtr.Fill(daset, "tblMusteri");
            dataGridView1.DataSource = daset.Tables["tblMusteri"];
            cnn.Close();
        }

        private void FormMusteriListele_Load(object sender, EventArgs e)
        {
            Kayit_Goster();
        }
        
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTc.Text = dataGridView1.CurrentRow.Cells["Tc"].Value.ToString();
            txtAdSoyad.Text = dataGridView1.CurrentRow.Cells["Adsoyad"].Value.ToString();
            txtTelefon.Text = dataGridView1.CurrentRow.Cells["Telefon"].Value.ToString();
            txtAdres.Text = dataGridView1.CurrentRow.Cells["Adres"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["Email"].Value.ToString();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            cnn.Open();
            SqlCommand con = new SqlCommand("update tblMusteri set AdSoyad=@adsoyad,Telefon=@telefon,Adres=@adres,Email=@email where Tc=@tc",cnn);
            con.Parameters.AddWithValue("@tc", txtTc.Text);
            con.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
            con.Parameters.AddWithValue("@telefon", txtTelefon.Text);
            con.Parameters.AddWithValue("@adres", txtAdres.Text);
            con.Parameters.AddWithValue("@email", txtEmail.Text);

            con.ExecuteNonQuery();
            cnn.Close();
            daset.Tables["tblMusteri"].Clear();
            Kayit_Goster(); 
            MessageBox.Show("Müşteri kaydı güncellendi");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            cnn.Open();
            SqlCommand con = new SqlCommand("delete from tblMusteri where Tc='" + dataGridView1.CurrentRow.Cells["Tc"].Value.ToString()+"'", cnn);
            con.ExecuteNonQuery();
            cnn.Close();
            daset.Tables["tblMusteri"].Clear();
            Kayit_Goster();
            MessageBox.Show("Kayıt Silindi");
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tbl = new DataTable();
            cnn.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from tblMusteri where Tc like '%"+txtAra.Text+ "%'", cnn);
            adtr.Fill(tbl);
            dataGridView1.DataSource = tbl;
            cnn.Close();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

