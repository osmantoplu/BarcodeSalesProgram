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
    public partial class frmStoklar : Form
    {
        public frmStoklar()
        {
            InitializeComponent();
        }
        private bool IfStoklarExists(SqlConnection con, string ProductCode)
        {

            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Stoklar] WHERE [barkod] ='" + tbUrunKod.Text + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool IfRecordExist()  // update butonu için
        {
            return true;
        }
        string SiparisVeren = frmGiris.kullanici;
        public string kullaniciAdi { get; set; }

        public void GridDoldur()
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
        void Temizle()
        {
            tbUrunKod.Clear();
            tbUrunAdi.Clear();
            tbUrunAdet.Clear();
        }
        private void frmStoklar_Load(object sender, EventArgs e)
        {
            GridDoldur();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {

            }
            else
            {
                tbUrunKod.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                tbUrunAdi.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                tbUrunAdet.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            conn.Open();
            if (IfStoklarExists(conn, tbUrunKod.Text))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Stoklar SET miktar += '" + tbUrunAdet.Text + "'  WHERE barkod = '" + tbUrunKod.Text + "'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                MessageBox.Show("kayıt bulunamadı");
            }
            Temizle();
            GridDoldur();
        }

        private void btnAzalt_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            conn.Open();
            if (IfStoklarExists(conn, tbUrunKod.Text))
            {
               
               
                    SqlCommand cmd = new SqlCommand("UPDATE Stoklar SET miktar -= '" + tbUrunAdet.Text + "'  WHERE barkod = '" + tbUrunKod.Text + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    if(Convert.ToInt16(tbUrunAdet.Text) > Convert.ToSingle(dataGridView1.SelectedRows[0].Cells[3].Value      ))
                    {
                        MessageBox.Show("Stok eksiye düşemez");
                    }
                
               
            }
            else
            {
                MessageBox.Show("kayıt bulunamadı");
            }
            Temizle();
            GridDoldur();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            conn.Open();
            if (IfStoklarExists(conn, tbUrunKod.Text))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Stoklar SET miktar = '" + tbUrunAdet.Text + "'  WHERE barkod = '" + tbUrunKod.Text + "'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                MessageBox.Show("Güncellemek istediğiniz kayıt bulunamadı");
            }
            Temizle();
            GridDoldur();
        }

        private void tbStokArama_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter(@"SELECT StokID,urun_adi,Urunler.barkod,miktar,KantinAdi FROM Urunler,Stoklar WHERE Urunler.barkod = Stoklar.barkod and KantinAdi ='" + SiparisVeren + "' and urun_adi like '%" + tbStokArama.Text + "%' order by miktar asc ", conn);
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
    }
}
