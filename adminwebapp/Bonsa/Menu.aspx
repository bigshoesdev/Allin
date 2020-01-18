<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Bonsa_Menu" %>
<%@ Import Namespace="System.Data" %>
<%string d = System.DateTime.Now.ToString("yyyyMMddHHmmssfff"); %>

<html>
<head>
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="../css/bootstrap-theme.css" />
    <link rel="stylesheet" type="text/css" href="../css/new_main.css?t=<%=d%>" />
    <link rel="stylesheet" type="text/css" href="../css/new_common.css?t=<%=d%> %>" />
    
     <link rel="stylesheet" type="text/css" href="../css/font-awesome.min.css" />
     <link rel="stylesheet" type="text/css" href="../css/accordion-menu.css?t=<%=d%> %>" />
     <link rel="stylesheet" type="text/css" href="../css/jquery-accordion-menu.css?t=<%=d%> %>" />
    
    <script src="../scripts/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script src="../scripts/socket.io.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(function () {
            var socket = window.socket;

            if (!socket) {
                socket = io.connect("<%= ConfigurationManager.ConnectionStrings["AllinSocketString"].ConnectionString %>");
                window.socket = socket;
            }

            socket.on("askQuestion", function () {
                location.reload();
            })
        });

        <% if (isQuestionAlert) {%>
        $(function () {
            var audio = new Audio("/Images/alarm.mp3");
            audio.play();
        });
        <% } %>

    </script>
    <style>
        .sub-menu-alarm {
            display: inline-block;
            margin-left: 5px;
            background-color: red;
            border-radius: 50%;
            font-size: 12px;
            width: 12px;
            height: 12px;
            font-weight: bold;
            text-align: center;
            color: #fff;
            line-height: 17px;
            vertical-align: top;
        }
    </style>
    <title></title>
</head>

<body style="overflow-x:hidden;background:#000"">
      <div style="width:190px; padding:0px; margin:0px; height:36px; text-align:center; border-bottom:1px solid #1b1b1b; border-top:1px solid #1b1b1b">
            <span style="color:#ddd;"><h4>Sky Game</h4></span>
      </div>
    <form id="form1" runat="server">
    <div style="width:190px; padding-right:10px;">
    <ul id="accordion" class="accordion">
    <%  int i = 0;
        foreach (DataRow row in menu)
        {%>
        <li>
			<div class="link"><i class="<%=icons[i++].ToString()%> main-menu"></i><%=row["depth1"].ToString() %><i class="fa fa-chevron-down"></i>
                <%if (row["depth1"].ToString() == "공지관리" && isQuestionAlert)
                    { %>
                <span class="sub-menu-alarm"></span>
                <% }%>
			</div>
			<ul class="submenu">
			<% ArrayList sub_menu = menuManager.getSubMenu(row);
                if (sub_menu != null)
                    foreach (DataRow subRow in sub_menu)
                    {%>
                <li>
                <a href="<%=subRow["link_url"].ToString() + "?mid=" + subRow["id"].ToString() %>" target="content" id="submenu-<%=subRow["id"].ToString()%>" class="sub-menu"><%=subRow["depth2"].ToString() %>
                    <%if (subRow["depth2"].ToString() == "1:1문의" && isQuestionAlert)
                        {%>
                    <span class="sub-menu-alarm"></span>
                    <% } %>
                </a>
                </li>
            <%} %>
			</ul>
		</li> 
    <% } %>  		
	</ul> 
        
    </div>  
    
    </form>
    
    <script src="../scripts/accordion-menu.js?t=<%=d%> %>" type="text/javascript"></script>
   
</body>
</html>
