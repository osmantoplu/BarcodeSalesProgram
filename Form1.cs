using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stok_ve_Satış_Takip_Sistemi
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        private void FormAcikmi(Form AcilacakForm)//birden fazla aynı formu açma kontrolü
        {
            bool acikmi = false;
            for (int i = 0; i < this.MdiChildren.Length; i++)
            {
                if (AcilacakForm.Name == MdiChildren[i].Name)
                {
                    this.MdiChildren[i].Focus();
                    acikmi = true;
                }
            }
            if (acikmi == false)
            {
                AcilacakForm.MdiParent = this;
                AcilacakForm.Show();
            }
            else
            {
                AcilacakForm.Dispose();
            }
        }


        PopUp p1 = new PopUp();
        public void talepkontrol()
        {
            int a = 1;
            int c = 0;
            string SiparisAlan = frmGiris.kullanici;
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-OU0MFLR;Initial Catalog=StokSatis;Integrated Security=True");
            conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select * From Sepet WHERE TalepAlan = '" + SiparisAlan + "' and uyari = '" + a + "' ", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            int b = dt.Rows.Count;
            if (b > 0)
            {
                p1.ShowPopup("'" + b + "' tane yeni talebiniz vardır. >> Ürün Talepleri >> Gelen Talep", 450, 85);
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[Sepet] SET [uyari] = '" + c + "' WHERE TalepAlan = '" + SiparisAlan + "' and uyari = '" + a + "' ", conn);
                cmd.ExecuteNonQuery();
                frmGelenTalep f = new frmGelenTalep();
                f.MdiParent = this;
                f.Show();
            }

            conn.Close();
        }
        public string kullaniciAdi { get; set; }
        public string KantinAdi { get; set; }

        private void btnUrunEkle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmUrunler frm = new frmUrunler();
            FormAcikmi(frm);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnStoklar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmStoklar f = new frmStoklar();
            FormAcikmi(f);
        }

        private void btnUrunTalepEt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmTalepler f = new frmTalepler();
            FormAcikmi(f);
        }

        private void btnUrunTalepleri_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmTaleplerim f = new frmTaleplerim();
            FormAcikmi(f);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            talepkontrol();
            timer1.Start();
            timer1.Interval = 3000000;
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source= DESKTOP-OU0MFLR;Initial Catalog=StokSatis;Integrated Security=True");

                SqlCommand kadi = new SqlCommand("SELECT yetki FROM Giris where kullanici_adi = '" + kullaniciAdi + "' ", conn);

                conn.Open();

                string yetki = kadi.ExecuteScalar().ToString();

                if ( yetki == "1")
                {
                    ribbonPage1.Visible = false;
                    ribbonPage4.Visible = false;
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
            
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmGelenTalep f = new frmGelenTalep();
            FormAcikmi(f);
        }
        private void btnKullaniciEkle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmKullanici f = new frmKullanici();
            FormAcikmi(f);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmSatis f = new frmSatis();
            FormAcikmi(f);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
              
            DateTime.Now.ToString();
            DateTime.Now.ToShortTimeString();
            DateTime.Now.ToShortDateString();
            DateTime.Now.ToLongDateString();
            DateTime.Now.ToLongTimeString();

            // timer1.Enabled = true;


            if (timer1.Enabled == true)
            {

                talepkontrol();
            }

    }

        private void btnRaporlar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmUrunlerRapor f = new frmUrunlerRapor();
            FormAcikmi(f);
        }

        private void btnStokRapor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmStokRapor f = new frmStokRapor();
            FormAcikmi(f);
        }

        private void btnSatisRapor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmSatisRapor f = new frmSatisRapor();
            FormAcikmi(f);
        }

        private void btnTalepRapor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmTalepRapor f = new frmTalepRapor();
            FormAcikmi(f);
        }
    }
}
