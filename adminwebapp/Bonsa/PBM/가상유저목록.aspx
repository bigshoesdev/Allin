<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="가상유저목록.aspx.cs" Inherits="게임관리_가상유저목록" Title="코리아 게임 관리자페이지" %>

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
                <h5>가상유저관리</h5>
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
                <td width="150px" class="srcTit" nowrap>
                   <i class="fa fa-circle-o" ></i>
                    &nbsp;검색된 가상유저수
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]명
                </td>
                <td width=50%></td>
                <!--
                <td align="right" nowrap>
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
                    <asp:Label ID="lblTrialSortExpr" runat="server" CssClass="clsLabelE" Font-Size=10pt>&nbsp;로 정돈</asp:Label>
                </td>
                -->
                
            </tr>
        </table>
    </div>
    <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="False" 
        Width=100% AllowPaging="True" DataKeyNames="ID" AllowSorting="True" 
        onpageindexchanging="grdLisTBL_PageIndexChanging" PageSize=20
        onrowdatabound="grdLisTBL_RowDataBound" OnRowDeleting="grdLisTBL_RowDeleting" 
        onsorting="grdLisTBL_Sorting" EmptyDataText="검색된 자료가 없습니다."  
        >
        <RowStyle CssClass="GridRow" />
        <Columns>
            <%--<asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkViewSub" NavigateUrl='<%# "회원목록.aspx?parentid=" + Eval("ID").ToString() %>' runat="server" CausesValidation="False"
                        Text="하위보기"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="번호" SortExpression="ID">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkNo" runat="server" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" Width="30px" Wrap=false HorizontalAlign=Center />
            </asp:TemplateField>
            <asp:BoundField DataField="Nickname" HeaderText="닉네임" 
                SortExpression="Nickname" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="money" DataFormatString="{0:N0}" HeaderText="보유머니" 
                SortExpression="money" >
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap=false />
            </asp:BoundField>
            <asp:BoundField DataField="avatarNo" HeaderText="아바타" SortExpression="avatar" 
                NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:TemplateField HeaderText="사운드" SortExpression="SexySound">
                <ItemTemplate>
                    <asp:Label ID="lblSexySound" runat="server" Text='<%# Eval("SexySound").ToString() == "0" ? "일반사운드" : "섹시사운드" %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" Wrap="False" />
            </asp:TemplateField>
            <asp:BoundField DataField="allinwincount" HeaderText="올인승" SortExpression="pokerwincount" DataFormatString="{0:N0}번" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="allinlosecount" DataFormatString="{0:N0}번" 
                HeaderText="올인패" SortExpression="pokerlosecount" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:TemplateField HeaderText="등록날자" SortExpression="changedate">
                <ItemTemplate>
                    <asp:Label ID="lblRegDate" runat="server" Text='<%# ((DateTime)Eval("changedate")).ToString("yyyy-MM-dd") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" />
            </asp:TemplateField>
            
             <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkCharge" NavigateUrl='<%# "javascript:openNewWindow(\"RobotCharge.aspx\", " + Eval("id").ToString() + ")"%>' runat="server" CausesValidation="False"
                        Text="충전"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
             <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkWithCharge" NavigateUrl='<%# "javascript:openNewWindow(\"RobotWithdraw.aspx\", " + Eval("id").ToString() + ")"%>' runat="server" CausesValidation="False"
                        Text="출금"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
            
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" NavigateUrl='<%# "가상유저등록.aspx?id=" + Eval("ID").ToString() %>' runat="server" CausesValidation="False"
                        Text="수정"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
            
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('정말로 삭제하시겠습니까?');" runat="server"
                        CausesValidation="False" CommandName="Delete" Text="삭제"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" />
        <PagerStyle HorizontalAlign="Center" CssClass="clsButton" />
        <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
        <HeaderStyle CssClass="GridHeader" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="#ecf8ff" />
    </asp:GridView>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:Button ID="btnNew" runat="server" class="btn btn-info" Text="새로등록" onclick="btnNew_Click" />
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="목록새로고침" 
            onclick="btnRefresh_Click" />
    </asp:Panel>
</asp:Content>

