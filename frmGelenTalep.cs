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
    public partial class frmGelenTalep : Form
    {
        public frmGelenTalep()
        {
            InitializeComponent();
        }
        string SiparisAlan = frmGiris.kullanici;

        public void GridDoldur()
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From Sepet WHERE TalepAlan = '"+SiparisAlan+"' and Onay = 'Beklemede' ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            dataGridView1.Columns[8].Visible = false;
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                
                dataGridView1.Rows[n].Cells[0].Value = item["SepetID"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["Barkod"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["urun_adi"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["Miktar"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["TalepEden"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["TalepAlan"].ToString();
                dataGridView1.Rows[n].Cells[6].Value = item["tarih"].ToString();
                dataGridView1.Rows[n].Cells[7].Value = item["Onay"].ToString();
                dataGridView1.Rows[n].Cells[8].Value = item["ID"].ToString();

            }
            

        }
        void Temizle()
        {
            tbUrunKod.Clear();
            tbUrunAdi.Clear();
            tbUrunAdet.Clear();
            tbKantinAdi.Clear();
        }
        private void frmGelenTalep_Load(object sender, EventArgs e)
        {
            GridDoldur();
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Beklemede olan bir talep yok !");
            }
        }
        private bool IfStoklarExists(SqlConnection con, string ProductCode)
        {

            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Sepet] WHERE [barkod] ='" + tbUrunKod.Text + "'", con);
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
        private void btnTalep_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            conn.Open();
         
            if (IfStoklarExists(conn, tbUrunKod.Text))
            {
                int UrunAdet = Convert.ToInt16(tbUrunAdet.Text);
                if (UrunAdet > Convert.ToInt16(dataGridView1.SelectedRows[0].Cells[3].Value))
                {
                    MessageBox.Show("Talep edilen üründen fazla ürün gönderilemez !");
                }
                else if (UrunAdet < 0)
                {
                    MessageBox.Show("Ürün adedi sıfırdan küçük olamaz !");
                }
                else
                {
                    DialogResult secenek = MessageBox.Show("Talebi onaylıyor musunuz? Stoklarınızdan "+tbUrunAdet.Text+" adet "+tbUrunAdi.Text+" ürünü düşülecektir!!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (secenek == DialogResult.Yes)
                    {                       
                            SqlCommand cmd = new SqlCommand("UPDATE Sepet SET Onay = '" + "Onaylandı" + "' , miktar = '" + tbUrunAdet.Text + "' WHERE ID = '" + textBox1.Text + "'", conn);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Talep onaylandı");
                            Temizle();
                            conn.Close();
                    }
                    else if (secenek ==DialogResult.No)
                    {
                        MessageBox.Show("Talep onayı iptal edildi !");
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir kayıt seçiniz");

            }
            GridDoldur();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {

            }
            else
            {
                tbUrunKod.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                tbUrunAdi.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                tbUrunAdet.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                tbKantinAdi.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            }
        }
    }
}
