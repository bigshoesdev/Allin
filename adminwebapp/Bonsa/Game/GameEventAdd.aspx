<%@ Page Language="C#" MasterPageFile="../User/회원관리.master" AutoEventWireup="true" CodeFile="GameEventAdd.aspx.cs"  Inherits="Bonsa_Game_GameEventAdd" %>

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
                <h5>이벤트 배너 관리</h5>
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
                이벤트 배너 등록
            </td>
        </tr>
    </table>
    <table width="100%" >
        <tr>
            <td class="clsFieldName" width="150px" nowrap>
            <asp:HiddenField ID="hidEventID" runat="server" Value="0" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                    ControlToValidate="tbxEventName" Display="Dynamic" ErrorMessage="이벤트명을 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                이벤트 배너명</td>
            <td class="clsFieldValue" width="350px" nowrap>
                <asp:TextBox ID="tbxEventName" runat="server" CssClass="clsEdit" MaxLength=20 ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" width=150px nowrap>
                이미지
            </td>
            <td class="clsFieldValue" width=550px >
                
                <asp:FileUpload ID="tbxImageFile"  runat="server" />
                <asp:Label ID="lblFileMsg" ForeColor="Red" Text=""></asp:Label>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbxImageFile"
ErrorMessage="이미지 파일만 가능합니다." ValidationExpression="^[a-zA-Z]:(\\.+)(.JPEG|.jpeg|.JPG|.jpg|.GIF|.gif|.PNG|.png)$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        
        <tr>
            <td class="clsFieldName">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="tbxLinkUrl" Display="Dynamic" ErrorMessage="이벤트 배너 링크주소를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                링크주소</td>
              <td class="clsFieldValue" width="350px" nowrap>
                <asp:TextBox ID="tbxLinkUrl" runat="server" CssClass="clsTextLong" MaxLength="200"></asp:TextBox>
              </td>
            
        </tr>
       
        <tr>
            <td class="clsFieldName">
                
                사용여부</td>
            <td class="clsFieldValue">
                <asp:DropDownList ID="ddlIsActive" runat="server">                                
                    <asp:ListItem Value="1">사용중</asp:ListItem> 
                    <asp:ListItem Value="0">미사용</asp:ListItem>                        
                </asp:DropDownList>                
            </td>
        </tr>
        
        <tr>
            <td colspan=2>
                <input id="hdnID" runat=server type="hidden" />
                <input id="hdnLoginID" runat=server type="hidden" />
                <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
                    <asp:CustomValidator ID="cvResult" runat="server" ValidationGroup="userreg">*</asp:CustomValidator>
                    <asp:Button ID="btnSave" runat="server" class="btn btn-info" onclick="btnSave_Click" Text=" 등 록 " 
                        ToolTip=""  ValidationGroup="userreg" />&nbsp;
                    <asp:Button ID="btnList" runat="server" class="btn btn-warning" onclick="btnLisTBL_Click" Text="목록보기" ToolTip="" />
                </asp:Panel>
                <asp:ValidationSummary ID="vsErrors" runat="server"  ValidationGroup="userreg" />
            </td>
        </tr>
    </table>
</asp:Content>

