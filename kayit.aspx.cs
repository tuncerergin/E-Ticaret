﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using Npgsql;

public partial class kayıt : System.Web.UI.Page
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
        tCon.Close();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["kullanici"] = null;
        if (!Page.IsPostBack)
        {
            tSQL = "select baroAd from baro_bilgi";
            tCon.Open();
            tCommand.Connection = tCon;
            tCommand.CommandText = tSQL;
            tDataReader = tCommand.ExecuteReader();
            while (tDataReader.Read())
            {
                baro.Items.Add("" + tDataReader["baroAd"]);
            }
            tCon.Close();
        }
    }


    protected void gonder_Click(object sender, EventArgs e)
    {
        try
        {
            if (adi.Value.Trim() != "" && soyadi.Value.Trim() != "" && firma.Value.Trim() != "" && tcno.Value.Trim() != "" &&
                baro.SelectedValue.Trim() != "" && baro.SelectedIndex==0 && sicilno.Value.Trim() != "" && birliksicilno.Value.Trim() != "")
            {
                tSQL = "INSERT INTO kisi_bilgi(kisiturid,ad,soyad,firma,tck,tarihsaat)" +
                       "VALUES('1','"+adi.Value.Trim()+"','"+soyadi.Value.Trim()+"','"+firma.Value.Trim()+"','"+tcno.Value.Trim()+"',CURRENT_TIMESTAMP);" +
                       "INSERT INTO avukat_bilgi(kisiid, baroid, sicilno, birliksicilno, aciklama, aktif, tarihsaat)" +
                       "VALUES((SELECT max(kisiid) FROM kisi_bilgi),'"+baro.SelectedIndex+"','"+sicilno.Value.Trim()+"','"+birliksicilno.Value.Trim()+"','Açıklama girilmemiş',true,CURRENT_TIMESTAMP);" +
                       "INSERT INTO kisi_bakiye(kisiid, bakiyeturid, kisibakiye)" +
                       "VALUES((SELECT max(kisiid) FROM kisi_bilgi),'1','0');" +
                       "INSERT INTO kisi_kimlik(kisiid)" +
                       "VALUES((SELECT max(kisiid) FROM kisi_bilgi));" +
                       "INSERT INTO kisi_adres(kisiid)" +
                       "VALUES((SELECT max(kisiid) FROM kisi_bilgi));" +
                       "INSERT INTO kisi_telefon(kisiid)" +
                       "VALUES((SELECT max(kisiid) FROM kisi_bilgi));" +
                       "INSERT INTO kisi_mail(kisiid, mail, aciklama, tarihsaat)" +
                       "VALUES((SELECT max(kisiid) FROM kisi_bilgi),'"+email.Value.Trim()+"','Açıklama Yok',CURRENT_TIMESTAMP);" +
                       "INSERT INTO kisi_web(kisiid)" +
                       "VALUES((SELECT max(kisiid) FROM kisi_bilgi));" +
                       "INSERT INTO kisi_giris(kisiid, bloke)" +
                       "VALUES((SELECT max(kisiid) FROM kisi_bilgi),false);" +
                       "UPDATE kisi_bilgi SET avukatid = (SELECT max(avukatid) FROM avukat_bilgi)" +
                       "WHERE kisiid = (SELECT max(kisiid) FROM kisi_bilgi); ";
                
                PublicExecuteNonQuery();
                Response.Redirect("login.aspx");
            }
            else
            {
                //Label1.Text = "Tüm alanları doldurmanız gerekli!!!";
            }
        }
        catch
        {
          
        }
    }
}