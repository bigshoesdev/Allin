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

public partial class Bonsa_Game_ManualEventSetting : PageBase
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

        if (!IsPostBack)
        {
            string id = Request.QueryString["id"];
            string event_name = Request.QueryString["event_name"];
            string event_id = Request.QueryString["id"];

            lblEventName.Text = event_name;
            hidID.Value = event_id;

            string strQuery = "SELECT * FROM TBL_MANUALEVENT WHERE id=" + event_id;
            DBManager dm = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet ds = dm.RunMSelectQuery(cmd);

            BindBadukiData();
            GridDataBind();
            grdList.Caption = "바둑이방정보";
            hidGameKind.Value = "BADUKI";

            /*if (ds != null)
            {
                DataRow row = ds.Tables[0].Rows[0];
                string e_all = row["all_event"].ToString();
                string p_e_all = row["poker_all_event"].ToString();
                string b_e_all = row["baduki_all_event"].ToString();
                string m_e_all = row["matgo_all_event"].ToString();

                if (e_all.Equals("1"))
                    chkAll.Checked = true;

                if (p_e_all.Equals("1"))
                    chkPoker.Checked = true;

                if (b_e_all.Equals("1"))
                    chkBaduki.Checked = true;

                if (m_e_all.Equals("1"))
                    chkMatgo.Checked = true;
            }
            BindPokerData();
            GridDataBind();*/
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

    void GridDataBind()
    {
        /*
        if (검색정보들 == null)
        {
            BindDataSource();
        }
        */
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
        //grdList.PageIndex = 현재페지위치;
        grdList.DataBind();

        lblSortExpr.Text = "《" + 정돈항 + "》";
        pnlDescSortLbl.Visible = 정돈방향 == SortDirection.Descending;
        pnlAscSortLbl.Visible = 정돈방향 == SortDirection.Ascending;
    }

    protected void BindPokerData()
    {
        if (검색정보들 != null)
        {
            검색정보들.Dispose();
            검색정보들 = null;
        }

        string strQuery = "SELECT row_number() over(order by id desc)as RowNum, * FROM V_POKER_ROOM_INFO";
        DBManager dm = new DBManager();
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet ds = dm.RunMSelectQuery(cmd);

        검색정보들 = ds;
    }

   
    protected void BindBadukiData()
    {
        if (검색정보들 != null)
        {
            검색정보들.Dispose();
            검색정보들 = null;
        }

        string strQuery = "SELECT row_number() over(order by id desc)as RowNum, * FROM V_BADUKI_ROOM_INFO";
        DBManager dm = new DBManager();
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet ds = dm.RunMSelectQuery(cmd);

        검색정보들 = ds;
    }

    protected void BindMatgoData()
    {
        if (검색정보들 != null)
        {
            검색정보들.Dispose();
            검색정보들 = null;
        }

        string strQuery = "SELECT row_number() over(order by id desc)as RowNum, * FROM V_MATGO_ROOM_INFO";
        DBManager dm = new DBManager();
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet ds = dm.RunMSelectQuery(cmd);

        검색정보들 = ds;
    }

    protected void lnkPoker_Click(object sender, EventArgs e)
    {
        BindPokerData();

        GridDataBind();
        grdList.Caption = "포커방정보";
        hidGameKind.Value = "POKER";
    }

    protected void lnkBaduki_Click(object sender, EventArgs e)
    {
        BindBadukiData();

        GridDataBind();
        grdList.Caption = "바둑이방정보";
        hidGameKind.Value = "BADUKI";
    }

    protected void lnkMatgo_Click(object sender, EventArgs e)
    {
        BindMatgoData();

        GridDataBind();
        grdList.Caption = "맞고방정보";
        hidGameKind.Value = "MATGO";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (CountChecked() == 0)
        {
            lblMsg.Text = "적용할 내용이 없습니다.";
            return;
        }

        string all_event = "0";
        string poker_all_event = "0";
        string baduki_all_event = "0";
        string matgo_all_event = "0";

        if (chkAll.Checked)
            all_event = "1";

        if (chkPoker.Checked)
            poker_all_event = "1";

        if (chkBaduki.Checked)
            baduki_all_event = "1";

        if (chkMatgo.Checked)
            matgo_all_event = "1";

        string event_id = hidID.Value;
        string game_kind = hidGameKind.Value;

        DBManager dm = new DBManager();

        string updQuery = "UPDATE TBL_ManualEvent SET use_yn='0'";
        dm.RunMQuery(new SqlCommand(updQuery));

        string strQuery = "UPDATE TBL_MANUALEVENT SET use_yn='1', all_event=" + all_event + ",poker_all_event=" + poker_all_event
            + ",baduki_all_event=" + baduki_all_event + ",matgo_all_event=" + matgo_all_event
            + ",change_time=GetDate() "
            + ",uuid=newid() "  // 방에서 이벤트 유일성 판단하기 위함 2018-01-31
            + " where id=" + event_id;
        
        SqlCommand cmd = new SqlCommand(strQuery);
        dm.RunMQuery(cmd);

        // 전에 이벤트들을  지우고 다시 넣는다
        string delQuery;
        //if (all_event.Equals("1"))
        {
            delQuery = "delete from TBL_ManualEventUsing ";
            dm.RunMQuery(new SqlCommand(delQuery));
            //lblMsg.Text = "저장 되었습니다.";
            //return;
        }

        /*// 게임별 전체가 체크되면 방별은 뺀다
        if (poker_all_event.Equals("1") || baduki_all_event.Equals("1") || matgo_all_event.Equals("1"))
        {
            delQuery = "delete from TBL_ManualEventUsing where game_kind='" + game_kind + "'";
            dm.RunMQuery(new SqlCommand(delQuery));
        }*/
       
        for (int i = 0; i < grdList.Rows.Count; i++)
        {
            if (((CheckBox)grdList.Rows[i].FindControl("chkRoom")).Checked)
            {
                string query = "INSERT INTO Tbl_ManualEventUsing (event_id, game_kind, room_id)"
                    + " values(" + event_id + ",'" + game_kind + "'," + ((CheckBox)grdList.Rows[i].FindControl("chkRoom")).ToolTip + ")";

                dm.RunMQuery(new SqlCommand(query));
            }
        }

        lblMsg.Text = "저장 되었습니다.";
    }

    protected int CountChecked()
    {
        /*if (chkAll.Checked || chkPoker.Checked || chkBaduki.Checked || chkMatgo.Checked)
            return 1;*/

        for (int i = 0; i < grdList.Rows.Count; i++)
        {
            if (((CheckBox)grdList.Rows[i].FindControl("chkRoom")).Checked)
                return 1;
        }

        return 0;
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManualEventList.aspx");
    }
}
