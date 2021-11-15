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
    public partial class frmGiris : Form
    {
        public frmGiris()
        {
            InitializeComponent();
        }
        public static string kullanici;

        private void button1_Click(object sender, EventArgs e)
        {
            frmStoklar fs = new frmStoklar();
            fs.kullaniciAdi = textBox1.Text;
            try
            {
                SqlConnection con = new SqlConnection(@"Data Source= DESKTOP-OU0MFLR;Initial Catalog=StokSatis;Integrated Security=True");
                SqlDataAdapter sda = new SqlDataAdapter(@"SELECT * FROM Giris where kullanici_adi = '" + textBox1.Text + "' and sifre = '" + textBox2.Text + "'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                SqlCommand yetki = new SqlCommand(@"SELECT yetki FROM Giris where kullanici_adi = '" + textBox1.Text + "' ", con);
                con.Open();
                label5.Text = yetki.ExecuteScalar().ToString();
                if (dt.Rows.Count == 1 && label5.Text !="2")
                {
                    SqlConnection conn = new SqlConnection(@"Data Source= DESKTOP-OU0MFLR;Initial Catalog=StokSatis;Integrated Security=True");
                    SqlCommand kadi = new SqlCommand(@"SELECT KantinAdi FROM Giris where kullanici_adi = '" + textBox1.Text + "' ", conn);
                    conn.Open();
                    label3.Text = kadi.ExecuteScalar().ToString();
                    kullanici = label3.Text;
                    label4.Text = textBox1.Text;
                    //this.Hide();                    
                    Form1 f = new Form1();
                    f.kullaniciAdi = textBox1.Text;
                    frmStoklar f1 = new frmStoklar();
                    f1.kullaniciAdi = label4.Text;
                    f.Show();
                }
                else if (label5.Text == "2")
                {
                    MessageBox.Show("Sisteme girişiniz admin tarafından kaldırıldı");

                }
                else
                {
                    MessageBox.Show("Yanlıs kullanıcı adı veya sifre girdiniz", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch(Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
        }
        private void frmGiris_Load(object sender, EventArgs e)
        {

        }
    }
}
