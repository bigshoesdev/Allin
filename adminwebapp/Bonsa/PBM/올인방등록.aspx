<%@ Page Language="C#" MasterPageFile="게임관리.master" AutoEventWireup="true" CodeFile="올인방등록.aspx.cs" Inherits="게임관리_올인방등록" Title="코리아 게임 관리자페이지" %>

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
        var socket = window.socket;

        if (!socket) {
            socket = io.connect("<%= ConfigurationManager.ConnectionStrings["AllinSocketString"].ConnectionString %>");
            window.socket = socket;
        }

        socket.on("add_room", function (data) {
            $("#register").prop('disabled', false);

            if (data.success) {
                alert("성공적으로 보관되엿습니다.");
                location.href = '올인방목록.aspx';
            } else {
                alert(data.message);
            }
        })

        socket.on("get_room", function (data) {
            $("#name").val(data.name);
            $("#password").val(data.password);
        })

        $(function () {
            var id = $(".roomID").val();

            if (id) {
                socket.emit('get_room', id);
            }

            $("#register").click(function () {
                if ($("#name").val() == "") {
                    alert("방이름을 입력하세요.");
                    return;
                }
                var data = {};

                data.id = id;
                data.name = $("#name").val();
                data.buyin = $("#buyin").val();
                data.password = $("#password").val();

                socket.emit("add_room", data);

                $("#register").prop('disabled', true);
            });
        })
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_sysTit1.gif" />
            </td>
            <td width="7px" nowrap></td>
            <td class="clsSysTitle">
                <h5>올인방관리</h5>
            </td>
        </tr>
    </table>
    <!-- 타이틀밑의 두선 -->
    <table cellpadding="0" border="1" bordercolor="#E7E3E7" cellspacing="0" class="clsLineTable">
        <tr>
            <td></td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <img src="../../Images/ico_syst001.gif" />
            </td>
            <td width="7px" nowrap></td>
            <td class="clsSubTitle">올인방등녹 및 수정</td>
        </tr>
    </table>
    <table border="1" bordercolor="#E7E3E7" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <input id="roomID" runat="server" type="hidden" class="roomID" />
            </tr>
            <tr>
                <td class="clsFieldName" width="150px">블라인드선택</td>
                <td class="clsFieldValue" width="350px">
                    <select name="buyin" id="buyin" style="width: 150px">
                        <option value="0">100/200</option>
                        <option value="1">200/400</option>
                        <option value="2">500/1000</option>
                        <option value="3">1000/2000</option>
                        <option value="4">2000/4000</option>
                        <option value="5">3000/6000</option>
                        <option value="6">5000/10000</option>
                        <option value="7">10000/20000</option>
                        <option value="8">20000/40000</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="clsFieldName" width="150px" nowrap="">
                    <span style="color: Red; display: none;">*</span>
                    비밀번호설정</td>
                <td class="clsFieldValue" width="350px" nowrap="">
                    <input name="password" type="text" maxlength="50" id="password" class="clsEdit" style="ime-mode: disabled">
                </td>
            </tr>
        </tbody>
    </table>
    <asp:Panel ID="pnlControlBar" runat="server" CssClass="clsControlBar">
        <input type="button" name="register" value="확인" id="register" class="btn btn-info" style="width: 132px;">
        <asp:Button ID="btnNew" runat="server" class="btn btn-danger" Text="목록보기" OnClick="btnList_Click" />
    </asp:Panel>
</asp:Content>

