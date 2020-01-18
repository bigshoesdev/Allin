<%@ Page Language="C#" MasterPageFile="../EnterpriseExchange/업체입출금관리.master" AutoEventWireup="true" CodeFile="업체별정산.aspx.cs" Inherits="업체입출금관리_업체별정산" Title="코리아 게임 관리자페이지" %>

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
                <h5>업체정산</h5>
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
                <td class="srcTit" nowrap>
                <i class="fa fa-circle-o" ></i>&nbsp;업체명:&nbsp;
                    <asp:DropDownList ID=ddlEnterprise runat=server Width=80px AutoPostBack="True" 
                        onselectedindexchanged="ddlEnterprise_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td class="srcTit" nowrap>
                    &nbsp;<i class="fa fa-circle-o" ></i>&nbsp;
                    날자:
                    <asp:TextBox ID="tbxStartDate" runat="server" CssClass="clsEdit" 
                        Width="150px" onclick="Calendar(this)" ></asp:TextBox>
                    부터&nbsp;&nbsp;<asp:TextBox ID="tbxEndDate" runat="server" CssClass="clsEdit"
                        Width="150px" onclick="Calendar(this)"></asp:TextBox>
                    까지
                </td>
                <td align=left>
                    <asp:Button ID="btnSearch" runat="server" Text="검색" class="btn btn-danger" OnClick="btnSearch_Click" />
                </td>
                <td width=80%>
                    <asp:Button ID="btn1" runat="server" CommandArgument="0" class="btn btn-success" EnableViewState=false
                        onclick="btnDay_Click" Text="오늘" />
&nbsp;<asp:Button ID="btn2" runat="server" CommandArgument="1" class="btn btn-success" EnableViewState=false
                        onclick="btnDay_Click" Text="어제" />                        
&nbsp;<asp:Button ID="btn3" runat="server" CommandArgument="2" class="btn btn-success" EnableViewState=false
                        onclick="btnDay_Click" Text="2일전" />                       
&nbsp;<asp:Button ID="btn4" runat="server" CommandArgument="3" class="btn btn-success" EnableViewState=false
                        onclick="btnDay_Click" Text="3일전" />                       
&nbsp;<asp:Button ID="btn5" runat="server" CommandArgument="4" class="btn btn-success" EnableViewState=false
                        onclick="btnDay_Click" Text="4일전" />                       
&nbsp;<asp:Button ID="btn6" runat="server" CommandArgument="6" class="btn btn-success" EnableViewState=false
                        onclick="btnDay_Click" Text="5일전" />                       
&nbsp;<asp:Button ID="btn7" runat="server" CommandArgument="6" class="btn btn-success" EnableViewState=false
                        onclick="btnDay_Click" Text="6일전" />
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="False" 
        Width=100% AllowPaging="True" AllowSorting="True" 
        onpageindexchanging="grdLisTBL_PageIndexChanging" 
        EmptyDataText="검색된 자료가 없습니다."  
         ShowFooter="True" 
        onsorting="grdLisTBL_Sorting">
        <RowStyle CssClass="GridRow" />
        <PagerSettings Mode="NumericFirstLast" />
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="업체명" SortExpression="Name">
                <ItemStyle HorizontalAlign="center" />
                <FooterStyle HorizontalAlign="center" />
            </asp:BoundField>
           
            <asp:BoundField DataField="c_sum" HeaderText="총입금액" 
                DataFormatString="{0:N0}" NullDisplayText="0원" SortExpression="c_sum">
                <ItemStyle HorizontalAlign="right" />
                <FooterStyle HorizontalAlign=right />
            </asp:BoundField>
            <asp:BoundField DataField="c_count" HeaderText="입금회수" 
                DataFormatString="{0:N0}회" NullDisplayText="0회" SortExpression="c_count">
                <ItemStyle HorizontalAlign="center" />
                <FooterStyle HorizontalAlign=center />
            </asp:BoundField>
            <asp:BoundField DataField="w_sum" HeaderText="총출금액" 
                DataFormatString="{0:N0}" NullDisplayText="0원" SortExpression="Withdw_sumrawMoney">
                <ItemStyle HorizontalAlign="right" />
                <FooterStyle HorizontalAlign=right />
            </asp:BoundField>
            <asp:BoundField DataField="w_count" HeaderText="출금회수" 
                DataFormatString="{0:N0}회" NullDisplayText="0회" SortExpression="w_count">
                <ItemStyle HorizontalAlign="center" />
                <FooterStyle HorizontalAlign=center />
            </asp:BoundField>
             <asp:BoundField DataField="fee" HeaderText="발생금액" 
                DataFormatString="{0:N0}" NullDisplayText="0" SortExpression="fee">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign=Right />
            </asp:BoundField>
             <asp:BoundField DataField="classpercent" HeaderText="수익지분율" 
                DataFormatString="{0:N1}%" NullDisplayText="0" SortExpression="classpercent">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign=Right />
            </asp:BoundField>
            <asp:BoundField DataField="ent_fee" HeaderText="업체수익" 
                DataFormatString="{0:N0}" NullDisplayText="0" SortExpression="ent_fee">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign=Right />
            </asp:BoundField>
             <asp:BoundField DataField="real_fee" HeaderText="본사수익" 
                DataFormatString="{0:N0}" NullDisplayText="0" SortExpression="ent_fee">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign=Right />
            </asp:BoundField>
           
        </Columns>
        <FooterStyle BackColor="White" Font-Bold="true"  ForeColor="#858585" />
        <PagerStyle HorizontalAlign="Center" CssClass="clsButton" />
        <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
        <HeaderStyle CssClass="GridHeader" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="#ecf8ff" />
    </asp:GridView>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="새로고침" 
            OnClick="btnRefresh_Click" />
        &nbsp;<asp:Button ID="btnViewByDate" runat="server" class="btn btn-info" Text="날자별로보기" 
            OnClick="btnViewByUser_Click" />
    </asp:Panel>
    <br />
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>

