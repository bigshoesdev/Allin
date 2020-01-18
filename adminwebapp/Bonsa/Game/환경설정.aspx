<%@ Page Language="C#" MasterPageFile="../Game/게임관리.master" AutoEventWireup="true" CodeFile="환경설정.aspx.cs" Inherits="게임관리_환경설정" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" Runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSysTitle">
                <h5>계정설정</h5>
            </td>
        </tr>
    </table>
    <!-- 타이틀밑의 두선 -->
    <table cellpadding="0" border="1" bordercolor="#E7E3E7" cellspacing="0" class="clsLineTable">
        <tr>
            <td>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_syst001.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSubTitle">
                관리자암호변경</td>
        </tr>
    </table>
    <table style="border-right: #cacaca 1px solid; font-size: 12px; border-left: #cacaca 1px solid;
        border-top: #cacaca 1px solid; border-bottom: #cacaca 1px solid; border-collapse: collapse"
        bordercolor="#f1f1f1" cellspacing="0"  rules="rows" width="100%"
        align="center" border="1">
        <tr>
            <td class="clsFieldName" align="right" width=200px nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbxPassword"
                    ErrorMessage="비밀번호를 입력하세요" ValidationGroup="changepassword" 
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                새 비밀번호:
            </td>
            <td class="clsFieldValue" align="left">
                <asp:TextBox ID="tbxPassword" TextMode=Password runat="server" 
                    CssClass="clsEditableFieldValue" Width="170px" MaxLength="20"></asp:TextBox>
                &nbsp;
                새 비밀번호를 입력하세요.</td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right" nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbxPassword1"
                    ErrorMessage="비밀번호를 한번 더 입력하세요" ValidationGroup="changepassword" 
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator6" runat="server" 
                    ControlToCompare="tbxPassword" ControlToValidate="tbxPassword1" 
                    ErrorMessage="비밀번호가 정확치 않습니다" ValidationGroup="changepassword">*</asp:CompareValidator>
                새 비밀번호확인:
            </td>
            <td class="clsFieldValue" align="left">
                <asp:TextBox ID="tbxPassword1" TextMode=Password runat="server" 
                    CssClass="clsEditableFieldValue" Width="170px" MaxLength="20"></asp:TextBox>
                &nbsp;
                새 비밀번호를 한번 더 입력하세요.</td>
        </tr>
    </table>
    <asp:Panel ID="Panel1" runat="server" CssClass="clsControlBar">
        <asp:CustomValidator ID="cvResult1" runat="server" 
            ValidationGroup="changepassword">*</asp:CustomValidator>
        <asp:Button ID="btnChangePassword" class="btn btn-info" runat="server" 
            Text="확인" OnClick="btnChangePassword_Click"
            Width="132px" ValidationGroup="changepassword" />
    </asp:Panel>
    <asp:ValidationSummary ID="vsErrors1" runat="server" ValidationGroup="changepassword" />
    <br />
    <asp:Panel ID="pnlInfo" runat=server>
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_syst001.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSubTitle">
                은행계좌 설정</td>
        </tr>
    </table>
    <table style="border-right: #cacaca 1px solid; font-size: 12px; border-left: #cacaca 1px solid;
        border-top: #cacaca 1px solid; border-bottom: #cacaca 1px solid; border-collapse: collapse"
        bordercolor="#cacaca" cellspacing="0"  rules="rows" width="100%"
        align="center" border="1">
        <tr>
            <td class="clsFieldName" align="right" nowrap>
                <asp:RequiredFieldValidator ID="rfvBankname" runat="server" ControlToValidate="tbxBankname"
                    ErrorMessage="은행명을 입력하세요." ValidationGroup="admininfo">*</asp:RequiredFieldValidator>
                은행명:
            </td>
            <td class="clsFieldValue" align="left">
                <asp:TextBox ID="tbxBankname" runat="server" CssClass="clsEditableFieldValueE" ValidationGroup="userreg"
                    Width="100px" MaxLength="10"></asp:TextBox>
                &nbsp;예: 국민은행  
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right" nowrap>
                <asp:RequiredFieldValidator ID="rfvBanknum" runat="server" ControlToValidate="tbxBanknum"
                    ErrorMessage="계좌번호를 입력하세요." ValidationGroup="admininfo">*</asp:RequiredFieldValidator>
                계좌번호:
            </td>
            <td class="clsFieldValue" align="left">
                <asp:TextBox ID="tbxBanknum" runat="server" CssClass="clsEditableFieldValueE" ValidationGroup="userreg"
                    Width="200px" MaxLength="20"></asp:TextBox>
                &nbsp;예: 011-1234-2323 
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right" nowrap>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbxName"
                    ErrorMessage="예금주명을 입력하세요." ValidationGroup="admininfo">*</asp:RequiredFieldValidator>
                예금주명:
            </td>
            <td class="clsFieldValue" align="left">
                <asp:TextBox ID="tbxName" runat="server" CssClass="clsEditableFieldValueE" ValidationGroup="userreg"
                    Width="100px" MaxLength="10"></asp:TextBox>
                &nbsp;예: 이수철
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlControlBar1" runat="server" CssClass="clsControlBar">
        <asp:CustomValidator ID="cvResult2" runat="server" ValidationGroup="admininfo">*</asp:CustomValidator>
        <asp:Button ID="btnSave" class="btn btn-info" runat="server" Text="확인" OnClick="btnSave_Click"
            Width="132px" ValidationGroup="admininfo" />
    </asp:Panel>
    <asp:ValidationSummary ID="vsErrors2" runat="server" 
        ValidationGroup="admininfo" />
    </asp:Panel>
    <br />
</asp:Content>

