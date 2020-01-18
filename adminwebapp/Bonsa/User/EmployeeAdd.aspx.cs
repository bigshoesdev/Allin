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

public partial class Bonsa_User_EmployeeAdd : PageBase
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
            lblRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            string strID = Request.QueryString["id"];
            try { int.Parse(strID); }
            catch { strID = "0"; }
            lblID.Text = strID;
            //tbxName.ReadOnly = false;
            //fntChange.Visible = false;
           
            DBManager dbManager = new DBManager();
            string strQuery = "SELECT * FROM TBL_Employee WHERE ID=" + strID;
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsEmployee = dbManager.RunMSelectQuery(cmd);
            if (dsEmployee.Tables.Count == 0)
            {
                // Response.Redirect("공지사항목록.aspx");
                return;
            }

            if (dsEmployee.Tables[0].Rows.Count == 0)
            {
                // Response.Redirect("공지사항목록.aspx");
                return;
            }
            BindInfo(dsEmployee);

            if (GetClass() < 4)
            {
                ddlAllowChat.SelectedIndex = 1;
                ddlAllowChat.Visible = false;
            }

            
            
        }
    }

    private void BindInfo(DataSet ds)
    {
        DataRow row = ds.Tables[0].Rows[0];
        tbxLoginID.Text = row["loginid"].ToString();
        tbxLoginID.ReadOnly = true;
        tbxPWD.Text = row["password"].ToString();
        tbxName.Text = row["name"].ToString();
        tbxTelNum.Text = row["tel_num"].ToString();
        ddlNologin.Text = row["is_used"].ToString();
        if (Convert.ToInt16(row["allow_chat"]) == 1)
            ddlAllowChat.SelectedIndex = 0;
        else
            ddlAllowChat.SelectedIndex = 1;
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeList.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DBManager dbManager = new DBManager();
        string strId = lblID.Text;
        string strLoginID = tbxLoginID.Text.Trim();
        string strPassword = tbxPWD.Text.Trim();
        string strName = tbxName.Text.Trim();
        string strTel = tbxTelNum.Text;
        string strAllowChat = ddlAllowChat.SelectedValue;
        
        int nNoLogin = int.Parse(ddlNologin.SelectedValue);
        
        if (CheckExistID(strLoginID, strId))
        {
            cvResult.ErrorMessage = "이미 등록된 아이디입니다.";
            cvResult.IsValid = false;
            return;
        }

        if (strId.Equals("0") == false)
        {
            updateInfo();
            return;
        }
        int nClass = int.Parse(인증회원.Tables[0].Rows[0]["Class"].ToString());

        // 본사직원 아니면 채팅금지
        if (GetClass() < 4)
            strAllowChat = "0";

        string strQuery = "INSERT INTO TBL_Employee(loginid, password, name, tel_num, is_used, enterprise_id, allow_chat) ";
        strQuery += "VALUES(@loginid, @password, @name, @tel_num, @is_used, @enterprise_id, @allow_chat)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@loginid", strLoginID));
        cmd.Parameters.Add(new SqlParameter("@password", strPassword));
        cmd.Parameters.Add(new SqlParameter("@name", strName));
        cmd.Parameters.Add(new SqlParameter("@tel_num", strTel));
        cmd.Parameters.Add(new SqlParameter("@is_used", nNoLogin));
        cmd.Parameters.Add(new SqlParameter("@enterprise_id", int.Parse(인증회원.Tables[0].Rows[0]["id"].ToString())));
        cmd.Parameters.Add(new SqlParameter("@allow_chat", strAllowChat));
     
        try
        {
            dbManager.RunMQuery(cmd);
            strQuery = "SELECT TOP 1 id FROM TBL_Employee  ORDER BY reg_time desc";
            cmd = new SqlCommand(strQuery);
            DataSet dsSet = dbManager.RunMSelectQuery(cmd);

            if (dsSet.Tables.Count == 0 || dsSet.Tables[0].Rows.Count == 0)
                return; 

            string employeeID = dsSet.Tables[0].Rows[0]["id"].ToString();

            strQuery = "SELECT M.* FROM TBL_Menu M WHERE M.allow_class<='" + GetClass() + "' and M.id <> 16 order by M.group_id, M.depth1 desc, M.id";
            cmd = new SqlCommand(strQuery);
            dsSet = dbManager.RunMSelectQuery(cmd);

            if (dsSet.Tables.Count == 0 || dsSet.Tables[0].Rows.Count == 0)
                return;

            for(int i = 0; i < dsSet.Tables[0].Rows.Count; i++)
            {
                string menuID = dsSet.Tables[0].Rows[i]["id"].ToString();
                string depth1 = dsSet.Tables[0].Rows[i]["depth1"].ToString();

                if (depth1 == "")
                {
                    strQuery = "INSERT INTO TBL_EmpPermission (menu_id, emp_id, permission) values (" + menuID + "," + employeeID + "," + "2" + ")";
                    cmd = new SqlCommand(strQuery);
                    dbManager.RunMQuery(cmd);
                }
                else
                {
                    strQuery = "INSERT INTO TBL_EmpPermission (menu_id, emp_id, permission) values (" + menuID + "," + employeeID + "," + "0" + ")";
                    cmd = new SqlCommand(strQuery);
                    dbManager.RunMQuery(cmd);
                }
            }

            cvResult.ErrorMessage = "회원정보가 등록되였습니다.";
        }
        catch (Exception ex)
        {
            cvResult.ErrorMessage = "회원정보등록에서 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult.IsValid = false;
    }

    private bool CheckExistID(string strLoginID, string strID)
    {
        DBManager dbManager = new DBManager();

        // 회원 아이디만 같고 로그인시 업체구분안됨 AND enterprise_id=" + 인증회원.Tables[0].Rows[0]["id"].ToString();
        string strQuery = "SELECT * FROM V_ENT_EMP_USERS WHERE LoginID=@LoginID "; 
        if (strID.Equals("0") == false)
        {
            strQuery += " AND id <> " + strID;
        }
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        sqlQuery.Parameters.Add("@LoginID", SqlDbType.NVarChar);
        sqlQuery.Parameters["@LoginID"].Value = strLoginID;
        DataSet dsUser = dbManager.RunMSelectQuery(sqlQuery);

        bool bExist = false;
        if (dsUser.Tables.Count == 0)
        {
            bExist = false;
        }
        else if (dsUser.Tables[0].Rows.Count == 0)
        {
            bExist = false;
        }
        else if (dsUser.Tables[0].Rows[0]["LoginID"].ToString() == strLoginID)
        {
            bExist = true;
        }
        return bExist;
    }

    private void updateInfo()
    {
        DBManager dbManager = new DBManager();
        string strId = lblID.Text;
        string strLoginID = tbxLoginID.Text.Trim();
        string strPassword = tbxPWD.Text.Trim();
        string strName = tbxName.Text.Trim();
        string strTel = tbxTelNum.Text;
        string strAllowChat = ddlAllowChat.SelectedValue;

        int nNoLogin = int.Parse(ddlNologin.SelectedValue);
        int nClass = int.Parse(인증회원.Tables[0].Rows[0]["Class"].ToString());

        string strQuery = "update TBL_Employee set password=@password, name=@name, tel_num=@tel_num, is_used=@is_used, allow_chat=@allow_chat WHERE id=" + strId;
        
        SqlCommand cmd = new SqlCommand(strQuery);
        
        cmd.Parameters.Add(new SqlParameter("@password", strPassword));
        cmd.Parameters.Add(new SqlParameter("@name", strName));
        cmd.Parameters.Add(new SqlParameter("@tel_num", strTel));
        cmd.Parameters.Add(new SqlParameter("@is_used", nNoLogin));
        cmd.Parameters.Add(new SqlParameter("@allow_chat", strAllowChat));
       
        try
        {
            dbManager.RunMQuery(cmd);
            cvResult.ErrorMessage = "회원정보가 수정 되었습니다." + strId;
        }
        catch (Exception ex)
        {
            cvResult.ErrorMessage = "회원정보수정에서 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult.IsValid = false;
    }


}
