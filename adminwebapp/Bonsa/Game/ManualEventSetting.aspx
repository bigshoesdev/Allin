<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="ManualEventSetting.aspx.cs" Inherits="Bonsa_Game_ManualEventSetting" %>

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
                <h5>이벤트 적용</h5>
                
            </td>
        </tr>
    </table>
        <!-- 타이틀밑의 두선 -->
    <table cellpadding="0" border="1" bordercolor="#E7E3E7" cellspacing="0" class="clsLineTable">
        <tr>
            <td bgcolor="#265a88">
            </td>
        </tr>
    </table> 
    <table class="clsFieldValue">
    <tr>
        <td>이벤트명:</td>
        <td><asp:Label ID="lblEventName" Text="" runat="server"></asp:Label>
            <asp:HiddenField ID="hidID" Value="" runat="server" />
            <asp:HiddenField ID="hidGameKind" Value="POKER" runat="server" />
        </td>
    </tr>
    </table>
    
    <table class="clsFieldValue" width="100%">
    <tr>
        <td class="clsFieldValue">
            <asp:RadioButton runat="server" GroupName="chkType" ID="chkAll" runat="server" Checked="true"/></td><td>전체적용
        </td>
        <td class="clsFieldValue">
            <asp:RadioButton ID="chkPoker" GroupName="chkType" runat="server" /></td><td>포커방 전체
        </td>
        <td class="clsFieldValue">
           <asp:RadioButton ID="chkBaduki" GroupName="chkType" runat="server" /></td><td>바둑이방 전체
        </td>
        <td class="clsFieldValue">
           <asp:RadioButton ID="chkMatgo" GroupName="chkType" runat="server" /></td><td>맞고방 전체
        </td>

        <td align="center">
        <asp:Button ID="btnSave" runat="server" class="btn btn-info" OnClientClick="return confirm('등록하시겠습니까?');" Text=" 이벤트 적용 " OnClick="btnSave_Click" ValidationGroup="userreg"/>
        <asp:Button ID="btnList" runat="server" class="btn btn-warning" onclick="btnLisTBL_Click" Text="목록보기" ToolTip="" />
        <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    
    </table>
    
    <table width="100%">
    <tr><td style="height:50px"></td></tr>
    <tr><td><i class="fa fa-circle-o" ></i>&nbsp;개별방 이벤트 적용</td></tr>
    <tr>
        <td align="center" style="width:33.3%">
        <asp:LinkButton ID="lnkPoker" runat="server" Text="포커" OnClick="lnkPoker_Click" class="btn btn-large btn-block btn-primary"></asp:LinkButton>
        </td>
        <td align="center"style="width:33.3%">
        <asp:LinkButton ID="lnkBaduki" runat="server" Text="바둑이" OnClick="lnkBaduki_Click" class="btn btn-large btn-block btn-primary"></asp:LinkButton>
        </td>
        <td align="center"style="width:33.3%">
        <asp:LinkButton ID="lnkMatgo" runat="server" Text="맞고" OnClick="lnkMatgo_Click" class="btn btn-large btn-block btn-primary"></asp:LinkButton>
        </td>
    </tr>
    <tr>
    <td colspan="3">
    <div class='PageToolBar'>
        <table width="100%" border="0" cellpadding="7" cellspacing="1">
            <tr valign="middle">
                <td width="10%" class="srcTit" nowrap >
                   <i class="fa fa-circle-o" ></i>
                    &nbsp;결과&nbsp;: 총
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]방
                </td>
                 <!--
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
                    <asp:Label ID="lblTrialSortExpr" runat="server" CssClass="clsLabelE" Font-Size=10pt>&nbsp;로 정돈</asp:Label>
                </td>
                -->
                
            </tr>
        </table>
    </div>
    </td>
    </tr>
    <tr>
        <td colspan="3">
        <asp:GridView ID="grdList" runat="server" AllowSorting="true" width="100%"
            AutoGenerateColumns="false" Caption="포커방정보" OnSorting="grdList_Sorting"
            AllowPaging="false">
        <RowStyle CssClass="GridRow" />
        <Columns>
            <asp:TemplateField HeaderText="번호" SortExpression="ID">
                <ItemTemplate>
                    <asp:Label ID="lnkNo" runat="server" Text='<%# Eval("RowNum") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" Width="30px" Wrap="false" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="roomname" HeaderText="방이름" SortExpression="roomname" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Center"/>
            </asp:BoundField>
            <asp:BoundField DataField="user_count" HeaderText="유저수" SortExpression="user_count" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Right"/>
            </asp:BoundField>
            <asp:BoundField DataField="bingmoney" HeaderText="삥머니" SortExpression="bingmoney" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Right"/>
            </asp:BoundField>
            <asp:BoundField DataField="enterminmoney" HeaderText="입장머니" SortExpression="enterminmoney" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="Right"/>
            </asp:BoundField>
            <asp:BoundField DataField="regdate" HeaderText="등록시간" SortExpression="regdate" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign="center"/>
            </asp:BoundField>
            <asp:TemplateField HeaderText="선택" >
                <ItemTemplate>
                    <asp:CheckBox ID="chkRoom" runat="server" ToolTip='<%# Eval("id").ToString() %>'/>
                </ItemTemplate>
                <ItemStyle  HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns> 
        <FooterStyle BackColor="White" Font-Bold="True" ForeColor="#858585" />
        <PagerStyle HorizontalAlign="Center" CssClass="clsButton" />
        <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
        <HeaderStyle CssClass="GridHeader" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="#ecf8ff" /> 
        </asp:GridView>
        </td>
        </td> 
        
        
        
    </tr>
    
    </table>
</asp:Content>