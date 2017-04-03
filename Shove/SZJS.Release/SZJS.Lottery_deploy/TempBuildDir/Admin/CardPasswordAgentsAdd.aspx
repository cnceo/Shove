﻿<%@ page language="C#" autoeventwireup="true" inherits="Admin_CardPasswordAgentsAdd, App_Web__1orsh4m" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="style/style01.css" rel="stylesheet" type="text/css" />
    <title></title>
    <style type="text/css">
        .style1
        {
            text-align: right;
            width: 30%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="title" style="text-align: center">
            增加卡密代理商
        </div>
        <table align="center" style="width: 100%;" class="baseTab">
            <tr>
                <td class="style1">
                    代理商用编号：
                </td>
                <td>
                    <asp:TextBox ID="tbAgentNO" runat="server" Width="300px"></asp:TextBox><br />
                    <span style="color: Red">必填项，4位数字 如：1001，否则该代理商无效。</span>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    用户名：
                </td>
                <td>
                    <asp:TextBox ID="tbAgentName" runat="server" Width="300px"></asp:TextBox><br />
                    <span>必填项</span>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    密码：
                </td>
                <td>
                    <asp:TextBox ID="tbAgentPassword" runat="server" Width="300px"></asp:TextBox><br />
                    <span>该密码作为代理平台的管理员帐号密码</span>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    代理商公司名称：
                </td>
                <td>
                    <asp:TextBox ID="tbAgentCompanyName" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    代理商网址：
                </td>
                <td>
                    <asp:TextBox ID="tbAgentSiteName" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    金额：
                </td>
                <td>
                    <asp:TextBox ID="tbMoney" runat="server" Width="300px"></asp:TextBox><br />
                    <span>必填项</span>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="height: 40px;">
                    <asp:Button ID="btnOK" runat="server" Text="添加" Width="62px" OnClick="btnOK_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
