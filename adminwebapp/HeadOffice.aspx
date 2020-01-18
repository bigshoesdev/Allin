<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HeadOffice.aspx.cs" Inherits="HeadOffice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<script src="./scripts/jquery-1.11.2.min.js" type="text/javascript"></script>

    <title>JSK Studio 관리시스템</title>
</head>
<frameset rows="30, *" frameborder="0" border="0">
    <frame id="head" src="./Bonsa/Header.aspx" noresize border="0"/>
    
    <frameset id="contains" cols="195, *" frameborder="0" border="0">
        <frame id="menu" name="menu" src="./Bonsa/Menu.aspx" noresize border="0"/>
        <frame id="content" name="content" src="./Bonsa/Game/상황판.aspx" border="0"/>
    </frameset>
</frameset>
</html>

