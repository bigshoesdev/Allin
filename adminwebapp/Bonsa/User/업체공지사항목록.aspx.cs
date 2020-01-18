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

public partial class 회원관리_업체공지사항목록 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:회원관리:업체공지사항목록:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:회원관리:업체공지사항목록:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:회원관리:업체공지사항목록:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:회원관리:업체공지사항목록:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:회원관리:업체공지사항목록:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:회원관리:업체공지사항목록:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:회원관리:업체공지사항목록:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:회원관리:업체공지사항목록:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:회원관리:업체공지사항목록:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:회원관리:업체공지사항목록:끝날자:";

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
                //btnSave.Visible = false;
                btnNew.Visible = false;
                btnAllDelete.Visible = false;
                //btnNew.Visible = false;
                //btnSave.Visible = false;
                //grdList.Columns[grdList.Columns.Count - 1].Visible = false;
                //grdList.Columns[grdList.Columns.Count - 2].Visible = false;
                //grdList.Columns[5].Visible = false;
                //grdList.Columns[6].Visible = false;
            }
        }
    }

    public void BindDataSource()
    {
        if (검색정보들 != null)
        {
            검색정보들.Dispose();
            검색정보들 = null;
        }

        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_AdminBoard WHERE senderid=" + 인증회원번호;
        // strQuery += "WHERE RegDate >= @StartDate AND RegDate <= @EndDate";
        SqlCommand cmd = new SqlCommand(strQuery);
        //cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        //cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        DataSet dsNotice = dbManager.RunMSelectQuery(cmd);
        검색정보들 = dsNotice;
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

        lblSortExpr.Text = "《" + 정돈항 + "》";
        pnlDescSortLbl.Visible = 정돈방향 == SortDirection.Descending;
        pnlAscSortLbl.Visible = 정돈방향 == SortDirection.Ascending;
    }

    #region 그리드사건처리부
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
            return;
        }

        string strId = lnkNo.CommandArgument;
        if (strId == "") return;
        // 삭제처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_AdminBoard WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        int nResult = dbManager.RunMQuery(cmd);
        BindDataSource();

    }
    #endregion

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("업체공지사항수정.aspx?id=0");
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        tbxSearchValue.Text = "";
        ddlSearchKey.SelectedIndex = 0;
        BindDataSource();
    }
    protected void btnAllDelete_Click(object sender, EventArgs e)
    {
        DBManager dbManager = new DBManager();
        if (검색정보들 != null && 검색정보들.Tables.Count > 0)
        {
            for (int i = 0; i < 검색정보들.Tables[0].Rows.Count; i++ )
            {
                string strQuery = "DELETE TBL_AdminBoard WHERE ID=" + 검색정보들.Tables[0].Rows[i]["ID"].ToString();
                SqlCommand cmd = new SqlCommand(strQuery);
                int nResult = dbManager.RunMQuery(cmd);
            }
        }
        BindDataSource();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strSearchKey = ddlSearchKey.SelectedValue;
        string strSearchValue = tbxSearchValue.Text;

        검색질문식 = strSearchKey + " LIKE '%" + strSearchValue + "%'";

        //if (tbxStartDate.Text != "" && tbxEndDate.Text != "")
        //{
        //    첫날자 = DateTime.Parse(tbxStartDate.Text);
        //    끝날자 = DateTime.Parse(tbxEndDate.Text);
        //    BindDataSource();
        //}
    }
}
