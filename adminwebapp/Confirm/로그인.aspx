<%@ Page Language="C#" AutoEventWireup="true" CodeFile="로그인.aspx.cs" Inherits="확인등록_로그인" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.css" />
    
    <style type="text/css">
    body 
    {
    	
    }
    table 
    {
    	font-family:"굴림", "굴림체", "system";
    	font-weight:200;
    	font-size:12pt;
    	color:Gray;
    }
    
    .form 
    {
    	background:url('../Images/manager.jpg'); 
    	background-repeat:no-repeat; 
    	width:320px; 
    	vertical-align:middle; 
    	margin-top:10%; 
    	
    }
    </style>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>관리자로그인</title>
</head>
<body>
<div id="container" class="container" style="vertical-align:middle; ">
    <form id="form1" runat="server">
    
    <table align=center class="form">
        <tr height=100px>
        <td></td>
        </tr>
        <tr>
            <td align="right" style="padding-right:6%; padding-top:20px;">
              <div style="position: relative; margin-top:50%; border: solid 1px #ccc; padding:20px;border-radius: 10px; ">  
              <div class="row" style="text-align: center; font-size:18px; font-weight: bold;  margin-top:0px; margin-bottom:10px; color:#000">ADMINISTRATOR</div>
                <asp:RadioButtonList ID="LoginType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True" Value="ENT">관리자&nbsp;</asp:ListItem>
                    <asp:ListItem Value="EMP">사원</asp:ListItem>
                </asp:RadioButtonList>
                <asp:Login ID="Login1" runat="server"
                    DisplayRememberMe="False" FailureText="아이디 혹은 비밀번호가 정확하지 않습니다."                     
                    LoginButtonText="확인" PasswordLabelText="비밀번호:"  TitleText=""
                    PasswordRequiredErrorMessage="비밀번호를 입력하세요." RememberMeText="아이디 기억." 
                    UserNameLabelText="아이디:" 
                    UserNameRequiredErrorMessage="아이디를 입력하세요." VisibleWhenLoggedIn="False" 
                    onauthenticate="Login1_Authenticate">  
                 
                 <LabelStyle CssClass="col-sm-5 control-label" />
                 <TextBoxStyle CssClass="form-control" />
                 <LoginButtonStyle CssClass="btn btn-info" />
                 <InstructionTextStyle CssClass="clsText" />
                               
                </asp:Login>
                  </div>
            </td>
        </tr>
    </table>
  
    </form>
</div>
</body>
</html>
<script language="javascript" type="text/javascript">

if(top.location != self.location)
    top.location = self.location;

</script>

