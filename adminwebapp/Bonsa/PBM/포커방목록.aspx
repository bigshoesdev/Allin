<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="포커방목록.aspx.cs" Inherits="게임관리_포커방목록" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" Runat="Server">
    <style>
        a
        {
        	font-size:9pt;
        }
        span
        {
        	font-size:9pt;
        }
    </style>
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
                <h5>포커 - 
                <asp:Label ID="lblChannel" runat="server"></asp:Label>
                &nbsp;게임방정보</h5>
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
                <td width="10%" class="srcTit" nowrap >
                   <i class="fa fa-circle-o" ></i>
                    &nbsp;결과&nbsp;: 총 
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]개
                </td>
                <td width="10%" class="srcTit" nowrap style="display: none">
                    &nbsp;
                    &nbsp;&nbsp;</td>
                <td style="display: none">
                    &nbsp;
                    </td>
                <td>
                    &nbsp;
                </td>
                <td>
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
    <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="False" 
        Width=100% AllowPaging="True" DataKeyNames="ID" AllowSorting="True" 
        onpageindexchanging="grdList_PageIndexChanging" 
        onrowdatabound="grdList_RowDataBound" OnRowDeleting="grdList_RowDeleting" 
        onsorting="grdList_Sorting" EmptyDataText="검색된 자료가 없습니다." >
        <RowStyle CssClass="GridRow" />
        <Columns>
            <asp:TemplateField HeaderText="번호">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkNo" CommandName="Select" CommandArgument='<%# Bind("ID") %>'
                        runat="server"></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle ForeColor="White" Width="60px" Wrap="false" />
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width=40px />
            </asp:TemplateField>
            <asp:BoundField DataField="RoomName" HeaderText="방이름" SortExpression="RoomName">
                <ItemStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="BingMoney" HeaderText="삥머니" SortExpression="BingMoney" DataFormatString="{0}원">
                <ItemStyle HorizontalAlign="right" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="입장머니">
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("EnterMinMoney", "{0:N0}") + "/" + Eval("EnterMaxMoney", "{0:N0}") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="center" />
            </asp:TemplateField>
            <asp:BoundField DataField="Password" HeaderText="방비번" SortExpression="Password" NullDisplayText="없음">
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="게임방식" SortExpression="GameMode">
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("GameMode").ToString() == "1" ? "서비스카드4장" : "서비스카드3장" %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="상태" SortExpression="IsUsed">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("IsUsed").ToString() == "1" ? "경기중" : "대기중" %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="CurUserCount" HeaderText="접속자수" 
                SortExpression="CurUserCount" DataFormatString="{0:N0}명">
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="아이디">
                <ItemTemplate>
                    <asp:Table ID="tblUserIDList" runat=server CellPadding=3></asp:Table>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="접속아이피">
                <ItemTemplate>
                    <asp:Table ID="tblLoginIPList" runat=server CellPadding=3></asp:Table>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="접속시간">
                <ItemTemplate>
                    <asp:Table ID="tblRegDateList" runat=server CellPadding=3></asp:Table>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="center" />
            </asp:TemplateField>
            <asp:BoundField DataField="regdate" HeaderText="등록시간" SortExpression="regdate" DataFormatString="{0:yyyy-MM-dd HH:mm}">
                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap=false />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="#CCCC99" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" 
            CssClass="PagerButton" />
        <SelectedRowStyle BackColor="#F7F7DE" Font-Bold="True" />
        <HeaderStyle CssClass="GridHeader"  />
        <AlternatingRowStyle BackColor="#ecf8ff" />
    </asp:GridView>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:Button ID="btnList" runat="server" CssClass="clsButton" Text="목록보기" 
            OnClick="btnList_Click" Visible=false />
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="새로고침" OnClick="btnRefresh_Click" />
        &nbsp;<asp:Button ID="btnAllDelete" CssClass="clsButton" OnClientClick="return confirm('실제로 모두 삭제하시겠습니까?');"
            runat="server" Text="전체삭제" OnClick="btnAllDelete_Click" Visible=false />
    </asp:Panel>
    <br />
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>

