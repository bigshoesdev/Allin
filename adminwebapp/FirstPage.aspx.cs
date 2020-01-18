using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class FirstPage : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLoginStatus();
        if (!IsPostBack)
        {

        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // CheckLoginStatus();
    }

    void CheckLoginStatus()
    {
        if (인증회원 == null)
        {            
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            //Logout();
            logout();
            Response.End();
        }
    }

    protected void logout()
    {
        Response.Write("<script>window.parent.location.href='~/Confirm/로그인.aspx'; </script>");
    }
}
