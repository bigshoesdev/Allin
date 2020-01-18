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

public partial class Bonsa_User_EmpFuncPermission : PageBase
{
    string emp_id;
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

        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:회원관리:회원목록:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:회원관리:회원목록:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:회원관리:회원목록:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:회원관리:회원목록:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:회원관리:회원목록:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:회원관리:회원목록:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:회원관리:회원목록:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:회원관리:회원목록:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:회원관리:회원목록:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:회원관리:회원목록:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";
            BindDataSource();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        GridDataBind();
    }

    private void BindDataSource()
    {
        emp_id = Request.QueryString["emp_id"].ToString();

        DBManager dbManager = new DBManager();
        string strQuery1 = "SELECT name FROM TBL_Employee WHERE id=" + emp_id;
        SqlCommand cmd1 = new SqlCommand(strQuery1);
        DataSet dsUser1 = dbManager.RunMSelectQuery(cmd1);
        lblName.Text = dsUser1.Tables[0].Rows[0]["name"].ToString();
        hidID.Value = emp_id;

        //DataBound(emp_id);

        SqlCommand cmd = null;
        string strQuery = "SELECT M.*, P.emp_id, P.permission FROM TBL_FuncPermission M Left Join TBL_FuncPermissionSet P "
                        + " ON M.id=P.func_id and P.emp_id=" + emp_id + " "
                        + "  order by M.id";
        //lblName.Text = strQuery;
        cmd = new SqlCommand(strQuery);
        DataSet dsUser = dbManager.RunMSelectQuery(cmd);
        검색정보들 = dsUser;
    }

    void GridDataBind()
    {
        if (검색정보들 == null)
        {
            BindDataSource();
        }


        //DataSet src = 검색정보얻기(검색질문식);

        DataSet src = 검색정보들;
        //lblName.Text = src.Tables[0].Rows.Count.ToString();
        if (src != null)
        {
            DataView dv = src.Tables[0].DefaultView;
            grdList.DataSource = dv;
        }
        else
        {
            grdList.DataSource = null;
        }
        //grdList.PageIndex = 현재페지위치;
        grdList.DataBind();

        //lblName.Text = src.Tables[0].Rows.Count.ToString() + " -- " + grdList.Rows.Count.ToString();
        for (int i = 0; i < src.Tables[0].Rows.Count; i++)
        { 
            string permission = src.Tables[0].Rows[i]["permission"].ToString();

            if (permission.Equals("1"))
                ((CheckBox)grdList.Rows[i].FindControl("CheckBoxEdit")).Checked = true;
            else
                ((CheckBox)grdList.Rows[i].FindControl("CheckBoxEdit")).Checked = false;
        }
    }

    protected void grdLisTBL_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header ||
            e.Row.RowType == DataControlRowType.Footer ||
            e.Row.RowType == DataControlRowType.Separator)
            return;
        
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        lblResultMsg.Text = "";
        string str_emp_id = hidID.Value;
        DBManager dbManager = new DBManager();
        string strQuery1 = "DELETE FROM TBL_FuncPermissionSet WHERE emp_id=" + str_emp_id;
        SqlCommand cmd1 = new SqlCommand(strQuery1);
        dbManager.RunMQuery(cmd1);

        DataTable dataSource = 검색정보들.Tables[0];

        for (int i = 0; i < grdList.Rows.Count; i++)
        {
            int permission = 0;
            if (((CheckBox)grdList.Rows[i].FindControl("CheckBoxEdit")).Checked)
                permission = 1; // can view
            else
                permission = 0;
           
            string func_id = dataSource.Rows[i]["id"].ToString();

            string query = "INSERT INTO TBL_FuncPermissionSet (func_id, emp_id, permission) values (" + func_id + "," + str_emp_id + "," + permission + ")";
            SqlCommand cmd = new SqlCommand(query);
            dbManager.RunMQuery(cmd);
        }


        lblResultMsg.Text = "*권한 수정되었습니다.";
        BindDataSource();
        GridDataBind();

    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeList.aspx?mid=42");
    }
}
