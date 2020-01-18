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

public partial class Bonsa_Game_ManualEventList : PageBase
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

            string menu_id = Request.QueryString["mid"];
            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            if (permission <= 1)
            {
                //btnNew.Visible = false;
                btnNew.Visible = false;
                //btnSave.Visible = false;
                grdList.Columns[grdList.Columns.Count - 1].Visible = false;
                //grdList.Columns[grdList.Columns.Count - 2].Visible = false;
                grdList.Columns[5].Visible = false;
                grdList.Columns[6].Visible = false;
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
        string strQuery = "SELECT * FROM TBL_ManualEvent order by id";
        SqlCommand cmd = new SqlCommand(strQuery);
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
    
    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManualEventMng.aspx");
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        BindDataSource();
    }

    protected void lbOperate_Click(object sender, EventArgs e)
    {
        string event_id = ((LinkButton)sender).CommandArgument.ToString();

        string strQuery = "UPDATE TBL_MANUALEVENT SET "
            + " all_event=0,poker_all_event=0,baduki_all_event=0,matgo_all_event=0,use_yn=0,change_time=GetDate() WHERE id=" + event_id;

        DBManager dm = new DBManager();
        SqlCommand cmd = new SqlCommand(strQuery);
        dm.RunMQuery(cmd);
        BindDataSource();
        GridDataBind();
    }

    protected void lbDelete_Click(object sender, EventArgs e)
    {
        string event_id = ((LinkButton)sender).CommandArgument.ToString();

        DBManager dm = new DBManager();

        string strQuery = "select use_yn from tbl_ManualEvent WHERE id=" + event_id;
        DataSet chkDS = dm.RunMSelectQuery(new SqlCommand(strQuery));
        string use_yn = chkDS.Tables[0].Rows[0][0].ToString();
        // 2017-04-19 진행중 이벤트를 그냥 삭제하면 서버단에서 실시간 처리가 안될수있다
        if (use_yn.Equals("1"))
        {
            Response.Write("<script>alert('진행중인 이벤트는 중지후 3분 지난뒤 삭제를 하십시오.');</script>");
            return;
        }
       

        strQuery = "DELETE FROM TBL_ManualEvent where id=" + event_id;
       
        SqlCommand cmd = new SqlCommand(strQuery);
        dm.RunMQuery(cmd);

        strQuery = "DELETE FROM TBL_ManualEventUsing where event_id=" + event_id;        
        SqlCommand cmd2 = new SqlCommand(strQuery);
        dm.RunMQuery(cmd2);
        BindDataSource();
        GridDataBind();
    }
}
