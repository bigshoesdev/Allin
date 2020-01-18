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
using System.Data.SqlClient;

public partial class Bonsa_Services_GetLoginCount : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (인증회원 == null)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Logout();
            Response.End();
            return;
        }

        if (GetClass() < 4)
            return;

        if (!IsPostBack)
        {
            DBManager dbManager = new DBManager();
            string strQuery = "SELECT dbo.F_GET_LOGIN_COUNT() AS LOGIN_COUNT";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsCount = dbManager.RunMSelectQuery(cmd);
            int count = Convert.ToInt32(dsCount.Tables[0].Rows[0]["LOGIN_COUNT"]);
            Response.Write(count);
            Response.End();
        }
    }
}
