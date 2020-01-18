<%@ Page Language="C#" MasterPageFile="../Exchange/입출금관리.master" AutoEventWireup="true" CodeFile="정산처리.aspx.cs" Inherits="입출금관리_정산처리" Title="코리아 게임 관리자페이지" %>

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
                회원정산
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
                <td class="srcTit" nowrap>
                    &nbsp;<img src="../../Images/ico_sqr02.gif" width="3" height="3" align="absmiddle">&nbsp;
                    날자:
                    <asp:TextBox ID="tbxStartDate" runat="server" CssClass="clsEdit" 
                        Width="80px" onclick="Calendar(this)" ></asp:TextBox>
                    부터&nbsp;&nbsp;<asp:TextBox ID="tbxEndDate" runat="server" CssClass="clsEdit"
                        Width="80px" onclick="Calendar(this)"></asp:TextBox>
                    까지
                </td>
                <td align=left>
                    <asp:Button ID="btnSearch" runat="server" Text="검색" class="btn btn-danger" OnClick="btnSearch_Click" />
                </td>
                <td width=80%>
                    <asp:Button ID="btn1" runat="server" CommandArgument="0" CssClass="clsButton"  EnableViewState=false
                        onclick="btnDay_Click" Text="오늘" />
&nbsp;<asp:Button ID="btn2" runat="server" CommandArgument="1" CssClass="clsButton"  EnableViewState=false
                        onclick="btnDay_Click" Text="어제" />
&nbsp;<asp:Button ID="btn3" runat="server" CommandArgument="2" CssClass="clsButton"  EnableViewState=false
                        onclick="btnDay_Click" Text="2일전" />
&nbsp;<asp:Button ID="btn4" runat="server" CommandArgument="3" CssClass="clsButton"  EnableViewState=false
                        onclick="btnDay_Click" Text="3일전" />
&nbsp;<asp:Button ID="btn5" runat="server" CommandArgument="4" CssClass="clsButton"  EnableViewState=false
                        onclick="btnDay_Click" Text="4일전" />
&nbsp;<asp:Button ID="btn6" runat="server" CommandArgument="5" CssClass="clsButton"  EnableViewState=false
                        onclick="btnDay_Click" Text="5일전" />
&nbsp;<asp:Button ID="btn7" runat="server" CommandArgument="6" CssClass="clsButton"  EnableViewState=false
                        onclick="btnDay_Click" Text="6일전" />
                </td>
            </tr>
        </table>
    </div>
    <asp:GridView ID="grdList" runat="server" AutoGenerateColumns="False" Width=100% AllowPaging="True" AllowSorting="True" 
        onpageindexchanging="grdLisTBL_PageIndexChanging" EmptyDataText="검색된 자료가 없습니다."  PageSize=100
        ForeColor="#333333" Font-Size=9pt ShowFooter="True">
        <RowStyle CssClass="GridRow" />
        <PagerSettings Mode="NumericFirstLast" />
        <Columns>
            <asp:TemplateField HeaderText="날자">
                <ItemTemplate>
                    <asp:Label ID="lblState" runat="server" CausesValidation="False" 
                        Text='<%# Eval("Date") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="center" Width="150px" Wrap="False" />
                <FooterStyle HorizontalAlign=Center />
            </asp:TemplateField>
             
            <asp:BoundField DataField="ChargeMoney" HeaderText="총입금액" 
                DataFormatString="{0:N0}" >
                <ItemStyle HorizontalAlign="right" />
                <FooterStyle HorizontalAlign=right />
            </asp:BoundField>
            <asp:BoundField DataField="ChargeCount" HeaderText="입금회수" 
                DataFormatString="{0:N0}회" >
                <ItemStyle HorizontalAlign="right" />
                <FooterStyle HorizontalAlign=right />
            </asp:BoundField>
            <asp:BoundField DataField="WithdrawMoney" HeaderText="총출금액" 
                DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="right" />
                <FooterStyle HorizontalAlign=right />
            </asp:BoundField>
            <asp:BoundField DataField="WithdrawCount" HeaderText="출금회수" 
                DataFormatString="{0:N0}회" >
                <ItemStyle HorizontalAlign="right" />
                <FooterStyle HorizontalAlign=right />
            </asp:BoundField>
            <asp:BoundField DataField="BenefitMoney" HeaderText="순수익" 
                DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="right"  />
                <FooterStyle HorizontalAlign=right  />
            </asp:BoundField>
            <asp:BoundField DataField="EnterpriseMoney" HeaderText="업체수익" 
                DataFormatString="{0:N0}">
                <ItemStyle HorizontalAlign="right" ForeColor=Red />
                <FooterStyle HorizontalAlign=right  />
            </asp:BoundField>
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
        &nbsp;<asp:Button ID="btnViewByUser" runat="server" CssClass="clsButton" Text="회원별로보기" 
            OnClick="btnViewByUser_Click" />
    </asp:Panel>
    <br />
    <asp:CustomValidator ID="cvResult" Visible="false" runat="server" Display="Dynamic"
        ValidationGroup="list"></asp:CustomValidator>
    <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="list" />
</asp:Content>

