﻿<%@ page language="C#" autoeventwireup="true" inherits="CPS_Administrator_CpsAgentList, App_Web_mmkwpj80" enableEventValidation="false" %>

<%@ Register Assembly="Shove.Web.UI.4 For.NET 3.5" Namespace="Shove.Web.UI" TagPrefix="ShoveWebUI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="style/style01.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="../../Components/My97DatePicker/WdatePicker.js"></script>

</head>
<body class="center">
    <form id="form1" runat="server">
    <div>
        <div class="title">
            代理商列表
        </div>
        <asp:HiddenField ID="hfCurFilterExpress" runat="server" />
        <table cellpadding="0" cellspacing="0" width="100%" class="baseTab">
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblSearch" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="TodayNew">今日新增代理商</asp:ListItem>
                        <asp:ListItem Value="All">全部代理商</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td valign="middle">
                    &nbsp;开始日期
                    <asp:TextBox runat="server" ID="tbBeginTime" CssClass="in_text_hui" Width="100px"
                        onblur="if(this.value=='') this.value=document.getElementById('hBeginTime').value"
                        name="tbBeginTime" onFocus="WdatePicker({el:'tbBeginTime',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})"
                        Height="15px" />
                    &nbsp;结束日期
                    <asp:TextBox runat="server" ID="tbEndTime" CssClass="in_text_hui" Width="100px" name="tbEndTime"
                        onblur="if(this.value=='') this.value=document.getElementById('hEndTime').value"
                        onFocus="WdatePicker({el:'tbEndTime',dateFmt:'yyyy-MM-dd', maxDate:'%y-%M-%d'})"
                        Height="15px" />
                    &nbsp;代理商ID<asp:TextBox runat="server" ID="tbUserName" Width="93px" Height="16px" />
                    <asp:Button ID="btnSearch" runat="server" Text="查询" Height="22px" Width="40px" BorderColor="Silver"
                        BorderStyle="Solid" BorderWidth="1px" OnClick="btnSearchByID_Click" />
                    &nbsp;网站名称<asp:TextBox runat="server" ID="tbCpsSiteName" Width="102px" Height="16px" />
                    <asp:Button ID="btnSearchBySiteName" runat="server" Text="查询" Height="22px" Width="40px"
                        BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" OnClick="btnSearchBySiteName_Click" />
                </td>
            </tr>
        </table>
    </div>
    <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="center">
                <asp:DataGrid ID="g" runat="server" CellPadding="0" BackColor="White" BorderWidth="2px"
                    CssClass="baseTab" BorderStyle="None" BorderColor="#E0E0E0" Font-Names="宋体" PageSize="20"
                    AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" Width="100%"
                    OnItemCommand="g_ItemCommand">
                    <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
                    <SelectedItemStyle Font-Bold="True" ForeColor="#663399"></SelectedItemStyle>
                    <HeaderStyle HorizontalAlign="Center" ForeColor="RoyalBlue" VerticalAlign="Middle"
                        BackColor="#E9F1F8" Height="25px"></HeaderStyle>
                    <ItemStyle Height="21px" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="序号">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                            <ItemTemplate>
                                <%# Container.ItemIndex+1%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="UserName" SortExpression="UserName" HeaderText="代理商ID">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CpsName" SortExpression="CpsName" HeaderText="网站名称">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false" Width="150px"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Url" SortExpression="Url" HeaderText="网址">
                            <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DomainName" SortExpression="DomainName" HeaderText="推广网址">
                            <HeaderStyle HorizontalAlign="Left" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CpsDateTime" SortExpression="CpsDateTime" HeaderText="开通时间"
                            DataFormatString="{0:yy-MM-dd}">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="MemberCount" SortExpression="MemberCount" HeaderText="累计会员数">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TodayMemberCount" SortExpression="TodayMemberCount" HeaderText="今日新增会员数">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="LastMonthTrade" SortExpression="LastMonthTrade" HeaderText="上月交易量"
                            DataFormatString="{0:N}">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ThisMonthTrade" SortExpression="ThisMonthTrade" HeaderText="本月交易量"
                            DataFormatString="{0:N}">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="TodayTrade" SortExpression="TodayTrade" HeaderText="今日交易量"
                            DataFormatString="{0:N}">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="操作">
                            <HeaderStyle HorizontalAlign="Center" Wrap="false"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" ForeColor="Red" Wrap="false"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetaile" runat="server" CommandName="MemberDetail"
                                    ForeColor="Red">
                                    会员明细</asp:LinkButton>
                                <asp:LinkButton ID="ShoveLinkButton1" runat="server" CommandName="IncomeDetail"
                                    ForeColor="Red">
                                    交易明细</asp:LinkButton>
                                <asp:LinkButton ID="SiteLogin" runat="server" CommandName="SiteLogin" ForeColor="Red">
                                   代理商后台</asp:LinkButton>
                                <asp:LinkButton ID="SetInfo" runat="server" CommandName="SetInfo" ForeColor="Red">
                                   资料设置</asp:LinkButton>
                                <asp:LinkButton ID="lbStop" runat="server" CommandName="Stop" ForeColor="Red" OnClientClick="return confirm('你真的要暂停吗？');">
                                    暂停</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="ON"></asp:BoundColumn>
                        <asp:BoundColumn DataField="cpsID" SortExpression="cpsID" HeaderText="CpsID" Visible="false">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="OwnerUserID" HeaderText="OwnerUserID" Visible="false">
                        </asp:BoundColumn>
                    </Columns>
                    <PagerStyle Visible="False" HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC">
                    </PagerStyle>
                </asp:DataGrid>
                <ShoveWebUI:ShoveGridPager ID="gPager" runat="server" Width="100%" ShowSelectColumn="False"
                    DataGrid="g" AlternatingRowColor="#F7FEFA" GridColor="#CCCCCC" HotColor="#FFE4E1"
                    Visible="true" OnPageWillChange="gPager_PageWillChange" OnSortBefore="gPager_SortBefore"
                    PageSize="20"></ShoveWebUI:ShoveGridPager>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hBeginTime" runat="server" />
    <asp:HiddenField ID="hEndTime" runat="server" />
    </div>
    </form>
</body>
</html>
