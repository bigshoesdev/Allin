<%@ Page Language="C#" MasterPageFile="회원관리.master" AutoEventWireup="true" CodeFile="EmployeeList.aspx.cs" Inherits="Bonsa_User_EmployeeList" %>

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
                <h5>사원계정관리</h5>
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
    <table  class="clsLineTable">
        <tr>
            <td>
            </td>
        </tr>
    </table>
    <div class='PageToolBar'>
        <table width="100%" border="0" cellpadding="7" cellspacing="1">
            <tr valign="middle">
                <td width="10%" class="srcTit" nowrap>
                            &nbsp;<i class="fa fa-circle-o" ></i>
                             &nbsp;결과&nbsp;: 총
                    [<asp:Label ID="lblRowCount" runat="server"></asp:Label>]명
                </td>
                <td class="srcTit" nowrap>
                    &nbsp;<i class="fa fa-circle-o" ></i>
                    <asp:DropDownList ID="ddlSearchKey" runat="server">
                        <asp:ListItem Value="LoginID">회원ID</asp:ListItem>
                        <asp:ListItem Value="Name">이름</asp:ListItem>
                    </asp:DropDownList>
                </td>
                 <td>
                    <asp:TextBox ID="tbxSearchValue" runat="server" CssClass="clsEdit" 
                        OnTextChanged="btnSearch_Click" Width="100px"></asp:TextBox>
                </td>
                 <td class="srcTit" nowrap>
                     &nbsp;<i class="fa fa-circle-o" ></i>
                    &nbsp;사용여부:&nbsp;
                    <asp:DropDownList ID="ddlIsUsed" runat="server">
                        <asp:ListItem Value="">전체</asp:ListItem>
                        <asp:ListItem Value="1">사용중</asp:ListItem>
                        <asp:ListItem Value="0">미사용</asp:ListItem>
                    </asp:DropDownList>
                </td>
               
               
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="검색" class="btn btn-danger" OnClick="btnSearch_Click" />
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
            <asp:BoundField DataField="LoginID" HeaderText="아이디" SortExpression="LoginID" >
                <ItemStyle CssClass="GridItem" Wrap=false />
            </asp:BoundField>
           
            <asp:BoundField DataField="Name" HeaderText="이름" SortExpression="Name" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
           
            <asp:BoundField DataField="tel_num" HeaderText="연락처" SortExpression="tel_num" >
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            
            <asp:BoundField DataField="reg_time" HeaderText="등록시간" SortExpression="reg_time" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                <ItemStyle CssClass="GridItem" Wrap=false HorizontalAlign=Center />
            </asp:BoundField>
            
              
            <asp:TemplateField HeaderText="상담허용" SortExpression="allow_chat">
                <ItemTemplate>
                    <asp:Label ID="lblAllowChat" runat="server" Text='<%# Eval("allow_chat").ToString() == "1" ? "허용" : "금지" %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" Wrap=false />
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="사용여부" SortExpression="is_used">
                <ItemTemplate>
                    <asp:Label ID="lblIsUsed" runat="server" Text='<%# Eval("is_used").ToString() == "1" ? "사용중" : "미사용" %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="GridItem" HorizontalAlign="Center" Wrap=false />
            </asp:TemplateField>
          
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkPermissionFunc" NavigateUrl='<%# "EmpFuncPermission.aspx?emp_id=" + Eval("ID").ToString() %>' runat="server" CausesValidation="False"
                        Text="기능별 권한관리"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
            
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkPermission" NavigateUrl='<%# "EmployeePermission.aspx?emp_id=" + Eval("ID").ToString() %>' runat="server" CausesValidation="False"
                        Text="메뉴별 권한관리"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
            
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkEdit" NavigateUrl='<%# "EmployeeAdd.aspx?id=" + Eval("ID").ToString() %>' runat="server" CausesValidation="False"
                        Text="수정"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" CssClass="GridCommandButton" />
            </asp:TemplateField>
            
            <asp:TemplateField ShowHeader="False" >
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDelete" OnClientClick="return confirm('정말로 삭제하시겠습니까?');" runat="server"
                        CausesValidation="False" CommandName="Delete" Text="삭제"></asp:LinkButton>
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
        <asp:Button ID="btnNew" runat="server" class="btn btn-info" Text="새로등록" onclick="btnNew_Click" />
        <asp:Button ID="btnRefresh" runat="server" class="btn btn-warning" Text="목록새로고침" />
    </asp:Panel>
</asp:Content>
