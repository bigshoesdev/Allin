<%@ Page Language="C#" MasterPageFile="../EnterpriseExchange/업체입출금관리.master" AutoEventWireup="true" CodeFile="업체입금요청.aspx.cs" Inherits="업체입출금관리_업체입금요청" Title="코리아 게임 관리자페이지" %>

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
                업체입금요청
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
                    &nbsp;&nbsp;검색된 요청수
                </td>
                <td class="cmtTit" width="15%">
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]
                </td>
                <td width="10%" class="srcTit" nowrap style="display: none">
                    &nbsp;
                    &nbsp;&nbsp;</td>
                <td style="display: none">
                    &nbsp;
                    </td>
                <td>
                    &nbsp;
                </td>
                <td>
                </td>
                <td align="right">
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
         
        onrowediting="grdLisTBL_RowEditing">
        <RowStyle CssClass="GridRow" />
        <PagerSettings Mode="NumericFirstLast" />
        <Columns>
            <asp:TemplateField HeaderText="번호">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkNo" CommandName="Select" CommandArgument='<%# Bind("ID") %>' ToolTip='<%# Bind("EnterpriseID") %>' 
                        runat="server"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width=40px />
            </asp:TemplateField>
            <asp:BoundField DataField="LoginID" HeaderText="아이디" SortExpression="LoginID" >
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:BoundField DataField="emoney" HeaderText="보유금액" SortExpression="emoney" DataFormatString="{0:N0}">
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
            <asp:BoundField DataField="regdate" HeaderText="요청시간" SortExpression="regdate" DataFormatString="{0:yyyy.M.d HH:mm}">
                <ItemStyle HorizontalAlign="Center" Width="120px" Wrap=false />
            </asp:BoundField>
            <asp:TemplateField HeaderText="비고">
                <ItemTemplate>
                    <asp:Label ID="lblMemo" runat=server Text='<%# Eval("memo").ToString() == "" ? "없음" : "보기" %>' 
                        ForeColor='<%# Eval("memo").ToString() == "" ? System.Drawing.Color.Black : System.Drawing.Color.Red %>' 
                        ToolTip='<%# Bind("memo") %>' Font-Bold='<%# Eval("memo").ToString() != "" %>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEdit" OnClientClick="return confirm('정말로 승인하시겠습니까?');" runat="server"
                        CausesValidation="False" CommandName="Edit" Text="승인"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width=40px Wrap=false />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('정말로 취소하시겠습니까?');" runat="server"
                        CausesValidation="False" CommandName="Delete" Text="취소"></asp:LinkButton>
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
        <asp:Button ID="btnAllDelete" runat="server" CssClass="clsButton" Text="전체삭제" 
            OnClick="btnAllDelete_Click" 
            OnClientClick="return confirm('실제로 모두 삭제하시겠습니까?');" />
    </asp:Panel>
    <br />
    <table class="clsTipPanel">
        <tr>
            <td width=20px></td>
            <td class="clsTip">
                - 메모는 보기버튼에 마우스를 가져가면 틀팁으로 확인할수 있습니다.
            </td>
        </tr>
        <tr>
            <td width=20px></td>
            <td class="clsTip">
                - 충전승인하면 업체의 금액이 충전됩니다.
            </td>
        </tr>
    </table>
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>

