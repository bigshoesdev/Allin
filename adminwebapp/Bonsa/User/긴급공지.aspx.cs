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

public partial class 게임관리_긴급공지 : PageBase
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
            DBManager dbManager = new DBManager();
            string strQuery = "SELECT A.*, Env.EnvValue FROM TBL_AdminInfo A, (SELECT EnvValue FROM TBL_SYSENV WHERE EnvCode='MobileConfirm') Env";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsAdminInfo = dbManager.RunMSelectQuery(cmd);
            if (dsAdminInfo.Tables.Count > 0 && dsAdminInfo.Tables[0].Rows.Count > 0)
            {
                BindInfo(dsAdminInfo);
            }

            string menu_id = Request.QueryString["mid"];
            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            if (permission <= 1)
            {
                btnSave.Visible = false;
            }
        }
    }

    void BindInfo(DataSet dsAdminInfo)
    {
        if (dsAdminInfo == null) return;
        if (dsAdminInfo.Tables.Count == 0) return;
        if (dsAdminInfo.Tables[0].Rows.Count == 0) return;
        tbxNoticeCotent.Text = dsAdminInfo.Tables[0].Rows[0]["notice_content"].ToString();
        tbxNoticeTitle.Text = dsAdminInfo.Tables[0].Rows[0]["notice_title"].ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strNoticeCotent = tbxNoticeCotent.Text;
            string strNoticeTitle = tbxNoticeTitle.Text;

            string strQuery = "UPDATE TBL_AdminInfo SET ";
            strQuery += "notice_title=@notice_title, notice_content=@notice_content, changedate=getDate(); ";
            DBManager dbManager = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@notice_title", strNoticeTitle));
            cmd.Parameters.Add(new SqlParameter("@notice_content", strNoticeCotent));
            dbManager.RunMQuery(cmd);
            cvResult2.ErrorMessage = "긴급공지정보가 변경되였습니다.";
        }
        catch (Exception ex)
        {
            cvResult2.ErrorMessage = "긴급공지정보가변경에 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult2.IsValid = false;
    }
}
