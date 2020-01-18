<%@ Page Language="C#" MasterPageFile="../Game/게임관리.master" AutoEventWireup="true" CodeFile="시스템설정.aspx.cs" Inherits="게임관리_환경설정" Title="코리아 게임 관리자페이지" %>

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
                <h5>시스템 설정</h5>
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
   
    <table style="display:none; border-right: #cacaca 1px solid; font-size: 12px; border-left: #cacaca 1px solid;
        border-top: #cacaca 1px solid; border-bottom: #cacaca 1px solid; border-collapse: collapse"
        bordercolor="#f1f1f1" cellspacing="0"  rules="rows" width="100%"
        align="center" border="1">
        <tr>
            <td class="clsFieldName" align="right" width=150px nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbxPassword"
                    ErrorMessage="비밀번호를 입력하세요" ValidationGroup="changepassword" 
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                새 비밀번호:
            </td>
            <td class="clsFieldValue" align="left">
                <asp:TextBox ID="tbxPassword" TextMode=Password runat="server" 
                    CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="170px" MaxLength="20"></asp:TextBox>
                &nbsp;
                새 비밀번호를 입력하세요.</td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right" nowrap>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbxPassword1"
                    ErrorMessage="비밀번호를 한번 더 입력하세요" ValidationGroup="changepassword" 
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator6" runat="server" 
                    ControlToCompare="tbxPassword" ControlToValidate="tbxPassword1" 
                    ErrorMessage="비밀번호가 정확치 않습니다" ValidationGroup="changepassword">*</asp:CompareValidator>
                새 비밀번호확인:
            </td>
            <td class="clsFieldValue" align="left">
                <asp:TextBox ID="tbxPassword1" TextMode=Password runat="server" 
                    CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="170px" MaxLength="20"></asp:TextBox>
                &nbsp;
                새 비밀번호를 한번 더 입력하세요.</td>
        </tr>
    </table>
    <asp:Panel ID="Panel1" runat="server" CssClass="clsControlBar" Visible="false">
        <asp:CustomValidator ID="cvResult1" runat="server" 
            ValidationGroup="changepassword">*</asp:CustomValidator>
        <asp:Button ID="btnChangePassword" CssClass="clsButton" runat="server" 
            Text="확인" OnClick="btnChangePassword_Click"
            Width="132px" ValidationGroup="changepassword" />
    </asp:Panel>
    <asp:ValidationSummary ID="vsErrors1" runat="server" 
        ValidationGroup="changepassword" />
    <br />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_syst001.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSubTitle">
                시스템 설정</td>
        </tr>
    </table>
    <table style="border-right: #cacaca 1px solid; font-size: 12px; border-left: #cacaca 1px solid;
        border-top: #cacaca 1px solid; border-bottom: #cacaca 1px solid; border-collapse: collapse"
        bordercolor="#cacaca" cellspacing="0"  rules="rows" width="100%"
        align="center" border="1">
        <tr>
            <td align="right" class="clsFieldName" nowrap style="padding-right: 15px" width=150px>
                <asp:RequiredFieldValidator ID="rfvSubtract" runat="server" 
                    ControlToValidate="tbxSubtract" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="환전수수료를 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvSubtract" runat="server" 
                    ControlToValidate="tbxSubtract" Display="Dynamic" 
                    ErrorMessage="0~20사이의 값을 입력하세요" MaximumValue="20" MinimumValue="0" 
                    Type="Integer" ValidationGroup="userreg">*</asp:RangeValidator>
                <asp:CompareValidator ID="cpvSubtract" runat="server" ControlToValidate="tbxSubtract"
                    ErrorMessage="환전수수료는 자연수 값이여야합니다." Operator="DataTypeCheck" Type="Integer" 
                    ValidationGroup="userreg">*</asp:CompareValidator>
                환전수수료:
            </td>
             <td align="left" class="clsFieldValue" nowrap >
                <asp:TextBox ID="tbxSubtract" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="40px" MaxLength="3">5</asp:TextBox>
                &nbsp;% 환전금에서 제하는 운영비 적립율 (0~20사이의 값). </td>
        </tr>
        <tr>
            <td align="right" class="clsFieldName" nowrap style="padding-right: 15px">
                <asp:RequiredFieldValidator ID="rfvCardSubtract" runat="server" 
                    ControlToValidate="tbxCardSubtract" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="카드수수료를 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rvCardSubtract" runat="server" 
                    ControlToValidate="tbxCardSubtract" Display="Dynamic" 
                    ErrorMessage="0~100사이의 값을 입력하세요" MaximumValue="100" MinimumValue="0" 
                    Type="Integer" ValidationGroup="userreg">*</asp:RangeValidator>
                <asp:CompareValidator ID="cvCardSubtract" runat="server" ControlToValidate="tbxCardSubtract"
                    ErrorMessage="카드수수료는 수값을 입력하세요." Operator="DataTypeCheck" Type="Integer" 
                    ValidationGroup="userreg">*</asp:CompareValidator>
                카드수수료:
            </td>
             <td align="left" class="clsFieldValue" nowrap>
                <asp:TextBox ID="tbxCardSubtract" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="40px" MaxLength="3">5</asp:TextBox>
                &nbsp;% 당첨금에서 제하는 운영비적립률 (0~100사이의 값). </td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">
                <asp:RequiredFieldValidator ID="rfvStartMoney" runat="server" 
                    ControlToValidate="tbxStartMoney" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="초기적립금을 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cpvStartMoney" runat="server" ControlToValidate="tbxStartMoney"
                    ErrorMessage="초기적립금은 수값을 입력하세요." Operator="DataTypeCheck" Type="Integer" 
                    ValidationGroup="userreg">*</asp:CompareValidator>
                초기적립금:
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:TextBox ID="tbxStartMoney" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="60px" MaxLength="7">100</asp:TextBox>
                &nbsp;원&nbsp; 온라인 회원이 새로 가입할때 주는 게임머니
            </td>
        </tr>
              
        <tr>
            <td class="clsFieldName" align="right">
                <asp:RequiredFieldValidator ID="rfvExchangeRatio" runat="server" 
                    ControlToValidate="tbxExchangeRatio" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage=" 달러->원환전비율은 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvExchangeRatio" runat="server" ControlToValidate="tbxExchangeRatio"
                    ErrorMessage=" 달러->원환전비율은 수값을 입력하세요." Operator="DataTypeCheck" Type="Double" ValidationGroup="userreg">*</asp:CompareValidator>
                달러->원환전비율:
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:TextBox ID="tbxExchangeRatio" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="60px" MaxLength="7">100</asp:TextBox>
                &nbsp;원&nbsp;1달러당 원화&nbsp;</td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">
                <asp:RequiredFieldValidator ID="rfvMinChargeMoney" runat="server" 
                    ControlToValidate="tbxMinChargeMoney" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="최소현금충전금액을 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvMinChargeMoney" runat="server" ControlToValidate="tbxMinChargeMoney"
                    ErrorMessage="최소현금충전금액은 수값을 입력하세요." Operator="DataTypeCheck" Type="Integer" ValidationGroup="userreg">*</asp:CompareValidator>
                최소현금충전금액:
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:TextBox ID="tbxMinChargeMoney" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="60px" MaxLength="12">30000</asp:TextBox>
                &nbsp;원&nbsp; 입금최소제한금액</td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">
                <asp:RequiredFieldValidator ID="rfvMinCardChargeMoney" runat="server" 
                    ControlToValidate="tbxMinCardChargeMoney" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="최소카드충전금액을 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvMinCardChargeMoney" runat="server" ControlToValidate="tbxMinCardChargeMoney"
                    ErrorMessage="최소카드충전금액은 수값을 입력하세요." Operator="DataTypeCheck" Type="Integer" ValidationGroup="userreg">*</asp:CompareValidator>
                최소카드충전금액:
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:TextBox ID="tbxMinCardChargeMoney" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="60px" MaxLength="10">100</asp:TextBox>
                &nbsp;$&nbsp; 최소카드충전금액</td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">
                <asp:RequiredFieldValidator ID="rfvMinWithdrawMoney" runat="server" 
                    ControlToValidate="tbxMinWithdrawMoney" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="최소현금환전금액을 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvMinWithdrawMoney" runat="server" ControlToValidate="tbxMinWithdrawMoney"
                    ErrorMessage="최소현금환전금액은 수값을 입력하세요." Operator="DataTypeCheck" Type="Integer" ValidationGroup="userreg">*</asp:CompareValidator>
                최소현금환전금액:
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:TextBox ID="tbxMinWithdrawMoney" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="60px" MaxLength="12">30000</asp:TextBox>
                &nbsp;원&nbsp; 출금최소제한금액</td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">
                <asp:RequiredFieldValidator ID="rfvMinCardWithdrawMoney" runat="server" 
                    ControlToValidate="tbxMinCardWithdrawMoney" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="최소카드환전금액을 입력하세요." SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvMinCardWithdrawMoney" runat="server" ControlToValidate="tbxMinCardWithdrawMoney"
                    ErrorMessage="최소카드환전금액은 자연수값을 입력하세요." Operator="DataTypeCheck" Type="Integer" ValidationGroup="userreg">*</asp:CompareValidator>
                최소카드환전금액:
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:TextBox ID="tbxMinCardWithdrawMoney" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="60px" MaxLength="10">100</asp:TextBox>
                &nbsp;$&nbsp; 최소카드환전금액</td>
        </tr>
         <tr>
            <td class="clsFieldName" align="right">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="tbxRecommendPercent" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="추천인 판돈 지급율을 입력하세요." SetFocusOnError="True"
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeValidator1" runat="server" 
                    ControlToValidate="tbxRecommendPercent" Display="Dynamic" 
                    ErrorMessage="0~99사이의 자연수값이여야 합니다." MaximumValue="99" MinimumValue="0" 
                    Type="Integer" ValidationGroup="userreg">*</asp:RangeValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="tbxMinCardWithdrawMoney"
                    ErrorMessage="0부터 99 까지인 자연수값이여야 합니다." Operator="DataTypeCheck" Type="Integer" ValidationGroup="userreg">*</asp:CompareValidator>
                추천인 판돈 지급율:
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:TextBox ID="tbxRecommendPercent" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="60px" MaxLength="10">0</asp:TextBox>
                &nbsp;%&nbsp; 0 부터 99 사이</td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="tbxNoLoginDays" CssClass="clsErrorMark" Display="Dynamic" 
                    ErrorMessage="회원리스트 휴면일자를 입력하세요." SetFocusOnError="True"
                    ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeValidator2" runat="server" 
                    ControlToValidate="tbxNoLoginDays" Display="Dynamic" 
                    ErrorMessage="0~9999사이의 자연수값이여야 합니다." MaximumValue="9999" MinimumValue="0" 
                    Type="Integer" ValidationGroup="userreg">*</asp:RangeValidator>
                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tbxNoLoginDays"
                    ErrorMessage="0부터 9999 까지인 자연수값이여야 합니다." Operator="DataTypeCheck" Type="Integer" ValidationGroup="userreg">*</asp:CompareValidator>
                회원 휴면일자:
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:TextBox ID="tbxNoLoginDays" runat="server" CssClass="clsEditableFieldValue" ValidationGroup="userreg"
                    Width="60px" MaxLength="4">0</asp:TextBox>&nbsp;일
                 0 부터 9999 사이</td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">
                카드입금
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:CheckBox ID="cbxAllowCardCharge" runat="server" CssClass="clsEditableFieldValue"
                     style="vertical-align:middle;display:inline-block;"></asp:CheckBox>
                     <span style="vertical-align:middle; display:inline-block; padding-top:5px;">허용</span>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">
                전체회원채팅
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:CheckBox ID="cbxAllStopChat" runat="server" CssClass="clsEditableFieldValue"
                     style="vertical-align:middle;display:inline-block;"></asp:CheckBox>
                     <span style="vertical-align:middle; display:inline-block; padding-top:5px;">금지</span>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">
                회원초대기능
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:RadioButton ID="rbEnableInvokeOn" runat="server" GroupName="enableinvoke"  style="vertical-align:middle;display:inline-block;"/>
                <span style="vertical-align:middle; display:inline-block; padding-top:5px;">사용</span>
                &nbsp;
                <asp:RadioButton ID="rbEnableInvokeOff" runat="server" GroupName="enableinvoke" style="vertical-align:middle;display:inline-block;"/>
                <span style="vertical-align:middle; display:inline-block; padding-top:5px;">사용안함</span>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName" align="right">
                신규회원가입<br />
                아이피중복검사
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:CheckBox ID="cbxDupIPCheck" runat="server" CssClass="clsEditableFieldValue"
                    style="vertical-align:middle;display:inline-block;"></asp:CheckBox>
                    <span style="vertical-align:middle; display:inline-block; padding-top:5px;">사용</span>
            </td>
        </tr>
          <tr>
            <td class="clsFieldName" align="right">
                휴대폰 인증 여부
            </td>
            <td class="clsFieldValue" nowrap align="left">
                <asp:CheckBox ID="cbxMobileConfirm" runat="server" CssClass="clsEditableFieldValue"
                    style="vertical-align:middle;display:inline-block;"></asp:CheckBox>
                    <span style="vertical-align:middle; display:inline-block; padding-top:5px;">사용</span>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlControlBar" runat="server" CssClass="clsControlBar">
        <asp:CustomValidator ID="cvResult2" runat="server" ValidationGroup="userreg">*</asp:CustomValidator>
        <asp:Button ID="btnSave" class="btn btn-info" runat="server" Text="확인" OnClick="btnSave_Click"
            Width="132px" ValidationGroup="userreg" />
    </asp:Panel>
    <asp:ValidationSummary ID="vsErrors2" runat="server" 
        ValidationGroup="userreg" />
</asp:Content>

