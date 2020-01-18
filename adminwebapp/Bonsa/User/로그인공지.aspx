<%@ Page Language="C#" MasterPageFile="../Game/게임관리.master" AutoEventWireup="true" CodeFile="로그인공지.aspx.cs" Inherits="게임관리_로그인공지" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap></td>
            <td class="clsSysTitle">
                <h5>로그인공지</h5>
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

    <table style="border-right: #cacaca 1px solid; font-size: 12px; border-left: #cacaca 1px solid; border-top: #cacaca 1px solid; border-bottom: #cacaca 1px solid; border-collapse: collapse"
        bordercolor="#f1f1f1" cellspacing="0" rules="rows" width="100%"
        align="center" border="1">
        <tr>
            <td class="clsFieldName" align="center" style="width:160px;">
                 <Image src="<%= LoginImg %>" style="max-width: 150px;max-height:150px;"></Image>
                        
            </td>
            <td class="clsFieldName" align="right">로그인공지:
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:fileupload id="tbxImageFile" runat="server" />
                <asp:label id="lblFileMsg" forecolor="Red" text=""></asp:label>
                <asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" controltovalidate="tbxImageFile"
                    errormessage="이미지 파일만 가능합니다." validationexpression="^[a-zA-Z]:(\\.+)(.JPEG|.jpeg|.JPG|.jpg|.GIF|.gif|.PNG|.png)$"></asp:regularexpressionvalidator>
            </td>
            <td class="clsFieldName" width="80">
                <p style="margin-bottom:0px;">보이기:</p>
                <asp:CheckBox ID="tbxShowLogin" runat="server" />
            </td>
        </tr>
    </table>
    <asp:panel id="pnlControlBar" runat="server" cssclass="clsControlBar">
        <asp:CustomValidator ID="cvResult2" runat="server" ValidationGroup="userreg">*</asp:CustomValidator>
        <asp:Button ID="btnSave" class="btn btn-info" runat="server" Text="확인" OnClick="btnSave_Click"
            Width="132px" ValidationGroup="userreg" />
    </asp:panel>
    <asp:validationsummary id="vsErrors2" runat="server" cssclass="clsControlSummary"
        validationgroup="userreg" />
</asp:Content>

