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
    public partial class frmKullanici : Form
    {
        public frmKullanici()
        {
            InitializeComponent();
        }
        void GridDoldur()
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter da = new SqlDataAdapter("Select * from Giris", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.Rows.Clear();            
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["kullanici_adi"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["sifre"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["KantinAdi"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["yetki"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["ID"].ToString();

            }
        }
        void Temizle()
        {
            tbKullaniciAdi.Text = "";
            tbSifre.Text = "";
            tbKantinAdi.Text = "";
        }
        private void frmKullanici_Load(object sender, EventArgs e)
        {
            GridDoldur();
            cbYetki.SelectedIndex = 1;
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {

            }
            else
            {
                tbKullaniciAdi.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                tbSifre.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                tbKantinAdi.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                cbYetki.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
                SqlCommand cmd = new SqlCommand("insert into Giris(kullanici_adi,sifre,KantinAdi,yetki) values ('" + tbKullaniciAdi.Text + "','" + tbSifre.Text + "','" + tbKantinAdi.Text + "','" + cbYetki.SelectedValue + "')", conn);
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

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            
            try
            {
                SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
                conn.Open();
                
                    if (dataGridView1.SelectedRows[0].Cells[3].Value.ToString() == "0")
                    {
                        MessageBox.Show("Tam yetkili kişinin bilgileri değiştirilemez");
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE Giris SET [kullanici_adi] = '" + tbKullaniciAdi.Text + "' ,[sifre] = '" + tbSifre.Text + "',[KantinAdi] = '" + tbKantinAdi.Text + "',[yetki] = '"+cbYetki.SelectedItem+"' WHERE [ID] = '" + textBox1.Text + "'", conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                
                
                Temizle();
                GridDoldur();
            }
            catch
            {
                MessageBox.Show("Hatalı giriş yaptınız");
            }           
        }
        private bool IfKullaniciExists(SqlConnection conn, string text)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Giris] WHERE [KantinAdi] ='" + tbKantinAdi.Text + "'", conn);
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


    }
}
