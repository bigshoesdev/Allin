<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="맞고잭팟이력.aspx.cs" Inherits="게임로그조회_맞고잭팟이력" Title="코리아 게임 관리자페이지" %>

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
                <h5>맞고
                <asp:Label ID="lblChannel" runat="server"></asp:Label>
&nbsp;잭팟이력조회</h5>
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
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]건
                </td>
                <td width="5%" class="srcTit" nowrap="nowrap">
                    &nbsp;<i class="fa fa-circle-o" ></i>
                    &nbsp;당첨액합계
                </td>
                <td class="cmtTit" width="8%" nowrap="nowrap" align="right">
                    <b><asp:Label ID="lblAllMoney" runat="server"></asp:Label></b>원
                </td>
                <td width="15%" class="srcTit" nowrap>
                    &nbsp;&nbsp;<i class="fa fa-circle-o" ></i>&nbsp; <asp:DropDownList ID="ddlSearchKey" runat="server">
                        <asp:ListItem Value="NickName">닉네임</asp:ListItem>
                        <asp:ListItem Value="ChannelName" >채널명</asp:ListItem>
                        <asp:ListItem Value="RoomName">방이름</asp:ListItem>
                        <asp:ListItem Value="Card">족보</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="tbxSearchValue" runat="server" CssClass="clsEdit" 
                        OnTextChanged="btnSearch_Click" Width="150px"></asp:TextBox>
                </td>
                <td class="srcTit" nowrap="nowrap" width="20%">
                    &nbsp;<i class="fa fa-circle-o" ></i>&nbsp;
                    날자:
                    <asp:TextBox ID="tbxStartDate" runat="server" CssClass="clsEdit" 
                        Width="150px" onclick="Calendar(this)" ></asp:TextBox>
                    부터&nbsp;&nbsp;<asp:TextBox ID="tbxEndDate" runat="server" CssClass="clsEdit"
                        Width="150px" onclick="Calendar(this)"></asp:TextBox>
                    까지
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="검색" class="btn btn-danger" OnClick="btnSearch_Click" />
                </td>
                <!--
                <td width="50%">
                </td>
                <td align="right" nowrap="nowrap">
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
                -->
                
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
                    <asp:LinkButton ID="lnkNo" CommandName="Select" ToolTip='<%# Bind("RoomID") %>' CommandArgument='<%# Bind("ID") %>'
                        runat="server"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width=40px />
            </asp:TemplateField>
            <asp:BoundField DataField="regdate" HeaderText="날자" 
                SortExpression="regdate" DataFormatString="  {0:yyyy-MM-dd HH:mm:ss}  ">
                <ItemStyle HorizontalAlign="center" Width="150px" Wrap="true" CssClass="BrDateGridRow" />
            </asp:BoundField>
            <asp:BoundField DataField="RoomName" NullDisplayText="-" HeaderText="방이름" SortExpression="RoomName">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="NickName" NullDisplayText="-" HeaderText="당첨자" SortExpression="NickName">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="Money" NullDisplayText="-" HeaderText="당첨금액" SortExpression="Money" DataFormatString="{0:N0}원">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="Card" NullDisplayText="-" HeaderText="족보" SortExpression="Card">
                <ItemStyle HorizontalAlign="Center" />
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

