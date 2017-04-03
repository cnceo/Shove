﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardPasswordManage.aspx.cs"
    Inherits="Admin_CardPasswordManage" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="style/style01.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="title" style="text-align: center">
            卡密管理中心
        </div>
        <table width="100%" border="0" cellpadding="0" cellspacing="1" class="baseTab">
            <tr class="title" style="height: 28px;">
                <td style="height: 30px">
                    &nbsp;卡号：
                    <asp:TextBox ID="tbCardPasswordID" runat="server"></asp:TextBox>&nbsp; &nbsp;时间：
                    <asp:TextBox ID="tbDateTime" runat="server"></asp:TextBox>&nbsp;
                    <ShoveWebUI:ShoveConfirmButton ID="btnQuery" runat="server" Text="查找" Width="78px"
                        OnClick="btnQuery_Click" />
                    <asp:RadioButton ID="rbAll" runat="server" Checked="True" GroupName="rb" Text="全部" />
                    <asp:RadioButton ID="rbExp" runat="server" GroupName="rb" Text="已过期" />
                    <asp:RadioButton ID="rbUse" runat="server" GroupName="rb" Text="已使用" />
                    <asp:RadioButton ID="rbNoUse" runat="server" GroupName="rb" Text="未使用" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:DataGrid ID="g" runat="server" CellPadding="0" BackColor="White" BorderWidth="2px"
                        BorderStyle="None" BorderColor="#E0E0E0" Font-Names="宋体" PageSize="20" AllowSorting="True"
                        AutoGenerateColumns="False" AllowPaging="True" OnItemDataBound="g_ItemDataBound"
                        Width="100%">
                        <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                        <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                        <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
                            BackColor="LightGray"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn HeaderText="卡密号">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Money" SortExpression="Money" HeaderText="卡密金额">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="State" SortExpression="State" HeaderText="卡密状态">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Period" SortExpression="Period" HeaderText="过期时间">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="UseDateTime" SortExpression="UseDateTime" HeaderText="充值时间">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="RealityName" SortExpression="RealityName" HeaderText="用户姓名">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="AgentID"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                        </PagerStyle>
                    </asp:DataGrid>
                    <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" ShowSelectColumn="False"
                        DataGrid="g" AlternatingRowColor="Linen" GridColor="#E0E0E0" HotColor="MistyRose"
                        Visible="False" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore">
                    </ShoveWebUI:ShoveGridPager>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnExcel" runat="server" Text="导出Excel" Height="22px" Width="90px"
                        OnClick="btnExcel_Click" />
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>
</html>
