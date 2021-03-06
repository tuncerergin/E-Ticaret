﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;

public partial class Kisiler_dava : System.Web.UI.Page
{
    NpgsqlConnection tCon = new NpgsqlConnection(System.Configuration.ConfigurationManager
        .ConnectionStrings["NpgsqlConnectionStrings"].ConnectionString);

    NpgsqlCommand tCommand = new NpgsqlCommand();
    NpgsqlDataReader tDataReader;
    String tSQL;


    public void PublicExecuteNonQuery()
    {
        NpgsqlCommand tCommand = new NpgsqlCommand(tSQL, tCon);

        if (tCon.State == System.Data.ConnectionState.Open)
        {
            tCon.Close();
        }

        tCommand.Connection = tCon;
        tCommand.CommandType = System.Data.CommandType.Text;
        tCommand.CommandTimeout = 60000;
        tCommand.CommandText = tSQL;
        tCon.Open();

        tCommand.ExecuteNonQuery();
        try
        {
        }
        catch (Exception e)
        {
        }

        tCon.Close();
    }

    static int[] tKisiId = new int[1000];
    static int i = 0;
    static int[] tMahkemeId = new int[1000];
    static int MahkemeSayisi = 0;
    static int[] tDavaTurId = new int[1000];
    static int davaTurSayisi = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            verileriGoster();
            successalert.Visible = false;
            txtDurusmaTarihi.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (!IsPostBack)
            {
                drpDavaTuru.Items.Clear();
                tSQL = " SELECT davaturid, davaturad FROM dava_tur WHERE avukatid =" +
                       "(Select avukatid from kisi_bilgi WHERE tck ='" + Session["kullanici"] + "'); ";
                tCon.Open();
                tCommand.Connection = tCon;
                tCommand.CommandText = tSQL;
                tDataReader = tCommand.ExecuteReader();
                while (tDataReader.Read())
                {
                    drpDavaTuru.Items.Add("" + tDataReader["davaturad"]);
                    tDavaTurId[davaTurSayisi] = Int32.Parse("" + tDataReader["davaturid"]);
                    davaTurSayisi++;
                }
                tCon.Close();

                drpMahkeme.Items.Clear();
                tSQL = "SELECT mahkemeid, mahkemead FROM mahkeme_bilgi;";
                tCon.Open();
                tCommand.Connection = tCon;
                tCommand.CommandText = tSQL;
                tDataReader = tCommand.ExecuteReader();
                while (tDataReader.Read())
                {
                    drpMahkeme.Items.Add("" + tDataReader["mahkemead"]);
                    tMahkemeId[MahkemeSayisi] = Int32.Parse("" + tDataReader["mahkemeid"]);
                    MahkemeSayisi++;
                }
                tCon.Close();

                drpTarafTur.Items.Clear();
                tSQL = "SELECT davatarafturad FROM dava_taraf_tur;";
                tCon.Open();
                tCommand.Connection = tCon;
                tCommand.CommandText = tSQL;
                tDataReader = tCommand.ExecuteReader();
                while (tDataReader.Read())
                {
                    drpTarafTur.Items.Add("" + tDataReader["davatarafturad"]);
                }
                tCon.Close();


                drpKisiAdiSoyadi.Items.Clear();
                tSQL =
                    "SELECT kisiid,ad, soyad FROM kisi_bilgi Where avukatid = (SELECT avukatid FROM kisi_bilgi WHERE tck = '" +
                    Session["kullanici"] + "'); ";
                tCon.Open();
                tCommand.Connection = tCon;
                tCommand.CommandText = tSQL;
                tDataReader = tCommand.ExecuteReader();
                while (tDataReader.Read())
                {
                    drpKisiAdiSoyadi.Items.Add("" + tDataReader["ad"] + " " + tDataReader["soyad"]);
                    tKisiId[i] = Int32.Parse("" + tDataReader["kisiid"]);
                    i++;
                }
                tCon.Close();
            }
        }
        catch (Exception exception)
        {
        }
    }

    private void verileriGoster()
    {
        try
        {
            tSQL =
                "SELECT mahkeme_bilgi.mahkemead, CONCAT (kisi_bilgi.ad,' ',kisi_bilgi.soyad) as ad,kisi_bilgi.soyad, dava_tur.davaturad, dava_bilgi.davano, dava_bilgi.dosyaurl, CASE WHEN dava_bilgi.aktif  = 'f' THEN 'Hayır' ELSE 'Evet' END AS davaaktif , dava_taraf_tur.davatarafturad, to_char(durusma_bilgi.tarihsaat,'dd.mm.YYYY') as tarihsaat ,dava_bilgi.aciklama as davaaciklama,durusma_bilgi.aciklama, CASE WHEN durusma_bilgi.aktif  = 'f' THEN 'Hayır' ELSE 'Evet' END AS aktif" +
                " FROM dava_bilgi" +
                " LEFT OUTER JOIN dava_tur ON dava_bilgi.davaturid = dava_tur.davaturid" +
                " LEFT OUTER JOIN dava_mahkeme ON dava_mahkeme.davaid = dava_bilgi.davaid" +
                " LEFT OUTER JOIN dava_taraf ON dava_taraf.davaid = dava_bilgi.davaid" +
                " LEFT OUTER JOIN dava_taraf_tur ON dava_taraf_tur.davatarafturid = dava_taraf.davatarafturid" +
                " LEFT OUTER JOIN durusma_bilgi ON durusma_bilgi.davaid = dava_bilgi.davaid" +
                " LEFT OUTER JOIN kisi_bilgi ON kisi_bilgi.kisiid = dava_taraf.kisiid" +
                " LEFT OUTER JOIN mahkeme_bilgi ON mahkeme_bilgi.mahkemeid = dava_mahkeme.mahkemeid" +
                " WHERE dava_bilgi.avukatid = (SELECT avukatid FROM kisi_bilgi WHERE tck = '" + Session["kullanici"] +
                "') ORDER BY dava_bilgi.davano";
            tCon.Open();
            tCommand.Connection = tCon;
            tCommand.CommandText = tSQL;
            tDataReader = tCommand.ExecuteReader();
            list2.DataSource = tDataReader;
            list2.DataBind();
            tCon.Close();
        }
        catch (Exception e)
        {
        }
    }

    protected void btnKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDavaNo.Text != "" && txtDavaAciklama.Text != "" && txtDurusmaAciklama.Text != "")
            {
                String dosyaUrl = "";
                HttpPostedFile yuklenecekDosya = fileUpload_Dosya.PostedFile;
                if (yuklenecekDosya != null)
                {
                    FileInfo dosyaBilgisi = new FileInfo(yuklenecekDosya.FileName);
                    string yuklemeYeri = Server.MapPath("~/dosyalar/" + dosyaBilgisi.Name);
                    fileUpload_Dosya.SaveAs(yuklemeYeri);
                    dosyaUrl = "/dosyalar/" + dosyaBilgisi.Name;
                }

                tSQL = "INSERT INTO dava_bilgi(davaturid,avukatid,davano,aciklama,aktif,tarihsaat,dosyaurl) VALUES (" +
                       tDavaTurId[drpDavaTuru.SelectedIndex] + ",(SELECT avukatid FROM kisi_bilgi WHERE tck='" +
                       Session["kullanici"] + "'),'" + txtDavaNo.Text.Trim().Replace("'", "") + "','" + txtDavaAciklama.Text.Trim().Replace("'", "") + "','" +
                       chckDavaAktif.Checked + "',CURRENT_TIMESTAMP,'" + dosyaUrl + "');";

                tSQL += " INSERT INTO dava_mahkeme(davaid, mahkemeid, tarihsaat)VALUES(" +
                        "(SELECT MAX(davaid) FROM dava_bilgi WHERE avukatid = (SELECT avukatid FROM kisi_bilgi WHERE tck = '" +
                        Session["kullanici"] + "')),'" + tMahkemeId[drpMahkeme.SelectedIndex] + "',CURRENT_TIMESTAMP);";

                tSQL += " INSERT INTO dava_taraf(davaid, davatarafturid, kisiid,  tarihsaat) VALUES(" +
                        "(SELECT MAX(davaid) FROM dava_bilgi WHERE avukatid = (SELECT avukatid FROM kisi_bilgi WHERE tck = '" +
                        Session["kullanici"] + "'))," +
                        "(SELECT davatarafturid from dava_taraf_tur WHERE davatarafturad = '" +
                        drpTarafTur.SelectedValue + "'),'" + tKisiId[drpKisiAdiSoyadi.SelectedIndex] +
                        "',CURRENT_TIMESTAMP); ";

                tSQL += "INSERT INTO durusma_bilgi(davaid, tarihsaat, aciklama, aktif,islemtarihsaat)" +
                        " VALUES((SELECT MAX(davaid) FROM dava_bilgi WHERE avukatid = (SELECT avukatid FROM kisi_bilgi WHERE tck = '" +
                        Session["kullanici"] + "')), '" + txtDurusmaTarihi.Text.Trim().Replace("'", "") + "','" + txtDurusmaAciklama.Text.Trim().Replace("'", "") +
                        "','" +
                        chckDurusmaAktif.Checked +
                        "',CURRENT_TIMESTAMP); ";


                txtDavaNo.Text = "";
                txtDavaAciklama.Text = "";
                txtDurusmaAciklama.Text = "";
                PublicExecuteNonQuery();
                successalert.Visible = true;
                verileriGoster();
            }
        }
        catch (Exception exception)
        {
        }
    }
}