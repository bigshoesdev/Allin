<%@ Page Language="C#" MasterPageFile="../User/회원관리.master" AutoEventWireup="true" CodeFile="상황판.aspx.cs" Inherits="게임관리_상황판" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" Runat="Server">
    <table cellpadding="0" cellspacing="0" width=100%>
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSysTitle" nowrap>
                 <h5>종합상황판</h5>
            </td>
            <td align=right style="font-size:9pt;color:Red;font-weight:bold" width=100%>
                
            </td>
        </tr>
    </table>
    <!-- 타이틀밑의 두선 -->
    <table cellpadding="0" border="1" bordercolor="#E7E3E7" cellspacing="0" class="clsLineTable">
        <tr>
            <td bgcolor="#ecf8ff">
            </td>
        </tr>
    </table>
    <br />
    
    
    <form>
    <table width="100%">
    <tr>
    
     <td width="50%" valign="top" align="left">
        <asp:GridView ID="grdChargeInfo" runat="server" Caption="당일 입금현황 (Top5)"
                    AutoGenerateColumns="False" Width="95%"
                    EmptyDataText="검색된 자료가 없습니다."  >               
            <RowStyle CssClass="GridRow" />
           <Columns>
               <asp:BoundField DataField="name" HeaderText="업체명">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="all_count" HeaderText="건수">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="all_sum" HeaderText="금액"  DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
                 <asp:BoundField DataField="f_count" HeaderText="처리완료">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="f_sum" HeaderText="완료금액"  DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
                <asp:BoundField DataField="n_count" HeaderText="미처리 건수">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                 <asp:BoundField DataField="n_sum" HeaderText="미처리 금액"  DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
            </Columns>
            
            <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" 
                CssClass="PagerButton" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle CssClass="GridHeader" />
            <AlternatingRowStyle BackColor="#ecf8ff" />
        </asp:GridView>
        <div style="text-align:right; padding:10px; margin-right:50px">
        <asp:HyperLink ID="lnkChargeDetail" runat="server" NavigateUrl="../Exchange/입금승인내역.aspx">더보기&nbsp;<i class="fa fa-plus"></i></asp:HyperLink>
         </div>
        </td>
       <td align="left" valign="top">
        <asp:GridView ID="grdWithdrawInfo" runat="server" Caption="당일 출금현황 (Top5)"
                    AutoGenerateColumns="False" 
                    Width="95%"
                    EmptyDataText="검색된 자료가 없습니다."  >               
            <RowStyle CssClass="GridRow" />
            <Columns>
               <asp:BoundField DataField="name" HeaderText="업체명">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="all_count" HeaderText="건수">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="all_sum" HeaderText="금액"  DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
                 <asp:BoundField DataField="f_count" HeaderText="처리완료">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="f_sum" HeaderText="완료금액"  DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
                <asp:BoundField DataField="n_count" HeaderText="미처리 건수">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                 <asp:BoundField DataField="n_sum" HeaderText="미처리 금액"  DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
            </Columns>
            
            <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" 
                CssClass="PagerButton" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle CssClass="GridHeader" />
            <AlternatingRowStyle BackColor="#ecf8ff" />
        </asp:GridView>
        <div style="text-align:right; padding:10px; margin-right:50px">
        <asp:HyperLink ID="lnkWithdrawDetail" runat="server" NavigateUrl="../Exchange/출금승인내역.aspx" >더보기&nbsp;<i class="fa fa-plus"></i></asp:HyperLink>
          </div>
        </td>
        </tr>
        <tr>  
        <td  width="50%" valign="top" align="left">
        <asp:GridView ID="grdBussines" runat="server" Caption="당일수익현황 (Top5)"
                    AutoGenerateColumns="False" ShowFooter="True"
                    Width="95%" EmptyDataText="검색된 자료가 없습니다." >               
            <RowStyle CssClass="GridRow" />
            <Columns>
               <asp:BoundField DataField="name" HeaderText="업체명">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="login_count" HeaderText="접속 유저수" NullDisplayText="0">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="user_count" HeaderText="게임 참여수" NullDisplayText="0">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="sum_servicefee" HeaderText="발생금액" NullDisplayText="0" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
                <asp:BoundField DataField="classpercent" HeaderText="수수료율" DataFormatString="{0:N1}%">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
                 <asp:BoundField DataField="real_classfee" HeaderText="수익금" NullDisplayText="0" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
            </Columns>
           
            <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" HorizontalAlign="center"/>
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" 
                CssClass="PagerButton" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle CssClass="GridHeader" />
            <AlternatingRowStyle BackColor="#ecf8ff" />
        </asp:GridView>
        <div style="text-align:right; padding:10px; margin-right:50px">
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="../EnterpriseExchange/로링식업체별정산.aspx">더보기&nbsp;<i class="fa fa-plus"></i></asp:HyperLink>
        </div>
        </td>
        <td width="50%" align="left" valign="top">
  <%--      <asp:GridView ID="grdGameStatusView" runat="server" Caption="게임현황"
                    AutoGenerateColumns="False" 
                    Width="95%"
                    EmptyDataText="검색된 자료가 없습니다."  >               
            <RowStyle CssClass="GridRow" />
            <Columns>
               <asp:BoundField DataField="status" HeaderText="상태">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:TemplateField HeaderText="포커" >
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkViewDetail1" Text='<%# Eval("poker_count") %>'  NavigateUrl='<%# "../PBM/포커방목록.aspx" %>' runat="server" CausesValidation="False"></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="바둑이" >
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkViewDetail2" Text='<%# Eval("baduki_count") %>'  NavigateUrl='<%# "../PBM/바둑이방목록.aspx" %>' runat="server" CausesValidation="False"></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="맞고" >
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkViewDetail3" Text='<%# Eval("matgo_count") %>'  NavigateUrl='<%# "../PBM/맞고방목록.aspx" %>' runat="server" CausesValidation="False"></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
                </asp:TemplateField>
                <asp:BoundField DataField="sum_count" HeaderText="합계">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
            </Columns>
            
            <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" 
                CssClass="PagerButton" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle CssClass="GridHeader" />
            <AlternatingRowStyle BackColor="#ecf8ff" />
        </asp:GridView>--%>
        
        </td>
        
       </tr>
        <tr>  
        <td  width="50%" valign="top" align="left">
        <asp:GridView ID="grdBetView" runat="server" Caption="당일베팅현황 (Top5)"
                    AutoGenerateColumns="False" ShowFooter="True"
                    Width="95%" EmptyDataText="검색된 자료가 없습니다." >               
            <RowStyle CssClass="GridRow" />
            <Columns>
                <asp:BoundField DataField="nickname" HeaderText="유저명">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="bonsa" HeaderText="본사">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="bubonsa" HeaderText="부본사">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="chongpan" HeaderText="총판">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="maejang" HeaderText="매장">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="betmoney" HeaderText="베팅액" NullDisplayText="0" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" HorizontalAlign="center"/>
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" 
                CssClass="PagerButton" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle CssClass="GridHeader" />
            <AlternatingRowStyle BackColor="#ecf8ff" />
        </asp:GridView>
        <div style="text-align:right; padding:10px; margin-right:50px">
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="../Game/베팅내역.aspx">더보기&nbsp;<i class="fa fa-plus"></i></asp:HyperLink>
        </div>
        </td>
        <td width="50%" align="left" valign="top">
        <asp:GridView ID="grdDealView" runat="server" Caption="당일딜비현황 (Top5)"
                    AutoGenerateColumns="False" ShowFooter="True"
                    Width="95%" EmptyDataText="검색된 자료가 없습니다." >               
            <RowStyle CssClass="GridRow" />
            <Columns>
                <asp:BoundField DataField="nickname" HeaderText="유저명">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="bonsa" HeaderText="본사">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="bubonsa" HeaderText="부본사">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="chongpan" HeaderText="총판">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="maejang" HeaderText="매장">
                    <ItemStyle HorizontalAlign="center"/>
                </asp:BoundField>
                <asp:BoundField DataField="dealmoney" HeaderText="딜러비" NullDisplayText="0" DataFormatString="{0:N0}">
                    <ItemStyle HorizontalAlign="right" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" HorizontalAlign="center"/>
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" 
                CssClass="PagerButton" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle CssClass="GridHeader" />
            <AlternatingRowStyle BackColor="#ecf8ff" />
        </asp:GridView>
        <div style="text-align:right; padding:10px; margin-right:50px">
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="../Game/딜비내역.aspx">더보기&nbsp;<i class="fa fa-plus"></i></asp:HyperLink>
        </div>
        </td>
        
       </tr>
       <tr>
       
       
    </tr>
    <tr>
        <td colspan="2" style="padding-top:50px;">
        
        </td>
    </tr>
    </table>
    <asp:TextBox ID="tbxQuery" runat="server" TextMode="MultiLine" Visible="false"></asp:TextBox>
    </form>
    <p style="width:100%">&nbsp;</p>
</asp:Content>