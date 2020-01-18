<%@ Page Language="C#" MasterPageFile="../Game/게임관리.master" AutoEventWireup="true" CodeFile="금칙어관리.aspx.cs" Inherits="게임관리_금칙어관리" Title="코리아 게임 관리자페이지" %>

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
                <h5>채팅 금칙어관리</h5>
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
        <table width="100%" border="0" cellpadding="7" cellspacing="1">
            <tr valign="middle">
                <td width="10%" class="srcTit" nowrap>
                    &nbsp;<img src="../../Images/ico_sqr02.gif" width="3" height="3" align="absmiddle">
                    &nbsp;&nbsp;차단된 금칙어수
                </td>
                <td class="cmtTit" nowrap>
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="pnlList" runat="server">
        <asp:GridView ID="grdList" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" CellPadding="3" CellSpacing="1" EmptyDataText="등록된 자료가 없습니다."
            ForeColor="#333333" GridLines="None" HorizontalAlign="Center" OnPageIndexChanging="grdLisTBL_PageIndexChanging"
            OnRowDataBound="grdLisTBL_RowDataBound" OnRowDeleting="grdLisTBL_RowDeleting" OnSorting="grdLisTBL_Sorting"
            Width="100%" DataKeyNames="id" Font-Size="10pt">
            <PagerSettings Mode="NumericFirstLast" />
            <Columns>
                <asp:TemplateField HeaderText="번호">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkNo" CommandName="Select" CommandArgument='<%# Bind("id") %>'
                            runat="server"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle ForeColor="White" Width="30px" Wrap="false" />
                    <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
                </asp:TemplateField>
                <asp:BoundField DataField="word" HeaderText="아이피" SortExpression="word">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('정말로 삭제하시겠습니까?');" runat="server"
                            CausesValidation="False" CommandName="Delete" Text="삭제"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" />
            <RowStyle BackColor="#EFF3FB" Height="20px" />
            <PagerStyle CssClass="PageButton"  HorizontalAlign="Center"
                Font-Bold="False" Font-Names="Verdana" Font-Size="10pt" />
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
            <HeaderStyle Height="25px" VerticalAlign="Middle" 
                ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="#EAEAEA" />
        </asp:GridView>
        <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
            <asp:Label ID="Label1" runat="server" Text="금칙어입력: "></asp:Label>
            <asp:TextBox ID="tbxWord" runat="server" CssClass="clsEdit" MaxLength="15" 
                ValidationGroup="list" Width="126px"></asp:TextBox>
                <asp:RequiredFieldValidator
                ID="rfvEndIP" runat="server" ControlToValidate="tbxWord" ErrorMessage="차단할 금칙어를 입력하세요"
                ValidationGroup="list">*</asp:RequiredFieldValidator>
                
            &nbsp;<asp:Button ID="btnAdd"
                        runat="server" class="btn btn-danger" OnClick="btnAdd_Click" Text="추가" ValidationGroup="list" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="새로고침" 
                OnClick="btnRefresh_Click" />
            <asp:Button ID="btnAllDelete" class="btn btn-info" OnClientClick="return confirm('실제로 모두 삭제하시겠습니까?');"
                runat="server" Text="전체삭제" OnClick="btnAllDelete_Click" />
        </asp:Panel>
    </asp:Panel>
    <br />
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>

