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

public partial class 게임관리_환경설정 : PageBase
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
            BindInfo();

            string menu_id = Request.QueryString["mid"];
            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            if (permission <= 1)
            {
                //btnNew.Visible = false;
                //grdList.Columns[grdList.Columns.Count - 1].Visible = false;
                //grdList.Columns[grdList.Columns.Count - 2].Visible = false;
                //grdList.Columns[grdList.Columns.Count - 3].Visible = false;
                btnSave.Visible = false;
                btnChangePassword.Visible = false;
            }
        }
    }

    void BindInfo()
    {
        if (인증회원.Tables[0].Rows[0]["class"].ToString() == ROLES_BONSA)
        {
            pnlInfo.Visible = true;
            tbxBankname.Text = 인증회원.Tables[0].Rows[0]["bankname"].ToString();
            tbxBanknum.Text = 인증회원.Tables[0].Rows[0]["banknum"].ToString();
            tbxName.Text = 인증회원.Tables[0].Rows[0]["mastername"].ToString();
        }
        else
        {
            pnlInfo.Visible = false;
        }
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        try
        {
            string strPassword = tbxPassword.Text.Trim();
            string strQuery = "UPDATE TBL_Enterprise SET ";
            strQuery += "loginpwd=@loginpwd WHERE id=@id";
            DBManager dbManager = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@loginpwd", strPassword));
            cmd.Parameters.Add(new SqlParameter("@id", 인증회원번호));
            dbManager.RunMQuery(cmd);
            cvResult1.ErrorMessage = "관리자정보가 변경되였습니다.";
        }
        catch(Exception ex)
        {
            cvResult1.ErrorMessage = "관리자정보변경에 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult1.IsValid = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strBankName = tbxBankname.Text.Trim();
            string strBankNum = tbxBanknum.Text.Trim();
            string strName = tbxName.Text.Trim();
            string strQuery = "UPDATE TBL_Enterprise SET ";
            strQuery += "bankName=@bankName, banknum=@banknum, mastername=@name WHERE id=@id";
            DBManager dbManager = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@bankName", strBankName));
            cmd.Parameters.Add(new SqlParameter("@banknum", strBankNum));
            cmd.Parameters.Add(new SqlParameter("@name", strName));
            cmd.Parameters.Add(new SqlParameter("@id", 인증회원번호));
            dbManager.RunMQuery(cmd);
            cvResult2.ErrorMessage = "관리자정보가 변경되였습니다.";

            인증회원.Tables[0].Rows[0]["bankname"] = strBankName;
            인증회원.Tables[0].Rows[0]["banknum"] = strBankNum;
            인증회원.Tables[0].Rows[0]["mastername"] = strName;
        }
        catch (Exception ex)
        {
            cvResult2.ErrorMessage = "관리자정보변경에 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult2.IsValid = false;
    }
}
