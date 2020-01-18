<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="ManualEventMng.aspx.cs" Inherits="Bonsa_Game_ManualEventMng" %>

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
                <h5>게임 이벤트 관리</h5>
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
    
    <div class='PageToolBar'>
    </div>
    
    <div class="">
    <table>
    <caption>이벤트 속성</caption>
    <tr>
        <td class="clsFieldName">이벤트 유형</td>
        <td class="clsFieldValue">        
        <asp:DropDownList ID="ddlEventType" runat="server" style="width:100px; font-size:14px;">
            <asp:ListItem Value="1">고래</asp:ListItem>
            <asp:ListItem Value="2">상어</asp:ListItem>
            <asp:ListItem Value="3">거북</asp:ListItem>
            <asp:ListItem Value="0">해파리</asp:ListItem>
        </asp:DropDownList>
        <font color="red">*해파리이벤트는 가상 이벤트입니다.</font>
        </td>
    </tr>
    <tr>
        <td class="clsFieldName">이벤트명</td>
        <td class="clsFieldValue">
        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="tbxEventName" Display="Dynamic" ErrorMessage="이벤트명을 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
        <asp:TextBox ID="tbxEventName" runat="server" MaxLength="20" Text=""></asp:TextBox>
        
        </td>
    </tr>
    <tr>
        <td class="clsFieldName">게임 라운드 회수</td>
        <td class="clsFieldValue">
            <asp:RangeValidator ID="rvClassPercent" runat="server" 
                    ControlToValidate="tbxRaceCount" Display="Dynamic" 
                    ErrorMessage="레이스 회수는 1부터9999까지의 값이어야 합니다." MaximumValue="9999" MinimumValue="1" 
                    SetFocusOnError="True" Type="Double" ValidationGroup="userreg">*</asp:RangeValidator>
            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToValidate="tbxRaceCount" Display="Dynamic" ErrorMessage="레이스 회수는 자연수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Double" SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:CompareValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="tbxRaceCount" Display="Dynamic" ErrorMessage="레이스 회수를 입력하세요" 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
            <asp:TextBox ID="tbxRaceCount" MaxLength="4" Text="" runat="server"></asp:TextBox>
            <font color="red">* 1~9999 까지 (이벤트 발생 주기)</font>
        </td>
    </tr>
    <tr>
        <td class="clsFieldName">이벤트 증정 금액</td>
        <td class="clsFieldValue">
            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                    ControlToValidate="tbxEventMoney" Display="Dynamic" 
                    ErrorMessage="이벤트 증정금액은 0부터 1,000,000까지의 값이어야 합니다." MaximumValue="1000000" MinimumValue="0" 
                    SetFocusOnError="True" Type="Double" ValidationGroup="userreg">*</asp:RangeValidator>
            <asp:CompareValidator ID="CompareValidator2" runat="server" 
                    ControlToValidate="tbxEventMoney" Display="Dynamic" ErrorMessage="이벤트 증정금액은 자연수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Double" SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:CompareValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="tbxEventMoney" Display="Dynamic" ErrorMessage="이벤트 금액을 입력하세요" 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
            
            <asp:TextBox ID="tbxEventMoney" Text="0" MaxLength="6" runat="server"></asp:TextBox>
            <font color="red">* 0~1,000,000 까지 (승자 증정 금액)</font>
        </td>
    </tr>
    <tr style="display:none;">
        <td class="clsFieldName">사용여부</td>
        <td class="clsFieldValue">
            <asp:DropDownList ID="ddlUseYn" runat="server">
                <asp:ListItem Value="1">사용</asp:ListItem>
                <asp:ListItem Value="0" Selected="True">미사용</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr height="30">
        <td colspan="2">
        <asp:CustomValidator ID="cvResult" runat="server" ValidationGroup="userreg">*</asp:CustomValidator>
        <asp:Button ID="btnSave" runat="server" class="btn btn-info" Text=" 등록 " OnClick="btnSave_Click" ValidationGroup="userreg"/>
        <asp:Button ID="btnList" runat="server" class="btn btn-warning" onclick="btnLisTBL_Click" Text="목록보기" ToolTip="" />
        <asp:ValidationSummary ID="vsErrors" runat="server" Font-Size=9pt ValidationGroup="userreg" />
        </td>
    </tr>
    </table>
    </div>
</asp:Content>