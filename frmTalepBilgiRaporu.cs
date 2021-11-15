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
    public partial class frmTalepBilgiRaporu : Form
    {
        public frmTalepBilgiRaporu()
        {
            InitializeComponent();
        }
        public int SatirSayisi = 0;

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs ffff)
        {
            try
            {   

                frmTaleplerim f = (frmTaleplerim)Application.OpenForms["frmTaleplerim"];

                //ÇİZİM BAŞLANGICI
                Font myFont = new Font("Calibri", 7); //font oluşturduk
                SolidBrush sbrush = new SolidBrush(Color.Black);//fırça oluşturduk
                Pen myPen = new Pen(Color.Black); //kalem oluşturduk

                ffff.Graphics.DrawString("Düzenlenme Tarihi: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(), myFont, sbrush, 50, 25);
                ffff.Graphics.DrawLine(myPen, 50, 45, 770, 45); // Çizgi çizdik... 1. Kalem, 2. X, 3. Y Koordinatı, 4. Uzunluk, 5. BitişX

                myFont = new Font("Calibri", 15, FontStyle.Bold);//Fatura başlığı yazacağımız için fontu kalın yaptık ve puntoyu büyütüp 15 yaptık.
                ffff.Graphics.DrawString("Ürün Listesi", myFont, sbrush, 350, 65);
                ffff.Graphics.DrawLine(myPen, 50, 95, 770, 95); //çizgi çizdik.

                myFont = new Font("Calibri", 10, FontStyle.Bold); //Detay başlığını yazacağımız için fontu kalın yapıp puntoyu 10 yaptık.
                ffff.Graphics.DrawString("SepetID", myFont, sbrush, 50, 110); //Detay başlığı
                ffff.Graphics.DrawString("Ürün Barkod", myFont, sbrush, 110, 110); //Detay başlığı
                ffff.Graphics.DrawString("Ürün Adı", myFont, sbrush, 210, 110); // Detay başlığı
                ffff.Graphics.DrawString("Miktar", myFont, sbrush, 290, 110); //Detay başlığı          
                ffff.Graphics.DrawString("Talep Eden", myFont, sbrush, 360, 110); //Detay başlığı
                ffff.Graphics.DrawString("Talep Alan", myFont, sbrush, 430, 110); //Detay başlığı          
                ffff.Graphics.DrawString("Tarih", myFont, sbrush, 520, 110); //Detay başlığı
                ffff.Graphics.DrawString("Onay Durumu", myFont, sbrush, 660, 110); //Detay başlığı

                ffff.Graphics.DrawLine(myPen, 50, 125, 770, 125); //Çizgi çizdik.

                int y = 150; //y koordinatının yerini belirledik.(Verilerin yazılmaya başlanacağı yer)

                myFont = new Font("Calibri", 10); //fontu 10 yaptık.

                int i = 0;//satır sayısı için değişken tanımladık.
                while (i <= f.dataGridView1.Rows.Count)//döngüyü son satırda sonlandıracağız.
                {
                    //for(int k = 0; k < f.dataGridView1.RowCount; k++)
                    //{
                       // if (SepetID == f.dataGridView1.Rows[i].Cells[0].Value.ToString())
                        //{
                            ffff.Graphics.DrawString(f.dataGridView1[1, i].Value.ToString(), myFont, sbrush, 50, y);//1.sütun
                            ffff.Graphics.DrawString(f.dataGridView1[2, i].Value.ToString(), myFont, sbrush, 110, y);//2.sütun
                            ffff.Graphics.DrawString(f.dataGridView1[3, i].Value.ToString(), myFont, sbrush, 210, y);//3.sütun
                            ffff.Graphics.DrawString(f.dataGridView1[4, i].Value.ToString(), myFont, sbrush, 290, y);//4.sütun           
                            ffff.Graphics.DrawString(f.dataGridView1[5, i].Value.ToString(), myFont, sbrush, 360, y);//5.sütun
                            ffff.Graphics.DrawString(f.dataGridView1[6, i].Value.ToString(), myFont, sbrush, 430, y);//6.sütun
                            ffff.Graphics.DrawString(f.dataGridView1[7, i].Value.ToString(), myFont, sbrush, 520, y);//7.sütun
                            ffff.Graphics.DrawString(f.dataGridView1[8, i].Value.ToString(), myFont, sbrush, 660, y);//8.sütun

                        //}
                    //}
                    y += 20; //y koordinatını arttırdık.
                    i += 1; //satır sayısını arttırdık

                    //yeni sayfaya geçme kontrolü
                    if (y > 1000)
                    {
                        ffff.Graphics.DrawString("(Devamı -->)", myFont, sbrush, 700, y + 50);
                        y = 50;
                        break; //burada yazdırma sınırına ulaştığımız için while döngüsünden çıkıyoruz
                               //çıktığımızda while baştan başlıyor i değişkeni değer almaya devam ediyor
                               //yazdırma yeni sayfada başlamış oluyor
                    }
                }
                //toplam fiyat
                //ffff.Graphics.DrawString(f.tbToplamFiyat.Text, myFont, sbrush, 80, y); 


                //çoklu sayfa kontrolü
                if (i < SatirSayisi)
                {
                    ffff.HasMorePages = true;
                }
                else
                {
                    ffff.HasMorePages = false;
                    i = 0;
                }
                StringFormat myStringFormat = new StringFormat();
                myStringFormat.Alignment = StringAlignment.Far;
            }
            catch
            {

            }
        }

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            DialogResult yazdirmaIslemi;
            yazdirmaIslemi = printDialog1.ShowDialog();
            if (yazdirmaIslemi == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
        string SepetID = frmTaleplerim.SepetID;
        private void frmTalepBilgiRaporu_Load(object sender, EventArgs e)
        {

            printDocument1.DefaultPageSettings.PaperSize = printDocument1.PrinterSettings.PaperSizes[4];


        }
    }
}
