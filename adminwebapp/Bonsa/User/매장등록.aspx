<%@ Page Language="C#" MasterPageFile="../User/회원관리.master" AutoEventWireup="true" CodeFile="매장등록.aspx.cs" Inherits="회원관리_매장상세정보" Title="코리아 게임 관리자페이지" %>

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
                매장관리
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
                매장등록
            </td>
        </tr>
    </table>
    <table border=1 bordercolor=#E7E3E7 cellpadding=0 cellspacing=0 >
        <tr>
            <td class="clsFieldName" width="150px" nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                    ControlToValidate="tbxLoginID" Display="Dynamic" ErrorMessage="아이디를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                아이디</td>
            <td class="clsFieldValue" width="350px" nowrap>
                <asp:TextBox ID="tbxLoginID" runat="server" CssClass="clsEdit" MaxLength=20 style="ime-mode:disabled"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" width=150px nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="tbxPWD" Display="Dynamic" ErrorMessage="비밀번호를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                비밀번호</td>
            <td class="clsFieldValue" width=350px nowrap>
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
                <asp:TextBox ID="tbxName" runat="server" CssClass="clsEdit" MaxLength=20></asp:TextBox>&nbsp;<font id="fntChange" runat=server color=red>* 변경불가</font>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                보유머니</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxMoney" runat="server" CssClass="clsEdit" Enabled="false" Text=0></asp:TextBox>
                <table runat="server" id="tblChangeMoney" border="0" cellspacing="4" cellpadding="0" width="100%">
                	<tr>
                		<td width="40px">
                		    <asp:DropDownList ID="optGameMoneyUpdate" runat="server" Width="34px">
                                <asp:ListItem>+</asp:ListItem>
                                <asp:ListItem>-</asp:ListItem>
                            </asp:DropDownList>
                		</td>
                		<td width="120px">
                		    <asp:TextBox ID="tbxUpdateGameMoney" runat="server" Width="120px"></asp:TextBox>
                		</td>
                		<td>
                		    <asp:Button ID="btnUpdateGameMoney" ValidationGroup="vgUpdateMoney" runat="server" Text="확인" OnClientClick="return confirm('변경하시겠습니까?');" onclick="btnUpdateGameMoney_Click" />
                            <asp:CustomValidator ID="cvUpdateMoney" runat="server" ValidationGroup="vgUpdateMoney">*</asp:CustomValidator>
                		</td>
                	</tr>
                </table>
                <asp:ValidationSummary ValidationGroup="vgUpdateMoney" ID="vsUpdateMoney" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                상위업체</td>
            <td class="clsFieldValue">
                <asp:DropDownList runat="server" ID="ddlParent"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="tbxPartner" Display="Dynamic" ErrorMessage="파트너코드를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                파트너코드</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxPartner" runat="server" CssClass="clsEdit" MaxLength=20></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                <asp:RangeValidator ID="rvClassPercent" runat="server" 
                    ControlToValidate="tbxClassPercent" Display="Dynamic" 
                    ErrorMessage="매장지분율은 0부터 100사이의 값이어야 합니다." MaximumValue="100" MinimumValue="0" 
                    SetFocusOnError="True" Type="Double" ValidationGroup="userreg">*</asp:RangeValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToValidate="tbxClassPercent" Display="Dynamic" ErrorMessage="매장지분율은 수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Double" SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="tbxClassPercent" Display="Dynamic" ErrorMessage="매장지분율을 입력하세요" 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                매장지분율</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxClassPercent" runat="server" CssClass="clsEdit" Width=60px></asp:TextBox> &nbsp;%
                &nbsp;<asp:Label ID="lblClassPercent" runat=server ForeColor=Red></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" width="150px" nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="tbxBankName" Display="Dynamic" ErrorMessage="은행명을 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                은행명</td>
            <td class="clsFieldValue" width="350px" nowrap>
                <asp:TextBox ID="tbxBankName" runat="server" CssClass="clsEdit" MaxLength=50 style="ime-mode:disabled"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" width="150px" nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="tbxMaster" Display="Dynamic" ErrorMessage="예금주를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                예금주</td>
            <td class="clsFieldValue" width="350px" nowrap>
                <asp:TextBox ID="tbxMaster" runat="server" CssClass="clsEdit" MaxLength=50 style="ime-mode:disabled"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" width="150px" nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                    ControlToValidate="tbxBankNum" Display="Dynamic" ErrorMessage="계좌번호를 입력하세요." 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                계좌번호</td>
            <td class="clsFieldValue" width="350px" nowrap>
                <asp:TextBox ID="tbxBankNum" runat="server" CssClass="clsEdit" MaxLength=50 style="ime-mode:disabled"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                사용여부
            </td>
            <td>
                <asp:RadioButtonList ID="rdoUseYn" runat="server">               
                <asp:ListItem Value="1" Selected="True">&nbsp;사용</asp:ListItem>
                <asp:ListItem Value="0">&nbsp;금지</asp:ListItem>
            </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td colspan=2>
                <input id="hdnID" runat=server type="hidden" />
                <input id="hdnLoginID" runat=server type="hidden" />
                <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
                    <asp:CustomValidator ID="cvResult" runat="server" ValidationGroup="userreg">*</asp:CustomValidator>
                    <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" class="btn btn-info" Text="등록" 
                        ToolTip="" Width="84px" ValidationGroup="userreg" />&nbsp;
                    <asp:Button ID="btnList" runat="server"  class="btn btn-warning" onclick="btnLisTBL_Click" Text="목록보기" ToolTip="" />
                </asp:Panel>
                <asp:ValidationSummary ID="vsErrors" runat="server" Font-Size=9pt ValidationGroup="userreg" />
            </td>
        </tr>
    </table>
</asp:Content>