<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Header.aspx.cs" Inherits="Bonsa_Header" ValidateRequest="false"%>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%string d = System.DateTime.Now.ToString("yyyyMMddHHmmssfff"); %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

<link rel="stylesheet" type="text/css" href="~/css/bootstrap.css" />
<link rel="stylesheet" type="text/css" href="~/css/bootstrap-theme.css" />
<link rel="stylesheet" type="text/css" href="~/css/font-awesome.min.css" />

<script src="../scripts/jquery-1.11.2.min.js" type="text/javascript"></script>

<style type="text/css">
body {
    background-color: #000;
}

.item 
{
	color:White;
	margin-left:10px;
	cursor: pointer;
}

i
{
	padding-right:3px;
}
</style>
<script language=javascript>
    function RefreshPage() {
        ifrmNotify.document.location.href = "../bonsa/Services/Notify.aspx";
        window.setTimeout("RefreshPage()", 10000);
    }
    </script>
    <title>JSK Studio 관리자솔루션</title>
</head>
<body onload="window.setTimeout('RefreshPage()', 1000);" style="overflow:hidden; min-width:1500px;">
<div id="container">
     
    <div style="position:relative; float:left; clear:none; width:auto; text-align:center; font-size:1.5em;  padding-top:-5px; color:White;">
         <b>ADMINISTRATOR</b>
    </div>
        <form id="form1" runat="server"> 
        <div style="position:relative; float:left; clear:none; top:5px; font-size:10pt; padding-left:20px; color:#d9d2e9; width: calc(100% - 190px);">        
            <div style="position:relative; float:left;">
                <asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogouTBL_Click"> 
                <img src="../Images/logout.png" width="20" height="20" /> LOGOUT</asp:LinkButton>
            </div>
            
            <div style="position:relative; float:left; margin-left:20px;"> 
                사용자:&nbsp;<asp:Label ID="lblAdmin" runat="server"></asp:Label> 님
            </div>    
            <div runat="server"  style="position:relative; float:left; margin-left:20px; width: calc(100% - 260px);">
                <iframe id="ifrmNotify" src = "../bonsa/Services/Notify.aspx" width="100%" style="border-style:none"></iframe>
            </div>
        </div>  
        </form> 
</div>  
</body>
</html>