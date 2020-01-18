<%@ Page Language="C#" MasterPageFile="회원관리.master" AutoEventWireup="true" CodeFile="회원등록.aspx.cs" Inherits="회원관리_회원상세정보" Title="코리아 게임 관리자페이지" %>

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
                <h5></h5>회원관리</h5>
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
                회원새로등록
            </td>
        </tr>
    </table>
    <table border=1 bordercolor=#E7E3E7 cellpadding=0 cellspacing=0 width="100%" >
        <tr>
            <td class="clsFieldName" width=150px>등록번호</td>
            <td class="clsFieldValue" width=350px>
                <asp:Label ID="lblID" runat="server" Text="[새로 등록]"></asp:Label>
            </td>
            <td class="clsFieldName" width=150px>등록날자</td>
            <td class="clsFieldValue" width=350px>
                <asp:Label ID="lblRegDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="tbxLoginID" Display="Dynamic" ErrorMessage="로그인아이디를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                아이디
            </td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxLoginID" runat="server" CssClass="clsEdit" MaxLength=20 style="ime-mode:disabled"></asp:TextBox>
            </td>
            <td class="clsFieldName">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="tbxPWD" Display="Dynamic" ErrorMessage="비밀번호를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                비밀번호</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxPWD" runat="server" CssClass="clsEdit" MaxLength=50 style="ime-mode:disabled"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="tbxNickName" Display="Dynamic" ErrorMessage="닉네임을 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                닉네임</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxNickName" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
            <td class="clsFieldName">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="tbxName" Display="Dynamic" ErrorMessage="이름을 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                이름</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxName" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" style="display:none">
                <asp:RangeValidator ID="rvGameMoney" runat="server" 
                    ControlToValidate="tbxGameMoney" Display="Dynamic" MinimumValue="0" 
                    SetFocusOnError="True" Type="Currency" ValidationGroup="userreg">*</asp:RangeValidator>
                <asp:CompareValidator ID="CompareValidator2" runat="server" 
                    ControlToValidate="tbxGameMoney" Display="Dynamic" 
                    ErrorMessage="게임머니는 수값이여야 합니다." Operator="DataTypeCheck" Type="Currency" 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="tbxGameMoney" Display="Dynamic" 
                    ErrorMessage="게임머니를 입력하세요" SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                게임머니</td>
            <td class="clsFieldValue" style="display:none">
                <asp:TextBox ID="tbxGameMoney" runat="server" CssClass="clsEdit" Text="0"></asp:TextBox>
            </td>
            <td class="clsFieldName">전화번호</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxTelNum" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
            <td class="clsFieldName">환전비밀번호</td>
            <td class="clsFieldValue">
                 <asp:TextBox ID="currencyExPassword" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">계정상태</td>
            <td class="clsFieldValue">
                <asp:DropDownList ID="ddlNologin" runat="server">
                    <asp:ListItem Value="0">사용허용</asp:ListItem>
                    <asp:ListItem Value="1">사용정지</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="clsFieldName">채팅기능</td>
            <td class="clsFieldValue">
                <asp:DropDownList ID="ddlStopChat" runat="server">
                    <asp:ListItem Value="0">사용허용</asp:ListItem>
                    <asp:ListItem Value="1">사용정지</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">메모</td>
            <td class="clsFieldValue" colspan=3>
                <asp:TextBox ID="tbxMemo" runat="server" TextMode=MultiLine Width=100% Height=60px CssClass="clsEdit"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:CustomValidator ID="cvResult" runat="server" ValidationGroup="userreg">*</asp:CustomValidator>
        <asp:Button ID="btnSave" runat="server" class="btn btn-info" onclick="btnSave_Click" Text="등록" 
            ToolTip="" Width="84px" ValidationGroup="userreg" />&nbsp;
        <asp:Button ID="btnList" runat="server" class="btn btn-warning"  onclick="btnLisTBL_Click" Text="목록보기" ToolTip="" />
    </asp:Panel>
    <asp:ValidationSummary ID="vsErrors" runat="server" ValidationGroup="userreg" />
</asp:Content>