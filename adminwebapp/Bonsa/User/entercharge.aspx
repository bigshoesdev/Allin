<%@ Page Language="C#" AutoEventWireup="true" CodeFile="entercharge.aspx.cs" Inherits="MngMember_usercharge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>회원강제충전페지</title>
    <link href="../css/CommonTemplete.css" rel="stylesheet" type="text/css" />
    <link href="../css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 28px;
            
        }
        .style2
        {
            height: 29px;
        }
        .style3
        {
            height: 30px;
        }
        .style4
        {
            height: 35px;
            font-size:12pt;
            font-weight:bold;
            background-color:#F4F4F4;
        }
    </style>
    <script>
    // window.location.href = window.location.href;
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <table id="tblMailInfo" runat=server align=center width=300px style="border:solid 1px black;">
        <tr>
            <td align=center colspan=3 class="style4">
                    <asp:Label ID="lblUserNickname" runat="server" Font-Size="12pt"></asp:Label>
                    님의 충전
            </td>
        </tr>
        <tr>
            <td align="right" width=100px class="style1" bgcolor="#F4F4F4">
                회원아이디:&nbsp;
            </td>
            <td class="style1" colspan="2">
                <asp:Label ID="lblUserID" runat="server"></asp:Label>
                <input id="hdnID" type="hidden" runat=server />
            </td>
        </tr>
        <tr>
            <td align="right" class="style1" bgcolor="#F4F4F4">
                보유머니:&nbsp;</td>
            <td class="style1" colspan="2">
                <asp:Label ID="lblMoney" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="style3" bgcolor="#F4F4F4">
                <asp:RangeValidator ID="rvMoney" runat="server" ControlToValidate="tbxAddMoney" 
                    MinimumValue="0" MaximumValue="100000000" Type="Currency" 
                    ErrorMessage="충전금액이 너무 큽니다.">*</asp:RangeValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="tbxAddMoney" Display="Dynamic" ErrorMessage="충전금액을 입력하세요" 
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToValidate="tbxAddMoney" ErrorMessage="충전금액은 수값이여야 합니다." 
                    Operator="DataTypeCheck" SetFocusOnError="True" Type="Currency">*</asp:CompareValidator>
                머니충전: </td>
            <td class="style3">
                <asp:TextBox ID="tbxAddMoney" CssClass="clsEdit" runat="server" Width="92px">0</asp:TextBox>
            &nbsp;원</td>
            <td>
                <asp:Button ID="btnOK" runat="server" Text="확인" onclick="btnOK_Click" />
            </td>
        </tr>
    </table>
    <table align=center>
        <tr>
            <td align=center colspan=3>
                &nbsp;&nbsp;
                <input type=button value="닫기" onclick="window.close();" />
                <asp:CustomValidator ID="cvResult"
                    runat="server" Display="Dynamic"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td align=center colspan=3>
                <asp:Label ID="lblResult" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align=center colspan=3>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
