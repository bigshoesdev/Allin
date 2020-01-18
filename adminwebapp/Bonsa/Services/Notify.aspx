<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notify.aspx.cs" Inherits="Services_Notify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <bgsound id="sndBack" runat=server loop=1 />
    
    <script src="../../scripts/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script src="../../scripts/socket.io.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            var socket = window.socket;

            if (!socket) {
                socket = io.connect("<%= ConfigurationManager.ConnectionStrings["AllinSocketString"].ConnectionString %>");
                window.socket = socket;
            }

            socket.on("registerNewUser_app", function () {
                location.reload();
            })
        });

    </script>
</head>
<body style="margin:0">
    <form id="form2" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0" border="0" style="color:#d9d2e9; font-size:10pt; text-align:left;">
            <tr>
                <td align="center" nowrap>보유머니:<asp:Label ID="lblCoin" runat="server" ForeColor="Red"></asp:Label></td>
                <td align="center" nowrap runat="server" id="tdAdmin1">&nbsp;&nbsp;금일입금:<asp:Label runat="server" ID="lblTodayCharge" ForeColor="Red"></asp:Label></td>
                <td align="center" nowrap runat="server" id="tdAdmin2">&nbsp;&nbsp;금일출금:<asp:Label runat="server" ID="lblTodayWithdraw" ForeColor="Red"></asp:Label></td>
                <td align="center" nowrap runat="server" id="tdAdmin3">&nbsp;&nbsp;금일파트너입금:<asp:Label runat="server" ID="lblPartnerCharge" ForeColor="Red"></asp:Label></td>
                <td align="center" nowrap runat="server" id="tdAdmin4">&nbsp;&nbsp;금일파트너출금:<asp:Label runat="server" ID="lblPartnerWithdraw" ForeColor="Red"></asp:Label></td>

                <td align="center" nowrap runat="server" id="tdAdmin5">&nbsp;&nbsp;현재접속자:<asp:Label runat="server" ID="lblCurConnector" ForeColor="Red"></asp:Label></td>
                <td align="center" nowrap runat="server" id="tdAdmin6">&nbsp;&nbsp;입금요청:<a target="content" href="/Bonsa/Exchange/입금요청.aspx"><asp:Label ID="lblChargeCount" runat="server" ForeColor="Red"></asp:Label></a></td>
                <td align="center" nowrap runat="server" id="tdAdmin7">&nbsp;&nbsp;출금요청:<a target="content" href="/Bonsa/Exchange/출금요청.aspx"><asp:Label ID="lblWithdrawCount" runat="server" ForeColor="Red"></asp:Label></a></td>
                <td align="center" nowrap runat="server" id="tdAdmin8">&nbsp;&nbsp;업체입금요청:<a target="content" href="/Bonsa/EnterpriseExchange/업체입금요청.aspx"><asp:Label ID="lblPartnerChargeCount" runat="server" ForeColor="Red"></asp:Label></a></td>
                <td align="center" nowrap runat="server" id="tdAdmin9">&nbsp;&nbsp;업체출금요청:<a target="content" href="/Bonsa/EnterpriseExchange/업체출금요청.aspx"><asp:Label ID="lblPartnerWithdrawCount" runat="server" ForeColor="Red"></asp:Label></a></td>
                <td align="center" nowrap runat="server" id="td1">&nbsp;&nbsp;1:1문의:<a target="content" href="/Bonsa/User/11문의.aspx"><asp:Label ID="lblQuestionCount" runat="server" ForeColor="Red"></asp:Label></a></td>
                <td align="center" nowrap runat="server" id="td2">&nbsp;&nbsp;로그인승인요청:<a target="content" href="/Bonsa/User/회원목록.aspx"><asp:Label ID="lblUserCount" runat="server" ForeColor="Red"></asp:Label></a></td>
            </tr>
        </table>
        </div>
    </div>
    </form>
</body>
</html>
