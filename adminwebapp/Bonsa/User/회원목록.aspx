<%@ Page Language="C#" MasterPageFile="회원관리.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="회원목록.aspx.cs" Inherits="회원관리_회원목록" Title="코리아 게임 관리자페이지" %>

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
            <td>
                 <h5>회원관리</h5>
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
    <table  class="clsLineTable">
        <tr>
            <td>
            </td>
        </tr>
    </table>
    <div class='PageToolBar'>
        <table width="100%" border="0" cellpadding="1" cellspacing="1">
            <tr valign="middle">
                <td width="10%" class="srcTit" nowrap >
                   <i class="fa fa-circle-o" ></i>
                    &nbsp;결과&nbsp;: 총 [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]명
                </td>
                <td class="srcTit" nowrap align="right" valign="middle">
                <i class="fa fa-circle-o" ></i>&nbsp;업체별:
                <asp:DropDownList ID="ddlEnterprise"  runat="server" ></asp:DropDownList>
                </td>
                <td class="srcTit" nowrap>
                   <i class="fa fa-circle-o" ></i>&nbsp;회원별 :&nbsp; 
                    <asp:DropDownList ID="ddlSearchKey"  runat="server">
                        <asp:ListItem Value="LoginID">회원ID</asp:ListItem>
                        <asp:ListItem Value="NickName">닉네임</asp:ListItem>
                        <asp:ListItem Value="loginip">아이피</asp:ListItem>                        
                    </asp:DropDownList>
                    <asp:TextBox ID="tbxSearchValue" runat="server" Class="input_day" 
                        OnTextChanged="btnSearch_Click" Width="100px" MaxLength="12"></asp:TextBox>&nbsp;
                </td>
                <td>
                <i class="fa fa-circle-o" ></i>
                    날자:
                    <asp:TextBox ID="tbxStartDate" runat="server" CssClass="clsEdit" 
                        Width="88px" onclick="Calendar(this)" ></asp:TextBox>
                    부터&nbsp;&nbsp;<asp:TextBox ID="tbxEndDate" runat="server" CssClass="clsEdit"
                        Width="88px" onclick="Calendar(this)"></asp:TextBox>
                    까지
                </td>
                 <td>
                    <asp:Button ID="btnSearch" runat="server" Text=" 검 색 "  class="btn btn-danger"  OnClick="btnSearch_Click" />
                </td>
                
                <td>
                <asp:Button ID="btnNoLoginUser" runat="server" Text="휴면회원" class="btn btn-primary" OnClick="btnNoLoginUser_Click"/>
                <asp:HiddenField ID="hidIsNoLoginUser" runat="server" Value="0" />
                </td>    
                
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
                                <i class="fa fa-sort-alpha-asc" aria-hidden="true"></i>
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
                                   <i class="fa fa-sort-alpha-desc" aria-hidden="true"></i>
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
            <asp:TemplateField HeaderText="번호" SortExpression="ID">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkNo" runat="server" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" Width="30px" Wrap=false HorizontalAlign=Center />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="아이디" SortExpression="ID">
                <ItemTemplate>
                    <asp:Label ID="lnkViewDetail1" Text='<%# Eval("loginid") %>'   runat="server" ToolTip='<%# "게임회수:" + Eval("game_count") + "    발생금액:" + string.Format("{0:N0}", Eval("service_fee") ) %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" CssClass="GridCommandButton" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="닉네임" SortExpression="NickName">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkViewDetail" Text='<%# Eval("NickName") %>'  NavigateUrl='<%# "../User/회원이력정보.aspx?id=" + Eval("ID").ToString() + "&ReturnUrl=" + Request.Path %>' runat="server" CausesValidation="False"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" CssClass="GridCommandButton" />
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="이름" SortExpression="Name" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Center"/>
            </asp:BoundField>
             <asp:BoundField DataField="telnum" HeaderText="연락처" SortExpression="telNum" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Center"/>
            </asp:BoundField>
            <asp:TemplateField HeaderText="피추천수" SortExpression="recommender_count">
                <ItemTemplate>
                    <asp:Label ID="lblRecommenderCount" runat="server" Text='<%# Eval("recommender_count") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:BoundField DataField="recommend_money" DataFormatString="{0:N0}" HeaderText="추천발생머니" 
                SortExpression="recommend_money" >
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap=false />
            </asp:BoundField>
             <asp:BoundField DataField="event_give_money" DataFormatString="{0:N0}" HeaderText="이벤트머니" 
                SortExpression="recommend_money" >
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap=false />
            </asp:BoundField>
             <asp:BoundField DataField="sum_charge" HeaderText="충전금액" SortExpression="sum_charge"  NullDisplayText="" DataFormatString="{0:N0}">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Right" />
            </asp:BoundField>
             <asp:BoundField DataField="sum_withdraw" HeaderText="출금액" SortExpression="sum_withdraw"  NullDisplayText="" DataFormatString="{0:N0}">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="gamemoney" DataFormatString="{0:N0}" HeaderText="보유머니" 
                SortExpression="gamemoney" >
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap=false />
            </asp:BoundField>
            <asp:BoundField DataField="dealmoney" DataFormatString="{0:N0}" HeaderText="포인트" 
                SortExpression="wallet_money" >
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap=false />
            </asp:BoundField>
            <asp:BoundField DataField="dealpercent" DataFormatString="{0:N1}%" HeaderText="딜러비" 
                SortExpression="dealpercent" >
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap=false />
            </asp:BoundField>
            <asp:BoundField DataField="loginip" HeaderText="아이피" SortExpression="loginip" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="EntName" HeaderText="소속업체" SortExpression="EntName" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="chongpan" HeaderText="총판" SortExpression="chongpan" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="maejang" HeaderText="매장" SortExpression="maejang" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="nologindays" HeaderText="휴면일" SortExpression="nologindays" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="등록날자" SortExpression="RegDate">
                <ItemTemplate>
                    <asp:Label ID="lblRegDate" runat="server" Text='<%# ((DateTime)Eval("RegDate")).ToString("yyyy-MM-dd") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="position" HeaderText="위치"  NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" NavigateUrl='<%# "회원상세정보.aspx?id=" + Eval("ID").ToString() %>' runat="server" CausesValidation="False"
                        Text="수정"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
           
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkCharge" NavigateUrl='<%# "javascript:openNewWindow(\"usercharge.aspx\", " + Eval("id").ToString() + ")"%>' runat="server" CausesValidation="False"
                        Text="충전"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkWithdraw" NavigateUrl='<%# "javascript:openNewWindow(\"userwithdraw.aspx\", " + Eval("id").ToString() + ")"%>' runat="server" CausesValidation="False"
                        Text="출금"></asp:HyperLink>
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
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="목록새로고침" />
        <asp:Button ID="btnExcel" runat="server" class="btn btn-info" Text="Excel출력" onclick="btnExcel_OnClick" />
        
    </asp:Panel>
</asp:Content>

