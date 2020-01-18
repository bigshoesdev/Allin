<%@ Page Language="C#" MasterPageFile="회원관리.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="회원뷰어관리.aspx.cs" Inherits="회원관리_회원뷰어관리" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap></td>
            <td>
                <h5>회원뷰어관리</h5>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        $(function () {
            $('select').selectlist({
                zIndex: 10,
                width: 120,
                height: 27
            });
        })
    </script>
    <!-- 타이틀밑의 두선 -->
    <table class="clsLineTable">
        <tr>
            <td></td>
        </tr>
    </table>
    <div class='PageToolBar'>
        <table width="100%" border="0" cellpadding="1" cellspacing="1">
            <tr valign="middle">
                <td width="10%" class="srcTit" nowrap>
                    <i class="fa fa-circle-o"></i>
                    &nbsp;결과&nbsp;: 총 [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]명
                </td>
                <td class="srcTit" nowrap align="right" valign="middle">
                    <i class="fa fa-circle-o"></i>&nbsp;업체별:
                <asp:DropDownList ID="ddlEnterprise" runat="server"></asp:DropDownList>
                </td>
                <td class="srcTit" nowrap>
                    <i class="fa fa-circle-o"></i>&nbsp;회원별 :&nbsp; 
                    <asp:DropDownList ID="ddlSearchKey" runat="server">
                        <asp:ListItem Value="LoginID">회원ID</asp:ListItem>
                        <asp:ListItem Value="NickName">닉네임</asp:ListItem>
                        <asp:ListItem Value="loginip">아이피</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="tbxSearchValue" runat="server" Class="input_day"
                        OnTextChanged="btnSearch_Click" Width="100px" MaxLength="12"></asp:TextBox>&nbsp;
                </td>
                <td>
                    <i class="fa fa-circle-o"></i>
                    날자:
                    <asp:TextBox ID="tbxStartDate" runat="server" CssClass="clsEdit"
                        Width="88px" onclick="Calendar(this)"></asp:TextBox>
                    부터&nbsp;&nbsp;<asp:TextBox ID="tbxEndDate" runat="server" CssClass="clsEdit"
                        Width="88px" onclick="Calendar(this)"></asp:TextBox>
                    까지
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text=" 검 색 " class="btn btn-danger" OnClick="btnSearch_Click" />
                </td>

                <td>
                    <asp:Button ID="btnNoLoginUser" runat="server" Text="휴면회원" class="btn btn-primary" OnClick="btnNoLoginUser_Click" />
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
        Width="100%" AllowPaging="True" DataKeyNames="ID" AllowSorting="True"
        OnPageIndexChanging="grdLisTBL_PageIndexChanging" PageSize="20"
        OnRowDataBound="grdLisTBL_RowDataBound" OnRowDeleting="grdLisTBL_RowDeleting"
        OnSorting="grdLisTBL_Sorting" EmptyDataText="검색된 자료가 없습니다.">
        <RowStyle CssClass="GridRow" />
        <Columns>
            <asp:TemplateField HeaderText="번호" SortExpression="ID">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkNo" runat="server" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" Width="30px" Wrap="false" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="아이디" SortExpression="ID">
                <ItemTemplate>
                    <asp:Label ID="lnkViewDetail1" Text='<%# Eval("loginid") %>' runat="server" ToolTip='<%# "게임회수:" + Eval("game_count") + "    발생금액:" + string.Format("{0:N0}", Eval("service_fee") ) %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" CssClass="GridCommandButton" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="닉네임" SortExpression="NickName">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkViewDetail" Text='<%# Eval("NickName") %>' NavigateUrl='<%# "../User/회원이력정보.aspx?id=" + Eval("ID").ToString() + "&ReturnUrl=" + Request.Path %>' runat="server" CausesValidation="False"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" CssClass="GridCommandButton" />
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="이름" SortExpression="Name">
                <ItemStyle CssClass="GridItem" Wrap="false" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="telnum" HeaderText="연락처" SortExpression="telNum">
                <ItemStyle CssClass="GridItem" Wrap="false" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="gamemoney" DataFormatString="{0:N0}" HeaderText="보유머니"
                SortExpression="gamemoney">
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap="false" />
            </asp:BoundField>
            <asp:BoundField DataField="dealmoney" DataFormatString="{0:N0}" HeaderText="포인트"
                SortExpression="wallet_money">
                <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap="false" />
            </asp:BoundField>
            <asp:BoundField DataField="EntName" HeaderText="소속업체" SortExpression="EntName" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap="false" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="chongpan" HeaderText="총판" SortExpression="chongpan" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap="false" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="maejang" HeaderText="매장" SortExpression="maejang" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap="false" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="등록날자" SortExpression="RegDate">
                <ItemTemplate>
                    <asp:Label ID="lblRegDate" runat="server" Text='<%# ((DateTime)Eval("RegDate")).ToString("yyyy-MM-dd") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="position" HeaderText="위치" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap="false" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="뷰어상태" SortExpression="RegDate">
                <ItemTemplate>
                    <asp:Label ID="lblViewerState1" runat="server" Visible='<%# (Eval("isviewer")).ToString() == "1" %>' Text="패조종뷰어"></asp:Label>
                    <asp:Label ID="lblViewerState2" runat="server" Visible='<%# (Eval("isviewer")).ToString() == "2" %>' Text="뷰어"></asp:Label>
                    <asp:Label ID="lblViewerState3" runat="server" Visible='<%# (Eval("isviewer")).ToString() == "0" %>' Text="일반"></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkCardViewEnable" OnClientClick="return confirm('정말로 패조정뷰어로 설정하시겠습니까?');" runat="server"
                        CausesValidation="False" CommandName="Delete" Visible='<%# Eval("isviewer").ToString() == "0" %>' OnClick="btnCardViewEnable_click" 
                        Text='패조정뷰어'></asp:LinkButton>
                    <br />
                    <asp:LinkButton ID="lnkViewEnable" OnClientClick="return confirm('정말로 뷰어로 설정하시겠습니까?');" runat="server"
                        CausesValidation="False" CommandName="Delete" Visible='<%# Eval("isviewer").ToString() == "0" %>' OnClick="btnViewEnable_click" 
                        Text='뷰어설정'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkViewDisable" OnClientClick="return confirm('정말로 뷰어를 해제하시겠습니까?');" runat="server"
                        CausesValidation="False" CommandName="Delete" Visible='<%# Eval("isviewer").ToString() == "1" || Eval("isviewer").ToString() == "2" %>'
                        OnClick="btnViewDisable_click" 
                        Text='뷰어해제'></asp:LinkButton>
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
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="목록새로고침" />

    </asp:Panel>
</asp:Content>

