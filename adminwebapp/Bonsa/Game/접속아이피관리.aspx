<%@ Page Language="C#" MasterPageFile="../Game/게임관리.master" AutoEventWireup="true" CodeFile="접속아이피관리.aspx.cs" Inherits="게임관리_접속아이피관리" Title="코리아 게임 관리자페이지" %>

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
                <h5>접속IP관리</h5>
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
                    &nbsp;&nbsp;차단된 접속IP수
                </td>
                <td class="cmtTit" nowrap>
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]
                </td>
                <td>
                    &nbsp;
                </td>
                <!--
                <td align="right">
                    <asp:Label ID="lblSortExpr" runat="server" CssClass="clsLabelE" Font-Size="10pt"
                        Height="11px">정돈 안함</asp:Label>
                    <asp:Panel ID="pnlAscSortLbl" Wrap="False" runat="server" CssClass="clsSortPanel"
                        Visible="False">
                        <table class="clsSortPanel" id="tblAscSortLbl" cellspacing="0" cellpadding="0" width="10"
                            align="center" border="0">
                            <tr>
                                <td valign="bottom">
                                    A
                                </td>
                                <td style="font-size: 12pt" valign="top" rowspan="2">
                                    ↓
                                </td>
                            </tr>
                            <tr>
                                <td style="color: red" valign="top">
                                    Z
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlDescSortLbl" Wrap="False" runat="server" CssClass="clsSortPanel"
                        Visible="False">
                        <table class="clsSortPanel" id="tblDescSortLbl" cellspacing="0" cellpadding="0" width="10"
                            align="center" border="0">
                            <tr>
                                <td style="color: red" valign="bottom">
                                    Z
                                </td>
                                <td style="font-size: 11pt" valign="middle" rowspan="2">
                                    ↓
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    A
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Label ID="lblTrialSortExpr" runat="server" CssClass="clsLabelE" Height="11px">&nbsp;로 정돈</asp:Label>
                </td>
                -->
                
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
                <%--<asp:BoundField DataField="startip" HeaderText="시작아이피" SortExpression="startip">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="endip" HeaderText="끝아이피" SortExpression="endip" >
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>--%>
                <asp:BoundField DataField="ipaddr" HeaderText="아이피" SortExpression="ipaddr">
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
            <asp:Label ID="Label1" runat="server" Text="차단할아이피: "></asp:Label>
            <%--<asp:RequiredFieldValidator
                ID="rfvIP" runat="server" ControlToValidate="tbxStartIP" ErrorMessage="차단할 시작아이피를 입력하세요"
                ValidationGroup="list">*</asp:RequiredFieldValidator>
            <asp:TextBox ID="tbxStartIP" runat="server"
                    CssClass="clsEdit" ValidationGroup="list" MaxLength="15" Width="126px"></asp:TextBox>
            &nbsp;~
            <asp:TextBox ID="tbxEndIP" runat="server" CssClass="clsEdit" MaxLength="15" 
                ValidationGroup="list" Width="126px"></asp:TextBox>
                <asp:RequiredFieldValidator
                ID="rfvEndIP" runat="server" ControlToValidate="tbxEndIP" ErrorMessage="차단할 마지막아이피를 입력하세요"
                ValidationGroup="list">*</asp:RequiredFieldValidator>--%>
            <asp:TextBox ID="tbxIPAddr" runat="server" CssClass="clsEdit" MaxLength="15" 
                ValidationGroup="list" Width="126px"></asp:TextBox>
                <asp:RequiredFieldValidator
                ID="rfvEndIP" runat="server" ControlToValidate="tbxIPAddr" ErrorMessage="차단할 아이피를 입력하세요"
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

