<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="올인환경설정.aspx.cs" Inherits="게임관리_올인환경설정" Title="코리아 게임 관리자페이지" %>

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
                <h5>올인환경설정</h5>
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
                올인환경설정</td>
        </tr>
    </table>
    <table class="setting_table" cellspacing="0" cellpadding="0" rules="rows" border="1">
        <tr>
            <td align="right" class="clsFieldName" nowrap="nowrap" width=150px>
                <asp:RequiredFieldValidator ID="rfvSubtract" runat="server" 
                    ControlToValidate="tbxMngSubtract" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="운영수수료를 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="register">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvSubtract" runat="server" 
                    ControlToValidate="tbxMngSubtract" Display="Dynamic" 
                    ErrorMessage="0~100사이의 값을 입력하세요" MaximumValue="100" MinimumValue="0" 
                    Type="Double" ValidationGroup="register">*</asp:RangeValidator>
                <asp:CompareValidator ID="cpvSubtract" runat="server" ControlToValidate="tbxMngSubtract"
                    ErrorMessage="운영수수료는 수값을 입력하세요." Operator="DataTypeCheck" Type="Double" 
                    ValidationGroup="register">*</asp:CompareValidator>
                운영수수료:
            </td>
             <td align="left" class="clsFieldValue" nowrap width=250px>
                <asp:TextBox ID="tbxMngSubtract" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="register"
                    Width="40px" MaxLength="3" >5</asp:TextBox>
                 &nbsp;% </td>
        </tr>
    </table>
    <asp:Panel ID="pnlControlBar" runat="server" CssClass="clsControlBar">
        <asp:CustomValidator ID="cvResult2" runat="server" ValidationGroup="register">*</asp:CustomValidator>
        <asp:Button ID="btnSave" class="btn btn-info" runat="server" Text="확인" OnClick="btnSave_Click"
            Width="132px" ValidationGroup="register" />
    </asp:Panel>
    <asp:ValidationSummary ID="vsErrors2" runat="server" 
        ValidationGroup="register" />
</asp:Content>

