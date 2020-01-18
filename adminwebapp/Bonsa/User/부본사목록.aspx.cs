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

public partial class 회원관리_부본사목록 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:부본사관리:부본사목록:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:부본사관리:부본사목록:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:부본사관리:부본사목록:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:부본사관리:부본사목록:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:부본사관리:부본사목록:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:부본사관리:부본사목록:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:부본사관리:부본사목록:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:부본사관리:부본사목록:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:부본사관리:부본사목록:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:부본사관리:부본사목록:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = " use_yn='1' ";
            정돈항 = "ID";
            정돈방향 = SortDirection.Descending;

            string strParentID = Request.QueryString["parentid"];
            try
            {
                선택정보번호 = int.Parse(strParentID);
            }
            catch
            {
                선택정보번호 = 인증회원번호;
            }
            BindDataSource();

            string menu_id = Request.QueryString["mid"];
            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            if (permission <= 1)
            {
                btnNew.Visible = false;
                grdList.Columns[grdList.Columns.Count - 4].Visible = false;
                //grdList.Columns[grdList.Columns.Count - 2].Visible = false;
                //grdList.Columns[grdList.Columns.Count - 3].Visible = false;
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
        string strQuery;
        if (GetClass() >= 4)
            strQuery = "SELECT *, "
                + "(select count(*) from tbl_enterprise where use_yn=1 and class=2 and (bubonsaid=ent.id or chongpanid=ent.id ) ) as cp_count, "
                + "(select count(*) from tbl_enterprise where use_yn=1 and class=1 and (bubonsaid=ent.id or chongpanid=ent.id )) as mj_count "
                + " FROM TBL_Enterprise ent WHERE Class=3 and use_yn=1";
        else
            strQuery = "SELECT *, "
                + "(select count(*) from tbl_enterprise where use_yn=1 and class=2 and (bubonsaid=ent.id or chongpanid=ent.id )) as cp_count, "
                + "(select count(*) from tbl_enterprise where use_yn=1 and class=1 and (bubonsaid=ent.id or chongpanid=ent.id )) as mj_count "
                + " FROM TBL_Enterprise WHERE use_yn=1 and Class=3 and id=" + 인증회원번호;
        //strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID)";
        SqlCommand cmd = new SqlCommand(strQuery);
        //cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        DataSet dsPartner = dbManager.RunMSelectQuery(cmd);
        검색정보들 = dsPartner;
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
        grdList.PageIndex = 현재페지위치;
        grdList.DataBind();

        lblSortExpr.Text = "《" + 정돈항 + "》";
        pnlDescSortLbl.Visible = 정돈방향 == SortDirection.Descending;
        pnlAscSortLbl.Visible = 정돈방향 == SortDirection.Ascending;

        //grdList.Columns[13].Visible = false;
        //grdList.Columns[12].Visible = false;
        //grdList.Columns[11].Visible = false;

        if (GetClass() < 4)
        {
            grdList.Columns[10].Visible = false;
            ddlBillKind.Visible = false;
            lblBillKind.Visible = false;
        }
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
        Response.Redirect("부본사등록.aspx");
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        BindDataSource();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        string use_yn = ddlUseYn.SelectedValue;
        string bill_kind = ddlBillKind.SelectedValue;
        string entprise_name = tbxName.Text.Trim();

        if (string.IsNullOrEmpty(use_yn) == false)
            검색질문식 += " use_yn='" + use_yn + "'";

        if (string.IsNullOrEmpty(bill_kind) == false)
            검색질문식 += " and bill_kind='" + bill_kind + "'";

        if(string.IsNullOrEmpty(entprise_name) == false)
            검색질문식 += " and name  like '%" + entprise_name + "%'";

        BindDataSource();

        //lblQuery.Text = 검색질문식;
        GridDataBind();
    }
}
