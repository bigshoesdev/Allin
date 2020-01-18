<%@ Page Language="C#" MasterPageFile="../User/Bbs.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="공지사항수정.aspx.cs" Inherits="게시글관리_공지사항수정" Title="코리아 게임 관리자페이지" %>

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
            <td class="clsSysTitle">공지사항등록
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
            <td class="clsSubTitle">공지사항등록
            </td>
        </tr>
    </table>
    <table width="100%" border="1" cellpadding="0" cellspacing="0">
        <tr>
            <td colspan="6" class="line_bar" height="2"></td>
        </tr>
        <tr height="28">
            <td class="clsFieldName" align="center" width="90" nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                    ControlToValidate="tbxTitle" Display="Dynamic" ErrorMessage="제목을 입력하세요."
                    SetFocusOnError="True" ValidationGroup="regist">*</asp:RequiredFieldValidator>
                제목
            </td>

            <td width="50%">
                <asp:TextBox ID="tbxTitle" runat="server" Text='<%# Bind("제목") %>' ToolTip='<%# Bind("번호") %>'
                    CssClass="clsEdit" MaxLength="200" Width="60%"></asp:TextBox>

            </td>
            <td width="300">
                <div style="display: none;">
                    <asp:CheckBox ID="chkIsPopup" runat="server" Style="vertical-align: middle; display: inline-block;" />
                    <span style="vertical-align: middle; display: inline-block; padding-top: 5px;">팝업으로 띄움</span>
                    &nbsp;&nbsp;
                 <asp:CheckBox ID="chkIsMobile" runat="server" Style="vertical-align: middle; display: inline-block;" />
                    <span style="vertical-align: middle; display: inline-block; padding-top: 5px;">모바일용</span>
                </div>
            </td>
        </tr>

        <tr>
            <td class="clsFieldName" align="center">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                    ControlToValidate="tbxContent" Display="Dynamic" ErrorMessage="내용을 입력하세요."
                    SetFocusOnError="True" ValidationGroup="regist">*</asp:RequiredFieldValidator>
                내용
            </td>

            <td colspan="2">
                <asp:TextBox ID="tbxContent" runat="server" CssClass="clsEdit" Width="95%" Height="250px"
                    TextMode="MultiLine" Text='<%# Bind("내용1") %>'></asp:TextBox>
            </td>

        </tr>
        <tr>
            <td colspan="6" class="line_bar" height="2"></td>
        </tr>
    </table>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:CustomValidator ID="cvResult" runat="server" ValidationGroup="regist">*</asp:CustomValidator>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="보관"
            ToolTip="" Width="84px" ValidationGroup="regist" />&nbsp;
        <asp:Button ID="btnList" runat="server" OnClick="btnLisTBL_Click" Text="목록보기" ToolTip="" />
    </asp:Panel>
    <input id="hdnID" runat="server" type="hidden" />
    <asp:ValidationSummary ID="vsErrors" runat="server" ValidationGroup="regist" />
    <script type="text/javascript">   
        <%--   CKEDITOR.replace('<%=tbxContent.ClientID.Replace("_","$") %>',
        {
            language: 'ko',
            image_previewText : ' ',
            filebrowserUploadUrl: "./Handler.ashx"
        });--%>
    </script>

</asp:Content>


