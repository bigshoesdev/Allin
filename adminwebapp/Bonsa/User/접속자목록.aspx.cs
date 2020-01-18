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

public partial class 회원관리_접속자목록 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:회원관리:접속자목록:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:회원관리:접속자목록:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:회원관리:접속자목록:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:회원관리:접속자목록:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:회원관리:접속자목록:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:회원관리:접속자목록:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:회원관리:접속자목록:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:회원관리:접속자목록:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:회원관리:접속자목록:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:회원관리:접속자목록:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";
            정돈항 = "ID";
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
        string strQuery = "SELECT TBL_LoginHist.*, TBL_UserList.*, '올인방' POSITION ";
        strQuery += " FROM TBL_LoginHist LEFT OUTER JOIN TBL_UserList ON TBL_UserList.ID=TBL_LoginHist.UserID ";
        strQuery += "WHERE( TBL_LoginHist.EndTime IS NULL AND TBL_USERLIST.loginFlag = 1 AND TBL_USERLIST.nologin = 0 ) AND ";
        strQuery += "(BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@Partner", 인증회원.Tables[0].Rows[0]["Partner"].ToString()));
        DataSet dsConnectedUser = dbManager.RunMSelectQuery(cmd);
        검색정보들 = dsConnectedUser;
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

       
        if (GetClass() < 4)
        {
            grdList.Columns[8].Visible = false;
            grdList.Columns[9].Visible = false;
        }
    }

    void UpdateButton()
    {
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
        // 강퇴처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "UPDATE TBL_UserList SET NoLogin=1, ChangeDate=getDate() WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        int nResult = dbManager.RunMQuery(cmd);

        strQuery = "UPDATE tbl_loginhist SET endtime=getDate() WHERE id IN ( SELECT id FROM tbl_loginhist WHERE userid=@userid and endtime is null)";
        cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@userid", strId));
        nResult = dbManager.RunMQuery(cmd);

        strQuery = "UPDATE tbl_loginhist SET endtime=getDate() WHERE id IN ( SELECT id FROM tbl_loginhist WHERE userid=@userid and endtime is null)";
        cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@userid", strId));
        nResult = dbManager.RunAllinQuery(cmd);

            LinkButton lnkDelete = (LinkButton)grdList.Rows[e.RowIndex].FindControl("lnkDelete");
        if (lnkDelete != null)
        {
            lnkDelete.Enabled = false;
            lnkDelete.Text = "강퇴처리중";
        }
        BindDataSource();
    }

    protected void cbxAlarm_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbxAlarm = (CheckBox)sender;
        if (cbxAlarm == null) return;

        LinkButton lnkNo = (LinkButton)cbxAlarm.Parent.Parent.FindControl("lnkNo");
        if (lnkNo == null) return;

        string strId = lnkNo.CommandArgument;
        if (strId == "") return;
        // 강퇴처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "UPDATE TBL_UserList SET stopchat=" + (cbxAlarm.Checked ? 1 : 0).ToString() + ", ChangeDate=getDate() WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        int nResult = dbManager.RunMQuery(cmd);
        BindDataSource();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strSearchKey = ddlSearchKey.SelectedValue;
        string strSearchValue = tbxSearchValue.Text;

        검색질문식 = strSearchKey + " LIKE '%" + strSearchValue + "%'";
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        tbxSearchValue.Text = "";
        ddlSearchKey.SelectedIndex = 0;
        BindDataSource();
    }
}
