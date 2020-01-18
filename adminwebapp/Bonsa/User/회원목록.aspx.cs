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

public partial class 회원관리_회원목록 : PageBase
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
       
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:회원관리:회원목록:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:회원관리:회원목록:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:회원관리:회원목록:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:회원관리:회원목록:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:회원관리:회원목록:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:회원관리:회원목록:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:회원관리:회원목록:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:회원관리:회원목록:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:회원관리:회원목록:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:회원관리:회원목록:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";
            정돈항 = "ID";
            정돈방향 = SortDirection.Descending;
            첫날자 = new DateTime(DateTime.Now.Year, 1, 1);

            BindDataSource();

            BindDropDownList();

            string menu_id = Request.QueryString["mid"];
            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            if (permission <= 1)
            {
                btnNew.Visible = false;
                grdList.Columns[grdList.Columns.Count - 1].Visible = false;
                grdList.Columns[grdList.Columns.Count - 2].Visible = false;
                grdList.Columns[grdList.Columns.Count - 3].Visible = false;
            }
            int funcChargePermission = pm.getFuncPermission("USER_CHARGE", 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            //int funcViewerPermission = pm.getFuncPermission("GAME_VIEWER", 인증회원.Tables[0].Rows[0]["user_type"].ToString());

            if (funcChargePermission < 1)
            {
                grdList.Columns[grdList.Columns.Count - 1].Visible = false;
                grdList.Columns[grdList.Columns.Count - 2].Visible = false;
            }

            /*if (GetClass() < 4)
            {
                btnNew.Visible = false;
                btnExcel.Visible = false;
            }*/
            if (인증회원.Tables[0].Rows[0]["user_type"].ToString().Equals("ENT") == false)
            {
                btnExcel.Visible = false;
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
        SqlCommand cmd = null;
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string fee_column;
        if (GetClass() < 4)
            fee_column = " S.ent_service_fee";
        else
            fee_column = " S.service_fee ";

        string strEnterpriseID = ddlEnterprise.SelectedValue;       

        string strQuery = "SELECT U.*, " + fee_column + " as service_fee , U.pokerwincount + U.pokerlosecount + U.badukiwincount + U.badukilosecount + U.matgowincount + U.matgolosecount game_count,";
        strQuery += " dbo.F_GET_GAME_POSITION(VP.lobby_userid, VP.gamekind, VP.userid, VP.roomname) position  ";
        strQuery += ", E.name EntName, isnull(login.userid, 0) status ";
        strQuery += ", (select sum(give_money) from tbl_ManualEventHist where userid=U.id and name=U.nickname) event_give_money ";
        strQuery += ", (select count(*) from tbl_userlist where recommender_dbid=U.id) as recommender_count ";
        strQuery += ", (select sum((poker_fee + baduki_fee + matgo_fee) * recommend_percent/100) from tbl_userrecommmoney where userid=U.id and used='1') as recommend_money ";
        strQuery += " FROM V_USER_LIST U left join TBL_Enterprise E on U.parentid=E.id ";
        strQuery += " left join (select * from tbl_loginhist where endtime is null) login on U.id=login.userid ";
        strQuery += " left join F_GET_USER_GAME_SUMMARY('2010-03-06', '" + today + "') S on U.id=S.id ";
        strQuery += " left join V_GAME_POSITION VP on VP.lobby_userid = U.id ";
        strQuery += " left join TBL_AdminInfo adm on 1=1 ";
        strQuery += "WHERE U.RegDate >= @StartDate AND U.RegDate <= @EndDate AND ";
        strQuery += "(U.BonsaID=@ParentID OR U.BuBonsaID=@ParentID OR U.ChongpanID=@ParentID OR U.MaejangID=@ParentID)";
        
        if (string.IsNullOrEmpty(strEnterpriseID) == false)
            strQuery += " and U.parentid = " + strEnterpriseID + " ";

        string isNoLoginUser = hidIsNoLoginUser.Value;

        strQuery += "   order by U.id desc";

        cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@Partner", 인증회원.Tables[0].Rows[0]["Partner"].ToString()));
        DataSet dsUser = dbManager.RunMSelectQuery(cmd);
        검색정보들 = dsUser;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {        
        UpdateButton();
        GridDataBind();
    }

    protected void BindDropDownList()
    {
        string sql;
        if(GetClass() >= 4)
            sql = "select * from tbl_enterprise where class< 5 order by id";
        else
            sql = "select * from tbl_enterprise where class< 5 and id=" + 인증회원.Tables[0].Rows[0]["id"].ToString();
        DBManager dbManager = new DBManager();
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = dbManager.RunMSelectQuery(cmd);
        if(GetClass() >= 4)
            ddlEnterprise.Items.Add(new ListItem("전체", ""));

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            ddlEnterprise.Items.Add(new ListItem(row["name"].ToString(), row["id"].ToString()));
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

        /*if (GetClass() < 4)
        {
            grdList.Columns[5].Visible = false;
            grdList.Columns[6].Visible = false;
            //grdList.Columns[12].Visible = false;
            //grdList.Columns[13].Visible = false;
            grdList.Columns[grdList.Columns.Count - 1].Visible = false;
            grdList.Columns[grdList.Columns.Count - 2].Visible = false;
            grdList.Columns[grdList.Columns.Count - 3].Visible = false;
        }*/
        if (인증회원.Tables[0].Rows[0]["user_type"].ToString().Equals("ENT") == false)
        {
            grdList.Columns[4].Visible = false;
        }


        //grdList.Columns[7].Visible = false; // chonpan
        grdList.Columns[13].Visible = false; // maejang
        grdList.Columns[14].Visible = false; // maejang
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
            DataView dv = (DataView)grdList.DataSource;
            lnkNo.Text = (dv.Table.Rows.Count - (grdList.PageIndex * grdList.PageSize + e.Row.RowIndex )).ToString();
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
        string strQuery = "DELETE TBL_UserList WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        int nResult = dbManager.RunMQuery(cmd);
        BindDataSource();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        hidIsNoLoginUser.Value = "0";

        string strSearchKey = ddlSearchKey.SelectedValue;
        string strSearchValue = tbxSearchValue.Text;

        검색질문식 = strSearchKey + " LIKE '%" + strSearchValue + "%'";

     
        //lblQuery.Text = 검색질문식;
        if (tbxStartDate.Text != "" && tbxEndDate.Text != "")
        {
            첫날자 = DateTime.Parse(tbxStartDate.Text + " 00:00:00");
            끝날자 = DateTime.Parse(tbxEndDate.Text + " 23:59:59");
            BindDataSource();
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("회원등록.aspx");
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        tbxSearchValue.Text = "";
        ddlSearchKey.SelectedIndex = 0;
        BindDataSource();
    }

    protected void btnNoLoginUser_Click(object sender, EventArgs e)
    {
        hidIsNoLoginUser.Value = "1";
        BindDataSource();
        GridDataBind();
    }

    protected void btnExcel_OnClick(object sender, EventArgs e)
    {
        int tempSize = grdList.PageSize;
        grdList.PageSize = 10000000;
        BindDataSource();
        GridDataBind();
        ExportExcel(grdList, "회원정보");
        grdList.PageSize = tempSize;
    }
}
