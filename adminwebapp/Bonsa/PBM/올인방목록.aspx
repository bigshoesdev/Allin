<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="올인방목록.aspx.cs" Inherits="게임관리_올인방목록" Title="코리아 게임 관리자페이지" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Subhead" runat="Server">
    <style>
        a {
            font-size: 9pt;
        }

        span {
            font-size: 9pt;
        }
    </style>

    <script src="../../scripts/socket.io.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {

            var roomView = $(".roomView").val() == '1' ? true: false;

            console.log(roomView);
            var socket = window.socket;
            
            if(!socket)
            {
                socket = io.connect("<%= ConfigurationManager.ConnectionStrings["AllinSocketString"].ConnectionString %>");
                window.socket = socket;
            }

            socket.on("delete_room", function (data) {
                if (data.success) {
                    alert("성공적으로 삭제되엿습니다..");
                    location.href = '올인방목록.aspx';
                } else {
                    alert(data.message);
                }
            })

            startTimeout();

            function startTimeout() {
                socket.emit('lobbyData', function (response) {
                    console.log(response);
                    if (response.success) {
                        lobbyTables = response.lobbyTables;
                        showRoomInfo(lobbyTables);
                    }
                });
                setTimeout(startTimeout, 2000);
            };

            function showRoomInfo(tables) {
                $("#roomListBody").empty();
                $("#roomListBody").append('<tr class="GridHeader"><th scope="col">번호</th><th scope="col">방이름</th><th scope="col">BB/SS</th><th scope="col">선수</th><th scope="col">비밀번호</th><th scope="col"></th><th scope="col"></th></tr>');
                for (var i = 0 ; i < tables.length; i++) {
                    $("#roomListBody").append("<tr>");
                    $("#roomListBody").append("<td style='width=40px' align='center'>" + (i + 1) + "</td>");
                    $("#roomListBody").append("<td align='center'>" + (roomView == true ? ("<a href='올인방보기.aspx?id=" + tables[i].id + "'>" + tables[i].name + "</a>") : (tables[i].name)) + "</td>");
                    $("#roomListBody").append("<td align='center'>" + tables[i].smallBlind + "/" + tables[i].bigBlind + "</td>");
                    $("#roomListBody").append("<td align='center'>" + tables[i].playersSeatedCount + "/" + tables[i].seatsCount + "</td");
                    $("#roomListBody").append("<td align='center'>" + tables[i].password + "</td");
                    $("#roomListBody").append("<td align='center'><a href='올인방등록.aspx?id=" + tables[i].id + "'>수정</a></td>");
                    $("#roomListBody").append("<td align='center'><a href='javascript::' class='delete' data-id='" + tables[i].id + "'>삭제</a></td>");
                    $("#roomListBody").append("</tr>");
                }

                $(".delete").click(function () {
                    socket.emit("delete_room", $(this).data('id'));
                })
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="Server">
    <input id="roomView" runat="server" type="hidden" class="roomView" />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap></td>
            <td class="clsSysTitle">
                <h5>올인
                    <asp:Label ID="lblChannel" runat="server"></asp:Label>&nbsp;실시간게임방정보</h5>
            </td>
        </tr>
    </table>
    <!-- 타이틀밑의 두선 -->
    <table cellpadding="0" border="1" bordercolor="#E7E3E7" cellspacing="0" class="clsLineTable">
        <tr>
            <td></td>
        </tr>
    </table>
    <div class='PageToolBar'>
        <table width="100%" border="0" cellpadding="7" cellspacing="1">
            <tr valign="middle">
                <td width="10%" class="srcTit" nowrap>
                    <i class="fa fa-circle-o"></i>총 80개
                </td>
                <td width="10%" class="srcTit" nowrap style="display: none">&nbsp;
                    &nbsp;&nbsp;</td>
                <td style="display: none">&nbsp;
                </td>
                <td>&nbsp;
                </td>
                <td></td>
            </tr>
        </table>
    </div>
    <div>
        <table cellspacing="0" rules="all" border="1" id="roomList" style="width: 100%; border-collapse: collapse;">
            <tbody id="roomListBody">
                <tr class="GridHeader">
                    <th scope="col">번호</th>
                    <th scope="col">방이름</th>
                    <th scope="col">BB/SS</th>
                    <th scope="col">선수</th>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:Panel ID="pnlListBar" runat="server" CssClass="clsControlBar">
        <asp:Button ID="btnNew" runat="server" class="btn btn-info" Text="새로등록" OnClick="btnNew_Click" />
    </asp:Panel>
</asp:Content>

