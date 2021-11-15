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

namespace Stok_ve_Satış_Takip_Sistemi
{
    public partial class frmSatisRapor : Form
    {
        public frmSatisRapor()
        {
            InitializeComponent();
        }
        string SiparisVeren = frmGiris.kullanici;

        void GridDoldur()
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * from Satislar where KantinAdi = '"+SiparisVeren+"' ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["miktar"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["fiyati"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["tarih"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["KantinAdi"].ToString();


            }
        }
        private void frmSatisRapor_Load(object sender, EventArgs e)
        {
            GridDoldur();
        }

        private void tbArama_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * from Satislar where KantinAdi = '" + SiparisVeren + "' and urun_adi like '%" + tbArama.Text + "%' ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["miktar"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["fiyati"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["tarih"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["KantinAdi"].ToString();


            }
        }

        private void btnYazdır_Click(object sender, EventArgs e)
        {
            frmSatisRaporYazdir f = new frmSatisRaporYazdir();
            f.ShowDialog();
        }

        private void btnGunluk_Click(object sender, EventArgs e)
        {
            string tarih1 = Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy");
            string tarih2 = Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy HH:mm");
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * from Satislar where tarih between '" + tarih1 + "' and '" + tarih2 + "'  ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            tbArama.Enabled = false;
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();

                dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["miktar"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["fiyati"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["tarih"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["KantinAdi"].ToString();


            }
        }

        private void btnHaftalik_Click(object sender, EventArgs e)
        {
          
            string tarih1 = Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy");
            string tarih2 = Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy HH:mm");
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("select* from Satislar where tarih between DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0) and '"+tarih2+"' ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            tbArama.Enabled = false;
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();

                dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["miktar"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["fiyati"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["tarih"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["KantinAdi"].ToString();


            }

        }

        private void btnAylik_Click(object sender, EventArgs e)
        {
            string tarih1 = Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy");
            string tarih2 = Convert.ToDateTime(dateTimePicker1.Value).ToString("dd/MM/yyyy HH:mm");
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("select* from Satislar where tarih between CONVERT(VARCHAR(10),DATEADD(dd,-(DAY(GETDATE())-1),GETDATE()),104) and '" + tarih2 + "' ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            tbArama.Enabled = false;
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();

                dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["miktar"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["fiyati"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["tarih"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["KantinAdi"].ToString();


            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
