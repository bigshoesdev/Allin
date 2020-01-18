<%@ Page Language="C#" MasterPageFile="../User/회원관리.master" AutoEventWireup="true" CodeFile="회원상세정보.aspx.cs" Inherits="회원관리_회원상세정보" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSysTitle">
                <h5>회원관리</h5>
                <!--<asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>-->
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
    <br />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_syst001.gif" />
            </td>
            <td width="7px" nowrap>
            </td>
            <td class="clsSubTitle">
                <asp:Label ID="lblTitleNickName" runat="server"></asp:Label> 님의 회원상세정보
            </td>
        </tr>
    </table>
    <table border=1 bordercolor=#E7E3E7 cellpadding=0 cellspacing=0  width="100%">
        <tr>
            <td class="clsFieldName" width=150px>등록번호</td>
            <td class="clsFieldValue" width=350px>
                <asp:Label ID="lblID" runat="server"></asp:Label>
            </td>
            <td class="clsFieldName" width=150px>등록날자</td>
            <td class="clsFieldValue" width=350px>
                <asp:Label ID="lblRegDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">아이디</td>
            <td class="clsFieldValue">
                <asp:Label ID="lblLoginID" runat="server"></asp:Label>
            </td>
            <td class="clsFieldName">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="tbxPWD" Display="Dynamic" ErrorMessage="비밀번호를 입력하세요." 
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                비밀번호</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxPWD" runat="server" CssClass="clsEdit" MaxLength=50 style="ime-mode:disabled"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="tbxNickName" Display="Dynamic" ErrorMessage="닉네임을 입력하세요." 
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                닉네임</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxNickName" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
            <td class="clsFieldName">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="tbxName" Display="Dynamic" ErrorMessage="이름을 입력하세요." 
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
                이름</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxName" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                게임머니</td>
            <td class="clsFieldValue">
                <asp:Label ID="lblGameMoney" runat="server"></asp:Label>
                <asp:HyperLink ID="lnkCharge"  runat="server" CausesValidation="False" Text="[충전]"></asp:HyperLink>
                <asp:HyperLink ID="lnkWithdraw" runat="server" CausesValidation="False" Text="[출금]"></asp:HyperLink>
            </td>
            <td class="clsFieldName">
                금고머니</td>
            <td class="clsFieldValue">
                <asp:Label ID="lblWalletMoney" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">
                총 딜머니
            </td>
            <td class="clsFieldValue">
                <asp:Label ID="lblDealMoney" runat="server" Text="0"></asp:Label>
            </td>
            <td class="clsFieldName">
                <asp:RangeValidator ID="rvClassPercent" runat="server" 
                    ControlToValidate="tbxDealPercent" Display="Dynamic" 
                    ErrorMessage="딜러비는 0부터 100사이의 값이어야 합니다." MaximumValue="100" MinimumValue="0" 
                    SetFocusOnError="True" Type="Double" ValidationGroup="userreg">*</asp:RangeValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToValidate="tbxDealPercent" Display="Dynamic" ErrorMessage="딜러비는 수값이여야 합니다." 
                    Operator="DataTypeCheck" Type="Double" SetFocusOnError="True" 
                    ValidationGroup="userreg">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="tbxDealPercent" Display="Dynamic" ErrorMessage="딜러비를 입력하세요" 
                    SetFocusOnError="True" ValidationGroup="userreg">*</asp:RequiredFieldValidator>
                딜러비
            </td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxDealPercent" runat="server" CssClass="clsEdit" Text="0" Width=60px></asp:TextBox> &nbsp;%<asp:Label ID="lblPartnerPer" runat="server" Text="0" ForeColor=Red></asp:Label>
            </td>
        </tr>
        <tr id=trPhone runat=server>

            <td class="clsFieldName">파트너코드</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxPartner" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">계정상태</td>
            <td class="clsFieldValue">
                <asp:DropDownList ID="ddlNologin" runat="server">
                    <asp:ListItem Value="0">사용허용</asp:ListItem>
                    <asp:ListItem Value="1">사용정지</asp:ListItem>
                </asp:DropDownList>
                 <asp:TextBox ID="ddlIsNew" runat="server" CssClass="clsEdit" style="display:none;"></asp:TextBox>
            </td>
            <td class="clsFieldName">채팅기능</td>
            <td class="clsFieldValue">
                <asp:DropDownList ID="ddlStopChat" runat="server">
                    <asp:ListItem Value="0">사용허용</asp:ListItem>
                    <asp:ListItem Value="1">사용정지</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">은행이름</td>
            <td class="clsFieldValue">
                 <asp:TextBox ID="bankName" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
             <td class="clsFieldName">은행계좌</td>
            <td class="clsFieldValue">
                 <asp:TextBox ID="bankAccountNumber" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">예금주</td>
            <td class="clsFieldValue">
                 <asp:TextBox ID="depositHolder" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
            <td class="clsFieldName">환전비밀번호</td>
            <td class="clsFieldValue">
                 <asp:TextBox ID="currencyExPassword" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">전화번호</td>
            <td class="clsFieldValue">
                <asp:TextBox ID="tbxTelNum" runat="server" CssClass="clsEdit"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="clsFieldName">메모</td>
            <td class="clsFieldValue" colspan=3>
                <asp:TextBox ID="tbxMemo" runat="server" TextMode=MultiLine Width=100% Height=60px CssClass="clsEdit"></asp:TextBox>
            </td>
        </tr>
    </table>
    <script>
        
    </script>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:CustomValidator ID="cvResult" runat="server">*</asp:CustomValidator>
        <asp:Button ID="btnSave" runat="server" class="btn btn-info" onclick="btnSave_Click" Text="수정" ToolTip="" Width="84px" />&nbsp;
        <asp:Button ID="btnList" runat="server" class="btn btn-warning" onclick="btnLisTBL_Click" Text="목록보기" ToolTip="" />
    </asp:Panel>
    <asp:ValidationSummary ID="vsErrors" runat="server" />
    
    <asp:UpdatePanel ID="UpdatePanel8" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <table>
                <tr>
                
                     <td width="100px" nowrap align="center">
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="0" OnClick="OnTypeClick" class="btn btn-info">접속내역</asp:LinkButton>
                      </td>
                     <td width="100px" nowrap align="center">
                        <asp:LinkButton ID="LinkButton2"  runat="server" CommandArgument="1" OnClick="OnTypeClick" class="btn btn-info" >입금내역</asp:LinkButton>
                     </td>
                     <td width="100px" nowrap align="center">
                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="2" OnClick="OnTypeClick" class="btn btn-info" >출금내역</asp:LinkButton> 
                    </td>
                    <td width="100px" nowrap align="center">
                         <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="3" OnClick="OnTypeClick" class="btn btn-info">당일 올인베팅내역</asp:LinkButton>
                    </td>
                    <td width="100%">
                    </td>
                </tr>
                <tr>
                    <td height=10px></td>
                </tr>
                <tr>
                    <td colspan="10" valign=top>
                        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="1">
                            <asp:View ID="View1" runat="server">
                                <table cellpadding="0" cellspacing="0"  width="100%">
                                    <tr>
                                        <td>
                                            <img src="../../Images/ico_syst001.gif" />
                                        </td>
                                        <td width="7px" nowrap>
                                        </td>
                                        <td class="clsSubTitle" nowrap>
                                            <asp:Label ID="lblNickName1" runat="server"></asp:Label>
                                            님의 접속내역
                                        </td>
                                        <td align="center" width=100%>
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                                DisplayAfter="100">
                                                <ProgressTemplate>
                                                    <font color="red" style="font-size: 10pt;">정보를 읽어들이고 있습니다.</font></ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>
                                </table>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdLoginHist" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            BackColor="White"  BorderStyle="None" BorderWidth="1px"
                                            CellPadding="4" DataKeyNames="ID" EmptyDataText="접속이력이 없습니다." Font-Size="9pt"
                                            ForeColor="Black" GridLines="Horizontal" OnPageIndexChanging="grdLoginHist_PageIndexChanging"
                                            OnRowDataBound="grdLoginHist_RowDataBound" OnRowDeleting="grdLoginHist_RowDeleting"
                                            Width="90%">
                                             <RowStyle CssClass="GridRow" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="번호">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkNo" runat="server" CommandArgument='<%# Bind("ID") %>' CommandName="Select"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="GridCommandButton" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IPAddr" HeaderText="아이피" SortExpression="IPAddr">
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ClientID" HeaderText="MAC" SortExpression="ClientID">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="starttime" DataFormatString="{0:yyyy.MM.dd HH:mm:ss}"
                                                    HeaderText="접속시간" SortExpression="regdate">
                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="endtime" DataFormatString="{0:yyyy.MM.dd HH:mm:ss}" HeaderText="탈퇴시간"
                                                    SortExpression="regdate" NullDisplayText="-">
                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                </asp:BoundField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                            OnClientClick="return confirm('정말로 삭제하시겠습니까?');" Text="삭제"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="GridCommandButton" HorizontalAlign="Center" Width="40px" Wrap="false" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle  ForeColor="Black" />
                                            <PagerStyle BackColor="White" CssClass="PagerButton"  HorizontalAlign="Right" />
                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle  CssClass="GridHeader"  />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <img src="../../Images/ico_syst001.gif" />
                                        </td>
                                        <td width="7px" nowrap>
                                        </td>
                                        <td class="clsSubTitle" nowrap>
                                            <asp:Label ID="lblNickName2" runat="server"></asp:Label>
                                            님의 입금내역
                                        </td>
                                        <td align="center" width="100%">
                                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2"
                                                DisplayAfter="100">
                                                <ProgressTemplate>
                                                    <font color="red" style="font-size: 10pt;">정보를 읽어들이고 있습니다.</font></ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>
                                </table>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdChargeHist" runat="server" AutoGenerateColumns="False" 
                                            Width=100% AllowPaging="True" DataKeyNames="ID" AllowSorting="True" 
                                            onpageindexchanging="grdChargeHist_PageIndexChanging" 
                                            onrowdatabound="grdChargeHist_RowDataBound" OnRowDeleting="grdChargeHist_RowDeleting" 
                                            EmptyDataText="입금내역이 없습니다."  
                                            >
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
                                                <asp:BoundField DataField="LoginID" HeaderText="아이디">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EntName" HeaderText="부본사사" NullDisplayText="-">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Chongpan" HeaderText="총판" NullDisplayText="-">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Maejang" HeaderText="매장" NullDisplayText="-">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="bankAccountNumber" HeaderText="입금계좌">
                                                    <ItemStyle HorizontalAlign="center" Wrap=false />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="depositHolder" HeaderText="예금주명">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="money" HeaderText="요청금액" DataFormatString="{0:N0}">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="telnum" HeaderText="연락처">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="regdate" HeaderText="요청시간" DataFormatString="{0:yyyy-MM-dd HH:mm}">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="Center" Width="120px" Wrap=false />
                                                </asp:BoundField>
                                               
                                                <asp:TemplateField HeaderText="비고">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMemo" runat=server Text='<%# Eval("memo").ToString() == "" ? "없음" : "보기" %>' 
                                                            ForeColor='<%# Eval("memo").ToString() == "" ? System.Drawing.Color.Black : System.Drawing.Color.Red %>' 
                                                            ToolTip='<%# Bind("memo") %>' Font-Bold='<%# Eval("memo").ToString() != "" %>' ></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('정말로 삭제하시겠습니까?');" runat="server"
                                                            CausesValidation="False" CommandName="Delete" Text="삭제"></asp:LinkButton>
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:View>
                            <asp:View ID="View3" runat="server">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <img src="../../Images/ico_syst001.gif" />
                                        </td>
                                        <td width="7px" nowrap>
                                        </td>
                                        <td class="clsSubTitle" nowrap>
                                            <asp:Label ID="lblNickName3" runat="server"></asp:Label>
                                            님의 출금내역
                                        </td>
                                        <td align="center" width="100%">
                                            <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel3"
                                                DisplayAfter="100">
                                                <ProgressTemplate>
                                                    <font color="red" style="font-size: 10pt;">정보를 읽어들이고 있습니다.</font></ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>
                                </table>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" RenderMode="Inline">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdWithdrawHist" runat="server" AutoGenerateColumns="False" 
                                            Width=100% AllowPaging="True" DataKeyNames="ID" AllowSorting="True" 
                                            onpageindexchanging="grdWithdrawHist_PageIndexChanging" 
                                            onrowdatabound="grdWithdrawHist_RowDataBound" OnRowDeleting="grdWithdrawHist_RowDeleting" 
                                            EmptyDataText="출금이력이 없습니다."  
                                            >
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
                                                <asp:BoundField DataField="LoginID" HeaderText="아이디">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Partner" HeaderText="파트너" NullDisplayText="-" Visible="false">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EntName" HeaderText="부본사사" NullDisplayText="-">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Chongpan" HeaderText="총판" NullDisplayText="-">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Maejang" HeaderText="매장" NullDisplayText="-">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="bankinfo" HeaderText="출금계좌">
                                                    <ItemStyle HorizontalAlign="center" Wrap=false />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="name" HeaderText="예금주명">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="money" HeaderText="요청금액" DataFormatString="{0:N0}">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="realmoney" HeaderText="실지금액" DataFormatString="{0:N0}">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="telnum" HeaderText="연락처">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="regdate" HeaderText="요청시간" DataFormatString="{0:yyyy-MM-dd HH:mm}">
                                                    <HeaderStyle Wrap="false" />
                                                    <ItemStyle HorizontalAlign="Center" Width="120px" Wrap=false />
                                                </asp:BoundField>
                                                
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('정말로 삭제하시겠습니까?');" runat="server"
                                                            CausesValidation="False" CommandName="Delete" Text="삭제"></asp:LinkButton>
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:View>
                            <asp:View ID="View4" runat="server">
                                <table cellpadding="0" cellspacing="0"  >
                                    <tr>
                                        <td>
                                            <img src="../../Images/ico_syst001.gif" />
                                        </td>
                                        <td width="7px" nowrap>
                                        </td>
                                        <td class="clsSubTitle" nowrap>
                                            <asp:Label ID="lblNickName4" runat="server"></asp:Label>
                                            님의 올인베팅내역
                                        </td>
                                        <td align="center" width="100%">
                                            <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel4"
                                                DisplayAfter="100">
                                                <ProgressTemplate>
                                                    <font color="red" style="font-size: 10pt;">정보를 읽어들이고 있습니다.</font></ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>
                                </table>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" RenderMode="Inline">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdBettingHist" runat="server" AutoGenerateColumns="False" Width="100%"
                                            AllowPaging="True"  OnPageIndexChanging="grdBettingHist_PageIndexChanging"
                                            EmptyDataText="베팅내역이 없습니다." OnRowDataBound="grdBettingHist_RowDataBound" 
                                            CellPadding="4" ForeColor="Black" GridLines="Horizontal" Font-Size="9pt" BackColor="White"
                                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                            <RowStyle CssClass="GridRow" />
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <Columns>
                                                
                                                <asp:BoundField DataField="regdate" HeaderText="시간" SortExpression="regdate" DataFormatString="{0:yyyy-MM-dd HH:mm}">
                                                    <ItemStyle HorizontalAlign="center" Width="100px" Wrap="true" CssClass="BrDateGridRow" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="panmoney" HeaderText="판돈" SortExpression="regdate" DataFormatString="{0:N0}">
                                                    <ItemStyle HorizontalAlign="center" Width="100px" Wrap="true" CssClass="BrDateGridRow" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="startmoney" HeaderText="시작머니" SortExpression="startmoney" DataFormatString="{0:N0}">
                                                    <ItemStyle HorizontalAlign="center" Width="100px" Wrap="true" CssClass="BrDateGridRow" />
                                                </asp:BoundField>
                                                
                                                 <asp:BoundField DataField="changemoney" HeaderText="끝머니" SortExpression="changemoney" DataFormatString="{0:N0}">
                                                    <ItemStyle HorizontalAlign="center" Width="100px" Wrap="true" CssClass="BrDateGridRow" />
                                                </asp:BoundField>
                                                
                                                
                                                 <asp:BoundField DataField="entrycard" HeaderText="패정보" SortExpression="entrycard" >
                                                    <ItemStyle HorizontalAlign="center" Width="100px" Wrap="true" CssClass="BrDateGridRow" />
                                                </asp:BoundField>
                                            </Columns>
                                            
                                            <PagerStyle HorizontalAlign="Center" CssClass="clsButton" />
                                            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="#ecf8ff" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:View>
                            
                        </asp:MultiView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>