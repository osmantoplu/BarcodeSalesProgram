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
    public partial class frmUrunlerRapor : Form
    {
        public frmUrunlerRapor()
        {
            InitializeComponent();
        }
        public int SatirSayisi = 0;

        void GridDoldur()
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter da = new SqlDataAdapter("Select * from Urunler", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["fiyati"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["aciklama"].ToString();
            }
        }
        private void frmUrunlerRapor_Load(object sender, EventArgs e)
        {
            GridDoldur();
        }

        private void tbArama_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter da = new SqlDataAdapter("Select * from Urunler where urun_adi like '%" + tbArama.Text + "%'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["fiyati"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["aciklama"].ToString();
            }
        }


        private void btnYazdır_Click(object sender, EventArgs e)
        {
            frmUrunRaporYazdir f = new frmUrunRaporYazdir();
            f.ShowDialog();

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK) //Pencerede kayıt düğmesine basıldıysa
            {
                
            }
        }
    }
}
