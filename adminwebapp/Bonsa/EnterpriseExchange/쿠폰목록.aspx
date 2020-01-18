<%@ Page Language="C#" MasterPageFile="업체입출금관리.master" AutoEventWireup="true" CodeFile="쿠폰목록.aspx.cs"
    Inherits="업체입출금관리_쿠폰목록" %>
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
                쿠폰목록
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
                쿠폰추가
            </td>
        </tr>
    </table>
    <table style="border-right: #cacaca 1px solid; font-size: 12px; border-left: #cacaca 1px solid;
        border-top: #cacaca 1px solid; border-bottom: #cacaca 1px solid; border-collapse: collapse"
        bordercolor="#f1f1f1" cellspacing="0"  rules="rows" align="center"
        border="1" width="100%" height=30px>
        <tr>
            <td runat="server" id="tdAdmin">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="clsFieldName" align="right" width="100px" nowrap>
                            <asp:CustomValidator ID="cvRegCouponResult" Visible="false" runat="server" Display="Dynamic" ControlToValidate="tbxCouponName"
                                ValidationGroup="vgRegCoupon"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="rfvCouponName" runat="server" ControlToValidate="tbxCouponName"
                                ErrorMessage="쿠폰명을 입력하세요." ValidationGroup="charge">*</asp:RequiredFieldValidator>
                            쿠폰명:
                        </td>
                        <td class="clsItemLabel" nowrap align="left" width="" width="400px" nowrap>
                            &nbsp;
                            <asp:TextBox ID="tbxCouponName" runat="server" CssClass="clsEditableFieldValueE" ValidationGroup="userreg"
                                Width="200px"></asp:TextBox>&nbsp;&nbsp;
                        </td>
                        <td class="clsFieldName" align="right" width="100px" nowrap>
                            쿠폰금액:
                        </td>
                        <td class="clsItemLabel" nowrap align="left" width="" width="400px" nowrap>
                            &nbsp;
                            <asp:DropDownList runat="server" ID="ddlCouponMoney">
                                <asp:ListItem Selected=True Text="1만" Value="10000"></asp:ListItem>
                                <asp:ListItem Text="2만" Value="20000"></asp:ListItem>
                                <asp:ListItem Text="5만" Value="50000"></asp:ListItem>
                                <asp:ListItem Text="10만" Value="100000"></asp:ListItem>
                                <asp:ListItem Text="50만" Value="500000"></asp:ListItem>
                                <asp:ListItem Text="100만" Value="1000000"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                        </td>
                        <td class="clsItemLabel">
                            <asp:Button ID="btnRegCoupon" runat="server" CssClass="clsButton" Text="등록하기" OnClick="btnRegCoupon_Click"
                                ValidationGroup="vgRegCoupon" />
                        </td>
                        <td class="clsFieldName" align="right" nowrap>
                            <asp:Button ID="btn10KList" runat="server" CssClass="clsButton" Text="1만원권등록" OnClick="btn10KList_Click" />&nbsp;
                            <asp:Button ID="btn20KList" runat="server" CssClass="clsButton" Text="2만원권등록" OnClick="btn20KList_Click" />&nbsp;
                            <asp:Button ID="btn50KList" runat="server" CssClass="clsButton" Text="5만원권등록" OnClick="btn50KList_Click" />&nbsp;
                            <asp:Button ID="btn100KList" runat="server" CssClass="clsButton" Text="10만원권등록" OnClick="btn100KList_Click" />&nbsp;
                            <asp:Button ID="btn500KList" runat="server" CssClass="clsButton" Text="50만원권등록" OnClick="btn500KList_Click" />&nbsp;
                            <asp:Button ID="btn1MList" runat="server" CssClass="clsButton" Text="100만원권등록" OnClick="btn1MList_Click" />&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnShowUse" runat="server" CssClass="clsButton" Text="사용가능쿠폰" OnClick="btnShowUse_Click" />&nbsp;
                <asp:Button ID="btnShowUnUse" runat="server" CssClass="clsButton" Text="사용된쿠폰" OnClick="btnShowUnUse_Click" />&nbsp;
                <asp:Button ID="btnShowAll" runat="server" CssClass="clsButton" Text="전체보기" OnClick="btnShowAll_Click" />&nbsp;
            </td>
        </tr>
        <tr>
            <td width=100% nowrap colspan=10>
                <asp:ValidationSummary ID="vsRegCouponError" runat="server" ValidationGroup="vgRegCoupon" DisplayMode="SingleParagraph" />
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
                쿠폰목록
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlList" runat="server">
        <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="False" 
        Width=100% AllowPaging="True" DataKeyNames="ID" AllowSorting="True" 
        onpageindexchanging="grdLisTBL_PageIndexChanging" PageSize=20 
        onrowdatabound="grdLisTBL_RowDataBound" OnRowDeleting="grdLisTBL_RowDeleting" 
        onsorting="grdLisTBL_Sorting" EmptyDataText="검색된 자료가 없습니다."  >
            <Columns>
                <asp:TemplateField HeaderText="번호" SortExpression="ID">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkNo" CommandName="Select" CommandArgument='<%# Bind("ID") %>'
                            runat="server"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="60px" CssClass="GridCommandButton" />
                </asp:TemplateField>
                <asp:BoundField DataField="money" HeaderText="금액" SortExpression="money" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="name" HeaderText="쿠폰명" SortExpression="name">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="enterprisename" HeaderText="구입업체" SortExpression="enterprisename" NullDisplayText="--">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="username" HeaderText="구입유저" SortExpression="username" NullDisplayText="--">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="RegDate" HeaderText="등록날자" SortExpression="RegDate" DataFormatString="{0:yyyy-MM-dd HH:mm}">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="usedate" HeaderText="사용날짜" SortExpression="usedate" DataFormatString="{0:yyyy-MM-dd HH:mm}" NullDisplayText="--">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('정말로 삭제하시겠습니까?');" runat="server"
                            CausesValidation="False" CommandName="Delete" Text="삭제"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width="40px" Wrap=false />
                </asp:TemplateField>
            </Columns>
            <RowStyle CssClass="GridRow" />
            <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" />
            <PagerStyle HorizontalAlign="Center" CssClass="clsButton" />
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
            <HeaderStyle CssClass="GridHeader" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="#ecf8ff" />
        </asp:GridView>
        <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
            <asp:Button ID="btnRefresh" runat="server" CssClass="clsButton" Text="목록수정" OnClick="btnRefresh_Click" />
        </asp:Panel>
    </asp:Panel>
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>
