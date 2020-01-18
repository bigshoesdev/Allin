<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="manage.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>환영합니다.</title>
    <LINK href="css/style.css" type="text/css" rel="stylesheet">
    <script language="javascript" type="text/javascript">
        window.onload=RefreshPage;
        function RefreshPage()
        {
            window.location.href="Confirm/로그인.aspx";
        }
    </script>
</head>
</html>
