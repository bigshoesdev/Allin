<%@ Page Language="C#" MasterPageFile="회원관리.master" AutoEventWireup="true" CodeFile="11문의.aspx.cs"
    Inherits="회원관리_11문의목록" %>
<%@ MasterType VirtualPath="회원관리.master" %>

<%@ Import Namespace="System.Data" %>
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
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSysTitle">
                <h5>1:1 문의</h5>
            </td>
        </tr>
    </table>
    
<script src="../../scripts/socket.io.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $('select').selectlist({
            zIndex: 10,
            width: 120,
            height: 27
        });
    });

    
    $(function () {
        var socket = window.socket;

        if (!socket) {
            socket = io.connect("<%= ConfigurationManager.ConnectionStrings["AllinSocketString"].ConnectionString %>");
            window.socket = socket;
        }

        socket.on("askQuestion", function () {
            location.reload();
        });

        setTimeout(function () {
            $(window.parent.menu.document).find(".sub-menu-alarm").remove();
        }, 500);
    });
</script>    
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
                <td width="10%" class="srcTit" nowrap>
                    &nbsp;<i class="fa fa-circle-o" ></i>
                    &nbsp;&nbsp;<asp:DropDownList ID="ddlSearchKey" runat="server">
                        <asp:ListItem Value="title">제목</asp:ListItem>
                        <asp:ListItem Value="ncontent">내용</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="tbxSearchValue" runat="server" CssClass="clsEdit" OnTextChanged="btnSearch_Click"></asp:TextBox>
                    &nbsp;
                    <asp:Button ID="btnSearch" runat="server" Text="검색" class="btn btn-danger" OnClick="btnSearch_Click" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="False" 
        Width=100% AllowPaging="True" DataKeyNames="ID" AllowSorting="True" 
        onpageindexchanging="grdLisTBL_PageIndexChanging" 
        onrowdatabound="grdLisTBL_RowDataBound" OnRowDeleting="grdLisTBL_RowDeleting" 
        onsorting="grdLisTBL_Sorting" EmptyDataText="검색된 자료가 없습니다."  
        >
        <RowStyle CssClass="GridRow" />
        <PagerSettings Mode="NumericFirstLast" />
        <Columns>
            <asp:TemplateField HeaderText="번호">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkNo" CommandName="Select" CommandArgument='<%# Bind("ID") %>'
                        runat="server"></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle ForeColor="White" Width="60px" Wrap="false" />
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width=50px />
            </asp:TemplateField>
            <asp:BoundField DataField="nickname" HeaderText="닉네임" SortExpression="title">
                <ItemStyle HorizontalAlign="Left" Width="15%" />
            </asp:BoundField>
            <asp:BoundField DataField="title" HeaderText="제목" SortExpression="title">
                <ItemStyle HorizontalAlign="Left" Width="30%" />
            </asp:BoundField>
            <asp:BoundField DataField="ncontent" HeaderText="내용" SortExpression="title">
                <ItemStyle HorizontalAlign="Left" Width="30%" />
            </asp:BoundField>
            <asp:BoundField DataField="answer" HeaderText="답변" SortExpression="title">
                <ItemStyle HorizontalAlign="Left" Width="30%" />
            </asp:BoundField>
            <asp:BoundField DataField="createdDate" HeaderText="문의온 시간" SortExpression="createdDate" DataFormatString="{0:yyyy-MM-dd HH-mm-ss}">
                <ItemStyle HorizontalAlign="Center" Width="250px" Wrap=false />
            </asp:BoundField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" NavigateUrl='<%# "11문의답변.aspx?id=" + Eval("ID").ToString() %>' runat="server" CausesValidation="False"
                        Text="답변"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width=80px Wrap=false />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('정말로 삭제하시겠습니까?');" runat="server"
                        CausesValidation="False" CommandName="Delete" Text="삭제"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width="80px" Wrap=false />
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
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="새로고침" OnClick="btnRefresh_Click" />
        <asp:Button ID="btnAllDelete" class="btn btn-danger" OnClientClick="return confirm('실제로 모두 삭제하시겠습니까?');"
            runat="server" Text="전체삭제" OnClick="btnAllDelete_Click" />
    </asp:Panel>
    <br />
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>
