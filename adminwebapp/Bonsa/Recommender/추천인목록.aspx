<%@ Page Language="C#" MasterPageFile="../User/회원관리.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="추천인목록.aspx.cs" Inherits="회원관리_회원목록" Title="코리아 게임 관리자페이지" %>

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
                추천인관리
            </td>
        </tr>
    </table>
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
                <td width="100px" class="srcTit" nowrap>
                    &nbsp;<img src="../../Images/ico_sqr02.gif" width="3" height="3" align="absmiddle">
                    &nbsp;&nbsp;검색된 추천인수
                </td>
                <td class="cmtTit" width="100px" nowrap>
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]
                </td>
                <td width="100"   class="srcTit" nowrap align="right" valign="middle">
                <img src="../../Images/ico_sqr02.gif" width="3" height="3" style="margin-top:-3px;">&nbsp;업체선택:
                </td>
                <td align="left" >
                <asp:DropDownList ID="ddlEnterprise" runat="server"></asp:DropDownList>
                </td>
                <td class="srcTit" nowrap>
                    &nbsp;<img src="../../Images/ico_sqr02.gif" width="3" height="3" align="absmiddle">
                    &nbsp;&nbsp;<asp:DropDownList ID="ddlSearchKey" runat="server">
                        <asp:ListItem Value="LoginID">추천인ID</asp:ListItem>
                        <asp:ListItem Value="NickName">닉네임</asp:ListItem>
                        <asp:ListItem Value="loginip">아이피</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="tbxSearchValue" runat="server" CssClass="clsEdit" 
                        OnTextChanged="btnSearch_Click" Width="100px"></asp:TextBox>
                </td>
                <td class="srcTit" nowrap>
                    &nbsp;<img src="../../Images/ico_sqr02.gif" width="3" height="3" align="absmiddle">
                    날자:
                    <asp:TextBox ID="tbxStartDate" runat="server" CssClass="clsEdit" 
                        Width="80px" onclick="Calendar(this)" ></asp:TextBox>
                    부터&nbsp;&nbsp;<asp:TextBox ID="tbxEndDate" runat="server" CssClass="clsEdit"
                        Width="80px" onclick="Calendar(this)"></asp:TextBox>
                    까지
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="검색" CssClass="clsButton" OnClick="btnSearch_Click" />
                </td>
                
               
                
                <td width=20%><asp:Label ID="lblQuery" Text="" runat="server"></asp:Label></td>
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
            <asp:TemplateField HeaderText="추천수" SortExpression="recommender_count">
                <ItemTemplate>
                    <asp:Label ID="lblRecommenderCount" runat="server" Text='<%# Eval("recommender_count") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:BoundField DataField="gamemoney" DataFormatString="{0:N0}" HeaderText="보유머니" 
                SortExpression="gamemoney" >
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap=false />
            </asp:BoundField>
            <asp:BoundField DataField="loginip" HeaderText="아이피" SortExpression="loginip" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="EntName" HeaderText="부본사사" SortExpression="EntName" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="chongpan" HeaderText="총판" SortExpression="chongpan" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="maejang" HeaderText="매장" SortExpression="maejang" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:TemplateField HeaderText="등록날자" SortExpression="RegDate">
                <ItemTemplate>
                    <asp:Label ID="lblRegDate" runat="server" Text='<%# ((DateTime)Eval("RegDate")).ToString("yyyy-MM-dd") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="계정상태" SortExpression="nologin">
                <ItemTemplate>
                    <asp:Label ID="lblActivate" runat="server" Text='<%# Eval("nologin").ToString() == "0" ? "허용" : "금지" %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" Wrap=false />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="접속상태" SortExpression="status">
                <ItemTemplate>
                    <asp:Label ID="lblActivate1" runat="server" Text='<%# Eval("status").ToString() == "0" ? "" : "접속중" %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" Wrap=false />
            </asp:TemplateField>
            <asp:BoundField DataField="position" HeaderText="위치"  NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
             <asp:TemplateField HeaderText="입출금내역" >
                <ItemTemplate>
                    <asp:HyperLink ID="lnkChargeDetail" NavigateUrl='<%# "../Game/회원별정산.aspx?loginid=" + Eval("loginid") %>' runat="server">상세보기</asp:HyperLink>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" Wrap=false />
            </asp:TemplateField>
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
        <AlternatingRowStyle BackColor="#EAEAEA" />
    </asp:GridView>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:Button ID="btnNew" runat="server" CssClass="clsButton" Text="새로등록" onclick="btnNew_Click" />
        <asp:Button ID="btnRefresh" runat="server" CssClass="clsButton" Text="목록새로고침" />
    </asp:Panel>
</asp:Content>

