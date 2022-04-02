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

namespace ProjeMaliyet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        SqlConnection baglanti = new SqlConnection(@"Data Source=DELLXPS;Initial Catalog=ProjeMaliyet;Integrated Security=True");

        //MALZEME LİSTESİ VOİD BASLA
        void MalzemeListe()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TBLMALZEMELER", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }
        //MALZEME LİSTESİ VOİD BİTİŞ

        //ÜRÜN LİSTESİ VOİD BAŞLA

        void UrunListe()
        {

            SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM TBLURUN", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;
        }

        //ÜRÜN LİSTESİ VOİD BİTİŞ

        //KASA LİSTESİ VOİD BAŞLA

        void Kasa()
        {

            SqlDataAdapter da3 = new SqlDataAdapter("SELECT * FROM TBLKASA", baglanti);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;
        }

        //KASA LİSTESİ VOİD BİTİŞ


        // VOID URUNLER BASLA

        void Urunler()
        {

            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TBLURUN", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbUrun.ValueMember = "URUNID";
            CmbUrun.DisplayMember = "AD";
            CmbUrun.DataSource = dt;
            baglanti.Close();

        }

        // VOID URUNLER BITIS

        // VOID MALZEMELER BASLA

        void Malzemeler()
        {
            baglanti.Open();
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM TBLMALZEMELER", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            CmbMalzeme.ValueMember = "MALZEMEID";
            CmbMalzeme.DisplayMember = "AD";
            CmbMalzeme.DataSource = dt2;
            baglanti.Close();
        }
        // VOID MALZEMELER BITIS

        private void Form1_Load(object sender, EventArgs e)
        {
            MalzemeListe();
            UrunListe();
            Urunler();
            Malzemeler();
        }

        private void BtnUrunList_Click(object sender, EventArgs e)
        {
            UrunListe();

        }

        private void BtnMalzemeLİst_Click(object sender, EventArgs e)
        {
            MalzemeListe();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            SqlCommand komut = new SqlCommand("INSERT INTO TBLMALZEMELER (AD,STOK,FIYAT,AÇIKLAMA) VALUES(@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtMalzAD.Text);
            komut.Parameters.AddWithValue("@p2", decimal.Parse(TxtMalzStok.Text));
            komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtMalzFiyat.Text));
            komut.Parameters.AddWithValue("@p4", TxtMalzAciklama.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Malzeme Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MalzemeListe();
        }

        private void BtnUGUEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO TBLURUN (AD,MFIYAT,SFIYAT,STOK) VALUES(@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtUGAd.Text);
            komut.Parameters.AddWithValue("@p2", decimal.Parse(TxtUGMFiyat.Text));
            komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtUGSFİyat.Text));
            komut.Parameters.AddWithValue("@p4", TxtUGStok.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UrunListe();
        }

        private void BtnUOEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO TBLFIRIN (URUNID,MALZEMEID,MIKTAR,MALİYET) VALUES(@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", CmbUrun.SelectedValue);
            komut.Parameters.AddWithValue("@p2", CmbMalzeme.SelectedValue);
            komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtMiktar.Text));
            komut.Parameters.AddWithValue("@p4", decimal.Parse(TxtMaliyet.Text));
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Malzeme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            listBox1.Items.Add(CmbMalzeme.Text + " - " + TxtMaliyet.Text);
        }

        private void TxtMiktar_TextChanged(object sender, EventArgs e)
        {
            double maliyet;
            if (TxtMiktar.Text == "")
            {
                TxtMiktar.Text = "0";
            }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM TBLMALZEMELER WHERE MALZEMEID=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", CmbMalzeme.SelectedValue);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                TxtMaliyet.Text = dr[3].ToString();
            }

            baglanti.Close();

            maliyet = Convert.ToDouble(TxtMaliyet.Text) / 1000 * Convert.ToDouble(TxtMiktar.Text);
            TxtMaliyet.Text = maliyet.ToString();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            TxtUGID.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtUGAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();

            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT SUM (MALİYET) FROM TBLFIRIN WHERE URUNID=@p1",baglanti);
            komut.Parameters.AddWithValue("@p1", TxtUGID.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                TxtUGMFiyat.Text = dr[0].ToString();

            }
            baglanti.Close();
        }
    }
}
