<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="가상유저등록.aspx.cs" Inherits="게임관리_가상유저상세정보" Title="코리아 게임 관리자페이지" %>

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
                <h5>가상유저관리</h5>
            </td>
        </tr>
    </table>
    
<script type="text/javascript">
     $(function() {
         $('select').selectlist({
             zIndex: 10,
             width: 120,
             height: 27
         });
     })
</script>    
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
                가상유저등록
            </td>
        </tr>
    </table>
    <table border=1 bordercolor=#E7E3E7 cellpadding=0 cellspacing=0 >
        <tr>
            <td class="clsFieldName" width="150px" nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                    ControlToValidate="tbxNickname" Display="Dynamic" ErrorMessage="닉네임을 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                닉네임</td>
            <td class="clsFieldValue" width="350px" nowrap colspan="3">
                <asp:TextBox ID="tbxNickname" runat="server" CssClass="clsEdit" MaxLength=20></asp:TextBox>
            </td>
        </tr>
        <tr style="display:none;">
            <td class="clsFieldName">
                <asp:CompareValidator ID="CompareValidator2" runat="server" 
                    ControlToValidate="tbxMoney" Display="Dynamic" ErrorMessage="보유머니는 0 또는 자연수 값이여야 합니다." 
                    Operator="DataTypeCheck" SetFocusOnError="True" Type="Integer" 
                    ValidationGroup="userreg">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                    ControlToValidate="tbxMoney" Display="Dynamic" ErrorMessage="보유머니를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                보유머니</td>
            <td class="clsFieldValue" colspan="3">
                <asp:TextBox ID="tbxMoney" runat="server" CssClass="clsEdit">0</asp:TextBox>
                <asp:Label ID="lblMoneyMsg" runat="server" ForeColor="Red"></asp:Label>
                </td>
        </tr>
        <tr>
            <td class="clsFieldName" width=150px nowrap>
                아바타</td>
            <td class="clsFieldValue" width=350px nowrap colspan="3">
                
                <asp:DropDownList ID="ddlAvatar" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToValidate="tbxAllinWinCount" Display="Dynamic" ErrorMessage="이긴 횟수는 수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Integer" SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="tbxAllinWinCount" Display="Dynamic" ErrorMessage="이긴 횟수를 입력하세요" 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                올인
                이긴 횟수</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxAllinWinCount" runat="server" CssClass="clsEdit" Width=60px>0</asp:TextBox> 
                &nbsp;번</td>
            <td class="clsFieldName">
                <asp:CompareValidator ID="CompareValidator3" runat="server" 
                    ControlToValidate="tbxAllinLoseCount" Display="Dynamic" ErrorMessage="진 횟수는 수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Integer" SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="tbxAllinLoseCount" Display="Dynamic" ErrorMessage="진 횟수를 입력하세요" 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                올인
                진 횟수</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxAllinLoseCount" runat="server" CssClass="clsEdit" 
                    Width=60px>0</asp:TextBox> 
                &nbsp;번</td>
        </tr>
        <tr>
            <td colspan=4>
                <input id="hdnID" runat=server type="hidden" />
                <input id="hdnOrgNickname" runat=server type="hidden" />
                <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
                    <asp:CustomValidator ID="cvResult" runat="server" ValidationGroup="userreg">*</asp:CustomValidator>
                    <asp:Button ID="btnSave" runat="server" class="btn btn-info" onclick="btnSave_Click" Text="등록" 
                        ToolTip="" Width="84px" ValidationGroup="userreg" />&nbsp;
                    <asp:Button ID="btnList" runat="server" class="btn btn-warning" onclick="btnLisTBL_Click" Text="목록보기" ToolTip="" />
                </asp:Panel>
                <asp:ValidationSummary ID="vsErrors" runat="server" Font-Size=9pt ValidationGroup="userreg" />
            </td>
        </tr>
    </table>
</asp:Content>