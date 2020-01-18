<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="가라방목록.aspx.cs" Inherits="게임관리_가라방목록" Title="코리아 게임 관리자페이지" %>

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
                <h5>가라방관리</h5>
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
                    &nbsp;검색된 가라방수
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]명
                </td>
                <td width=50%></td>
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
            <asp:TemplateField HeaderText="번호" SortExpression="ID">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkNo" runat="server" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" Width="30px" Wrap=false HorizontalAlign=Center />
            </asp:TemplateField>
            <asp:BoundField DataField="roomname" HeaderText="방이름" 
                SortExpression="roomname" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="bingmoney" DataFormatString="{0:N0}" HeaderText="삥머니" 
                SortExpression="bingmoney" >
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap=false />
            </asp:BoundField>
            <asp:BoundField DataField="curusercount" HeaderText="유저수" SortExpression="curusercount" 
                NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:TemplateField HeaderText="게임형태" SortExpression="gametype">
                <ItemTemplate>
                    <asp:Label ID="lblGameType" runat="server" Text='<%# Eval("gametype").ToString() == "1" ? "포커" : (Eval("gametype").ToString() == "2" ? "바둑이" : "맞고") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" Wrap="False" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="마감날자" SortExpression="expiretime">
                <ItemTemplate>
                    <asp:Label ID="lblExpireDate" runat="server" Text='<%# ((DateTime)Eval("expiretime")).ToString("yyyy-MM-dd HH:mm") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" />
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

