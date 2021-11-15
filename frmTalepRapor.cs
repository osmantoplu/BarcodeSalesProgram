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
    public partial class frmTalepRapor : Form
    {
        public frmTalepRapor()
        {
            InitializeComponent();
        }
        string SiparisAlan = frmGiris.kullanici;

        void GridDoldur()
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From Sepet where TalepAlan = '" + SiparisAlan + "' order by miktar desc ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns[0].Visible = false;
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ID"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["SepetID"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["Barkod"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["Miktar"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["TalepEden"].ToString();
                dataGridView1.Rows[n].Cells[6].Value = item["TalepAlan"].ToString();
                dataGridView1.Rows[n].Cells[7].Value = item["tarih"].ToString();
                dataGridView1.Rows[n].Cells[8].Value = item["Onay"].ToString();

            }
        }
        private void frmTalepRapor_Load(object sender, EventArgs e)
        {
            GridDoldur();
        }

        private void tbArama_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From Sepet where TalepAlan = '" + SiparisAlan + "' and urun_adi like '%" + tbArama.Text + "%' order by miktar desc ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns[0].Visible = false;
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ID"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["SepetID"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["Barkod"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["Miktar"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["TalepEden"].ToString();
                dataGridView1.Rows[n].Cells[6].Value = item["TalepAlan"].ToString();
                dataGridView1.Rows[n].Cells[7].Value = item["tarih"].ToString();
                dataGridView1.Rows[n].Cells[8].Value = item["Onay"].ToString();

            }

        }

        private void btnYazdır_Click(object sender, EventArgs e)
        {
            frmTalepRaporYazdir f = new frmTalepRaporYazdir();
            f.ShowDialog();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            
        }
    }
}
