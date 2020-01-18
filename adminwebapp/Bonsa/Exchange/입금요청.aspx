<%@ Page Language="C#" MasterPageFile="../Exchange/입출금관리.master" AutoEventWireup="true" CodeFile="입금요청.aspx.cs" Inherits="입출금관리_입금요청" Title="코리아 게임 관리자페이지" %>

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
                <h5>회원입금요청</h5>
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
                <td class="srcTit" nowrap>
                    &nbsp;<i class="fa fa-circle-o" ></i>
                    &nbsp;&nbsp;업체선택
                </td>
                <td>
                    <asp:DropDownList ID="ddlBonsa" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlBonsa_SelectedIndexChanged" Width="80px">
                    </asp:DropDownList>
                    &nbsp;<asp:DropDownList ID="ddlBubonsa" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlBubonsa_SelectedIndexChanged" Width="80px">
                    </asp:DropDownList>
                    &nbsp;<asp:DropDownList ID="ddlChongPan" runat="server" AutoPostBack="True" 
                         Width="80px" onselectedindexchanged="ddlChongPan_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;<asp:DropDownList ID="ddlMaejang" runat="server" AutoPostBack="True" 
                         Width="80px" onselectedindexchanged="ddlMaejang_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <!--
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
                -->
                
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
                    <asp:LinkButton ID="lnkNo" CommandName="Select" CommandArgument='<%# Bind("ID") %>' ToolTip='<%# Bind("UserID") %>'
                        runat="server"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" Width=40px />
            </asp:TemplateField>
            <asp:BoundField DataField="LoginID" HeaderText="아이디" SortExpression="LoginID" >
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="닉네임" SortExpression="name">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkViewDetail" Text='<%# Eval("name") %>'  NavigateUrl='<%# "../User/회원이력정보.aspx?id=" + Eval("UserID").ToString() + "&ReturnUrl=" + Request.Path %>' runat="server" CausesValidation="False"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" CssClass="GridCommandButton" />
            </asp:TemplateField>
              <asp:BoundField DataField="depositHolder" HeaderText="입금자명" SortExpression="depositHolder" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="Bubonsa" HeaderText="부본사" SortExpression="Bubonsa" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="Chongpan" HeaderText="총판" SortExpression="Chongpan" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="Maejang" HeaderText="매장" SortExpression="Maejang" NullDisplayText="-">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            <asp:BoundField DataField="couponname" HeaderText="쿠폰명" SortExpression="couponname">
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
            <asp:TemplateField HeaderText="알람">
                <ItemTemplate>
                    <asp:CheckBox ID="cbxAlarm" runat="server" OnCheckedChanged="cbxAlarm_CheckedChanged" CssClass="clsEditableFieldValue"
                     style="vertical-align:middle;display:inline-block;"/>
                     <span style="vertical-align:middle; display:inline-block; padding-top:5px;">중지</span>
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
        <AlternatingRowStyle BackColor="#ecf8ff" />
    </asp:GridView>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="새로고침" 
            OnClick="btnRefresh_Click" />
        
    </asp:Panel>
    <br />
    <table class="clsTipPanel">
        <tr>
            <td width=20px></td>
            <td class="clsTip">
                - 메모는 보기버튼에 마우스를 가져가면 툴팁으로 확인할수 있습니다.
            </td>
        </tr>
        <tr>
            <td width=20px></td>
            <td class="clsTip">
                - 충전승인하면 요청금액이 회원의 캐쉬로 충전됩니다.
            </td>
        </tr>
    </table>
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>

