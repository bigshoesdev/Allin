using System;
using System.Data.SqlClient;
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

public partial class 회원관리_업체공지사항수정 : PageBase
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
        if (!IsPostBack)
        {
            string strID = Request.QueryString["id"];
            try { int.Parse(strID); }
            catch { strID = "0"; }
            hdnID.Value = strID;
            //lblTitle.Text = strID;
            if (strID.Equals("0") == true)
            {
                lblTitle.Enabled = true;
                tbxContent.ReadOnly = false;
            }
            DBManager dbManager = new DBManager();
            string strQuery = "SELECT * FROM TBL_AdminBoard WHERE ID=" + strID;
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsNotice = dbManager.RunMSelectQuery(cmd);
            if (dsNotice.Tables.Count == 0)
            {
                // Response.Redirect("업체공지사항목록.aspx");
                return;
            }

            if (dsNotice.Tables[0].Rows.Count == 0)
            {
                // Response.Redirect("업체공지사항목록.aspx");
                return;
            }
            BindInfo(dsNotice);
        }
    }

    void BindInfo(DataSet dsNotice)
    {
        if (dsNotice == null) return;
        if (dsNotice.Tables.Count == 0) return;
        if (dsNotice.Tables[0].Rows.Count == 0) return;

        lblTitle.Text = dsNotice.Tables[0].Rows[0]["title"].ToString();
        string strNoticeType = "";
        switch (dsNotice.Tables[0].Rows[0]["class"].ToString())
        { 
            case "1":
                strNoticeType = "매장공지";
                break;
            case "2":
                strNoticeType = "총판공지";
                break;
            case "3":
                strNoticeType = "부본사공지";
                break;
            case "4":
                strNoticeType = "본사공지";
                break;
            case "5":
                strNoticeType = "슈퍼본사공지";
                break;
        }
        lblNoticeType.Text = strNoticeType;
        tbxContent.Text = dsNotice.Tables[0].Rows[0]["TextContent"].ToString();
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("공지사항목록.aspx");
    }
}
