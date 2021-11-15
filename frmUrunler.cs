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
    public partial class frmUrunler : Form
    {
        public frmUrunler()
        {
            InitializeComponent();
        }
        
        void GridDoldur()
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter da = new SqlDataAdapter("Select barkod,urun_adi,fiyati,aciklama from Urunler", conn);
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
        private void Form2_Load(object sender, EventArgs e)
        {   
            GridDoldur();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {                 
                    int miktar = 0;
                    string depo = "Depo";

                    SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
                    SqlCommand cmd = new SqlCommand("insert into Urunler(barkod,urun_adi,fiyati,aciklama) values ('" + tbBarkod.Text + "','" + tbUrunAdi.Text + "','" + tbFiyati.Text + "','" + tbAciklama.Text + "')");
                    conn.Open();
                    cmd.Connection = conn;
                    SqlCommand cmd2 = new SqlCommand("insert into Stoklar(barkod,miktar,KantinAdi) values ('" + tbBarkod.Text + "','" + miktar + "','" + depo + "')");
                    cmd2.Connection = conn;
                    cmd.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    conn.Close();
                    GridDoldur();
                        
            }
            catch(ArithmeticException ar)
            {
                MessageBox.Show(ar.Message);
            }
            catch(FormatException f)
            {
                MessageBox.Show(f.Message);
            }
            catch(SqlException ex)
            {
                if (ex.ErrorCode == -2146232060)
                {
                    MessageBox.Show("Girmiş olduğunuz kayıt kullanılmakta!!!");
                }
            }
            finally
            {
                tbBarkod.Text = "";
                tbUrunAdi.Text = "";
                tbFiyati.Text = "";
                tbAciklama.Text = "";

            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            tbBarkod.Text =   dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            tbUrunAdi.Text =  dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            tbFiyati.Text =   dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            tbAciklama.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
                conn.Open();
                if (IfUrunlerExists(conn, tbBarkod.Text))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE [dbo].[Urunler] SET [urun_adi] = '" + tbUrunAdi.Text + "',[fiyati] = '" + tbFiyati.Text + "',[aciklama] = '" + tbAciklama.Text + "' WHERE [barkod] = '" + tbBarkod.Text + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Güncellemek istediğiniz kayıt bulunamadı");

                }
                GridDoldur();
            }
            catch
            {
                MessageBox.Show("Hata oluştu");
            }
            finally
            {
                tbBarkod.Text = "";
                tbUrunAdi.Text = "";
                tbFiyati.Text = "";
                tbAciklama.Text = "";

            }
        }

        private bool IfUrunlerExists(SqlConnection conn, string text)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Urunler] WHERE [barkod] ='" + tbBarkod.Text + "'", conn);
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

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");

                var sqlQuery = "";

                DialogResult secenek = MessageBox.Show("Kaydı silmek istiyor musunuz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (secenek == DialogResult.Yes)
                {
                    if (IfUrunlerExists(conn, tbBarkod.Text))
                    {
                        conn.Open();
                        sqlQuery = @"DELETE FROM Urunler WHERE [barkod] = '" + tbBarkod.Text + "'";
                        SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Kayıt silindi");
                    }
                    else
                    {
                        MessageBox.Show("Silmek istediğiniz kayıt bulunamadı !");
                    }

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
            finally
            {
                tbBarkod.Text = "";
                tbUrunAdi.Text = "";
                tbFiyati.Text = "";
                tbAciklama.Text = "";

            }

        }

        private void tbFiyati_EditValueChanged(object sender, EventArgs e)
        {
            tbFiyati.Text = tbFiyati.Text.Replace(',', '.');
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
    }
}
