﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SupperCoBuy.aspx.cs" Inherits="Admin_SupperCoBuy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="style/style01.css" rel="stylesheet" type="text/css" />
</head>
<body class="center">
    <form id="form1" runat="server">
    <div>
        <div class="title">
            超级合买管理中心
        </div>
        <table id="Table1" cellspacing="1" cellpadding="0" width="100%" border="0" class="baseTab">
            <tr class="title" style="height: 28px">
                <td align="left">
                    请选择：
                    <asp:DropDownList ID="ddlLottery" runat="server" AutoPostBack="True" Width="100px"
                        OnSelectedIndexChanged="ddlLottery_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:DataGrid ID="g" runat="server" Width="100%" AllowPaging="True" PageSize="30"
                        BorderColor="#E0E0E0" BorderStyle="None" BorderWidth="2px" BackColor="White"
                        CellPadding="0" AutoGenerateColumns="False" AllowSorting="True" OnItemCreated="g_ItemCreated"
                        OnItemDataBound="g_ItemDataBound">
                        <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
                            BackColor="Silver"></HeaderStyle>
                        <Columns>
                            <asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="ID" DataNavigateUrlFormatString="../Home/Room/Scheme.aspx?id={0}"
                                DataTextField="SchemeNumber" SortExpression="SchemeNumber" HeaderText="方案编号">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:HyperLinkColumn>
                            <asp:BoundColumn DataField="InitiateName" SortExpression="InitiateName" HeaderText="发起人">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Money" SortExpression="Money" HeaderText="金额">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PlayTypeName" SortExpression="PlayTypeName" HeaderText="玩法">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Share" SortExpression="Share" HeaderText="份数">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Schedule" SortExpression="Schedule" HeaderText="进度">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="推荐">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <font face="宋体">
                                        <asp:CheckBox ID="cbCommend" runat="server" AutoPostBack="True"></asp:CheckBox></font>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn Visible="False" DataField="id" SortExpression="id"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                        </PagerStyle>
                    </asp:DataGrid>
                    <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" PageSize="30"
                        HotColor="MistyRose" GridColor="#E0E0E0" AlternatingRowColor="Linen" DataGrid="g"
                        ShowSelectColumn="False" Visible="False" OnPageWillChange="gPager_PageWillChange"
                        OnSortBefore="gPager_SortBefore" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>
</html>
