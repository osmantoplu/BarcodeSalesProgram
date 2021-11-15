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
    public partial class frmTalepler : Form
    {
      
        public frmTalepler()
        {
            InitializeComponent();
        }
        string SiparisVeren = frmGiris.kullanici;

        public void GridDoldur()
        {
            
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("SELECT urun_adi,Urunler.barkod,miktar,Stoklar.KantinAdi FROM Urunler,Stoklar WHERE Urunler.barkod = Stoklar.barkod and Stoklar.KantinAdi <> '"+SiparisVeren+"' and miktar > 0", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
           
           
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["miktar"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["KantinAdi"].ToString();

            }
        }
        void Temizle()
        {
            tbUrunKod.Clear();
            tbUrunAdi.Clear();
            tbUrunAdet.Clear();
            tbKantinAdi.Clear();
        }
        private void frmTalepler_Load(object sender, EventArgs e)
        {
            GridDoldur();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                tbUrunKod.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                tbUrunAdi.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                tbUrunAdet.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                tbKantinAdi.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();

            }
            catch 
            {
                MessageBox.Show("Lütfen ürün seçiniz");
            }
        }
        private bool IfUrunlerExists(SqlConnection conn, string tbUrunKod)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Stoklar] WHERE [barkod] ='" + tbUrunKod + "'", conn);
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
        private void btnTalep_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            conn.Open();
            string Tarih = Convert.ToDateTime(dtTarih.Value).ToString("dd/MM/yyyy HH:mm");
            Int32 curValue = 0;
            try
            {
               
                Random rand = new Random();
                List<Int32> result = new List<Int32>();
                for (Int32 i = 0; i < 500; i++)
                {
                    curValue = rand.Next(1, 100000);
                    while (result.Exists(value => value == curValue))
                    {
                        curValue = rand.Next(1, 100000);
                    }
                    result.Add(curValue);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    int a = 1;
                    int SepetID = curValue;
                    
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Sepet ([SepetID],[Barkod],[urun_adi],[Miktar],[TalepEden],[TalepAlan],[tarih],[Onay],[uyari])
                    VALUES
                    ('" + SepetID + "','" + dataGridView2.Rows[i].Cells[0].Value + "','" + dataGridView2.Rows[i].Cells[1].Value + "','" + dataGridView2.Rows[i].Cells[2].Value + "','" + SiparisVeren + "','" + dataGridView2.Rows[i].Cells[3].Value + "','" + Tarih + "','" + "Beklemede" + "','" + a + "')", conn);
                    cmd.ExecuteNonQuery();

                }
                MessageBox.Show("Talebiniz alındı.");
                conn.Close();
                dataGridView2.Rows.Clear();




                GridDoldur();
            }
            catch
            {
                MessageBox.Show("Hata oluştu !");
            }
        }

        private void btnSepeteEkle_Click(object sender, EventArgs e)
        {

            String UrunKod = tbUrunKod.Text;
            string UrunAdi = tbUrunAdi.Text;
            int UrunAdet = Convert.ToInt16(tbUrunAdet.Text);
            string KantinAdi = tbKantinAdi.Text;
            if (UrunAdet > Convert.ToInt16(dataGridView1.SelectedRows[0].Cells[2].Value))
            {
                MessageBox.Show("Stok adedinden fazla ürün talep edilemez !");
            }
            else if(UrunAdet <= 0)
            {
                MessageBox.Show("Ürün adedi sıfır ve sıfırdan küçük olamaz !");
            }
            else
            {
                dataGridView2.Rows.Add(UrunKod, UrunAdi, UrunAdet, KantinAdi);
                Temizle();
            }
        }

        private void btnUrunCikar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.RemoveAt(item.Index);
            }
        }

        private void tbArama_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("SELECT urun_adi,Urunler.barkod,miktar,Stoklar.KantinAdi FROM Urunler,Stoklar WHERE Urunler.barkod = Stoklar.barkod and Stoklar.KantinAdi <> '" + SiparisVeren + "' and miktar > 0 and urun_adi like '%" + tbArama.Text + "%'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["miktar"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["KantinAdi"].ToString();

            }

        }
    }
}
