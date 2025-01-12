﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TicariOtomasyon
{
    public partial class FrmBankalar : Form
    {
        public FrmBankalar()
        {
            InitializeComponent();
        }
        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Execute BankaBilgileri", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }
        private void FrmBankalar_Load(object sender, EventArgs e)
        {
            listele();
            SehirListesi();
            firmaListesi();
            temizle();
        }
        void temizle()
        {
            txtID.Text = "";
            txtBankaAdi.Text = "";
            cmbIl.Text = "";
            cmbIlce.Text = "";
            txtSube.Text = "";
            mskIban.Text = "";
            mskHno.Text = "";
            txtYetkili.Text = "";
            mskTelefon.Text = "";
            mskTarih.Text = "";
            txtHesapTürü.Text = "";
            lookFirma.Text = "";

        }
        void firmaListesi()
        {
            // firma comboboxsu için
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select ID,AD from TBL_FIRMALAR",bgl.baglanti());
            da.Fill(dt);
            lookFirma.Properties.ValueMember = "ID"; // id değerini alsın bize gözükmeyecek olan alan
            lookFirma.Properties.DisplayMember = "AD"; // firmanın ADI bize gözükecek olan alan
            lookFirma.Properties.DataSource = dt; // dt den gelen değer içine attım



        }
        void SehirListesi()
        {
            SqlCommand komut = new SqlCommand("Select SEHIR from TBL_İLLER", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader(); //verileri okutacaz sonra komut ile ilişkilendirdik
            while (dr.Read())//okuma işlemi sürdüğü sürece
            {
                cmbIl.Properties.Items.Add(dr[0]);
            }
            bgl.baglanti().Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("insert into TBL_BANKALAR(BANKAADI,IL,ILCE,SUBE,IBAN,HESAPNO,YETKILI,TELEFON,TARIH,HESAPTURU,FIRMAID)" +
                "VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1",txtBankaAdi.Text);
            komut.Parameters.AddWithValue("@p2",cmbIl.Text);
            komut.Parameters.AddWithValue("@p3",cmbIlce.Text);
            komut.Parameters.AddWithValue("@p4",txtSube.Text);
            komut.Parameters.AddWithValue("@p5",mskIban.Text);
            komut.Parameters.AddWithValue("@p6",mskHno.Text);
            komut.Parameters.AddWithValue("@p7",txtYetkili.Text);
            komut.Parameters.AddWithValue("@p8",mskTelefon.Text);
            komut.Parameters.AddWithValue("@p9", mskTarih.Text);
            komut.Parameters.AddWithValue("@p10", txtHesapTürü.Text);
            komut.Parameters.AddWithValue("@p11", lookFirma.EditValue);
            komut.ExecuteNonQuery();
            listele();
            bgl.baglanti().Close();
            MessageBox.Show("Banka bilgileri eklendi ","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void cmbIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // şimdi il comboboxsında herhangi bir değişikilk olduğunda ne olsun onu yapcaz
            //yani seçilen ile göre ilçe combosu otomatik o ile ait  ilçeleri getirmesi için

            cmbIlce.Properties.Items.Clear(); //seçildikten sonra comboyu temizle
            SqlCommand komut = new SqlCommand("Select ILCE from TBL_İLCELER where SEHİR=@p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", cmbIl.SelectedIndex + 1);//parametre  il combosundan seçilen index değeri diyoruz /+1 0 dan başladığı için biz 1 den başlamasını istiyoruz
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read()) // dr nesnesi okmayı yaptığı mütdetçe ilçeyi doldurcak
            {
                cmbIlce.Properties.Items.Add(dr[0]);
            }
            bgl.baglanti().Close();

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
          if(dr!= null)
            {
                txtID.Text = dr["ID"].ToString();
                txtBankaAdi.Text = dr["BANKAADI"].ToString();
                cmbIl.Text = dr["IL"].ToString();
                cmbIlce.Text = dr["ILCE"].ToString();
                txtSube.Text = dr["SUBE"].ToString();
                mskIban.Text = dr["IBAN"].ToString();
                mskHno.Text = dr["HESAPNO"].ToString();
                txtYetkili.Text = dr["YETKILI"].ToString();
                mskTelefon.Text = dr["TELEFON"].ToString();
                mskTarih.Text = dr["TARIH"].ToString();
                txtHesapTürü.Text = dr["HESAPTURU"].ToString();
  
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            SqlCommand sil = new SqlCommand("delete from TBL_BANKALAR where ID=@p1",bgl.baglanti());
            sil.Parameters.AddWithValue("@p1",txtID.Text);
            sil.ExecuteNonQuery();
            bgl.baglanti().Close();
            listele();
            temizle();
            MessageBox.Show("Banka bilgisi silindi ","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Hand);
        }
      
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("update TBL_BANKALAR set BANKAADI=@p1,IL=@p2,ILCE=@p3,SUBE=@p4,IBAN=@p5,HESAPNO=@p6,YETKILI=@p7,TELEFON=@p8," +
                "TARIH=@p9,HESAPTURU=@p10,FIRMAID=@p11 where ID=@p12",bgl.baglanti());

            komut.Parameters.AddWithValue("@p1", txtBankaAdi.Text);
            komut.Parameters.AddWithValue("@p2", cmbIl.Text);
            komut.Parameters.AddWithValue("@p3", cmbIlce.Text);
            komut.Parameters.AddWithValue("@p4", txtSube.Text);
            komut.Parameters.AddWithValue("@p5", mskIban.Text);
            komut.Parameters.AddWithValue("@p6", mskHno.Text);
            komut.Parameters.AddWithValue("@p7", txtYetkili.Text);
            komut.Parameters.AddWithValue("@p8", mskTelefon.Text);
            komut.Parameters.AddWithValue("@p9", mskTarih.Text);
            komut.Parameters.AddWithValue("@p10", txtHesapTürü.Text);
            komut.Parameters.AddWithValue("@p11", lookFirma.EditValue);
            komut.Parameters.AddWithValue("@p12",txtID.Text);
            komut.ExecuteNonQuery();
            listele();
            bgl.baglanti().Close();
            MessageBox.Show("Banka bilgileri Güncellendi ", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
