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

public partial class 회원관리_매장목록 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:매장관리:매장목록:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:매장관리:매장목록:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:매장관리:매장목록:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:매장관리:매장목록:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:매장관리:매장목록:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:매장관리:매장목록:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:매장관리:매장목록:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:매장관리:매장목록:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:매장관리:매장목록:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:매장관리:매장목록:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";
            정돈항 = "ID";
            정돈방향 = SortDirection.Descending;

            string strParentID = Request.QueryString["parentid"];   
            
            try
            {
                선택정보번호 = int.Parse(strParentID);
                btnNew.Visible = false;
            }
            catch
            {
                선택정보번호 = 인증회원번호;
            }
            BindDataSource();

            BindBubonsa();
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
        string strQuery = "SELECT * FROM TBL_Enterprise WHERE Class=" + ROLES_MAEJANG;
        strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        DataSet dsPartner = dbManager.RunMSelectQuery(cmd);
        검색정보들 = dsPartner;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        GridDataBind();
    }

    protected void BindBubonsa()
    {
        DBManager dm = new DBManager();
        DataSet bbsDS = dm.RunMSelectQuery(new SqlCommand("select * from tbl_enterprise where use_yn=1 and class=3 and parentid=" + 인증회원번호));
        ddlBubonsa.Items.Add(new ListItem("전체", ""));
        foreach (DataRow row in bbsDS.Tables[0].Rows)
        {
            ddlBubonsa.Items.Add(new ListItem(row["name"].ToString(), row["id"].ToString()));
        }
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

        if (GetClass() == 2)
        {
            grdList.Columns[5].Visible = false;
        }
        grdList.PageIndex = 현재페지위치;
        grdList.DataBind();

        lblSortExpr.Text = "《" + 정돈항 + "》";
        pnlDescSortLbl.Visible = 정돈방향 == SortDirection.Descending;
        pnlAscSortLbl.Visible = 정돈방향 == SortDirection.Ascending;
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
            return;
        }

        string strId = lnkNo.CommandArgument;
        if (strId == "") return;
        // 삭제처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_Enterprise WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        int nResult = dbManager.RunMQuery(cmd);

        strQuery = "UPDATE TBL_Enterprise SET ParentID=1, BonsaID=0, BuBonsaID=0, ChongPanID=0, Bonsa='', BuBonsa='', Chongpan='' WHERE ParentID=" + strId;
        dbManager.RunMQuery(new SqlCommand(strQuery));

        strQuery = "UPDATE TBL_UserList SET Partner='', ParentID=1, BonsaID=0, BuBonsaID=0, ChongPanID=0, MaeJangID=0, Bonsa='', BuBonsa='', Chongpan='', MaeJang='' WHERE ParentID=" + strId;
        dbManager.RunMQuery(new SqlCommand(strQuery));

        BindDataSource();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("매장등록.aspx");
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        BindDataSource();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        검색질문식 = " 1=1 ";

        if (string.IsNullOrEmpty(tbxChongpanName.Text.Trim()) == false)
        {
            검색질문식 += " and name like '%" + tbxChongpanName.Text.Trim() + "%'";
        }
        if (ddlBubonsa.SelectedIndex > 0)
        {
            검색질문식 += " and bubonsaid=" + ddlBubonsa.SelectedValue;
        }
        BindDataSource();
    }
}
