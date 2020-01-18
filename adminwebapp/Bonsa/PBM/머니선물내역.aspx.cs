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

public partial class 올인관리_머니선물내역 : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //this.Master.BindMenu();
            //for (int i = 0; i < this.Master._SubMenu.Items.Count; i++)
            //{
            //    this.Master._SubMenu.Items[i].Selected = (this.Master._SubMenu.Items[i].Value == "공지사항");
            //}
        }

        KEY_AUTOREFRESH = "CACHE:MTOURNAMENT:올인관리:올인선물이력:자동갱신:";
        KEY_CACHEINFOS = "CACHE:MTOURNAMENT:올인관리:올인선물이력:검색정보들:";
        KEY_CACHEQUERY = "CACHE:MTOURNAMENT:올인관리:올인선물이력:검색질문식:";
        KEY_CACHESELINF = "CACHE:MTOURNAMENT:올인관리:올인선물이력:선택된정보:";
        KEY_CACHESORT = "CACHE:MTOURNAMENT:올인관리:올인선물이력:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:MTOURNAMENT:올인관리:올인선물이력:정돈방향:";
        KEY_CACHEFILTER = "CACHE:MTOURNAMENT:올인관리:올인선물이력:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:MTOURNAMENT:올인관리:올인선물이력:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:MTOURNAMENT:올인관리:올인선물이력:첫날자:";
        KEY_CACHELASTDATE = "CACHE:MTOURNAMENT:올인관리:올인선물이력:끝날자:";

        if (!IsPostBack)
        {
            string strID = Request.QueryString["id"];
            try { int.Parse(strID); }
            catch { strID = "0"; }

            검색질문식 = "";
            정돈항 = "ID";
            정돈방향 = SortDirection.Descending;

            선택정보번호 = int.Parse(strID);
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
        string strQuery = "SELECT * FROM TBL_GIFTHISTORY ";
        strQuery += "WHERE TBL_GIFTHISTORY.createdDate >= @StartDate AND TBL_GIFTHISTORY.createdDate <= @EndDate";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        DataSet dsPokerRoom = dbManager.RunAllinSelectQuery(cmd);
        검색정보들 = dsPokerRoom;
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

        검색정보들 = 검색정보얻기(검색질문식);
        DataSet src = 검색정보들;
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

    void UpdateButton()
    {
        tbxStartDate.Text = 첫날자.ToString("yyyy-MM-dd");
        tbxEndDate.Text = 끝날자.ToString("yyyy-MM-dd");
    }

    #region 그리드사건처리부
    protected void grdList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header ||
            e.Row.RowType == DataControlRowType.Footer ||
            e.Row.RowType == DataControlRowType.Pager ||
            e.Row.RowType == DataControlRowType.Separator)
            return;

        LinkButton lnkNo = (LinkButton)e.Row.FindControl("lnkNo");
        if (lnkNo != null)
        {
            lnkNo.Text = (grdList.PageIndex * grdList.PageSize + e.Row.RowIndex + 1).ToString();
        }

    }
    protected void grdList_Sorting(object sender, GridViewSortEventArgs e)
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
    protected void grdList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        현재페지위치 = e.NewPageIndex;
    }
    protected void grdList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int rowIndex = e.RowIndex;
        LinkButton lnkNo = (LinkButton)grdList.Rows[e.RowIndex].FindControl("lnkNo");
        if (lnkNo == null)
        {
            return;
        }

        string strId = lnkNo.CommandArgument;
        string strRoomID = lnkNo.ToolTip;
        if (strId == "") return;
        // 삭제처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_GIFTHISTORY WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        dbManager.RunAllinQuery(cmd);

        BindDataSource();
    }
    #endregion

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        tbxSearchValue.Text = "";
        BindDataSource();
    }
    protected void btnAllDelete_Click(object sender, EventArgs e)
    {
        if (검색정보들 != null && 검색정보들.Tables.Count > 0)
        {
            DBManager dbManager = new DBManager();
            for (int i = 0; i < 검색정보들.Tables[0].Rows.Count; i++)
            {
                string strQuery = "DELETE TBL_GameHist WHERE ID=" + 검색정보들.Tables[0].Rows[i]["ID"].ToString();
                SqlCommand cmd = new SqlCommand(strQuery);
                int nResult = dbManager.RunAllinQuery(cmd);
            }
            BindDataSource();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strSearchValue = tbxSearchValue.Text;
        if (ddlSearchKey.SelectedIndex == 0) // 닉네임검색
        {
            if (strSearchValue != "")
            {
                string strQuery = "sentUserName LIKE '%" + strSearchValue + "%' OR ";
                strQuery += "receiveUserName LIKE '%" + strSearchValue + "%' ";
                검색질문식 = strQuery;
            }
        }
        if (tbxStartDate.Text != "" && tbxEndDate.Text != "")
        {
            첫날자 = DateTime.Parse(tbxStartDate.Text + " 00:00:00");
            끝날자 = DateTime.Parse(tbxEndDate.Text + " 23:59:59");
            BindDataSource();
        }
    }
}
