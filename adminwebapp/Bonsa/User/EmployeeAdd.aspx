<%@ Page Language="C#" MasterPageFile="회원관리.master" AutoEventWireup="true" CodeFile="EmployeeAdd.aspx.cs" Inherits="Bonsa_User_EmployeeAdd" %>


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
                <h5>사원관리</h5>
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
    <table cellpadding="0" cellspacing="0" >
        <tr height="20">
            <td valign="middle">
                <div style="height:100%; padding-top:10px;">
                <img src="../../Images/ico_syst001.gif" />
                </div>
            </td>
          
            <td class="clsSubTitle" >
                사원신규등록
            </td>
        </tr>
    </table>
    <table border=1 bordercolor=#E7E3E7 cellpadding=0 cellspacing=0 width="100%">
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
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="tbxName" Display="Dynamic" ErrorMessage="이름을 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                이름</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxName" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
            
            <td class="clsFieldName">전화번호</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxTelNum" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td class="clsFieldName">상담허용</td>
            <td class="clsFieldValue">
               <asp:DropDownList ID="ddlAllowChat" runat="server">
                    <asp:ListItem Value="1">허용</asp:ListItem>
                    <asp:ListItem Value="0">금지</asp:ListItem>
                </asp:DropDownList>
            </td>
            
            <td class="clsFieldName">계정상태</td>
            <td class="clsFieldValue">
                <asp:DropDownList ID="ddlNologin" runat="server">
                    <asp:ListItem Value="1">사용중</asp:ListItem>
                    <asp:ListItem Value="0">미사용</asp:ListItem>
                </asp:DropDownList>
            </td>
           
        </tr>
       
    </table>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:Button ID="btnSave" runat="server" class="btn btn-info" onclick="btnSave_Click" Text="등록" 
            ToolTip="" Width="84px" ValidationGroup="userreg" />&nbsp;
        <asp:Button ID="btnList" runat="server" class="btn btn-warning" onclick="btnLisTBL_Click" Text="목록보기" />
    </asp:Panel>
    <asp:CustomValidator ID="cvResult" runat="server" ValidationGroup="userreg">*</asp:CustomValidator>
    <asp:ValidationSummary ID="vsErrors" runat="server" ValidationGroup="userreg" />
</asp:Content>
