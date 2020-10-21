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
    public partial class FormMusteriEkle : Form
    {
        public FormMusteriEkle()
        {
            InitializeComponent();
        }
        SqlConnection cnn = new SqlConnection(@"Data source=DESKTOP-87VKS3P\SQLEXPRESS; Initial Catalog=StokTakip;Integrated Security=True");

        private void btnEkle_Click(object sender, EventArgs e)
        {
            cnn.Open();
            SqlCommand command = new SqlCommand("insert into tblMusteri(Tc,AdSoyad,Telefon,Adres,Email) values(@tc,@adsoyad,@telefon,@adres,@email)", cnn);
            command.Parameters.AddWithValue("@tc", txtTc.Text);
            command.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
            command.Parameters.AddWithValue("@telefon", txtTelefon.Text);
            command.Parameters.AddWithValue("@adres", txtAdres.Text);
            command.Parameters.AddWithValue("@email", txtEmail.Text);

            command.ExecuteNonQuery();
            cnn.Close();
            MessageBox.Show("Müşteri kaydı eklendi");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void FormMusteriEkle_Load(object sender, EventArgs e)
        {

        }
    }
}
