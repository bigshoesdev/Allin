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

public partial class 게임관리_맞고방목록 : PageBase
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

        KEY_AUTOREFRESH = "CACHE:MTOURNAMENT:게임관리:맞고방목록:자동갱신:";
        KEY_CACHEINFOS = "CACHE:MTOURNAMENT:게임관리:맞고방목록:검색정보들:";
        KEY_CACHEQUERY = "CACHE:MTOURNAMENT:게임관리:맞고방목록:검색질문식:";
        KEY_CACHESELINF = "CACHE:MTOURNAMENT:게임관리:맞고방목록:선택된정보:";
        KEY_CACHESORT = "CACHE:MTOURNAMENT:게임관리:맞고방목록:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:MTOURNAMENT:게임관리:맞고방목록:정돈방향:";
        KEY_CACHEFILTER = "CACHE:MTOURNAMENT:게임관리:맞고방목록:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:MTOURNAMENT:게임관리:맞고방목록:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:MTOURNAMENT:게임관리:맞고방목록:첫날자:";
        KEY_CACHELASTDATE = "CACHE:MTOURNAMENT:게임관리:맞고방목록:끝날자:";

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
        string strQuery = "SELECT * FROM TBL_ROOMINFO JOIN TBL_CHANNELINFO ON TBL_CHANNELINFO.ID=TBL_ROOMINFO.ChannelID WHERE TBL_ROOMINFO.IsUsed<2 "; //AND TBL_ROOMINFO.ChannelID=" + 선택정보번호;
        // strQuery += "WHERE RegDate >= @StartDate AND RegDate <= @EndDate";
        SqlCommand cmd = new SqlCommand(strQuery);
        //cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        //cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        DataSet dsMatgoRoom = dbManager.RunMatgoSelectQuery(cmd);
        검색정보들 = dsMatgoRoom;
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

        lblSortExpr.Text = "《" + 정돈항 + "》";
        pnlDescSortLbl.Visible = 정돈방향 == SortDirection.Descending;
        pnlAscSortLbl.Visible = 정돈방향 == SortDirection.Ascending;
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
            // e.Row.Attributes["onclick"] = ClientScript.GetPostBackEventReference(lnkNo, "");
            //e.Row.Attributes["onmouseover"] = "javascript:prevBGColor=this.style.backgroundColor; this.style.backgroundColor='#D1DDF1';this.style.cursor='hand';";
            //e.Row.Attributes["onmouseout"] = "javascript:this.style.backgroundColor=prevBGColor;this.style.cursor='default';";
            //e.Row.Attributes["mouseover"] = "cursor:hand";

            lnkNo.Text = (grdList.PageIndex * grdList.PageSize + e.Row.RowIndex + 1).ToString();

            string strRoomID = lnkNo.CommandArgument;
            string strQuery = "SELECT * FROM TBL_ROOMUSER JOIN T_User ON T_User.ID=TBL_ROOMUSER.UserID WHERE TBL_ROOMUSER.RoomID=" + strRoomID + " ORDER BY IsRoomAdmin DESC";
            DBManager dbManager = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsMatgoUser = dbManager.RunMatgoSelectQuery(cmd);
            if (dsMatgoUser != null && dsMatgoUser.Tables.Count > 0)
            {
                Table tblUserIDList = (Table)e.Row.FindControl("tblUserIDList");
                Table tblLoginIPList = (Table)e.Row.FindControl("tblLoginIPList");
                Table tblRegDateList = (Table)e.Row.FindControl("tblRegDateList");
                string[] strForeColors = new string[5] { "#7516A2", "#FF6600", "#3C5F2D", "#FF9C00", "#1658A2" };
                for (int i = 0; i < dsMatgoUser.Tables[0].Rows.Count; i++)
                {
                    if (tblUserIDList != null)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc =  new TableCell();
                        tc.CssClass = "GridItem";
                        tc.Font.Bold = true;
                        tc.ForeColor = System.Drawing.Color.FromName(strForeColors[i % 5]);
                        tc.Text = dsMatgoUser.Tables[0].Rows[i]["LoginID"].ToString();
                        tr.Cells.Add(tc);
                        tblUserIDList.Rows.Add(tr);
                    }
                    if (tblLoginIPList != null)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc = new TableCell();
                        tc.CssClass = "GridItem";
                        tc.Font.Bold = true;
                        tc.ForeColor = System.Drawing.Color.FromName(strForeColors[i%5]);
                        tc.Text = dsMatgoUser.Tables[0].Rows[i]["IPAddr"].ToString();
                        tr.Cells.Add(tc);
                        tblLoginIPList.Rows.Add(tr);
                    }
                    if (tblRegDateList != null)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc = new TableCell();
                        tc.CssClass = "GridItem";
                        tc.Font.Bold = true;
                        tc.ForeColor = System.Drawing.Color.FromName(strForeColors[i % 5]);
                        tc.Text = ((DateTime)dsMatgoUser.Tables[0].Rows[i]["RegDate"]).ToString("HH:mm:ss");
                        tr.Cells.Add(tc);
                        tblRegDateList.Rows.Add(tr);
                    }
                }
            }
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
        if (strId == "") return;
        // 삭제처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_ROOMUSER WHERE RoomID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        dbManager.RunMatgoQuery(cmd);
        strQuery = "DELETE TBL_ROOMINFO WHERE ID=" + strId;
        cmd = new SqlCommand(strQuery);
        dbManager.RunMatgoQuery(cmd);
        BindDataSource();
    }
    #endregion

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        BindDataSource();
    }
    protected void btnAllDelete_Click(object sender, EventArgs e)
    {
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_ROOMUSER WHERE ChannelID=" + 선택정보번호.ToString();
        SqlCommand cmd = new SqlCommand(strQuery);
        int nResult = dbManager.RunMatgoQuery(cmd);
        strQuery = "DELETE TBL_ROOMINFO WHERE channelid=" + 선택정보번호.ToString();
        cmd = new SqlCommand(strQuery);
        nResult = dbManager.RunMatgoQuery(cmd);
        BindDataSource();
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("맞고채널목록.aspx");
    }
}
