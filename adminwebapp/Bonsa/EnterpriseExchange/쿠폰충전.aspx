<%@ Page Language="C#" MasterPageFile="업체입출금관리.master" AutoEventWireup="true" CodeFile="쿠폰충전.aspx.cs"
    Inherits="업체입출금관리_쿠폰충전" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyPlaceHolder" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSysTitle">
                쿠폰충전
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
                쿠폰충전
            </td>
        </tr>
    </table>
    <table style="border-right: #cacaca 1px solid; font-size: 12px; border-left: #cacaca 1px solid;
        border-top: #cacaca 1px solid; border-bottom: #cacaca 1px solid; border-collapse: collapse"
        bordercolor="#f1f1f1" cellspacing="0"  rules="rows" align="center"
        border="1" width="100%" height=30px>
        <tr>
            <td class="clsFieldName" align="right" width="100px" nowrap>
                <asp:CustomValidator ID="cvChargeResult" Visible="false" runat="server" Display="Dynamic"
                    ValidationGroup="charge"></asp:CustomValidator>
                <asp:CompareValidator ID="cvMoney" runat="server" ControlToValidate="tbxChargeMoney" Display="Dynamic"
                    ErrorMessage="수값을 입력하세요" Operator="DataTypeCheck" SetFocusOnError="True" Type="Integer"
                    ValidationGroup="charge">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="rfvMoney" runat="server" ControlToValidate="tbxChargeMoney"
                    ErrorMessage="충전할 금액을 입력하세요." ValidationGroup="charge">*</asp:RequiredFieldValidator>
                충전머니:
            </td>
            <td class="clsItemLabel" nowrap align="left" width="" width="400px" nowrap>
                &nbsp;
                <asp:TextBox ID="tbxChargeMoney" runat="server" CssClass="clsEditableFieldValueE" ValidationGroup="userreg"
                    Width="70px"></asp:TextBox>원&nbsp;&nbsp;
            </td>
            <td class="clsItemLabel">
                <asp:Button ID="btnCharge" runat="server" CssClass="clsButton" Text="요청하기" OnClick="btnCharge_Click"
                    ValidationGroup="charge" />
            </td>
            <td class="clsFieldName" align="right" width="100px" nowrap>
            </td>
            <td class="clsItemLabel" nowrap align="left" width="" width="400px" nowrap>
            </td>
            <td class="clsItemLabel">
            </td>
        </tr>
        <tr>
            <td width=100% nowrap colspan=10>
                <asp:ValidationSummary ID="vsChargeError" runat="server" ValidationGroup="charge" DisplayMode="SingleParagraph" />
            </td>
        </tr>
    </table>
    <br />
    <table class="clsFuncPanel">
        <tr>
            <td>
                <img src="../../Images/ico_syst001.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSubTitle">
                쿠폰충전내역
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlList" runat="server">
        <asp:GridView ID="grdList" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" CellPadding="3" CellSpacing="1" EmptyDataText="등록된 자료가 없습니다."
            ForeColor="#333333" GridLines="None" HorizontalAlign="Center" OnPageIndexChanging="grdLisTBL_PageIndexChanging"
            OnRowDataBound="grdLisTBL_RowDataBound" OnRowDeleting="grdLisTBL_RowDeleting" OnSorting="grdLisTBL_Sorting"
            Width="100%" DataKeyNames="ID" OnRowEditing="grdLisTBL_RowEditing" Font-Size="10pt">
            <PagerSettings Mode="NumericFirstLast" />
            <Columns>
                <asp:TemplateField HeaderText="번호" SortExpression="ID">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkNo" CommandName="Select" CommandArgument='<%# Bind("ID") %>'
                            runat="server"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="60px" CssClass="GridCommandButton" />
                </asp:TemplateField>
                <asp:BoundField DataField="Money" HeaderText="요청금액" SortExpression="Money" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="bankname" HeaderText="은행명" SortExpression="bankname">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="banknum" HeaderText="계좌정보" SortExpression="banknum">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="mastername" HeaderText="예금주" SortExpression="mastername">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="RegDate" HeaderText="등록날자" SortExpression="RegDate" DataFormatString="{0:yyyy-MM-dd HH:mm}">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="상태" SortExpression="State">
                    <ItemTemplate>
                        <asp:Label ID="lblNo" Text='<%# Eval("State").ToString() == "0" ? "미완료" : "완료" %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" />
            <RowStyle BackColor="#EFF3FB" Height="20px" />
            <PagerStyle CssClass="PageButton"  HorizontalAlign="Center"
                Font-Bold="False" Font-Names="Verdana" Font-Size="10pt" />
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
            <HeaderStyle Height="25px" VerticalAlign="Middle" 
                HorizontalAlign="Center" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="#EAEAEA" />
        </asp:GridView>
        <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
            <asp:Button ID="btnRefresh" runat="server" CssClass="clsButton" Text="목록수정" OnClick="btnRefresh_Click" />
        </asp:Panel>
    </asp:Panel>
    <br />
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>
