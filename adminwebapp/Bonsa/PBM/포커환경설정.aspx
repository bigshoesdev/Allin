<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="포커환경설정.aspx.cs" Inherits="게임관리_포커환경설정" Title="코리아 게임 관리자페이지" %>

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
                <h5>포커환경설정</h5>
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
                포커환경설정</td>
        </tr>
    </table>
    <table class="setting_table" cellspacing="0" cellpadding="0" rules="rows" border="1">
        <tr>
            <td align="right" class="clsFieldName" nowrap="nowrap" width=150px>
                <asp:RequiredFieldValidator ID="rfvSubtract" runat="server" 
                    ControlToValidate="tbxMngSubtract" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="운영수수료를 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="register">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvSubtract" runat="server" 
                    ControlToValidate="tbxMngSubtract" Display="Dynamic" 
                    ErrorMessage="0~100사이의 값을 입력하세요" MaximumValue="100" MinimumValue="0" 
                    Type="Double" ValidationGroup="register">*</asp:RangeValidator>
                <asp:CompareValidator ID="cpvSubtract" runat="server" ControlToValidate="tbxMngSubtract"
                    ErrorMessage="운영수수료는 수값을 입력하세요." Operator="DataTypeCheck" Type="Double" 
                    ValidationGroup="register">*</asp:CompareValidator>
                운영수수료:
            </td>
             <td align="left" class="clsFieldValue" nowrap width=250px>
                <asp:TextBox ID="tbxMngSubtract" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="register"
                    Width="40px" MaxLength="3" Enabled="false">5</asp:TextBox>
                 &nbsp;% </td>
            <td align="right" class="clsFieldName" nowrap width=150px>
                <asp:RequiredFieldValidator ID="rfvCardSubtract" runat="server" 
                    ControlToValidate="tbxJackSubtract" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="잭팟수수료를 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="register">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvCardSubtract" runat="server" 
                    ControlToValidate="tbxJackSubtract" Display="Dynamic" 
                    ErrorMessage="0~100사이의 값을 입력하세요" MaximumValue="100" MinimumValue="0" 
                    Type="Double" ValidationGroup="register">*</asp:RangeValidator>
                <asp:CompareValidator ID="cvCardSubtract" runat="server" ControlToValidate="tbxJackSubtract"
                    ErrorMessage="잭팟수수료는 수값을 입력하세요." Operator="DataTypeCheck" Type="Double" 
                    ValidationGroup="register">*</asp:CompareValidator>
                잭팟수수료:
            </td>
             <td align="left" class="clsFieldValue" nowrap width=250px>
                <asp:TextBox ID="tbxJackSubtract" runat="server" 
                     CssClass="clsEditableFieldValue" ValidationGroup="register"
                    Width="40px" MaxLength="3">5</asp:TextBox>
                 &nbsp;% </td>
        </tr>
        <tr>
            <td class="clsFieldName">이벤트: </td>
            <td class="clsFieldValue">
            <table><tr>
            <td><asp:CheckBox ID="cbxEventFlag" runat="server" Text="" /></td>
            <td align="center"><span class="ch_text">사용</span></td>
            </tr></table>  
            </td>
            <td class="clsFieldName">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="tbxEventMoney"  ErrorMessage="이벤트금액을 입력하세요" 
                                    SetFocusOnError="True" ValidationGroup="register">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator3" runat="server" 
                                    ControlToValidate="tbxEventMoney"  
                                    ErrorMessage="이벤트 금액은 수값이여야 합니다." Operator="DataTypeCheck" 
                                    SetFocusOnError="True" Type="Integer" ValidationGroup="register">*</asp:CompareValidator>
                                이벤트금액: </td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxEventMoney" runat="server" CssClass="clsEdit" Width="60px">0</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">중복아이피방입장: </td>
            <td class="clsFieldValue" colspan=3>
             <table><tr>
            <td><asp:CheckBox ID="cbxAllowDuplIP" runat="server" Text="" /></td>
            <td align="center"><span class="ch_text">허용</span></td>
            </tr></table>  
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">잭팟설정: </td>
            <td colspan=3>
                <table border=1 bordercolor=#E7E3E7 cellpadding=0 cellspacing=0 width=100% style="border:none;">
                    <tr>
                        <td align="center" class="clsHeaderField">잭팟적립금</td>
                        <td align="center" class="clsHeaderField">지급방식</td>
                        <td align="center" class="clsHeaderField">족보</td>
                        <td align="center" class="clsHeaderField">잭팟지급률</td>
                        <td align="center" class="clsHeaderField">잭팟지급머니</td>
                    </tr>
                    <tr>
                        <td rowspan=3 align="center" class="clsFieldValue">
                        <asp:CompareValidator ID="CompareValidator12" runat="server" ControlToValidate="tbxTotalJackMoney" 
                             ErrorMessage="잭팟금액은 수값형이여야 합니다." 
                            Operator="DataTypeCheck" SetFocusOnError="True" Type="Integer" 
                            ValidationGroup="register">*</asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
                                ControlToValidate="tbxTotalJackMoney"  
                                ErrorMessage="잭팟머니를 입력하세요" SetFocusOnError="True" 
                                ValidationGroup="register">*</asp:RequiredFieldValidator>
                            
                            <asp:TextBox ID="tbxTotalJackMoney" runat="server" Text="0" Width="80px" 
                                CssClass="clsEdit"></asp:TextBox>원
                            
                        </td>
                        <td rowspan=3 align="center" class="clsFieldValue">
                           <table><tr>
                              <td><asp:RadioButton ID="rbJackMode1" runat="server" GroupName="JackMode" 
                                Text="" /></td>
                              <td align="center"><span class="ch_text">직접값 사용</span></td>
                              </tr>
                              <tr>
                           <td><asp:RadioButton ID="rbJackMode2" runat="server" GroupName="JackMode" 
                                Checked="True" Text="" /></td>
                            <td align="center"><span class="ch_text">지급률 사용</span></td>
                           </tr></table>
                          
                        </td>
                        <td align="center" class="clsFieldValue">로티플</td>
                        <td class="clsFieldValue" align="center">
                            <asp:TextBox ID="tbxJackRatio3" runat="server" CssClass="clsEdit" Width="60px"></asp:TextBox>
                            &nbsp;%
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                ControlToValidate="tbxJackRatio3"  
                                ErrorMessage="로티플 팟지급률을 입력하세요" SetFocusOnError="True" 
                                ValidationGroup="register">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator4" runat="server" 
                                ControlToValidate="tbxJackRatio3"  
                                ErrorMessage="로티플 잭팟지급률은 수값이여야 합니다." Operator="DataTypeCheck" 
                                SetFocusOnError="True" Type="Double" ValidationGroup="register">*</asp:CompareValidator>
                        </td>
                        <td class="clsFieldValue" align="center">
                            <asp:TextBox ID="tbxJackMoney3" runat="server" CssClass="clsEdit" Width="80px"></asp:TextBox>
                            &nbsp;원
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                ControlToValidate="tbxJackMoney3"  
                                ErrorMessage="로티플 잭팟지급머니를 입력하세요" SetFocusOnError="True" 
                                ValidationGroup="register">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator7" runat="server" 
                                ControlToValidate="tbxJackMoney3"  
                                ErrorMessage="로티플 잭팟지금머니는 수값이여야 합니다." Operator="DataTypeCheck" 
                                SetFocusOnError="True" Type="Integer" ValidationGroup="register">*</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="clsFieldValue">스티플</td>
                        <td class="clsFieldValue" align="center">
                            <asp:TextBox ID="tbxJackRatio2" runat="server" CssClass="clsEdit" Width="60px"></asp:TextBox>
                            &nbsp;%
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                ControlToValidate="tbxJackRatio2"  
                                ErrorMessage="스티플 잭팟지급률을 입력하세요" SetFocusOnError="True" 
                                ValidationGroup="register">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator5" runat="server" 
                                ControlToValidate="tbxJackRatio2"  
                                ErrorMessage="스티플 잭팟지급률은 수값이여야 합니다." Operator="DataTypeCheck" 
                                SetFocusOnError="True" Type="Double" ValidationGroup="register">*</asp:CompareValidator>
                        </td>
                        <td class="clsFieldValue" align="center">
                            <asp:TextBox ID="tbxJackMoney2" runat="server" CssClass="clsEdit" Width="80px"></asp:TextBox>
                            &nbsp;원
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                                ControlToValidate="tbxJackMoney2"  
                                ErrorMessage="스티플 잭팟지급머니를 입력하세요" SetFocusOnError="True" 
                                ValidationGroup="register">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator8" runat="server" 
                                ControlToValidate="tbxJackMoney2"  
                                ErrorMessage="스티플 잭팟지금머니는 수값이여야 합니다." Operator="DataTypeCheck" 
                                SetFocusOnError="True" Type="Integer" ValidationGroup="register">*</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="clsFieldValue">포카드</td>
                        <td class="clsFieldValue" align="center">
                            <asp:TextBox ID="tbxJackRatio1" runat="server" CssClass="clsEdit" Width="60px"></asp:TextBox>
                            &nbsp;%
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                ControlToValidate="tbxJackRatio1"  
                                ErrorMessage="포카드 잭팟지급률을 입력하세요" SetFocusOnError="True" 
                                ValidationGroup="register">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator6" runat="server" 
                                ControlToValidate="tbxJackRatio1"  
                                ErrorMessage="포카드 잭팟지급률은 수값이여야 합니다." Operator="DataTypeCheck" 
                                SetFocusOnError="True" Type="Double" ValidationGroup="register">*</asp:CompareValidator>
                        </td>
                        <td class="clsFieldValue" align="center">
                            <asp:TextBox ID="tbxJackMoney1" runat="server" CssClass="clsEdit" Width="80px"></asp:TextBox>
                            &nbsp;원
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                                ControlToValidate="tbxJackMoney1"  
                                ErrorMessage="포카드 잭팟지급머니를 입력하세요" SetFocusOnError="True" 
                                ValidationGroup="register">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator9" runat="server" 
                                ControlToValidate="tbxJackMoney1"  
                                ErrorMessage="포카드 잭팟지금머니는 수값이여야 합니다." Operator="DataTypeCheck" 
                                SetFocusOnError="True" Type="Integer" ValidationGroup="register">*</asp:CompareValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlControlBar" runat="server" CssClass="clsControlBar">
        <asp:CustomValidator ID="cvResult2" runat="server" ValidationGroup="register">*</asp:CustomValidator>
        <asp:Button ID="btnSave" class="btn btn-info" runat="server" Text="확인" OnClick="btnSave_Click"
            Width="132px" ValidationGroup="register" />
    </asp:Panel>
    <asp:ValidationSummary ID="vsErrors2" runat="server" 
        ValidationGroup="register" />
</asp:Content>

