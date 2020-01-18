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

public partial class 게임관리_로그인공지 : PageBase
{
    public string LoginImg { get; private set; }

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

        DBManager dbManager = new DBManager();

        string strQuery = "SELECT A.*, Env.EnvValue FROM TBL_AdminInfo A, (SELECT EnvValue FROM TBL_SYSENV WHERE EnvCode='MobileConfirm') Env";
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsAdminInfo = dbManager.RunMSelectQuery(cmd);

        if (!IsPostBack)
        {
            string menu_id = Request.QueryString["mid"];
            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            if (permission <= 1)
            {
                btnSave.Visible = false;
            }

            if (dsAdminInfo.Tables.Count > 0 && dsAdminInfo.Tables[0].Rows.Count > 0)
            {
                BindInfo(dsAdminInfo);
            }
        }


        LoginImg = dsAdminInfo.Tables[0].Rows[0]["login_url"].ToString();
    }

    void BindInfo(DataSet dsAdminInfo)
    {
        if (dsAdminInfo == null) return;
        if (dsAdminInfo.Tables.Count == 0) return;
        if (dsAdminInfo.Tables[0].Rows.Count == 0) return;
        tbxShowLogin.Checked = Int64.Parse(dsAdminInfo.Tables[0].Rows[0]["login_show"].ToString()) == 1 ? true : false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sitePath = HttpContext.Current.Request.PhysicalApplicationPath;
            string middlePath = @"\Images\Login\";
            string middleUrl = @"/Images/Login/";
            string host = "http://localhost:" + HttpContext.Current.Request.Url.Port;
            string fileName = "";
            int nDuplIpBlock = tbxShowLogin.Checked ? 1 : 0;
            if (tbxImageFile.HasFile)
            {
                fileName = tbxImageFile.FileName;
                string savePath = sitePath + middlePath + fileName;
                tbxImageFile.SaveAs(savePath);

                string strQuery = "UPDATE TBL_AdminInfo SET ";
                strQuery += "login_url=@login_url,login_show=@login_show, changedate=getDate(); ";
                DBManager dbManager = new DBManager();
                SqlCommand cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add(new SqlParameter("@login_url", host + middleUrl + fileName));
                cmd.Parameters.Add(new SqlParameter("@login_show", nDuplIpBlock));
                dbManager.RunMQuery(cmd);
                cvResult2.ErrorMessage = "로그인공지가 변경되였습니다.";
                LoginImg = host + middleUrl + fileName;
            }else
            {
                string strQuery = "UPDATE TBL_AdminInfo SET ";
                strQuery += "login_show=@login_show, changedate=getDate(); ";
                DBManager dbManager = new DBManager();
                SqlCommand cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add(new SqlParameter("@login_show", nDuplIpBlock));
                dbManager.RunMQuery(cmd);
                cvResult2.ErrorMessage = "로그인공지가 변경되였습니다.";
            }
        }
        catch (Exception ex)
        {
            cvResult2.ErrorMessage = "로그인공지변경에 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult2.IsValid = false;
    }
}
