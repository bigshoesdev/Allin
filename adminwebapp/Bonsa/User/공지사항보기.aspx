<%@ Page Language="C#" MasterPageFile="../User/회원관리.master" AutoEventWireup="true" CodeFile="공지사항보기.aspx.cs" Inherits="회원관리_업체공지사항수정" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" Runat="Server">
    <style>
        .line_bar { background-color:#92D3E4; }
    </style>
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
                <h5>공지사항등록</h5>
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
                공지사항상세보기
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr><td colspan="6" class="line_bar" height="2"></td></tr>
        <tr height="28">
            <td class="clsFieldName" align="center" width="90" nowrap>
                제목
            </td>
            <td width="5px" nowrap align="center">
                <table width="1" border="0" bgcolor="#92D3E4" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <img src="../../Images/blank.gif" height="20px" width="1">
                        </td>
                    </tr>
                </table>
            </td>
            <td width="70%">&nbsp;&nbsp;
                <asp:TextBox ID="lblTitle" runat="server" Text='<%# Bind("제목") %>' 
                    Width="95%"></asp:TextBox>
            </td>
            <td class="clsFieldName" align="center" width="90" nowrap>
                공지형태
            </td>
            <td align=center>&nbsp;
                <asp:Label ID=lblNoticeType runat=server Width=80px></asp:Label>
            </td>
        </tr>
        <tr height="1">
            <td colspan="3">
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" align="center">
                내용
            </td>
            <td align="center" valign="middle">
                <table width="1" border="0" bgcolor="#92D3E4" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <img src="../../Images/blank.gif" height="250" width="1">
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan=5>
                <asp:TextBox ID="tbxContent" runat="server" CssClass="clsEdit" Width="100%" Height="250px"
                    TextMode="MultiLine" ReadOnly=true Text='<%# Bind("내용1") %>'></asp:TextBox>
            </td>
        </tr>
        <tr><td colspan="6" class="line_bar" height="2"></td></tr>
    </table>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">

        <asp:Button ID="btnList" runat="server" class="btn btn-warning" onclick="btnLisTBL_Click" Text="목록보기" ToolTip="" />
    </asp:Panel>
    <input id="hdnID" runat=server type="hidden" />
    <asp:ValidationSummary ID="vsErrors" runat="server" ValidationGroup="regist" />
    
    
</asp:Content>

