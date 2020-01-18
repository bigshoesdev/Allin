<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FirstPage.aspx.cs" Inherits="FirstPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Welcome to Sky Game</title>
</head>
<frameset rows="30, *" frameborder="0">
    <frame id="head" src="./SuperBonsa/Header.aspx" noresize/>
    
    <frameset cols="195, *" frameborder="0" >
        <frame id="menu" name="menu" src="./SuperBonsa/Menu.aspx" noresize/>
        <frame id="content" name="content" src="" />
    </frameset>
</frameset>
</html>
