<%@ Page Language="C#" MasterPageFile="../Game/게임관리.master" AutoEventWireup="true" CodeFile="긴급공지.aspx.cs" Inherits="게임관리_긴급공지" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" runat="Server">
    <style>
        .line_bar {
            background-color: #92D3E4;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap></td>
            <td class="clsSysTitle">긴급공지
            </td>
        </tr>
    </table>
    <!-- 타이틀밑의 두선 -->
    <table cellpadding="0" border="1" bordercolor="#E7E3E7" cellspacing="0" class="clsLineTable">
        <tr>
            <td></td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_syst001.gif" />
            </td>
            <td width="7px" nowrap></td>
            <td class="clsSubTitle">긴급공지
            </td>
        </tr>
    </table>
    <script src="../../scripts/socket.io.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            var socket = window.socket;

            if (!socket) {
                socket = io.connect("<%= ConfigurationManager.ConnectionStrings["AllinSocketString"].ConnectionString %>");
                window.socket = socket;
            }

            socket.on("askQuestion", function () {
                location.reload();
            });

            if ($(".clsControlSummary ul li").html() == "긴급공지정보가 변경되였습니다.") {
                socket.emit('send_notice', { type: 'notice' });
            }
        });
    </script>    
    <table width="100%" border="1" cellpadding="0" cellspacing="0">
        <tr height="28">
            <td class="clsFieldName" align="center" width="90" nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                    ControlToValidate="tbxNoticeTitle" Display="Dynamic" ErrorMessage="제목을 입력하세요."
                    SetFocusOnError="True" ValidationGroup="regist">*</asp:RequiredFieldValidator>
                제목
            </td>
            <td width="50%">
                <asp:TextBox ID="tbxNoticeTitle" runat="server" Text='<%# Bind("제목") %>'
                    CssClass="clsEdit" MaxLength="200" Width="60%"></asp:TextBox>
            </td>
            <td width="300">
            </td>
        </tr>

        <tr>
            <td class="clsFieldName" align="center">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                    ControlToValidate="tbxNoticeCotent" Display="Dynamic" ErrorMessage="내용을 입력하세요."
                    SetFocusOnError="True" ValidationGroup="regist">*</asp:RequiredFieldValidator>
                내용
            </td>

            <td colspan="2">
                <asp:TextBox ID="tbxNoticeCotent" runat="server" CssClass="clsEdit" Width="95%" Height="250px"
                    TextMode="MultiLine" Text='<%# Bind("내용1") %>'></asp:TextBox>
            </td>

        </tr>
        <tr>
            <td colspan="6" class="line_bar" height="2"></td>
        </tr>
    </table>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:CustomValidator ID="cvResult2" runat="server" ValidationGroup="userreg">*</asp:CustomValidator>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="보관"
            ToolTip="" Width="84px" ValidationGroup="regist" />&nbsp;
    </asp:Panel>
    <input id="hdnID" runat="server" type="hidden" />
    <asp:ValidationSummary ID="vsErrors2" runat="server" CssClass="clsControlSummary"
        ValidationGroup="userreg" />
</asp:Content>


