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
    public partial class frmOzelUrun : Form
    {
        public frmOzelUrun()
        {
            InitializeComponent();
        }
        string SatisYapan = frmGiris.kullanici;
        void GridDoldur()
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter da = new SqlDataAdapter("Select * from OzelUrun where KantinAdi = '"+SatisYapan+"'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.Rows.Clear();            
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["barkod"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["fiyati"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["ID"].ToString();

            }
        }
        void Temizle()
        {
            tbUrunAdi.Text = "";
            tbFiyati.Text = "";
        }
        private void frmOzelUrun_Load(object sender, EventArgs e)
        {
            GridDoldur();
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            string Barkod = "";

            try
            {
                if (tbUrunAdi.Text.Length >= 3)
                {
                   Barkod = tbUrunAdi.Text.Substring(0, 3) + "00000";
                }

                else if (tbUrunAdi.Text.Length == 2)
                {
                    Barkod = tbUrunAdi.Text.Substring(0, 2) + "00000";
                }

                else
                {
                    Barkod = tbUrunAdi.Text.Substring(0, 1) + "00000";
                }
                SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
                SqlCommand cmd = new SqlCommand("insert into OzelUrun(urun_adi,barkod,fiyati,KantinAdi) values ('" + tbUrunAdi.Text + "','"+Barkod+"','" + tbFiyati.Text + "','" + SatisYapan + "')", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                Temizle();
                GridDoldur();
            }
            catch
            {
                MessageBox.Show("Hatalı giriş yaptınız !");
            }
        }

        private void tbFiyati_EditValueChanged(object sender, EventArgs e)
        {
            tbFiyati.Text = tbFiyati.Text.Replace(',', '.');
        }
        private bool IfUrunExists(SqlConnection conn, string text)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [OzelUrun] WHERE [urun_adi] ='" + tbUrunAdi.Text + "'", conn);
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
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
                conn.Open();
                if (IfUrunExists(conn, tbUrunAdi.Text))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE OzelUrun SET [urun_adi] = '" + tbUrunAdi.Text + "' ,[fiyati] = '" + tbFiyati.Text + "' WHERE [ID] = '" + textBox1.Text + "'", conn);
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
            catch
            {
                MessageBox.Show("Hatalı giriş yaptınız");
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
                DialogResult secenek = MessageBox.Show("Kaydı silmek istiyor musunuz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (secenek == DialogResult.Yes)
                {
                    if (IfUrunExists(conn, tbUrunAdi.Text))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM OzelUrun WHERE ID = '" + textBox1.Text + "'", conn);
                        cmd.ExecuteNonQuery();
                        Temizle();
                        GridDoldur();
                        conn.Close();
                        MessageBox.Show("Kayıt silindi");

                    }
                    else if (dataGridView1.Rows.Count == 0)
                    {
                        MessageBox.Show("Silmek istediğiniz kayıt bulunamadı !");
                    }
                    else
                    {
                        MessageBox.Show("Hata oluştu");
                    }
                    Temizle();
                    GridDoldur();
                }
                else if (secenek == DialogResult.No)
                {
                    MessageBox.Show("Silme işlemi iptal edilmiştir");
                }
            }
            catch
            {
                MessageBox.Show("Hata oluştu");
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {

            }
            else
            {
                tbUrunAdi.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                tbFiyati.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            }
        }

        private void tbUrunAdi_EditValueChanged(object sender, EventArgs e)
        {
            tbUrunAdi.Text = tbUrunAdi.Text.ToUpper();
            tbUrunAdi.SelectionStart = tbUrunAdi.Text.Length;
        }
    }
}
