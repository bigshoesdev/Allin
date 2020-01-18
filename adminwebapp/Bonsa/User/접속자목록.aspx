<%@ Page Language="C#" MasterPageFile="../User/회원관리.master" AutoEventWireup="true"
    CodeFile="접속자목록.aspx.cs" Inherits="회원관리_접속자목록" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSysTitle" nowrap>
                <h5>접속자목록</h5>
            </td>
            <td align="right" style="font-size: 9pt; color: Red; font-weight: bold" width="100%">
                <asp:UpdateProgress ID="prg1" runat="server" AssociatedUpdatePanelID="UpdatePannel1"
                    DisplayAfter="100">
                    <ProgressTemplate>
                        갱신중입니다...</ProgressTemplate>
                </asp:UpdateProgress>
                <asp:ScriptManager ID="scriptmanager1" runat="server">
                </asp:ScriptManager>
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
    <asp:UpdatePanel ID="UpdatePannel1" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timer1" runat="server" Enabled="true" Interval="3000">
            </asp:Timer>
            <div class='PageToolBar'>
                <table width="100%" border="0" cellpadding="7" cellspacing="1">
                    <tr valign="middle">
                        <td width="10%" class="srcTit" nowrap>
                            &nbsp;<i class="fa fa-circle-o" ></i>
                             &nbsp;결과&nbsp;: 총
                            [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]명
                        </td>
                        <td class="srcTit" nowrap>
                            &nbsp;<img src="../../Images/ico_sqr02.gif" width="3" height="3" align="absmiddle">
                            &nbsp;&nbsp;<asp:DropDownList ID="ddlSearchKey" runat="server">
                                <asp:ListItem Value="LoginID">회원ID</asp:ListItem>
                                <asp:ListItem Value="NickName">닉네임</asp:ListItem>
                                <asp:ListItem Value="IPAddr">IP주소</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td >
                           검색:
                        </td>
                        <td >
                            <asp:TextBox ID="tbxSearchValue" runat="server" CssClass="clsEdit" OnTextChanged="btnSearch_Click"
                                Width="100px"></asp:TextBox>
                        </td>
                        <td class="srcTit" nowrap>
                            <asp:Button ID="btnSearch" runat="server" Text="검색" class="btn btn-danger" OnClick="btnSearch_Click" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td width="50%">
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
                            <asp:Label ID="lblTrialSortExpr" runat="server" CssClass="clsLabelE" Font-Size="10pt">&nbsp;로 정돈</asp:Label>
                        </td>
                        -->
                    </tr>
                </table>
            </div>
            <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="False" Width="100%"
                AllowPaging="True" DataKeyNames="ID" AllowSorting="True" OnPageIndexChanging="grdLisTBL_PageIndexChanging"
                OnRowDataBound="grdLisTBL_RowDataBound" OnRowDeleting="grdLisTBL_RowDeleting" OnSorting="grdLisTBL_Sorting"
                EmptyDataText="검색된 자료가 없습니다." 
                >
                <RowStyle CssClass="GridRow" />
                <Columns>
                    <asp:TemplateField HeaderText="번호" SortExpression="ID">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkNo" runat="server" CommandArgument='<%# Eval("UserID") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle CssClass="GridItem" Width="30px" Wrap="false" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="LoginID" HeaderText="아이디" SortExpression="LoginID">
                        <ItemStyle CssClass="GridItem" Wrap="false" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="닉네임" SortExpression="NickName">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkViewDetail" Text='<%# Eval("NickName") %>'  NavigateUrl='<%# "../User/회원이력정보.aspx?id=" + Eval("UserID").ToString() + "&ReturnUrl=" + Request.Path %>' runat="server" CausesValidation="False"></asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" CssClass="GridCommandButton" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="GameMoney" DataFormatString="{0:N0}" HeaderText="게임머니" SortExpression="GameMoney">
                        <ItemStyle CssClass="GridItem" HorizontalAlign="Right" Wrap="false" />
                    </asp:BoundField>
                    <asp:BoundField DataField="IPAddr" HeaderText="접속IP" SortExpression="IPAddr">
                        <ItemStyle CssClass="GridItem" HorizontalAlign="Center" Wrap="false" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ClientID" HeaderText="MAC주소" SortExpression="ClientID">
                        <ItemStyle CssClass="GridItem" HorizontalAlign="Center" Wrap="false" />
                    </asp:BoundField>
                    <asp:BoundField DataField="position" HeaderText="현재위치" SortExpression="position">
                        <ItemStyle CssClass="GridItem" HorizontalAlign="Center" Wrap="false" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="접속시간" SortExpression="StartTime">
                        <ItemTemplate>
                            <asp:Label ID="lblRegDate" runat="server" Text='<%# ((DateTime)Eval("StartTime")).ToString("HH:mm:ss") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="GridItem" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="채팅">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbxAlarm" runat="server" Text="중지" Checked='<%# Eval("StopChat").ToString() == "1" %>' OnCheckedChanged="cbxAlarm_CheckedChanged" AutoPostBack=true />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('정말로 강퇴하시겠습니까?');" runat="server"
                                CausesValidation="False" CommandName="Delete" Enabled='<%# Eval("NoLogin").ToString() == "0" %>'
                                Text='<%# Eval("NoLogin").ToString() == "0" ? "강퇴" : "강퇴처리중" %>'></asp:LinkButton>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
