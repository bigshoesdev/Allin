<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="포인트전환내역.aspx.cs" Inherits="올인관리_포인트전환내역" Title="코리아 게임 관리자페이지" %>

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
                <h5>
                <asp:Label ID="lblChannel" runat="server"></asp:Label>
&nbsp;포인트전환내역</h5>
            </td>
        </tr>
    </table>

    
<script type="text/javascript">
     $(function() {
         $('select').selectlist({
             zIndex: 10,
             width: 120,
             height: 27
         });
     })
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
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]자료
                </td>
                <td width="10%" class="srcTit" nowrap>
                    &nbsp;<i class="fa fa-circle-o" ></i>&nbsp;선택&nbsp; <asp:DropDownList ID="ddlSearchKey" runat="server">
                        <asp:ListItem Value="EntryNickName">닉네임</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="15%">
                    <asp:TextBox ID="tbxSearchValue" runat="server" CssClass="clsEdit" 
                        OnTextChanged="btnSearch_Click" Width="150px"></asp:TextBox>
                </td>
                <td class="srcTit" nowrap="nowrap" width="20%">
                    &nbsp;<img src="../../Images/ico_sqr02.gif" width="3" height="3" align="absmiddle">
                    날자:
                    <asp:TextBox ID="tbxStartDate" runat="server" CssClass="clsEdit" 
                        Width="150px" onclick="Calendar(this)" ></asp:TextBox>
                    부터&nbsp;&nbsp;<asp:TextBox ID="tbxEndDate" runat="server" CssClass="clsEdit"
                        Width="150px" onclick="Calendar(this)"></asp:TextBox>
                    까지
                </td>
                <td >
                    <asp:Button ID="btnSearch" runat="server" Text="검색" class="btn btn-danger" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>
    </div>
    
    <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="False" 
        Width=100% AllowPaging="True" DataKeyNames="ID" AllowSorting="True" 
        onpageindexchanging="grdList_PageIndexChanging" 
        onrowdatabound="grdList_RowDataBound" OnRowDeleting="grdList_RowDeleting" 
        onsorting="grdList_Sorting" EmptyDataText="검색된 자료가 없습니다."  
        >
        <RowStyle CssClass="GridRow" />
        <Columns>
            <asp:TemplateField HeaderText="번호">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkNo" CommandName="Select" CommandArgument='<%# Bind("ID") %>'
                        runat="server"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width=40px />
            </asp:TemplateField>
            <asp:BoundField DataField="createdDate" HeaderText="날자" 
                SortExpression="createdDate" DataFormatString="  {0:yyyy-MM-dd HH:mm:ss}  ">
                <ItemStyle HorizontalAlign="center" Width="120px" Wrap="true" CssClass="BrDateGridRow" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="회원이름" SortExpression="userName">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkViewDetail1" Text='<%# Eval("userName") %>'  NavigateUrl='<%# "../User/회원이력정보.aspx?id=" + Eval("userId").ToString() + "&ReturnUrl=" + Request.Path %>' runat="server" CausesValidation="False"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" CssClass="GridCommandButton" />
            </asp:TemplateField>
            <asp:BoundField DataField="convertPoints" HeaderText="전환 포인트" SortExpression="convertPoints" DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="remaindPoints" HeaderText="남은 포인트" SortExpression="remaindPoints" DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="right" />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" />
        <PagerStyle  HorizontalAlign="Center" 
            CssClass="PagerButton" />
        <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
        <HeaderStyle CssClass="GridHeader"  
            ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="#ecf8ff" />
    </asp:GridView>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="새로고침" OnClick="btnRefresh_Click" />
    </asp:Panel>
    <br />
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>

