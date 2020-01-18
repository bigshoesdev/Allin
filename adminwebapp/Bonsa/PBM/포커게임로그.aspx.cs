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

public partial class 게임로그조회_포커게임로그 : PageBase
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

        KEY_AUTOREFRESH = "CACHE:MTOURNAMENT:게임로그조회:포커게임로그:자동갱신:";
        KEY_CACHEINFOS = "CACHE:MTOURNAMENT:게임로그조회:포커게임로그:검색정보들:";
        KEY_CACHEQUERY = "CACHE:MTOURNAMENT:게임로그조회:포커게임로그:검색질문식:";
        KEY_CACHESELINF = "CACHE:MTOURNAMENT:게임로그조회:포커게임로그:선택된정보:";
        KEY_CACHESORT = "CACHE:MTOURNAMENT:게임로그조회:포커게임로그:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:MTOURNAMENT:게임로그조회:포커게임로그:정돈방향:";
        KEY_CACHEFILTER = "CACHE:MTOURNAMENT:게임로그조회:포커게임로그:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:MTOURNAMENT:게임로그조회:포커게임로그:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:MTOURNAMENT:게임로그조회:포커게임로그:첫날자:";
        KEY_CACHELASTDATE = "CACHE:MTOURNAMENT:게임로그조회:포커게임로그:끝날자:";

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
        string strQuery = "SELECT * FROM TBL_GameHist JOIN ";
        strQuery += "TBL_CHANNELINFO ON TBL_CHANNELINFO.ID=TBL_GameHist.ChannelID LEFT OUTER JOIN ";
        strQuery += "TBL_ROOMINFO ON TBL_ROOMINFO.ID=TBL_GameHist.RoomID ";
        strQuery += "WHERE TBL_GameHist.RegDate >= @StartDate AND TBL_GameHist.RegDate <= @EndDate";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        DataSet dsPokerRoom = dbManager.RunPokerSelectQuery(cmd);
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

        lblSortExpr.Text = "《" + 정돈항 + "》";
        pnlDescSortLbl.Visible = 정돈방향 == SortDirection.Descending;
        pnlAscSortLbl.Visible = 정돈방향 == SortDirection.Ascending;
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
            // e.Row.Attributes["onclick"] = ClientScript.GetPostBackEventReference(lnkNo, "");
            //e.Row.Attributes["onmouseover"] = "javascript:prevBGColor=this.style.backgroundColor; this.style.backgroundColor='#D1DDF1';this.style.cursor='hand';";
            //e.Row.Attributes["onmouseout"] = "javascript:this.style.backgroundColor=prevBGColor;this.style.cursor='default';";
            //e.Row.Attributes["mouseover"] = "cursor:hand";

            lnkNo.Text = (grdList.PageIndex * grdList.PageSize + e.Row.RowIndex + 1).ToString();
            DataRowView drv = (DataRowView)e.Row.DataItem;
            Table tblUserIDList = (Table)e.Row.FindControl("tblUserIDList");
            Table tblStartMoneyList = (Table)e.Row.FindControl("tblStartMoneyList");
            Table tblEndMoneyList = (Table)e.Row.FindControl("tblEndMoneyList");
            Table tblChangeMoneyList = (Table)e.Row.FindControl("tblChangeMoneyList");
            Table tblResultList = (Table)e.Row.FindControl("tblResultList");
            string[] strForeColors = new string[5] { "#7516A2", "#FF6600", "#3C5F2D", "#FF9C00", "#1658A2" };
            int nEntryCount = (int)drv["EntryCount"];
            for (int i = 0; i < nEntryCount; i++)
            {
                if (tblUserIDList != null)
                {
                    TableRow tr = new TableRow();
                    TableCell tc =  new TableCell();
                    tc.CssClass = "GridItem";
                    tc.Font.Bold = true;
                    tc.ForeColor = System.Drawing.Color.FromName(strForeColors[i % 5]);
                    tc.Text = drv["EntryNickName" + (i + 1).ToString()].ToString() == "" ? "-" : drv["EntryNickName" + (i + 1).ToString()].ToString();
                    tr.Cells.Add(tc);
                    tblUserIDList.Rows.Add(tr);
                }
                if (tblStartMoneyList != null)
                {
                    TableRow tr = new TableRow();
                    TableCell tc = new TableCell();
                    tc.CssClass = "GridItem";
                    tc.Font.Bold = true;
                    tc.ForeColor = System.Drawing.Color.FromName(strForeColors[i%5]);
                    tc.Text = ((long)drv["EntryStartMoney" + (i+1).ToString()]).ToString("N0"); 
                    tr.Cells.Add(tc);
                    tblStartMoneyList.Rows.Add(tr);
                }
                if (tblEndMoneyList != null)
                {
                    TableRow tr = new TableRow();
                    TableCell tc = new TableCell();
                    tc.CssClass = "GridItem";
                    tc.Font.Bold = true;
                    tc.ForeColor = System.Drawing.Color.FromName(strForeColors[i % 5]);
                    tc.Text = ((long)drv["EntryChangeMoney" + (i+1).ToString()]).ToString("N0");
                    tr.Cells.Add(tc);
                    tblEndMoneyList.Rows.Add(tr);
                }
                if (tblChangeMoneyList != null)
                {
                    TableRow tr = new TableRow();
                    TableCell tc = new TableCell();
                    tc.CssClass = "GridItem";
                    tc.Font.Bold = true;
                    tc.ForeColor = System.Drawing.Color.FromName(strForeColors[i % 5]);
                    tc.Text = (((long)drv["EntryChangeMoney" + (i+1).ToString()]) - ((long)drv["EntryStartMoney" + (i+1).ToString()])).ToString("N0");
                    tr.Cells.Add(tc);
                    tblChangeMoneyList.Rows.Add(tr);
                }
                if (tblResultList != null)
                {
                    TableRow tr = new TableRow();
                    TableCell tc = new TableCell();
                    tc.CssClass = "GridItem";
                    tc.Font.Bold = true;
                    tc.ForeColor = System.Drawing.Color.FromName(strForeColors[i % 5]);
                    tc.Text = drv["EntryCard" + (i + 1).ToString()].ToString() == "" ? "-" : drv["EntryCard" + (i + 1).ToString()].ToString();
                    tr.Cells.Add(tc);
                    tblResultList.Rows.Add(tr);
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
        string strRoomID = lnkNo.ToolTip;
        if (strId == "") return;
        // 삭제처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_GameHist WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        dbManager.RunPokerQuery(cmd);

        // 이 로그의 방정보와 련결된 로그가 있는가 검사하고 없으면 방삭제
        strQuery = "SELECT * FROM TBL_GameHist WHERE RoomID=" + strRoomID;
        cmd = new SqlCommand(strQuery);
        DataSet dsLog = dbManager.RunPokerSelectQuery(cmd);
        if (dsLog.Tables.Count > 0 && dsLog.Tables[0].Rows.Count == 0)
        {
            // 방삭제
            strQuery = "DELETE TBL_ROOMINFO WHERE ID=" + strRoomID;
            cmd = new SqlCommand(strQuery);
            dbManager.RunPokerQuery(cmd);
        }
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
            System.Collections.Generic.List<string> strRoomIDList = new System.Collections.Generic.List<string>();
            for (int i = 0; i < 검색정보들.Tables[0].Rows.Count; i++)
            {
                string strQuery = "DELETE TBL_GameHist WHERE ID=" + 검색정보들.Tables[0].Rows[i]["ID"].ToString();
                SqlCommand cmd = new SqlCommand(strQuery);
                int nResult = dbManager.RunPokerQuery(cmd);

                string strRoomID = 검색정보들.Tables[0].Rows[i]["RoomID"].ToString();
                if(strRoomIDList.IndexOf(strRoomID) < 0)
                    strRoomIDList.Add(strRoomID);
            }

            // 이 로그의 방정보와 련결된 로그가 있는가 검사하고 없으면 방삭제
            foreach (string strRoomID in strRoomIDList)
            {
                string strQuery = "SELECT * FROM TBL_GameHist WHERE RoomID=" + strRoomID;
                SqlCommand cmd = new SqlCommand(strQuery);
                DataSet dsLog = dbManager.RunPokerSelectQuery(cmd);
                if (dsLog != null && dsLog.Tables.Count > 0 && dsLog.Tables[0].Rows.Count > 0)
                    continue;

                // 방삭제
                strQuery = "DELETE TBL_ROOMINFO WHERE ID=" + strRoomID;
                cmd = new SqlCommand(strQuery);
                dbManager.RunPokerQuery(cmd);
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
                string strQuery = "EntryNickName1 LIKE '%" + strSearchValue + "%' OR ";
                strQuery += "EntryNickName2 LIKE '%" + strSearchValue + "%' OR ";
                strQuery += "EntryNickName3 LIKE '%" + strSearchValue + "%' OR ";
                strQuery += "EntryNickName4 LIKE '%" + strSearchValue + "%' OR ";
                strQuery += "EntryNickName5 LIKE '%" + strSearchValue + "%' ";
                검색질문식 = strQuery;
            }
        }
        else if (ddlSearchKey.SelectedIndex == 1) // 채널명검색
        {
            검색질문식 = "ChannelName Like '%" + strSearchValue + "%'";
        }
        else if (ddlSearchKey.SelectedIndex == 2) // 방이름검색
        {
            검색질문식 = "RoomName Like '%" + strSearchValue + "%'";
        }

        if (tbxStartDate.Text != "" && tbxEndDate.Text != "")
        {
            첫날자 = DateTime.Parse(tbxStartDate.Text + " 00:00:00");
            끝날자 = DateTime.Parse(tbxEndDate.Text + " 23:59:59");
            BindDataSource();
        }
    }
}
