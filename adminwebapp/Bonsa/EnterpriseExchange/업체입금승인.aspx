<%@ Page Language="C#" MasterPageFile="업체입출금관리.master" AutoEventWireup="true" CodeFile="업체입금승인.aspx.cs"
    Inherits="업체입출금관리_업체입금승인" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSysTitle">
                업체입금승인
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
    <br />
    <table class="clsFuncPanel">
        <tr>
            <td class="clsFieldName" width="100px" >
                요청금액
            </td>
            <td class="clsFieldName" width="100px" align=center>
            </td>
            <td>
                <asp:Label runat="server" ID="lblReqMoney"></asp:Label>원
            </td class="clsFieldName" width="100px" align=center>
            <td>
            </td>
            <td>
            </td>
            <td class="clsFieldName" width="100px" align=center>
            </td>
            <td>
            </td>
            <td class="clsFieldName" width="100px" align=center>
            </td>
            <td>
            </td>
            <td class="clsFieldName" width="100px" align=center>
            </td>
            <td>
            </td>
            <td class="clsFieldName" width="100px" align=center>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" width="100px" >
                보유쿠폰
            </td>
            <td class="clsFieldName" width="100px" align=center>
                1만원권
            </td>
            <td>
                <asp:Label runat="server" ID="lbl10KCoupon"></asp:Label>장
            </td class="clsFieldName" width="100px" align=center>
            <td>
                2만원권
            </td>
            <td>
                <asp:Label runat="server" ID="lbl20KCoupon"></asp:Label>장
            </td>
            <td class="clsFieldName" width="100px" align=center>
                5만원권
            </td>
            <td>
                <asp:Label runat="server" ID="lbl50KCoupon"></asp:Label>장
            </td>
            <td class="clsFieldName" width="100px" align=center>
                10만원권
            </td>
            <td>
                <asp:Label runat="server" ID="lbl100KCoupon"></asp:Label>장
            </td>
            <td class="clsFieldName" width="100px" align=center>
                50만원권
            </td>
            <td>
                <asp:Label runat="server" ID="lbl500KCoupon"></asp:Label>장
            </td>
            <td class="clsFieldName" width="100px" align=center>
                100만원권
            </td>
            <td>
                <asp:Label runat="server" ID="lbl1MCoupon"></asp:Label>장
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" >
                승인할 쿠폰
            </td>
            <td>
            </td>
            <td>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToValidate="tbx10KCoupon" Display="Dynamic" ErrorMessage="쿠폰수는 수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Integer" SetFocusOnError="True" 
                    ValidationGroup="vgRegCoupon">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="tbx10KCoupon" Display="Dynamic" ErrorMessage="개수를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="vgRegCoupon">*</asp:RequiredFieldValidator>
                
                <asp:TextBox runat="server" ID="tbx10KCoupon" Text="0"></asp:TextBox>
            </td>
            <td>
            </td>
            <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="tbx20KCoupon" Display="Dynamic" ErrorMessage="개수를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="vgRegCoupon">*</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator2" runat="server" 
                    ControlToValidate="tbx20KCoupon" Display="Dynamic" ErrorMessage="쿠폰수는 수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Integer" SetFocusOnError="True" 
                    ValidationGroup="vgRegCoupon">*</asp:CompareValidator>
                <asp:TextBox runat="server" ID="tbx20KCoupon" Text="0"></asp:TextBox>
            </td>
            <td>
            </td>
            <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="tbx50KCoupon" Display="Dynamic" ErrorMessage="개수를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="vgRegCoupon">*</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator3" runat="server" 
                    ControlToValidate="tbx50KCoupon" Display="Dynamic" ErrorMessage="쿠폰수는 수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Integer" SetFocusOnError="True" 
                    ValidationGroup="vgRegCoupon">*</asp:CompareValidator>
                <asp:TextBox runat="server" ID="tbx50KCoupon" Text="0"></asp:TextBox>
            </td>
            <td>
            </td>
            <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="tbx100KCoupon" Display="Dynamic" ErrorMessage="개수를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="vgRegCoupon">*</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator4" runat="server" 
                    ControlToValidate="tbx100KCoupon" Display="Dynamic" ErrorMessage="쿠폰수는 수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Integer" SetFocusOnError="True" 
                    ValidationGroup="vgRegCoupon">*</asp:CompareValidator>
                <asp:TextBox runat="server" ID="tbx100KCoupon" Text="0"></asp:TextBox>
            </td>
            <td>
            </td>
            <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="tbx500KCoupon" Display="Dynamic" ErrorMessage="개수를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="vgRegCoupon">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator5" runat="server" 
                    ControlToValidate="tbx500KCoupon" Display="Dynamic" ErrorMessage="쿠폰수는 수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Integer" SetFocusOnError="True" 
                    ValidationGroup="vgRegCoupon">*</asp:CompareValidator>
                <asp:TextBox runat="server" ID="tbx500KCoupon" Text="0"></asp:TextBox>
            </td>
            <td>
            </td>
            <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="tbx1MCoupon" Display="Dynamic" ErrorMessage="개수를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="vgRegCoupon">*</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator6" runat="server" 
                    ControlToValidate="tbx1MCoupon" Display="Dynamic" ErrorMessage="쿠폰수는 수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Integer" SetFocusOnError="True" 
                    ValidationGroup="vgRegCoupon">*</asp:CompareValidator>
                <asp:TextBox runat="server" ID="tbx1MCoupon" Text="0"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:Button ID="btnOk" runat="server" class="btn btn-info" Text="승인" ValidationGroup="vgRegCoupon" onclick="btnOk_Click" OnClientClick="btnOk_Click"/>
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="목록새로고침"  onclick="btnRefresh_Click" />
        <asp:Button ID="btnList" runat="server" class="btn btn-info" Text="목록으로가기" onclick="btnList_Click" />
    </asp:Panel>
    <asp:CustomValidator ID="cvRegCouponResult" Visible="false" runat="server" Display="Dynamic" ControlToValidate="tbxCouponName" ValidationGroup="vgRegCoupon"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsRegCouponError" runat="server" ValidationGroup="vgRegCoupon" DisplayMode="List" />
</asp:Content>
