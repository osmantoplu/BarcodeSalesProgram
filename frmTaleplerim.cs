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
    public partial class frmTaleplerim : Form
    {
        public frmTaleplerim()
        {
            InitializeComponent();
        }
        string SiparisAlan = frmGiris.kullanici;
        public static string SepetID;

        public void GridDoldur()
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From Sepet where TalepEden = '"+SiparisAlan+"' ", conn);
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
        void Temizle()
        {
            tbUrunKod.Clear();
            tbUrunAdi.Clear();
            tbUrunAdet.Clear();
            tbKantinAdi.Clear();
        }
        private void frmTaleplerim_Load(object sender, EventArgs e)
        {
            GridDoldur();
            SepetID = tbSepetID.Text;
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {

            }
            else
            {
                tbUrunKod.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                tbUrunAdi.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                tbUrunAdet.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                tbKantinAdi.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
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
        private bool IfKantinStokExists(SqlConnection con, string ProductCode)
        {

            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Stoklar] WHERE [barkod] ='" + tbUrunKod.Text + "' and KantinAdi = '"+SiparisAlan+"'", con);
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
                if (dataGridView1.SelectedRows[0].Cells[8].Value.ToString() =="Siparis Tamamlandı")
                {
                    MessageBox.Show("Sipariş daha önceden tamamlanmış");
                }
                else if(dataGridView1.SelectedRows[0].Cells[8].Value.ToString() == "Beklemede")
                {
                    MessageBox.Show("Sipariş şu an beklemede");
                }
                else
                {
                    DialogResult secenek = MessageBox.Show("Talebi onaylıyor musunuz? Stoklarınıza " + tbUrunAdet.Text + " adet " + tbUrunAdi.Text + " ürünü eklenecektir!!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (secenek == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE Sepet SET Onay = 'Siparis Tamamlandı'  WHERE ID = '" + textBox1.Text + "'", conn);
                        SqlCommand cmd3 = new SqlCommand("UPDATE Stoklar SET miktar -= '" + tbUrunAdet.Text + "'  WHERE barkod = '" + tbUrunKod.Text + "' and KantinAdi ='" + tbKantinAdi.Text + "'", conn);
                        if (!IfKantinStokExists(conn, tbUrunKod.Text))
                        {
                            SqlCommand cmd1 = new SqlCommand("Insert into Stoklar(barkod,miktar,KantinAdi) values ('" + tbUrunKod.Text + "','0','" + SiparisAlan + "') ", conn);

                            SqlCommand cmd2 = new SqlCommand("UPDATE Stoklar SET miktar += '" + tbUrunAdet.Text + "'  WHERE barkod = '" + tbUrunKod.Text + "' and KantinAdi ='" + SiparisAlan + "'", conn);
                            cmd1.ExecuteNonQuery();
                            cmd2.ExecuteNonQuery();
                        }
                        else
                        {
                            SqlCommand cmd2 = new SqlCommand("UPDATE Stoklar SET miktar += '" + tbUrunAdet.Text + "'  WHERE barkod = '" + tbUrunKod.Text + "' and KantinAdi ='" + SiparisAlan + "'", conn);
                            cmd2.ExecuteNonQuery();
                        }
                        cmd.ExecuteNonQuery();
                        cmd3.ExecuteNonQuery();

                        MessageBox.Show("Ürün aktarımı gerçekleşti");
                        Temizle();
                        conn.Close();
                    }
                    else if(secenek == DialogResult.No)
                    {
                        MessageBox.Show("Talep onayı iptal edildi");
                    }

                }
            }
            else
            {
                MessageBox.Show("Kayıt bulunamadı");

            }
            GridDoldur();
        }

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            frmTalepBilgiRaporu f = new frmTalepBilgiRaporu();
            f.ShowDialog();
        }

        private void tbSepetID_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From Sepet where TalepEden = '" + SiparisAlan + "' and SepetID like '%" + tbSepetID.Text + "%' ", conn);
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
    }
}
