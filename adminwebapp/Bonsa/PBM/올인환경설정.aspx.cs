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

public partial class 게임관리_올인환경설정 : PageBase
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
            string strQuery = "SELECT * FROM TBL_SETTINGINFO";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsSettingInfo = dbManager.RunAllinSelectQuery(cmd);
            if (dsSettingInfo.Tables.Count > 0 && dsSettingInfo.Tables[0].Rows.Count > 0)
            {
                BindInfo(dsSettingInfo);
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

    void BindInfo(DataSet dsSettingInfo)
    {
        if (dsSettingInfo == null) return;
        if (dsSettingInfo.Tables.Count == 0) return;
        if (dsSettingInfo.Tables[0].Rows.Count == 0) return;

        tbxMngSubtract.Text = dsSettingInfo.Tables[0].Rows[0]["mngsubtract"].ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            double nMngSubtract = double.Parse(tbxMngSubtract.Text);
       
            string strQuery = "UPDATE TBL_SETTINGINFO SET ";
            strQuery += "mngsubtract=@mngsubtract, ";
            strQuery += "changedate=getDate() ";
            DBManager dbManager = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@mngsubtract", nMngSubtract));
            dbManager.RunAllinQuery(cmd);
            cvResult2.ErrorMessage = "환경설정정보가 변경되였습니다.";
        }
        catch (Exception ex)
        {
            cvResult2.ErrorMessage = "환경설정정보변경에 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult2.IsValid = false;
    }
}
