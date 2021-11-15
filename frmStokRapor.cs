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
    public partial class frmStokRapor : Form
    {
        public frmStokRapor()
        {
            InitializeComponent();
        }
        string SiparisVeren = frmGiris.kullanici;

        void GridDoldur()
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");

            SqlDataAdapter sda = new SqlDataAdapter(@"SELECT StokID,urun_adi,Urunler.barkod,miktar,KantinAdi FROM Urunler,Stoklar WHERE Urunler.barkod = Stoklar.barkod and KantinAdi ='" + SiparisVeren + "' order by miktar asc ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["StokID"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["miktar"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["KantinAdi"].ToString();

            }
        }
        private void frmStokRapor_Load(object sender, EventArgs e)
        {
            GridDoldur();
        }

        private void tbArama_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter(@"SELECT StokID,urun_adi,Urunler.barkod,miktar,KantinAdi FROM Urunler,Stoklar WHERE Urunler.barkod = Stoklar.barkod and KantinAdi ='" + SiparisVeren + "' and urun_adi like '%" + tbArama.Text + "%' order by miktar asc ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["StokID"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["miktar"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["KantinAdi"].ToString();

            }
        }

        private void btnYazdır_Click(object sender, EventArgs e)
        {
            frmStokRaporYazdir f = new frmStokRaporYazdir();
            f.ShowDialog();
        }
    }
}
