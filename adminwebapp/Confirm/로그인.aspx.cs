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
using System.Text.RegularExpressions;

public partial class 확인등록_로그인 : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        string strLoginID = Login1.UserName.Trim();
        string strPWD = Login1.Password.Trim();

        DBManager dbManager = new DBManager();
        DataSet dsUser = new DataSet();

        string loginType = LoginType.SelectedValue;
        string strQuery;
        
        if (loginType.Equals("ENT"))
        {
            strQuery = "SELECT *, loginid as ext_id, name as ext_name, 'ENT' as user_type FROM TBL_Enterprise WHERE LoginID=@LoginID AND LoginPWD=@LoginPWD and use_yn='1'";
        }
        else
        {
            strQuery = "select ent.*, emp.loginid as ext_id, emp.name as ext_name, 'EMP' as user_type "
                    + " from tbl_enterprise ent,TBL_employee emp "
                    + " where ent.id=emp.enterprise_id and emp.loginid=@LoginID and emp.password=@LoginPWD  and ent.use_yn='1'";
        }

        //Response.Write(strQuery); Response.End();
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        
        sqlQuery.Parameters.Add("@LoginID", SqlDbType.NVarChar);
        sqlQuery.Parameters.Add("@LoginPWD", SqlDbType.NVarChar);
        sqlQuery.Parameters["@LoginID"].Value = strLoginID;
        sqlQuery.Parameters["@LoginPWD"].Value = strPWD;
        
        //sqlQuery.Parameters.Add(new SqlParameter("@LoginID"
        dsUser = dbManager.RunMSelectQuery(sqlQuery);

        if (dsUser.Tables.Count == 0)
        {
            return;
        }
        if (dsUser.Tables[0].Rows.Count == 0)
        {
            return;
        }

       
        //dsUser.Tables[0].Rows["
        
        인증회원 = dsUser;
        
        // 사원 로그인이면 로그인 아이디를 다시 회사 로그인 아이디로 바꾼다
        if(loginType.Equals("EMP"))
            strLoginID = Convert.ToString(인증회원.Tables[0].Rows[0]["loginid"]);

        FormsAuthentication.SignOut();
        FormsAuthentication.SetAuthCookie(strLoginID, false);        
       
        int nClass = int.Parse(인증회원.Tables[0].Rows[0]["Class"].ToString());

        if (nClass.ToString() == ROLES_SUPERBONSA)
        {           
            Response.Redirect("../FirstPage.aspx");
        }
        else
        {
            Response.Redirect("../HeadOffice.aspx");
        }
    }
}
