﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AnaSayfaMaster.master" AutoEventWireup="true" CodeFile="dava-turu.aspx.cs" Inherits="Kullanici_dava" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <title>Dava Türü</title>
    <link href="/CssDosyalari/davaTeur.css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="pageDisDiv">
        <div class="ustDiv">
            <div class="lineOrta" style="border-bottom: solid silver thin; margin-bottom: 2%;">
                <asp:Label CssClass="label" runat="server" Text="Yeni Dava Türü" Font-Size="200%">Yeni Dava Türü Oluştur</asp:Label>
            </div>
            <div class="line">
                <div class="lineSolDiv">
                    <asp:Label CssClass="label" runat="server">Dava Türü:</asp:Label>
                </div>
                <div class="lineSagDiv">
                    <asp:TextBox ID="txtDavaTur" CssClass="TexBoxCss" runat="server"></asp:TextBox>
                </div>
            </div>
           
            <div class="line">
                <div class="lineSolDiv">
                    <asp:Label CssClass="label" runat="server">Dava Aktif:</asp:Label>
                </div>
                <div class="lineSagDiv">
                    <asp:CheckBox ID="chckDavaAktif" runat="server" Checked="True"/>
                </div>
            </div>
            <div class="line">
                <div class="lineSolDiv">
                    <asp:Label CssClass="label" runat="server">Açıklama:</asp:Label>
                </div>
                <div class="lineSagDiv">
                    <asp:TextBox ID="txtDavaAciklama" CssClass="TextBoxCssMulti" runat="server" TextMode="MultiLine"></asp:TextBox>
                    
                </div>
            </div>
            <div class="line">
                <%--Bu divi Silme Duruşma Aktifmi Satırıını sola yaslamak için yapıldı--%>
            </div>
            <div class="lineOrta">
                <button runat="server" id="btnKaydet" class="button" OnServerClick="btnKaydet_Click" Text="Kaydet">
                    <i class="fa fa-save fa-2x"></i>Kaydet
                </button>
            </div>

        </div>
        <div class="altDiv">
            <asp:ListView ID="list2" runat="server">
                <ItemTemplate>
                    <div class="ListeDiv">
                        <div class="line">
                            <div class="lineSolDiv">
                                <asp:Label CssClass="label" runat="server">Dava Türü:</asp:Label>
                            </div>
                            <div class="lineSagDiv">
                                
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("davaturad") %>'></asp:Label>
                            </div>
                        </div><div class="line">
                            <div class="lineSolDiv">
                                <asp:Label CssClass="label" runat="server">Dava Aktif:</asp:Label>
                            </div>
                            <div class="lineSagDiv">
                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("aktif") %>'></asp:Label>

                            </div>
                        </div>
                        <div class="line">
                            <div class="lineSolDiv">
                                <asp:Label CssClass="label" runat="server">Dava Açıklama:</asp:Label>
                            </div>
                            <div class="lineSagDiv">
                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("aciklama") %>'></asp:Label>
                            </div>
                        </div>

                        
                        <div class="line">
                            <div class="lineSolDiv">
                                <asp:Label CssClass="label" runat="server">Tarih:</asp:Label>
                            </div>
                            <div class="lineSagDiv">
                                <asp:Label ID="lblTarih" runat="server" Text='<%#Eval("tarihsaat") %>'></asp:Label>
                            </div>
                        </div>
                        <div class="line">
                            <%--Bu divi Silme Duruşma Aktifmi Satırıını sola yaslamak için yapıldı--%>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
    
</asp:Content>