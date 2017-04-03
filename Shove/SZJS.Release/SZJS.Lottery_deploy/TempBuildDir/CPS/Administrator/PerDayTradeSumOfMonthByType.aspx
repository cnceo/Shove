﻿<%@ page language="C#" autoeventwireup="true" inherits="CPS_Administrator_PerDayTradeSumOfMonthByType, App_Web_mmkwpj80" enableEventValidation="false" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="style/style01.css" rel="stylesheet" type="text/css" />
</head>
<body class="center">
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfYearMonth" runat="server" />
    <div>
        <div class="title">
            CPS
            <asp:Label ID="lblMonth" runat="server" Text="lblMonth" ForeColor="#CC0000"></asp:Label>
            &nbsp;交易明细表
        </div>
        <asp:DataGrid ID="g" runat="server" BorderStyle="None" AllowSorting="True" AllowPaging="True"
            PageSize="30" AutoGenerateColumns="False" CellPadding="2" CssClass="baseTab"
            Width="100%" Font-Names="宋体" CellSpacing="1" GridLines="None">
            <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
            <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
            <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
                BackColor="#E9F1F8" Height="25px"></HeaderStyle>
            <AlternatingItemStyle BackColor="#F7FEFA" />
            <ItemStyle Height="21px" BackColor="White"></ItemStyle>
            <Columns>
                <asp:BoundColumn DataField="Day" HeaderText="时间" DataFormatString="{0:d}">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="TotalCpsMemberCount" HeaderText="累计会员数">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="NewCpsMemberCount" HeaderText="新增会员数">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="NewTradeCpsMemberCount" HeaderText="新增交易会员数">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="TradeMoney" HeaderText="日交易量" DataFormatString="{0:N2}">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="BonusMoneySum" HeaderText="佣金收入" DataFormatString="{0:N2}">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="操作">
                    <ItemTemplate>
                        <a href="DayBuyDetailByType.aspx?Day=<%# DataBinder.Eval(Container.DataItem,"Day") %> ">
                            交易明细 </a>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle Visible="False"></PagerStyle>
        </asp:DataGrid>
    </div>
    </form>
</body>
</html>
