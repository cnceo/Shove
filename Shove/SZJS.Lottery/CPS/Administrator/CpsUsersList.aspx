﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CpsUsersList.aspx.cs" Inherits="CPS_Administrator_CpsUsersList" %>

<%@ Register TagPrefix="ShoveWebUI" Namespace="Shove.Web.UI" Assembly="Shove.Web.UI.4 For.NET 3.5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="style/style01.css" rel="stylesheet" type="text/css" />
</head>
<body class="center">
    <form id="form1" runat="server">
    <div class="title">
        会员明细
    </div>
    <div>
        <asp:HiddenField ID="hfID" runat="server" />
        <asp:HiddenField ID="hfFilterExpress" runat="server" />
        <div class="title" style="font-size: 14px; text-align: left; padding-left: 10px;">
            <asp:Label ID="lblTypeName" runat="server" Font-Bold="False" Text="CPS代理商"></asp:Label>
            <asp:Label ID="lblName" runat="server" Font-Bold="True" ForeColor="#CC0000" Text="lblName"></asp:Label>
            会员明细&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 会员ID：<asp:TextBox ID="tbUserName"
                runat="server" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Height="18px"></asp:TextBox>
            &nbsp;<asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click"
                Height="22px" Width="40px" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
        </div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="baseTab">
            <tr>
                <td align="center">
                    <asp:DataGrid ID="g" runat="server" CellPadding="0" BackColor="White" BorderWidth="2px"
                        CssClass="baseTab" BorderStyle="None" BorderColor="#E0E0E0" Font-Names="宋体" PageSize="20"
                        AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" OnItemDataBound="g_ItemDataBound"
                        Width="100%">
                        <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
                            BackColor="#E9F1F8" Height="25px"></HeaderStyle>
                        <ItemStyle Height="21px" />
                        <Columns>
                            <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="用户名">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Province" SortExpression="Province" HeaderText="省份">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="City" SortExpression="City" HeaderText="城市">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Sex" SortExpression="Sex" HeaderText="性别">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="BirthDay" SortExpression="BirthDay" HeaderText="生日" DataFormatString="{0:yyyy-MM-dd}">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Email" SortExpression="Email" HeaderText="邮箱">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="QQ" SortExpression="QQ" HeaderText="QQ">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="RegisterTime" SortExpression="RegisterTime" HeaderText="注册时间">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="UserType" SortExpression="UserType" HeaderText="类型">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="black12"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:HyperLinkColumn DataNavigateUrlField="ID" DataNavigateUrlFormatString="UserBuyDetail.aspx?ID={0}"
                                HeaderText="" Target="_blank" Text="交易明细" ItemStyle-ForeColor="#3399FF"></asp:HyperLinkColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                        </PagerStyle>
                    </asp:DataGrid>
                    <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" ShowSelectColumn="False"
                        DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#CCCCCC" HotColor="#FFE4E1"
                        Visible="False" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore">
                    </ShoveWebUI:ShoveGridPager>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <div align="center">
        <a href="#" onclick="history.go(-1)">
            <img src="../Images/return.gif" style="border: 0px;" /></a>
    </div>
    </form>
</body>
</html>
