<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RobotWithdraw.aspx.cs" Inherits="Bonsa_PBM_RobotWithdraw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>로붓 출금페지</title>   
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <table id="tblMailInfo" runat="server" align=center width=300px style="border:solid 1px black;">
        <tr>
            <td align=center colspan=3 class="clsFieldName">
                    <asp:Label ID="lblRobotNickname" runat="server" ></asp:Label>
                    의 출금
            </td>
        </tr>
        <tr style="display:none;">
            <td align="right"  class="clsFieldName" >
                로붓 아이디:
            </td>
            <td class="style1" colspan="2">
                <asp:Label ID="lblRobotID" runat="server"></asp:Label>
                <input id="hdnID" type="hidden" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right" class="clsFieldName">
            
                보유머니:&nbsp;</td>
            <td class="style1" colspan="2">
                <asp:Label ID="lblMoney" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" class="clsFieldName">
                <asp:RangeValidator ID="rvMoney" runat="server" ControlToValidate="tbxAddMoney" 
                    MinimumValue="1" MaximumValue="10000000" Type="Currency" 
                    ErrorMessage="출금 금액 범위 1 ~ 10,000,000">*</asp:RangeValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="tbxAddMoney" Display="Dynamic" ErrorMessage="충전금액을 입력하세요" 
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToValidate="tbxAddMoney" ErrorMessage="충전금액은 자연수 값이여야 합니다." 
                    Operator="DataTypeCheck" SetFocusOnError="True" Type="Currency">*</asp:CompareValidator>
                출금액: </td>
            <td class="style3">
                <asp:TextBox ID="tbxAddMoney" CssClass="clsEdit" runat="server" Width="92px">0</asp:TextBox>
            &nbsp;원</td>
            <td>
                <asp:Button ID="btnOK" runat="server" Text="확인" onclick="btnOK_Click" />
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">출금사휴:</td>
            <td colspan="2">
                <asp:TextBox MaxLength="100" ID="tbxMemo" runat="server" Rows="5"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table align=center>
        <tr>
            <td align=center colspan=3>
                &nbsp;&nbsp;
                <input type=button value="닫기" onclick="window.close();" />
                <asp:CustomValidator ID="cvResult" runat="server" Display="Dynamic"></asp:CustomValidator>
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