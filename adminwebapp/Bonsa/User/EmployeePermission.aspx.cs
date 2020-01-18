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

public partial class Bonsa_User_EmployeePermission : PageBase
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

        if(!IsPostBack)
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
        // 16 은 관리자 계정설정이다
        string strQuery = "SELECT M.*, P.emp_id, P.permission FROM TBL_Menu M Left Join TBL_EmpPermission P "
                        + " ON M.id=P.menu_id and P.emp_id=" + emp_id + " "
                        + " WHERE M.allow_class<='" + GetClass() + "' and M.id <> 16 order by M.group_id, M.depth1 desc, M.id";
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
            if (Convert.IsDBNull(src.Tables[0].Rows[i]["depth2"]))
            {
                ((CheckBox)grdList.Rows[i].FindControl("CheckBoxEdit")).Visible = false;
                ((CheckBox)grdList.Rows[i].FindControl("CheckBoxview")).Visible = false;
            }

            string permission = src.Tables[0].Rows[i]["permission"].ToString();

            if (permission.Equals("2"))
                ((CheckBox)grdList.Rows[i].FindControl("CheckBoxEdit")).Checked = true;

            if (permission.Equals("1"))
                ((CheckBox)grdList.Rows[i].FindControl("CheckBoxView")).Checked = true;
        }
    }

    protected void grdLisTBL_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header ||
            e.Row.RowType == DataControlRowType.Footer ||
            e.Row.RowType == DataControlRowType.Separator)
            return;
        /*
        LinkButton lnkNo = (LinkButton)e.Row.FindControl("lnkNo");
        if (lnkNo != null)
        {
            // e.Row.Attributes["onclick"] = ClientScript.GetPostBackEventReference(lnkNo, "");
            e.Row.Attributes["onmouseover"] = "javascript:prevBGColor=this.style.backgroundColor; this.style.backgroundColor='#D1DDF1';this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "javascript:this.style.backgroundColor=prevBGColor;this.style.cursor='default';";
            e.Row.Attributes["mouseover"] = "cursor:hand";
            lnkNo.Text = (grdList.PageIndex * grdList.PageSize + e.Row.RowIndex + 1).ToString();
        }
         * */
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        lblResultMsg.Text = "";
        string str_emp_id = hidID.Value;
        DBManager dbManager = new DBManager();
        string strQuery1 = "DELETE FROM TBL_EmpPermission WHERE emp_id=" + str_emp_id ; 
        SqlCommand cmd1 = new SqlCommand(strQuery1);
        dbManager.RunMQuery(cmd1);

        DataTable dataSource = 검색정보들.Tables[0];

        for (int i = 0; i < grdList.Rows.Count; i++)
        {
            int permission = 0;
            if (((CheckBox)grdList.Rows[i].FindControl("CheckBoxView")).Checked)
                permission = 1; // can view

            if (((CheckBox)grdList.Rows[i].FindControl("CheckBoxEdit")).Checked)
                permission = 2; // can write  orverwrite 1

            string menu_id = dataSource.Rows[i]["id"].ToString();

            string query = "INSERT INTO TBL_EmpPermission (menu_id, emp_id, permission) values (" + menu_id + "," + str_emp_id + "," + permission + ")";
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
