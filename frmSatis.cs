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
using Microsoft.PointOfService;

namespace Stok_ve_Satış_Takip_Sistemi
{
    public partial class frmSatis : Form
    {
        Random Rnd = new Random();
       
        public frmSatis()
        {
            InitializeComponent();
        }
        string SatisYapan = frmGiris.kullanici;
        //public static string SepetID;

        private void frmSatis_Load(object sender, EventArgs e)
        {
            //SepetID = KR_Invoices_Spt_ID.Text;
            textBox1.Text = DateTime.Now.Date.ToString().TrimEnd('0', ':');
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlCommand komut = new SqlCommand("SELECT * FROM OzelUrun",conn);
            SqlDataReader dr;
            conn.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                cbOzelUrun.Items.Add(dr["urun_adi"]);
            }
            conn.Close();
        }      
        private void tbBarkod_KeyDown(object sender, KeyEventArgs e)      
        {
            if (e.KeyCode == Keys.Enter)
            {
                SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
                SqlDataAdapter sda = new SqlDataAdapter("SELECT urun_adi,Stoklar.barkod,miktar,fiyati,KantinAdi FROM Urunler,Stoklar WHERE Urunler.barkod = Stoklar.barkod and Stoklar.barkod = '" + tbBarkod.Text + "' and KantinAdi ='" + SatisYapan + "' ", conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                SqlDataAdapter urunKontrol = new SqlDataAdapter("SELECT miktar FROM Stoklar WHERE Stoklar.barkod = '" + tbBarkod.Text + "' and KantinAdi ='" + SatisYapan + "' ", conn);
                DataTable urunTable = new DataTable();
                urunKontrol.Fill(urunTable);
                if (urunTable.Rows.Count == 0)
                {
                    MessageBox.Show("Ürün stoklarda yok !");
                    tbBarkod.Clear();
                }
                else
                {                
                    if (dataGridView1.RowCount == 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            int n = dataGridView1.Rows.Add();

                            dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                            dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                            dataGridView1.Rows[n].Cells[2].Value = Convert.ToInt16(tbMiktar.Text);
                            dataGridView1.Rows[n].Cells[3].Value = item["fiyati"].ToString();

                            tbBarkodBilgi.Text = item["barkod"].ToString();
                            tbUrunAdi.Text = item["urun_adi"].ToString();
                            tbFiyat.Text = item["fiyati"].ToString();


                        }
                    }
                    else
                    {
                        for (int i = 0; i < dataGridView1.RowCount; ++i)
                        {

                            if (tbBarkod.Text == dataGridView1.Rows[i].Cells[0].Value.ToString() && tbMiktar.Text =="1")
                            {
                                dataGridView1.Rows[i].Cells[2].Value = Convert.ToInt16(dataGridView1.Rows[i].Cells[2].Value) + 1;

                            }
                            else if(tbBarkod.Text == dataGridView1.Rows[i].Cells[0].Value.ToString() && tbMiktar.Text != "1")
                            {
                                dataGridView1.Rows[i].Cells[2].Value = Convert.ToInt16(dataGridView1.Rows[i].Cells[2].Value) + Convert.ToInt16(tbMiktar.Text);

                            }
                            else
                            {
                                foreach (DataRow item in dt.Rows)
                                {
                                    int n = dataGridView1.Rows.Add();

                                    dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                                    dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                                    dataGridView1.Rows[n].Cells[2].Value = Convert.ToInt16(tbMiktar.Text);
                                    dataGridView1.Rows[n].Cells[3].Value = item["fiyati"].ToString();

                                    tbBarkodBilgi.Text = item["barkod"].ToString();
                                    tbUrunAdi.Text = item["urun_adi"].ToString();
                                    tbFiyat.Text = item["fiyati"].ToString();


                                }
                            }
                        }
                    }
                    //toplam fiyat hesaplama
                        double toplam = 0;

                        for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                        {
                            toplam += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) * Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);

                        }
                        tbToplamFiyat.Text = toplam.ToString();
                        //toplam fiyat bitis


                        tbBarkod.Clear();

                        tbBarkod.Select();

                        tbBarkod.Focus();

                        tbMiktar.Text = "1";
                    
                }
            }
        }

        private void btnUrunCikar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
            float toplam = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                toplam -= Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value);
            }
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                tbBarkodBilgi.Text = "";
                tbUrunAdi.Text = "";
                tbFiyat.Text = "";
                tbToplamFiyat.Text = "";

            }
        }
        private void tbMiktar_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnSatisİptal_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            tbBarkodBilgi.Clear();
            tbUrunAdi.Clear();
            tbFiyat.Clear();
            tbToplamFiyat.Clear();
            tbBarkod.Focus();
        }

        private void tbMiktar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbMiktar.Text.Length > 2)
                {
                    MessageBox.Show("Barkod kısmına okutunuz !");
                    tbBarkod.Focus();
                    tbMiktar.Text = "1";
                }
            }
        }
        private bool IfStoklarExists(SqlConnection conn, string Barkod , string SatisYapan)
        {

            SqlDataAdapter sda = new SqlDataAdapter("Select 1 From [Stoklar] WHERE [barkod] ='" + Barkod + "' and KantinAdi ='"+SatisYapan+"'", conn);
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
       
        private void btnSatis_Click(object sender, EventArgs e)
        {
            try
            {
                string RandomValue = DateTime.Now.ToShortTimeString().Replace(":", "") + Rnd.Next(1, 100000).ToString() + DateTime.Now.Second.ToString();
                if (dataGridView1.RowCount == 0)
                {
                    MessageBox.Show("Lütfen satış yapabilmek için ürün ekleyin");
                }
                else
                {


                    SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
                    conn.Open();
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        tbGridFiyat.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        string Tarih = Convert.ToDateTime(dtTarih.Value).ToString("dd/MM/yyyy HH:mm");
                        if (IfStoklarExists(conn, dataGridView1.Rows[i].Cells[0].Value.ToString(),SatisYapan))
                        {
                            KR_Invoices_Spt_ID.Text = RandomValue;
                            SqlCommand StsSptCmd = new SqlCommand(@"INSERT INTO Satis_Sepet ([Sepet_ID],[Barkod],[UrunAdi],[Miktar],[Fiyat],[Tarih],[KantinAdi])
                        VALUES
                        ('"+RandomValue+"','" + dataGridView1.Rows[i].Cells[0].Value + "','" + dataGridView1.Rows[i].Cells[1].Value + "','" + dataGridView1.Rows[i].Cells[2].Value + "','" + tbGridFiyat.Text + "','" + Tarih + "','" + SatisYapan + "')", conn);
                            // Satış Sepet Ekleme
                            StsSptCmd.ExecuteNonQuery();

                            SqlCommand cmd = new SqlCommand(@"INSERT INTO Satislar ([barkod],[urun_adi],[miktar],[fiyati],[tarih],[KantinAdi])
                        VALUES
                        ('" + dataGridView1.Rows[i].Cells[0].Value + "','" + dataGridView1.Rows[i].Cells[1].Value + "','" + dataGridView1.Rows[i].Cells[2].Value + "','" + tbGridFiyat.Text + "','" + Tarih + "','" + SatisYapan + "')", conn);
                            cmd.ExecuteNonQuery();
                            //ozel ürün
                            SqlCommand cmd2 = new SqlCommand("UPDATE Stoklar SET miktar -= '" + dataGridView1.Rows[i].Cells[2].Value + "'  WHERE barkod = '" + dataGridView1.Rows[i].Cells[0].Value + "' and KantinAdi = '" + SatisYapan + "'", conn);
                            cmd2.ExecuteNonQuery();
                            KR_PrintInvoice f = new KR_PrintInvoice();
                            f.Sepet_ID = KR_Invoices_Spt_ID.Text;
                        }
                        else
                        {                   

                            SqlCommand cmd = new SqlCommand(@"INSERT INTO Satislar ([barkod],[urun_adi],[miktar],[fiyati],[tarih],[KantinAdi])
                        VALUES
                        ('" + dataGridView1.Rows[i].Cells[0].Value + "','" + dataGridView1.Rows[i].Cells[1].Value + "','" + dataGridView1.Rows[i].Cells[2].Value + "','" + tbGridFiyat.Text + "','" + Tarih + "','" + SatisYapan + "')", conn);
                            cmd.ExecuteNonQuery();
                            KR_Invoices_Spt_ID.Text = RandomValue;
                            SqlCommand StsSptCmd = new SqlCommand(@"INSERT INTO Satis_Sepet ([Sepet_ID],[Barkod],[UrunAdi],[Miktar],[Fiyat],[Tarih],[KantinAdi])
                        VALUES
                        ('" + RandomValue + "','" + dataGridView1.Rows[i].Cells[0].Value + "','" + dataGridView1.Rows[i].Cells[1].Value + "','" + dataGridView1.Rows[i].Cells[2].Value + "','" + tbGridFiyat.Text + "','" + Tarih + "','" + SatisYapan + "')", conn);
                            // Satış Sepet Ekleme
                            StsSptCmd.ExecuteNonQuery();
                            KR_PrintInvoice f = new KR_PrintInvoice();
                            f.Sepet_ID = KR_Invoices_Spt_ID.Text;
                        }

                    }
                    conn.Close();
                    // frmFaturaYazdir f = new frmFaturaYazdir();
                    //f.ShowDialog();
                    KR_PrintInvoice Frm = new KR_PrintInvoice();
                    Frm.Sepet_ID = KR_Invoices_Spt_ID.Text;
                    Frm.ShowDialog();
                    dataGridView1.Rows.Clear();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch
            {
                MessageBox.Show("Bir hata oluştu, stok bitmiş olabilir");
            }
           
        }
        private void tbGridFiyat_EditValueChanged(object sender, EventArgs e)
        {
            tbGridFiyat.Text = tbGridFiyat.Text.Replace(',', '.');

        }       
        private void btnOzelUrunIslemleri_Click(object sender, EventArgs e)
        {
            frmOzelUrun f = new frmOzelUrun();
            f.Show();
        }

        private void btnOzelUrunEkle_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI");
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM OzelUrun Where urun_adi = '"+cbOzelUrun.SelectedItem+"'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            conn.Open();
            for (int i = 0; i < dataGridView1.RowCount; ++i)
            {
                if (cbOzelUrun.Text == dataGridView1.Rows[i].Cells[1].Value.ToString())
                {
                    dataGridView1.Rows[i].Cells[2].Value = Convert.ToInt16(dataGridView1.Rows[i].Cells[2].Value) + 1;

                }

                else
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = item["barkod"].ToString();
                        dataGridView1.Rows[n].Cells[1].Value = item["urun_adi"].ToString();
                        dataGridView1.Rows[n].Cells[2].Value = "1";
                        dataGridView1.Rows[n].Cells[3].Value = item["fiyati"].ToString();

                        tbBarkodBilgi.Text = item["barkod"].ToString();
                        tbUrunAdi.Text = item["urun_adi"].ToString();
                        tbFiyat.Text = item["fiyati"].ToString();
                    }
                }

            }           
            


            double toplam = 0;

            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                toplam += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) * Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);

            }
            tbToplamFiyat.Text = toplam.ToString();

            conn.Close();
        }

    }
}