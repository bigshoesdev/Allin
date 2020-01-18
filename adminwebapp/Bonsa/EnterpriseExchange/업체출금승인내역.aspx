<%@ Page Language="C#" MasterPageFile="업체입출금관리.master" AutoEventWireup="true" CodeFile="업체출금승인내역.aspx.cs" Inherits="업체입출금관리_업체출금승인내역" Title="코리아 게임 관리자페이지" %>

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
                업체출금승인내역
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
                <td width="10%" class="srcTit" nowrap>
                    &nbsp;<img src="../../Images/ico_sqr02.gif" width="3" height="3" align="absmiddle">
                    &nbsp;&nbsp;검색된 업체출금처리수
                </td>
                <td class="cmtTit" width="15%">
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]
                </td>
                <td class="srcTit" nowrap>
                    &nbsp;<img src="../../Images/ico_sqr02.gif" width="3" height="3" align="absmiddle">
                    &nbsp;&nbsp;<asp:DropDownList ID="ddlSearchKey" runat="server">
                        <asp:ListItem Value="LoginID">회원ID</asp:ListItem>
                        <asp:ListItem Value="NickName">닉네임</asp:ListItem>
                        <asp:ListItem Value="Name">예금주명</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="tbxSearchValue" runat="server" CssClass="clsEdit" 
                        OnTextChanged="btnSearch_Click" Width="100px"></asp:TextBox>
                </td>
                <td class="srcTit" nowrap>
                    &nbsp;<img src="../../Images/ico_sqr02.gif" width="3" height="3" align="absmiddle">&nbsp;
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
                <td width=50%></td>
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
                    <asp:Label ID="lblTrialSortExpr" runat="server" CssClass="clsLabelE" Height="11px">&nbsp;로 정돈</asp:Label>
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
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width=40px />
            </asp:TemplateField>
            <asp:BoundField DataField="LoginID" HeaderText="아이디" SortExpression="LoginID" >
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
           <asp:BoundField DataField="money" HeaderText="보유금액" SortExpression="money" DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="name" HeaderText="이름" SortExpression="name" >
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:BoundField DataField="bankname" HeaderText="은행명" SortExpression="bankname">
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:BoundField DataField="banknum" HeaderText="계좌정보" SortExpression="banknum">
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:BoundField DataField="mastername" HeaderText="예금주" SortExpression="mastername">
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:BoundField DataField="money" HeaderText="요청금액" SortExpression="money" DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="regdate" HeaderText="등록날자" SortExpression="regdate" DataFormatString="{0:yyyy-MM-dd}">
                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap=false />
            </asp:BoundField>
            <asp:TemplateField HeaderText="비고">
                <ItemTemplate>
                    <asp:Label ID="lblMemo" runat=server Text='<%# Eval("memo").ToString() == "" ? "없음" : "보기" %>' 
                        ForeColor='<%# Eval("memo").ToString() == "" ? System.Drawing.Color.Black : System.Drawing.Color.Red %>' 
                        ToolTip='<%# Bind("memo") %>' Font-Bold='<%# Eval("memo").ToString() != "" %>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="상태" SortExpression="state">
                <ItemTemplate>
                    <asp:Label ID="lblState" runat="server" CausesValidation="False"
                        Text='<%# Eval("state").ToString() == "1" ? "완료" : "완료" %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="NoticeGridItem" HorizontalAlign="Center" Width="100px" Wrap=false />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('정말로 삭제하시겠습니까?');" runat="server"
                        CausesValidation="False" CommandName="Delete" Text="삭제" Visible=false></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width="40px" Wrap=false />
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
        <asp:Button ID="btnRefresh" runat="server" CssClass="clsButton" Text="새로고침" 
            OnClick="btnRefresh_Click" />
        &nbsp;<asp:Button ID="btnAllDelete" runat="server" CssClass="clsButton" Text="전체삭제" 
            OnClick="btnAllDelete_Click" 
            OnClientClick="return confirm('실제로 모두 삭제하시겠습니까?');" />
    </asp:Panel>
    <br />
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>

