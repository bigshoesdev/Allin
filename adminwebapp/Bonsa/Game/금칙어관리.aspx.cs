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

public partial class 게임관리_금칙어관리 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:슈퍼본사:게임관리:금칙어관리:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:슈퍼본사:게임관리:금칙어관리:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:슈퍼본사:게임관리:금칙어관리:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:슈퍼본사:게임관리:금칙어관리:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:슈퍼본사:게임관리:금칙어관리:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:슈퍼본사:게임관리:금칙어관리:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:슈퍼본사:게임관리:금칙어관리:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:슈퍼본사:게임관리:금칙어관리:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:슈퍼본사:게임관리:금칙어관리:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:슈퍼본사:게임관리:금칙어관리:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";
            정돈항 = "ID";
            정돈방향 = SortDirection.Descending;

            BindDataSource();

            string menu_id = Request.QueryString["mid"];
            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            if (permission <= 1)
            {
                btnAdd.Visible = false;
                btnAllDelete.Visible = false;
            }
        }
    }

    void BindDataSource()
    {
        if (검색정보들 != null)
        {
            검색정보들.Dispose();
            검색정보들 = null;
        }

        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_BLOCKWORD";
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsBlockIP = dbManager.RunMSelectQuery(cmd);
        검색정보들 = dsBlockIP;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        GridDataBind();
    }

    void GridDataBind()
    {
        if (검색정보들 == null)
        {
            BindDataSource();
        }
        DataSet src = 검색정보얻기(검색질문식);
        if (src != null)
        {
            DataView dv = src.Tables[0].DefaultView;
            string strTmpSort = (정돈방향 == SortDirection.Ascending) ? 정돈항 : 정돈항 + " DESC";
            dv.Sort = strTmpSort;

            grdList.DataSource = dv;
            lblRowCount.Text = src.Tables[0].Rows.Count.ToString();
        }
        else
        {
            lblRowCount.Text = "0";
            grdList.DataSource = null;
        }
        grdList.PageSize = 페지당현시개수;
        grdList.PageIndex = 현재페지위치;
        grdList.DataBind();
    }

    protected void grdLisTBL_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header ||
            e.Row.RowType == DataControlRowType.Footer ||
            e.Row.RowType == DataControlRowType.Pager ||
            e.Row.RowType == DataControlRowType.Separator)
            return;

        LinkButton lnkNo = (LinkButton)e.Row.FindControl("lnkNo");
        if (lnkNo != null)
        {
            // e.Row.Attributes["onclick"] = ClientScript.GetPostBackEventReference(lnkNo, "");
            e.Row.Attributes["onmouseover"] = "javascript:prevBGColor=this.style.backgroundColor; this.style.backgroundColor='#D1DDF1';this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "javascript:this.style.backgroundColor=prevBGColor;this.style.cursor='default';";
            e.Row.Attributes["mouseover"] = "cursor:hand";

            lnkNo.Text = (grdList.PageIndex * grdList.PageSize + e.Row.RowIndex + 1).ToString();
        }

    }
    protected void grdLisTBL_Sorting(object sender, GridViewSortEventArgs e)
    {
        // 요청되는 정돈항이 이전과 같으면 정돈방향을 바꾼다
        if (e.SortExpression == 정돈항)
        {
            정돈방향 = (정돈방향 == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        }
        else // 그렇지 안으면
        {
            // 정돈항에 새 정돈항을 넣어주고 정돈방향은 Descending 으로 설정한다.
            정돈항 = e.SortExpression;
            정돈방향 = SortDirection.Descending;
        }
    }
    protected void grdLisTBL_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        현재페지위치 = e.NewPageIndex;
    }
    protected void grdLisTBL_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int rowIndex = e.RowIndex;
        LinkButton lnkNo = (LinkButton)grdList.Rows[e.RowIndex].FindControl("lnkNo");
        if (lnkNo == null)
        {
            cvResult.ErrorMessage = "삭제하려는 자료를 찾을수 없습니다.";
            cvResult.IsValid = false;
            return;
        }

        string strId = lnkNo.CommandArgument;
        if (strId == "") return;
        // 삭제처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_BLOCKWORD WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        int nResult = dbManager.RunMQuery(cmd);
        BindDataSource();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        BindDataSource();
    }
    protected void btnAllDelete_Click(object sender, EventArgs e)
    {
        if (검색정보들 != null && 검색정보들.Tables.Count > 0)
        {
            DBManager dbManager = new DBManager();
            System.Collections.Generic.List<string> strRoomIDList = new System.Collections.Generic.List<string>();
            for (int i = 0; i < 검색정보들.Tables[0].Rows.Count; i++)
            {
                string strQuery = "DELETE TBL_BLOCKWORD WHERE ID=" + 검색정보들.Tables[0].Rows[i]["ID"].ToString();
                SqlCommand cmd = new SqlCommand(strQuery);
                int nResult = dbManager.RunMQuery(cmd);
            }
            BindDataSource();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string strWord = tbxWord.Text.Trim();

        DBManager dbManager = new DBManager();
        string strQuery = "INSERT INTO TBL_BLOCKWORD(word) VALUES(@word)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@word", strWord));
        int nResult = dbManager.RunMQuery(cmd);
        BindDataSource();
    }
}
