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

public partial class 게임관리_상황판 : PageBase
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

        BindStatus();
        BindChargeInfo();
        BindBussines();
        BindWithdrawInfo();
        BindBetInfo();
        BindDealInfo();

        if (GetClass() < 4)
        {
            grdChargeInfo.Caption = grdChargeInfo.Caption.Replace("(Top5)", "");
            grdWithdrawInfo.Caption = grdWithdrawInfo.Caption.Replace("(Top5)", "");
            grdBussines.Caption = grdBussines.Caption.Replace("(Top5)", "");
            //grdGameStatusView.Caption = grdGameStatusView.Caption.Replace("(Top5)", "");
            grdBetView.Caption = grdBetView.Caption.Replace("(Top5)", "");
            grdDealView.Caption = grdDealView.Caption.Replace("(Top5)", "");
        }

        if (GetClass() == 1)
        {
            HyperLink1.NavigateUrl = "../game/딜비내역.aspx";
        }

        string menu_id = Request.QueryString["mid"];
        if (string.IsNullOrEmpty(menu_id))
            menu_id = "14";

        PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                    인증회원.Tables[0].Rows[0]["user_type"].ToString());

        int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
        if (permission < 1)
        {
            Response.End();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        BindChargeList();
        BindWithdrawList();

       
        /*
        grdWithdraw.Columns[4].Visible = false;
        grdWithdraw.Columns[5].Visible = false;
        if (GetClass() < 4)
        {
            grdCharge.Columns[10].Visible = false;
            grdCharge.Columns[11].Visible = false;

            grdWithdraw.Columns[12].Visible = false;
            grdWithdraw.Columns[13].Visible = false;
        } */
            
    }

    void BindChargeList()
    {
        /*
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_Charge JOIN TBL_UserList ON TBL_UserList.ID = TBL_Charge.UserID WHERE TBL_Charge.status=0";
        strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@Partner", 인증회원.Tables[0].Rows[0]["Partner"].ToString()));
        DataSet dsChargeList = dbManager.RunMSelectQuery(cmd);

        //if (dsChargeList == null) return;
        //if (dsChargeList.Tables.Count == 0) return;
        //if (dsChargeList.Tables[0].Rows.Count == 0) return;

        grdCharge.DataSource = dsChargeList;
        grdCharge.DataBind(); */
    }
    void BindWithdrawList()
    {
        /*
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_Withdraw JOIN TBL_UserList ON TBL_UserList.ID = TBL_Withdraw.UserID WHERE TBL_Withdraw.status=0";
        strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@Partner", 인증회원.Tables[0].Rows[0]["Partner"].ToString()));
        DataSet dsWithdrawList = dbManager.RunMSelectQuery(cmd);

        //if (dsChargeList == null) return;
        //if (dsChargeList.Tables.Count == 0) return;
        //if (dsChargeList.Tables[0].Rows.Count == 0) return;

        grdWithdraw.DataSource = dsWithdrawList;
        grdWithdraw.DataBind(); */
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("회원목록.aspx");
    }

    
    protected void grdCharge_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /*
        if (e.Row.RowType == DataControlRowType.Header ||
            e.Row.RowType == DataControlRowType.Footer ||
            e.Row.RowType == DataControlRowType.Pager ||
            e.Row.RowType == DataControlRowType.Separator)
            return;

        LinkButton lnkNo = (LinkButton)e.Row.FindControl("lnkNo");
        if (lnkNo != null)
        {
            CheckBox cbxAlarm = (CheckBox)e.Row.FindControl("cbxAlarm");
            if (cbxAlarm != null)
            {
                cbxAlarm.Attributes["onclick"] = ClientScript.GetPostBackEventReference(cbxAlarm, "");
                cbxAlarm.Checked = (e.Row.DataItem != null && 알람정보들.IndexOf("charge" + ((DataRowView)e.Row.DataItem)["ID"].ToString() + ":") > -1);
            }

            // e.Row.Attributes["onclick"] = ClientScript.GetPostBackEventReference(lnkNo, "");
            e.Row.Attributes["onmouseover"] = "javascript:prevBGColor=this.style.backgroundColor; this.style.backgroundColor='#D1DDF1';this.style.cursor='hand';";
            e.Row.Attributes["onmouseout"] = "javascript:this.style.backgroundColor=prevBGColor;this.style.cursor='default';";
            e.Row.Attributes["mouseover"] = "cursor:hand";

            lnkNo.Text = (grdCharge.PageIndex * grdCharge.PageSize + e.Row.RowIndex + 1).ToString();
        }
         * */
    }
    
   
   

    
    

    void BindStatus()
    {
        DBManager dbManager = new DBManager();
        //string strQuery = "select 1 as status, 1 as poker_count, 2 as baduki_count, 3 as matgo_count, 6 as sum_count";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "P_GET_GAME_COUNT_STATUS";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@enterprise_id", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@class", GetClass()));
        cmd.Parameters.Add("@p_room_count", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@p_gamming", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@p_watting", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@b_room_count", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@b_gamming", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@b_watting", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@m_room_count", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@m_gamming", SqlDbType.Int).Direction = ParameterDirection.Output;
        cmd.Parameters.Add("@m_watting", SqlDbType.Int).Direction = ParameterDirection.Output;
        int result = dbManager.RunMQuery(cmd);

        int p_room_count = Convert.ToInt32(cmd.Parameters["@p_room_count"].Value);
        int p_gamming = Convert.ToInt32(cmd.Parameters["@p_gamming"].Value);
        int p_watting = Convert.ToInt32(cmd.Parameters["@p_watting"].Value);

        int b_room_count = Convert.ToInt32(cmd.Parameters["@b_room_count"].Value);
        int b_gamming = Convert.ToInt32(cmd.Parameters["@b_gamming"].Value);
        int b_watting = Convert.ToInt32(cmd.Parameters["@b_watting"].Value);

        int m_room_count = Convert.ToInt32(cmd.Parameters["@m_room_count"].Value);
        int m_gamming = Convert.ToInt32(cmd.Parameters["@m_gamming"].Value);
        int m_watting = Convert.ToInt32(cmd.Parameters["@m_watting"].Value);

        string sql1 = string.Format("select {0} as status,{1} poker_count,{2} baduki_count,{3} matgo_count, {4} as sum_count", "'게임방'", p_room_count, b_room_count, m_room_count, p_room_count + b_room_count + m_room_count);
        string sql2 = string.Format("select {0} as status,{1},{2},{3},{4} ", "'게임유저수'", p_gamming, b_gamming, m_gamming, p_gamming + b_gamming + m_gamming);
        string sql3 = string.Format("select {0} as status,{1},{2},{3},{4} ", "'대기실유저수'", p_watting, b_watting, m_watting, p_watting + b_watting + m_watting);

        string strQuery = sql1 + " union all " + sql2 + " union all " + sql3;
        SqlCommand cmd1 = new SqlCommand(strQuery);
        DataSet ds = dbManager.RunMSelectQuery(cmd1);
        if (ds == null) return;
        //grdGameStatusView.DataSource = ds;
        //grdGameStatusView.DataBind();
    }

    void BindChargeInfo()
    {
        DBManager dbManager = new DBManager();
        string where;
        if (GetClass() < 4)
            where = " where id IN (SELECT ID FROM TBL_ENTERPRISE WHERE USE_YN=1 AND (BONSAID=" + 인증회원번호
                + " OR BUBONSAID=" + 인증회원번호
                + " OR CHONGPANID=" + 인증회원번호
                + " OR ID=" + 인증회원번호 + ") ) ";
        else
            where = "";
        string strQuery = "select top 5 * from F_GET_CHARGE_STATUS('" + DateTime.Now.ToString("yyyy-MM-dd") + "') " + where + " order by f_sum desc";
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsGameStatus = dbManager.RunMSelectQuery(cmd);

        //if (dsChargeList == null) return;
        //if (dsChargeList.Tables.Count == 0) return;
        //if (dsChargeList.Tables[0].Rows.Count == 0) return;
        //grdGameStatusView.FooterRow.Cells[1].Text = "1";
        //grdGameStatusView.FooterRow.Cells[2].Text = "2";
        //grdGameStatusView.FooterRow.Cells[3].Text = "3";
        grdChargeInfo.DataSource = dsGameStatus;
        grdChargeInfo.DataBind();
    }

    void BindWithdrawInfo()
    {
        DBManager dbManager = new DBManager();
        string where;
        if (GetClass() < 4)
            where = " where id IN (SELECT ID FROM TBL_ENTERPRISE WHERE USE_YN=1 AND (BONSAID=" + 인증회원번호
               + " OR BUBONSAID=" + 인증회원번호
               + " OR CHONGPANID=" + 인증회원번호
               + " OR ID=" + 인증회원번호 + ") ) ";
        else
            where = "";

        string strQuery = "select top 5 * from F_GET_WITHDRAW_STATUS('" + DateTime.Now.ToString("yyyy-MM-dd") + "') " + where + " order by f_sum desc";
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsGameStatus = dbManager.RunMSelectQuery(cmd);

       
        grdWithdrawInfo.DataSource = dsGameStatus;
        grdWithdrawInfo.DataBind();
    }

    void BindBetInfo()
    {
        DBManager dbManager = new DBManager();
        string where;
        if (GetClass() < 4)
            where = " where regdate=@today and (bonsaid= " + 인증회원번호
                + " OR BUBONSAID=" + 인증회원번호
                + " OR CHONGPANID=" + 인증회원번호
                + " OR maejangid=" + 인증회원번호 + ")";
        else
            where = " where regdate=@today ";
        string strQuery = "select top 5 * from tbl_bethist " + where + " order by betmoney desc";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@today", DateTime.Now.ToShortDateString()));
        DataSet dsGameStatus = dbManager.RunMSelectQuery(cmd);

        grdBetView.DataSource = dsGameStatus;
        grdBetView.DataBind();

        if (!DataSetUtil.IsNullOrEmpty(dsGameStatus))
        {
            strQuery = "select sum(betmoney) from tbl_bethist " + where;
            cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@today", DateTime.Now.ToShortDateString()));
            dsGameStatus = dbManager.RunMSelectQuery(cmd);


            grdBetView.FooterRow.Cells[0].Text = "합계";
            grdBetView.FooterRow.Cells[5].Text = DataSetUtil.RowLongValue(dsGameStatus, 0, 0).ToString("N0");
            grdBetView.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
        }
    }
    void BindDealInfo()
    {
        DBManager dbManager = new DBManager();
        string where;
        if (GetClass() < 4)
            where = " where regdate=@today and ( bonsaid= " + 인증회원번호
                + " OR BUBONSAID=" + 인증회원번호
                + " OR CHONGPANID=" + 인증회원번호
                + " OR maejangid=" + 인증회원번호 + ")";
        else
            where = " where regdate=@today ";
        string strQuery = "select top 5 * from tbl_bethist " + where + " order by dealmoney desc";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@today", DateTime.Now.ToShortDateString()));
        DataSet dsGameStatus = dbManager.RunMSelectQuery(cmd);

        grdDealView.DataSource = dsGameStatus;
        grdDealView.DataBind();

        if (!DataSetUtil.IsNullOrEmpty(dsGameStatus))
        {
            strQuery = "select sum(dealmoney) from tbl_bethist " + where;
            cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@today", DateTime.Now.ToShortDateString()));
            dsGameStatus = dbManager.RunMSelectQuery(cmd);


            grdDealView.FooterRow.Cells[0].Text = "합계";
            grdDealView.FooterRow.Cells[5].Text = DataSetUtil.RowLongValue(dsGameStatus, 0, 0).ToString("N0");
            grdDealView.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
        }
    }
    void BindBussines()
    {
        DBManager dbManager = new DBManager();
        string where;
        string sum_classfee;

        if (GetClass() < 4)
        {
            where = " where Fee.id IN (SELECT ID FROM TBL_ENTERPRISE WHERE USE_YN=1 AND (BONSAID=" + 인증회원번호
                + " OR BUBONSAID=" + 인증회원번호
                + " OR CHONGPANID=" + 인증회원번호
                + " OR ID=" + 인증회원번호 + ") ) ";
            // 본사아닌경우 상황판에서는 자기의 지분율로 본다. 
            sum_classfee = " Floor(FEE.sum_BetMoney * " + 인증회원.Tables[0].Rows[0]["classpercent"].ToString() + "/100) real_classfee ";
        }
        else
        {
            where = "";

            sum_classfee = " FEE.sum_classfee real_classfee ";
        }
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string strQuery = "SELECT TOP 5 * FROM ("
            + "SELECT FEE.*, "
            + sum_classfee + ","
            + " ISNULL(L.P_LoginCount, 0) + "
            + " ISNULL(L.B_LoginCount, 0) + "
            + " ISNULL(L.M_LoginCount, 0) LOGIN_COUNT "
            + " FROM FN_GET_TODAY_GAME_FEE('" + today + "', '" + today + "') FEE "
            + " LEFT JOIN F_GET_GAME_LOGIN_COUNT() L ON FEE.ID = L.ID " + where + " "
            + ")A ORDER BY SUM_CLASSFEE DESC ";
        tbxQuery.Text = strQuery;
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsGameStatus = dbManager.RunMSelectQuery(cmd);

        int sum_login_count = 0;
        int sum_game_count = 0;
        long sum_fee = 0;
        long sum_ent_fee = 0;

        foreach (DataRow row in dsGameStatus.Tables[0].Rows)
        {
            if (Convert.IsDBNull(row["user_count"]) == false)
                sum_game_count += Convert.ToInt32(row["user_count"]);

            if (Convert.IsDBNull(row["login_count"]) == false)
                sum_login_count += Convert.ToInt32(row["login_count"]);

            if (Convert.IsDBNull(row["sum_servicefee"]) == false)
                sum_fee += Convert.ToInt64(row["sum_servicefee"]);

            if (Convert.IsDBNull(row["real_classfee"]) == false)
                sum_ent_fee += Convert.ToInt64(row["real_classfee"]);

        }
        //grdBussines.FooterRow.Cells[0].Text = "합계";
        if (GetClass() < 4)
        {
            grdBussines.Columns[3].Visible = false;
            grdBussines.Columns[4].Visible = false;
        }

        grdBussines.DataSource = dsGameStatus;
        grdBussines.DataBind();
        grdBussines.FooterRow.Cells[0].Text = "합계";
        grdBussines.FooterRow.Cells[1].Text = sum_login_count.ToString();
        grdBussines.FooterRow.Cells[2].Text = sum_game_count.ToString();
        grdBussines.FooterRow.Cells[3].Text = sum_fee.ToString("N0");
        grdBussines.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
        grdBussines.FooterRow.Cells[4].Text = "";
        grdBussines.FooterRow.Cells[5].Text = sum_ent_fee.ToString("N0");
        grdBussines.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
       
    }
}
