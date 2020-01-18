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

public partial class 게임관리_베팅내역 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:업체페지:게임관리:베팅내역:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:업체페지:게임관리:베팅내역:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:업체페지:게임관리:베팅내역:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:업체페지:게임관리:베팅내역:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:업체페지:게임관리:베팅내역:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:업체페지:게임관리:베팅내역:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:업체페지:게임관리:베팅내역:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:업체페지:게임관리:베팅내역:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:업체페지:게임관리:베팅내역:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:업체페지:게임관리:베팅내역:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";
            정돈항 = "userid";
            정돈방향 = SortDirection.Descending;

            BindDataSource();
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
        string strQuery = "SELECT userid, nickname, bonsa, bubonsa, chongpan, maejang, sum(betmoney) as betmoney, regdate FROM tbl_bethist ";
        strQuery += " WHERE RegDate >= @StartDate AND RegDate <= @EndDate ";
        if (GetClass() < 4)
            strQuery += " AND BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID ";
        strQuery += " Group by userid, nickname, bonsa, bubonsa, chongpan, maejang, regdate";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        if (GetClass() < 4)
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        DataSet dsChargeList = dbManager.RunMSelectQuery(cmd);
        검색정보들 = dsChargeList;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        UpdateButton();
        GridDataBind();
    }

    void GridDataBind()
    {
        if (검색정보들 == null)
        {
            BindDataSource();
        }

        long moneySum = 0;

        DataSet src = 검색정보얻기(검색질문식);
        if (src != null && (src.Tables[0] != null) && (src.Tables[0].Rows != null) && src.Tables[0].Rows.Count > 0)
        {
            DataView dv = src.Tables[0].DefaultView;
            string strTmpSort = (정돈방향 == SortDirection.Ascending) ? 정돈항 : 정돈항 + " DESC";
            dv.Sort = strTmpSort;

            grdList.DataSource = dv;
            lblRowCount.Text = src.Tables[0].Rows.Count.ToString();

            for (int i = 0; i < src.Tables[0].Rows.Count; i++)
            {
                moneySum += Convert.ToInt64(src.Tables[0].Rows[i]["betmoney"]);
            }
        }
        else
        {
            lblRowCount.Text = "0";
            grdList.DataSource = null;
        }
        grdList.PageSize = 페지당현시개수;
        grdList.PageIndex = 현재페지위치;
        grdList.DataBind();

        if (src != null && (src.Tables[0] != null) && (src.Tables[0].Rows != null) && src.Tables[0].Rows.Count > 0)
        {
            grdList.FooterRow.Cells[0].Text = "합계";
            grdList.FooterRow.Cells[7].Text = moneySum.ToString("N0");
        }
    }
    void UpdateButton()
    {
        tbxStartDate.Text = 첫날자.ToString("yyyy-MM-dd");
        tbxEndDate.Text = 끝날자.ToString("yyyy-MM-dd");
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
        string strQuery = "DELETE TBL_Charge WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        int nResult = dbManager.RunMQuery(cmd);
        BindDataSource();

    }
    #endregion

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strSearchKey = ddlSearchKey.SelectedValue;
        string strSearchValue = tbxSearchValue.Text;

        검색질문식 = strSearchKey + " LIKE '%" + strSearchValue + "%'";

        if (tbxStartDate.Text != "" && tbxEndDate.Text != "")
        {
            첫날자 = DateTime.Parse(tbxStartDate.Text + " 00:00:00");
            끝날자 = DateTime.Parse(tbxEndDate.Text + " 23:59:59");
            BindDataSource();
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        tbxSearchValue.Text = "";
        ddlSearchKey.SelectedIndex = 0;
        BindDataSource();
    }

    protected void btnExcel_OnClick(object sender, EventArgs e)
    {
        ExportExcel(grdList, "베팅내역");
    }
}
