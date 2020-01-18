<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="ManualEventList.aspx.cs" Inherits="Bonsa_Game_ManualEventList" %>

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
                <h5>이벤트 정보</h5>
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
                <td width="10%" class="srcTit" nowrap >
                   <i class="fa fa-circle-o" ></i>
                    &nbsp;결과&nbsp;: 총 
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]건
                </td>
                <td width=50%></td>
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
                    <asp:Label ID="lblTrialSortExpr" runat="server" CssClass="clsLabelE" Font-Size=10pt>&nbsp;로 정돈</asp:Label>
                </td>
                -->
                
            </tr>
        </table>
    </div>
    <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="False" 
        Width="100%" AllowPaging="True" DataKeyNames="ID" AllowSorting="True" 
        onpageindexchanging="grdLisTBL_PageIndexChanging" PageSize="20"
        onrowdatabound="grdLisTBL_RowDataBound"
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
            <asp:BoundField DataField="event_name" HeaderText="이벤트명" SortExpression="event_name" >
                <ItemStyle CssClass="GridItem" Wrap=false />
            </asp:BoundField>
            <asp:TemplateField HeaderText="이벤트 유형">
                <ItemTemplate>
                    <asp:Label ID="lnkEventType" runat="server"  CausesValidation="False"  Text='<%# (Eval("event_type").ToString() == "1" ? "고래" : (Eval("event_type").ToString() == "2" ? "상어" : (Eval("event_type").ToString() == "3" ? "거북" : "해파리"))) %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
            <asp:BoundField DataField="race_count" HeaderText="회차" SortExpression="race_count" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Right"/>
            </asp:BoundField>
            <asp:BoundField DataField="give_money" HeaderText="증정 금액" SortExpression="give_money" DataFormatString="{0:N0}" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Right"  />
            </asp:BoundField>
            <asp:TemplateField HeaderText="사용여부">
                <ItemTemplate>
                    <asp:Label ID="lnkUseYn" runat="server"  CausesValidation="False"  Text='<%# (Eval("use_yn").ToString() == "1" ? "진행중" : "") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
           <asp:TemplateField HeaderText="중지">
               <ItemTemplate>
                   <asp:LinkButton ID="lbOperate" OnClick="lbOperate_Click" CommandArgument='<%# Eval("id").ToString() %>' OnClientClick="return confirm('이벤트 정지시키겠습니까?');" runat="server" Text='<%# (Eval("use_yn").ToString() == "1" ? "중지" : "") %>'></asp:LinkButton>
               </ItemTemplate>
               <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
           </asp:TemplateField>
            
           <asp:TemplateField HeaderText="적용">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit2" NavigateUrl='<%# "ManualEventSetting.aspx?id=" + Eval("id").ToString() + "&event_name=" + Eval("event_name").ToString() %>' runat="server" CausesValidation="False"
                        Text='<%# (Eval("use_yn").ToString() == "1" ? "" : "적용") %>'></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
           </asp:TemplateField>
            
           <asp:TemplateField HeaderText="등록시간">
                <ItemTemplate>
                    <asp:Label ID="lblRegtime" runat="server"  CausesValidation="False"  Text='<%# Eval("reg_time").ToString() %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
           
           
            
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:LinkButton ID="lbDelete"  runat="server" CausesValidation="False" 
                        OnClientClick="return confirm('삭제 하시겠습니까?');" CommandArgument='<%# Eval("id").ToString() %>'
                        OnClick="lbDelete_Click"
                        Text='삭제'></asp:LinkButton>
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
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" 
            Text="목록새로고침" onclick="btnRefresh_Click" />
    </asp:Panel>
</asp:Content>