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

public partial class Bonsa_Header : PageBase
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
        if (인증회원 != null)
        {
            lblAdmin.Text = "<font color='red'>" + DataSetUtil.RowStringValue(인증회원, "ext_name", 0) + "</font>";
            //lblCoin.Text = long.Parse(인증회원.Tables[0].Rows[0]["money"].ToString()).ToString("N0");
            lnkLogout.Visible = lblAdmin.Visible = true;

            /*if (GetClass() < 4)
                pnlCharge.Visible = false;*/
        }
        else
        {
            lnkLogout.Visible = lblAdmin.Visible = false;
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            //Logout();
            HeaderLogout();
            Response.End();
        }
    }

    protected void lnkLogouTBL_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        FormsAuthentication.SignOut();
        //Logout();
        HeaderLogout();
        Response.End();
    }

}
