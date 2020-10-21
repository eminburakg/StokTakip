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
    public partial class FormSatisListele : Form
    {
        public FormSatisListele()
        {
            InitializeComponent();
        }
        SqlConnection cnn = new SqlConnection(@"Data source=DESKTOP-87VKS3P\SQLEXPRESS; Initial Catalog=StokTakip;Integrated Security=True");
        DataSet daset = new DataSet();

        private void SatisListele()
        {
            cnn.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from tblSatis", cnn);
            adtr.Fill(daset, "tblSatis");
            dataGridView1.DataSource = daset.Tables["tblSatis"];
            cnn.Close();

        }
        private void FormSatisListele_Load(object sender, EventArgs e)
        {
            SatisListele();
        }
    }
}
