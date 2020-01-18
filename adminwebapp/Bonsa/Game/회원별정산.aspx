<%@ Page Language="C#" MasterPageFile="게임관리.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="회원별정산.aspx.cs" Inherits="게임관리_회원별정산" Title="코리아 게임 관리자페이지" %>

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
                <h5>회원별입출금현황</h5>
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
                    <i class="fa fa-circle-o" ></i> 날자:&nbsp;
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
                  <td align=right>
                    <asp:Button ID="btnExcel" runat="server" Text="Excel출력" CssClass="clsButton" OnClick="btnExcel_OnClick" />
                </td>
            </tr>
            <tr>

                <td colspan=3>
                                     <i class="fa fa-circle-o" ></i>
                    업체선택
                    <asp:DropDownList ID="ddlBonsa" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlBonsa_SelectedIndexChanged" Width="80px">
                    </asp:DropDownList>
                    &nbsp;<asp:DropDownList ID="ddlBubonsa" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlBubonsa_SelectedIndexChanged" Width="80px">
                    </asp:DropDownList>
                    &nbsp;<asp:DropDownList ID="ddlChongPan" runat="server" AutoPostBack="True" Visible="false"
                         Width="80px" onselectedindexchanged="ddlChongPan_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;<asp:DropDownList ID="ddlMaejang" runat="server" AutoPostBack="True" Visible="false"
                         Width="80px" onselectedindexchanged="ddlMaejang_SelectedIndexChanged">
                    </asp:DropDownList>
                    
                 회원아이디:
                <asp:TextBox ID="tbxUserId" runat="server" Text="" MaxLength="12"></asp:TextBox>
                </td>
                <td align="left">
               
                </td>
                <td></td>
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
            <asp:BoundField DataField="LoginID" HeaderText="아이디" SortExpression="LoginID">
                <ItemStyle HorizontalAlign="center" />
                <FooterStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:BoundField DataField="EntName" HeaderText="부본사사" SortExpression="EntName">
                <ItemStyle HorizontalAlign="center" />
                <FooterStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:BoundField DataField="poker_bet" HeaderText="포커베팅금액" SortExpression="poker_bet" DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="baduki_bet" HeaderText="바두기베팅금액" SortExpression="baduki_bet" DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="matgo_bet" HeaderText="맞고베팅금액" SortExpression="matgo_bet" DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
            </asp:BoundField>
            
            <asp:BoundField DataField="gamemoney" HeaderText="보유금액" 
                DataFormatString="{0:N0}" SortExpression="money" >
                <ItemStyle HorizontalAlign="right" />
                <FooterStyle HorizontalAlign=right />
            </asp:BoundField>
            <asp:BoundField DataField="ChargeMoney" HeaderText="총입금액" 
                DataFormatString="{0:N0}" NullDisplayText="0원" SortExpression="ChargeMoney">
                <ItemStyle HorizontalAlign="right" />
                <FooterStyle HorizontalAlign=right />
            </asp:BoundField>
            <asp:BoundField DataField="ChargeCount" HeaderText="입금회수" 
                DataFormatString="{0:N0}회" NullDisplayText="0회" SortExpression="ChargeCount">
                <ItemStyle HorizontalAlign="center" />
                <FooterStyle HorizontalAlign=center />
            </asp:BoundField>
            <asp:BoundField DataField="WithdrawMoney" HeaderText="총출금액" 
                DataFormatString="{0:N0}" NullDisplayText="0원" SortExpression="WithdrawMoney">
                <ItemStyle HorizontalAlign="right" />
                <FooterStyle HorizontalAlign=right />
            </asp:BoundField>
            <asp:BoundField DataField="WithdrawCount" HeaderText="출금회수" 
                DataFormatString="{0:N0}회" NullDisplayText="0회" SortExpression="WithdrawCount">
                <ItemStyle HorizontalAlign="center" />
                <FooterStyle HorizontalAlign=center />
            </asp:BoundField>
            <asp:BoundField DataField="class_percent" HeaderText="업체지분율" 
                 SortExpression="class_percent">
                <ItemStyle HorizontalAlign="right" />
                <FooterStyle HorizontalAlign=right />
            </asp:BoundField>
            <asp:TemplateField HeaderText="순수익">
                <ItemTemplate>
                    <asp:Label ID="lblBenefitMoney" runat="server" 
                        Text='<%# ( ((Eval("ChargeMoney") != DBNull.Value ? (long)Eval("ChargeMoney") : 0) - (Eval("WithdrawMoney") != DBNull.Value ? (long)Eval("WithdrawMoney") : 0) - (Eval("GameMoney") != DBNull.Value ? (long)Eval("GameMoney") : 0))  * Convert.ToInt16(Eval("class_percent"))/100 ).ToString("N0") %>'></asp:Label>
                </ItemTemplate>
                <FooterStyle HorizontalAlign="Right" />
                <ItemStyle HorizontalAlign="Right" />
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

