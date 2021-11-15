using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;

namespace Stok_ve_Satış_Takip_Sistemi
{
    public partial class KR_PrintInvoice : Form
    {
        string Spt_ID = "";
        frmSatis SatisFrm = new frmSatis();
        float Toplam = 0;
        public KR_PrintInvoice()
        {
            InitializeComponent();
        }
        public string Sepet_ID { get; set; }

        public void ReportViewDoldur_yenile()
        {
            try
            {

                this.reportViewer1.Reset();
                this.reportViewer1.LocalReport.ReportPath =
                 (Application.StartupPath + "\\SatisFis.rdlc");
                DataTable tbl = new DataTable();
                tbl = DTOlustur();
                ReportDataSource rds = new ReportDataSource("DataSet1", tbl);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(rds);
                this.reportViewer1.LocalReport.Refresh();
                this.reportViewer1.RefreshReport();
                ReportParameter p1 = new ReportParameter("Toplam", Toplam.ToString());
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1 });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.ToString());
            }
        }

        private void KR_PrintInvoice_Load(object sender, EventArgs e)
        {
            Spt_ID = Sepet_ID;
            MessageBox.Show(Spt_ID);

            ReportViewDoldur_yenile();
        }
        

        private DataTable DTOlustur()
        {
            string Barkod = "";
            string UrunAdi = "";
            string Miktar = "";
            string Fiyat = "";
            string Tarih = "";
            string KantinAdi = "";
            DataTable table = new DataTable();
            DataColumn col1 = new DataColumn("Barkod");
            DataColumn col2 = new DataColumn("UrunAdi");
            DataColumn col3 = new DataColumn("Miktar");
            DataColumn col4 = new DataColumn("Fiyat");
            DataColumn col5 = new DataColumn("Tarih");
            DataColumn col6 = new DataColumn("KantinAdi");
            col1.DataType = System.Type.GetType("System.String");
            col2.DataType = System.Type.GetType("System.String");
            col3.DataType = System.Type.GetType("System.String");
            col4.DataType = System.Type.GetType("System.String");
            col5.DataType = System.Type.GetType("System.String");
            col5.DataType = System.Type.GetType("System.String");
            table.Columns.Add(col1);
            table.Columns.Add(col2);
            table.Columns.Add(col3);
            table.Columns.Add(col4);
            table.Columns.Add(col5);
            table.Columns.Add(col6);


            using (SqlConnection con = new SqlConnection("Data Source = DESKTOP-OU0MFLR; Initial Catalog=StokSatis; User Id=sa; password = 1; Integrated Security=SSPI"))
            {
                con.Open();
                SqlCommand commandy = new SqlCommand("select * from Satis_Sepet Where Sepet_ID = '" + Spt_ID + "'", con);
                SqlDataReader BaseData = commandy.ExecuteReader();
                while (BaseData.Read())
                {
                    Barkod = BaseData["Barkod"].ToString();
                    UrunAdi = BaseData["UrunAdi"].ToString();
                    Miktar = BaseData["Miktar"].ToString();
                    Fiyat = BaseData["Fiyat"].ToString();
                    Tarih = BaseData["Tarih"].ToString();
                    KantinAdi = BaseData["KantinAdi"].ToString();
                    DataRow row = table.NewRow();
                    row[col1] = Barkod;
                    row[col2] = UrunAdi;
                    row[col3] = Miktar;
                    row[col4] = Fiyat;
                    row[col5] = Tarih;
                    row[col6] = KantinAdi;
                    float TToplam = Convert.ToSingle(Fiyat) * Convert.ToSingle(Miktar);
                    Toplam += TToplam;

                    table.Rows.Add(row);
                }
                BaseData.Close();
                con.Close();
            }



            return table;
        }
    }
}
