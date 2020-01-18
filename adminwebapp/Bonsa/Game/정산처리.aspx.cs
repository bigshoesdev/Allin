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

public partial class 게임관리_정산처리 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:게임관리:정산처리:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:게임관리:정산처리:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:게임관리:정산처리:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:게임관리:정산처리:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:게임관리:정산처리:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:게임관리:정산처리:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:게임관리:정산처리:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:게임관리:정산처리:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:게임관리:정산처리:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:게임관리:정산처리:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";

            BindDataSource();

            int nClass = int.Parse(인증회원.Tables[0].Rows[0]["Class"].ToString());
            ddlBonsa.Items.Clear();
            ddlBubonsa.Items.Clear();
            ddlChongPan.Items.Clear();
            ddlMaejang.Items.Clear();
            switch (nClass.ToString())
            {
                case ROLES_BONSA:
                    ddlBonsa.Visible = false;
                    ddlBonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["name"].ToString(), 인증회원번호.ToString()));
                    BindBubonsa(인증회원번호);
                    BindChongpan(인증회원번호, 0);
                    BindMaejang(인증회원번호, 0, 0);
                    break;
                case ROLES_BUBONSA:
                    ddlBonsa.Visible = false;
                    ddlBonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["bonsa"].ToString(), 인증회원.Tables[0].Rows[0]["bonsaid"].ToString()));
                    ddlBubonsa.Visible = false;
                    ddlBubonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["name"].ToString(), 인증회원번호.ToString()));
                    BindChongpan(int.Parse(인증회원.Tables[0].Rows[0]["bonsaid"].ToString()), 인증회원번호);
                    BindMaejang(int.Parse(인증회원.Tables[0].Rows[0]["bonsaid"].ToString()), 인증회원번호, 0);
                    break;
                case ROLES_CHONGPAN:
                    ddlBonsa.Visible = false;
                    ddlBonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["bonsa"].ToString(), 인증회원.Tables[0].Rows[0]["bonsaid"].ToString()));
                    ddlBubonsa.Visible = false;
                    ddlBubonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["bubonsa"].ToString(), 인증회원.Tables[0].Rows[0]["bubonsaid"].ToString()));
                    ddlChongPan.Visible = false;
                    ddlChongPan.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["name"].ToString(), 인증회원번호.ToString()));
                    BindMaejang(int.Parse(인증회원.Tables[0].Rows[0]["bonsaid"].ToString()), int.Parse(인증회원.Tables[0].Rows[0]["bubonsaid"].ToString()), 인증회원번호);
                    break;
                case ROLES_MAEJANG:
                    ddlBonsa.Visible = false;
                    ddlBonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["bonsa"].ToString(), 인증회원.Tables[0].Rows[0]["bonsaid"].ToString()));
                    ddlBubonsa.Visible = false;
                    ddlBubonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["bubonsa"].ToString(), 인증회원.Tables[0].Rows[0]["bubonsaid"].ToString()));
                    ddlChongPan.Visible = false;
                    ddlChongPan.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["chongpan"].ToString(), 인증회원.Tables[0].Rows[0]["chongpanid"].ToString()));
                    ddlMaejang.Visible = false;
                    ddlMaejang.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["name"].ToString(), 인증회원번호.ToString()));
                    break;
            }
        }
    }

    void BindBubonsa(int nBonsaID)
    {
        string strQuery = "";
        /*
        if (nBonsaID == 0)
        {
            strQuery = "SELECT * FROM TBL_Enterprise WHERE Class=" + PageBase.ROLES_BUBONSA;
        }
        else
        {
            strQuery = "SELECT * FROM TBL_Enterprise WHERE Class=" + PageBase.ROLES_BUBONSA + " AND BonsaID=" + nBonsaID.ToString();
        } */
        if (GetClass() < 4)
            strQuery = "SELECT * FROM TBL_Enterprise WHERE id=" + 인증회원.Tables[0].Rows[0]["id"].ToString();
        else
            strQuery = "SELECT * FROM TBL_Enterprise where class < 5";

        DBManager dbManager = new DBManager();
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsEnterprise = dbManager.RunMSelectQuery(cmd);
        ddlBubonsa.DataSource = dsEnterprise;
        ddlBubonsa.DataTextField = "name";
        ddlBubonsa.DataValueField = "ID";
        ddlBubonsa.DataBind();
        ddlBubonsa.Items.Insert(0, new ListItem("전체", "0"));
    }
    void BindChongpan(int nBonsaID, int nBubonsaID)
    {
        string strQuery = "SELECT * FROM TBL_Enterprise WHERE Class=" + PageBase.ROLES_CHONGPAN;
        if (nBonsaID > 0)
        {
            strQuery += " AND BonsaID=" + nBonsaID.ToString();
        }
        if (nBubonsaID > 0)
        {
            strQuery += " AND BuBonsaID=" + nBubonsaID.ToString();
        }
        DBManager dbManager = new DBManager();
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsEnterprise = dbManager.RunMSelectQuery(cmd);
        ddlChongPan.DataSource = dsEnterprise;
        ddlChongPan.DataTextField = "loginid";
        ddlChongPan.DataValueField = "ID";
        ddlChongPan.DataBind();
        ddlChongPan.Items.Insert(0, new ListItem("전체", "0"));
    }
    void BindMaejang(int nBonsaID, int nBubonsaID, int nChongpanID)
    {
        string strQuery = "SELECT * FROM TBL_Enterprise WHERE Class=" + PageBase.ROLES_MAEJANG;
        if (nBonsaID > 0)
        {
            strQuery += " AND BonsaID=" + nBonsaID.ToString();
        }
        if (nBubonsaID > 0)
        {
            strQuery += " AND BuBonsaID=" + nBubonsaID.ToString();
        }
        if (nChongpanID > 0)
        {
            strQuery += " AND ChongpanID=" + nChongpanID.ToString();
        }
        DBManager dbManager = new DBManager();
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsEnterprise = dbManager.RunMSelectQuery(cmd);
        ddlMaejang.DataSource = dsEnterprise;
        ddlMaejang.DataTextField = "loginid";
        ddlMaejang.DataValueField = "ID";
        ddlMaejang.DataBind();
        ddlMaejang.Items.Insert(0, new ListItem("전체", "0"));
    }

    public void BindDataSource()
    {
        if (검색정보들 != null)
        {
            검색정보들.Dispose();
            검색정보들 = null;
        }

        DBManager dbManager = new DBManager();
        DataSet dsResult = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("Date");
        dt.Columns.Add("ChargeMoney", typeof(System.Int64));
        dt.Columns.Add("ChargeCount", typeof(System.Int64));
        dt.Columns.Add("WithdrawMoney", typeof(System.Int64));
        dt.Columns.Add("WithdrawCount", typeof(System.Int64));
        dt.Columns.Add("BenefitMoney", typeof(System.Int64));
        dt.Columns.Add("PartnerMoney", typeof(System.Int64));
        dt.Columns.Add("income_money", typeof(System.Int64));

        dt.Columns.Add("poker_bet", typeof(System.Int64));
        dt.Columns.Add("baduki_bet", typeof(System.Int64));
        dt.Columns.Add("matgo_bet", typeof(System.Int64));
        dsResult.Tables.Add(dt);

        for (DateTime date = 첫날자; date <= 끝날자; date = date.AddDays(1))
        {
            DataRow dr = dsResult.Tables[0].NewRow();
            dr["Date"] = date.ToString("yyyy-MM-dd");

            int nPartnerPercent = 0;

            string strQuery = "SELECT SUM(TBL_Charge.money) AS ChargeMoney, COUNT(TBL_Charge.money) AS ChargeCount ";
            strQuery += "FROM TBL_Charge JOIN TBL_UserList ON TBL_Charge.UserID = TBL_UserList.ID ";
            strQuery += "WHERE TBL_Charge.Status>0 AND TBL_Charge.RegDate >= @StartDate AND TBL_Charge.RegDate <= @EndDate ";
            strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID)";
            strQuery += (검색질문식 != "") ? " AND " + 검색질문식 : "";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@StartDate", date));
            cmd.Parameters.Add(new SqlParameter("@EndDate", date.AddDays(1)));
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
            DataSet dsChargeList = dbManager.RunMSelectQuery(cmd);
            long nChargeMoney = 0;
            long nChargeCount = 0;
            if (dsChargeList.Tables.Count > 0 && dsChargeList.Tables[0].Rows.Count > 0)
            {
                nChargeMoney = dsChargeList.Tables[0].Rows[0]["ChargeMoney"] == DBNull.Value ? 0 : long.Parse(dsChargeList.Tables[0].Rows[0]["ChargeMoney"].ToString());
                nChargeCount = dsChargeList.Tables[0].Rows[0]["ChargeCount"] == DBNull.Value ? 0 : long.Parse(dsChargeList.Tables[0].Rows[0]["ChargeCount"].ToString());
            }

            strQuery = "SELECT SUM(TBL_Withdraw.money) AS WithdrawMoney, COUNT(TBL_Withdraw.money) AS WithdrawCount ";
            strQuery += "FROM TBL_Withdraw JOIN TBL_UserList ON TBL_Withdraw.UserID = TBL_UserList.ID ";
            strQuery += "WHERE TBL_Withdraw.Status>0 AND TBL_Withdraw.RegDate >= @StartDate AND TBL_Withdraw.RegDate <= @EndDate ";
            strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID)";
            strQuery += (검색질문식 != "") ? " AND " + 검색질문식 : "";
            cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@StartDate", date));
            cmd.Parameters.Add(new SqlParameter("@EndDate", date.AddDays(1)));
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
            cmd.Parameters.Add(new SqlParameter("@Partner", 인증회원.Tables[0].Rows[0]["Partner"].ToString()));
            DataSet dsWithdrawList = dbManager.RunMSelectQuery(cmd);
            long nWithdrawMoney = 0;
            long nWithdrawCount = 0;
            if (dsWithdrawList.Tables.Count > 0 && dsWithdrawList.Tables[0].Rows.Count > 0)
            {
                nWithdrawMoney = dsWithdrawList.Tables[0].Rows[0]["WithdrawMoney"] == DBNull.Value ? 0 : long.Parse(dsWithdrawList.Tables[0].Rows[0]["WithdrawMoney"].ToString());
                nWithdrawCount = dsWithdrawList.Tables[0].Rows[0]["WithdrawCount"] == DBNull.Value ? 0 : long.Parse(dsWithdrawList.Tables[0].Rows[0]["WithdrawCount"].ToString());
            }

            /* 회원베팅금 합계 2017-03-21 */
            string where = "";
            if (GetClass() < 4)
            {
                where = " where parentid=" + 인증회원번호;
            }
            //strQuery = "SELECT isnull(sum(betMoneySum),0) pokerBetSum FROM dbo.F_GET_POKER_GAME_SUMMARY('" + date.ToString("yyyy-MM-dd") 
            //            + "', '" + date.ToString("yyyy-MM-dd") + "') " + where ;
            //cmd = new SqlCommand(strQuery);
            //DataSet dsPokerBet = dbManager.RunMSelectQuery(cmd);
            //Int64 nPokerBet = 0;
            //if (dsPokerBet.Tables.Count > 0 && dsPokerBet.Tables[0].Rows.Count > 0)
            //{
            //    nPokerBet = Convert.ToInt64(dsPokerBet.Tables[0].Rows[0]["pokerBetSum"]);
            //}

            //strQuery = "SELECT isnull(sum(betMoneySum),0) badukiBetSum FROM dbo.F_GET_BADUKI_GAME_SUMMARY('" + date.ToString("yyyy-MM-dd")
            //           + "', '" + date.ToString("yyyy-MM-dd") + "') " + where;
            //cmd = new SqlCommand(strQuery);
            //DataSet dsBadukiBet = dbManager.RunMSelectQuery(cmd);
            //Int64 nBadukiBet = 0;
            //if (dsBadukiBet.Tables.Count > 0 && dsBadukiBet.Tables[0].Rows.Count > 0)
            //{
            //    nBadukiBet = Convert.ToInt64(dsBadukiBet.Tables[0].Rows[0]["badukiBetSum"]);
            //}

            //strQuery = "SELECT isnull(sum(betMoneySum),0) matgoBetSum FROM dbo.F_GET_MATGO_GAME_SUMMARY('" + date.ToString("yyyy-MM-dd")
            //           + "', '" + date.ToString("yyyy-MM-dd") + "') " + where;
            //cmd = new SqlCommand(strQuery);
            //DataSet dsMatgoBet = dbManager.RunMSelectQuery(cmd);
            //Int64 nMatgoBet = 0;
            //if (dsMatgoBet.Tables.Count > 0 && dsMatgoBet.Tables[0].Rows.Count > 0)
            //{
            //    nMatgoBet = Convert.ToInt64(dsMatgoBet.Tables[0].Rows[0]["matgoBetSum"]);
            //}

            dr["ChargeMoney"] = nChargeMoney;
            dr["ChargeCount"] = nChargeCount;
            dr["WithdrawMoney"] = nWithdrawMoney;
            dr["WithdrawCount"] = nWithdrawCount;
            dr["BenefitMoney"] = nChargeMoney - nWithdrawMoney;
            dr["PartnerMoney"] = (nChargeMoney - nWithdrawMoney) * nPartnerPercent / 100;
            dr["income_money"] = nChargeMoney - nWithdrawMoney;

            //dr["poker_bet"] = nPokerBet;
            //dr["baduki_bet"] = nBadukiBet;
            //dr["matgo_bet"] = nMatgoBet;
            dsResult.Tables[0].Rows.Add(dr);
        }
        검색정보들 = dsResult;
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
        //DataSet src = 검색정보얻기(검색질문식);
        DataSet src = 검색정보들;
        if (src != null)
        {
            DataView dv = src.Tables[0].DefaultView;
            grdList.DataSource = dv;
        }
        else
        {
            grdList.DataSource = null;
        }
        //grdList.PageSize = 페지당현시개수;
        grdList.PageIndex = 현재페지위치;
        grdList.DataBind();
        // 합계계산
        if (검색정보들 != null && grdList.FooterRow != null)
        {
            grdList.FooterRow.Cells[0].Text = "합계";
            long chargeSum = 0;
            long chargeCount = 0;
            long withdrawSum = 0;
            long withdrawCount = 0;
            long benefitSum = 0;
            long incomeSum = 0;

            for (int i = 0; i < 검색정보들.Tables[0].Rows.Count; i++)
            {
                chargeSum += Convert.ToInt64(검색정보들.Tables[0].Rows[i]["ChargeMoney"] == DBNull.Value ? 0 : 검색정보들.Tables[0].Rows[i]["ChargeMoney"]);
                chargeCount += Convert.ToInt64(검색정보들.Tables[0].Rows[i]["ChargeCount"] == DBNull.Value ? 0 : 검색정보들.Tables[0].Rows[i]["ChargeCount"]);
                withdrawSum += Convert.ToInt64(검색정보들.Tables[0].Rows[i]["WithdrawMoney"] == DBNull.Value ? 0 : 검색정보들.Tables[0].Rows[i]["WithdrawMoney"]);
                withdrawCount += Convert.ToInt64(검색정보들.Tables[0].Rows[i]["WithdrawCount"] == DBNull.Value ? 0 : 검색정보들.Tables[0].Rows[i]["WithdrawCount"]);
                benefitSum += Convert.ToInt64(검색정보들.Tables[0].Rows[i]["BenefitMoney"] == DBNull.Value ? 0 : 검색정보들.Tables[0].Rows[i]["BenefitMoney"]);
                incomeSum += Convert.ToInt64(검색정보들.Tables[0].Rows[i]["income_money"] == DBNull.Value ? 0 : 검색정보들.Tables[0].Rows[i]["income_money"]);
            }


            /** 회원 보유금액 검색 시점에서만 유효, 옛날것들 없음*/
            string where = "";
            if (GetClass() < 4)
                where = " where parentid = " + 인증회원번호;

            string strQuery = "SELECT isnull(sum(gamemoney),0) gamemoneySum  FROM TBL_UserList " + where;
            SqlCommand cmd = new SqlCommand(strQuery);
            DBManager dbManager = new DBManager();
            DataSet dsGameMoney = dbManager.RunMSelectQuery(cmd);
            Int64 nGameMoney = 0;
            if (dsGameMoney.Tables.Count > 0 && dsGameMoney.Tables[0].Rows.Count > 0)
            {
                nGameMoney = Convert.ToInt64(dsGameMoney.Tables[0].Rows[0]["gamemoneySum"]);
            }

            int idx = 0;
            grdList.FooterRow.Cells[idx + 1].Text = chargeSum.ToString("N0") + "";
            grdList.FooterRow.Cells[idx + 2].Text = chargeCount.ToString("N0") + "회";
            grdList.FooterRow.Cells[idx + 3].Text = withdrawSum.ToString("N0") + "";
            grdList.FooterRow.Cells[idx + 4].Text = withdrawCount.ToString("N0") + "회";
            grdList.FooterRow.Cells[idx + 5].Text = (incomeSum).ToString("N0") + "";
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
    }

    protected void grdLisTBL_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        현재페지위치 = e.NewPageIndex;
    }
    #endregion

    protected void btnSearch_Click(object sender, EventArgs e)
    {
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
        int nClass = int.Parse(인증회원.Tables[0].Rows[0]["Class"].ToString());
        ddlBonsa.Items.Clear();
        ddlBubonsa.Items.Clear();
        ddlChongPan.Items.Clear();
        ddlMaejang.Items.Clear();
        switch (nClass.ToString())
        {
            case ROLES_BONSA:
                ddlBonsa.Visible = false;
                ddlBonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["name"].ToString(), 인증회원번호.ToString()));
                BindBubonsa(인증회원번호);
                BindChongpan(인증회원번호, 0);
                BindMaejang(인증회원번호, 0, 0);
                break;
            case ROLES_BUBONSA:
                ddlBonsa.Visible = false;
                ddlBonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["bonsa"].ToString(), 인증회원.Tables[0].Rows[0]["bonsaid"].ToString()));
                ddlBubonsa.Visible = false;
                ddlBubonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["name"].ToString(), 인증회원번호.ToString()));
                BindChongpan(int.Parse(인증회원.Tables[0].Rows[0]["bonsaid"].ToString()), 인증회원번호);
                BindMaejang(int.Parse(인증회원.Tables[0].Rows[0]["bonsaid"].ToString()), 인증회원번호, 0);
                break;
            case ROLES_CHONGPAN:
                ddlBonsa.Visible = false;
                ddlBonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["bonsa"].ToString(), 인증회원.Tables[0].Rows[0]["bonsaid"].ToString()));
                ddlBubonsa.Visible = false;
                ddlBubonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["bubonsa"].ToString(), 인증회원.Tables[0].Rows[0]["bubonsaid"].ToString()));
                ddlChongPan.Visible = false;
                ddlChongPan.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["name"].ToString(), 인증회원번호.ToString()));
                BindMaejang(int.Parse(인증회원.Tables[0].Rows[0]["bonsaid"].ToString()), int.Parse(인증회원.Tables[0].Rows[0]["bubonsaid"].ToString()), 인증회원번호);
                break;
            case ROLES_MAEJANG:
                ddlBonsa.Visible = false;
                ddlBonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["bonsa"].ToString(), 인증회원.Tables[0].Rows[0]["bonsaid"].ToString()));
                ddlBubonsa.Visible = false;
                ddlBubonsa.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["bubonsa"].ToString(), 인증회원.Tables[0].Rows[0]["bubonsaid"].ToString()));
                ddlChongPan.Visible = false;
                ddlChongPan.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["chongpan"].ToString(), 인증회원.Tables[0].Rows[0]["chongpanid"].ToString()));
                ddlMaejang.Visible = false;
                ddlMaejang.Items.Add(new ListItem(인증회원.Tables[0].Rows[0]["name"].ToString(), 인증회원번호.ToString()));
                break;
        }
        BindDataSource();
    }
    protected void btnViewByUser_Click(object sender, EventArgs e)
    {
        Response.Redirect("회원별정산.aspx");
    }
    protected void btnDay_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (btn == null) return;
        int nDay = -1 * int.Parse(btn.CommandArgument);
        첫날자 = DateTime.Today.AddDays(nDay);
        끝날자 = DateTime.Today.AddDays(nDay + 1).AddSeconds(-1);

        btn.ForeColor = System.Drawing.Color.White;
        btn.BackColor = System.Drawing.Color.Red;
        BindDataSource();
    }

    protected void ddlBonsa_SelectedIndexChanged(object sender, EventArgs e)
    {
        검색질문식 = "";
        if (ddlBonsa.SelectedIndex > 0)
            검색질문식 = "TBL_UserList.parentid=" + ddlBonsa.SelectedValue;
            //검색질문식 = "TBL_UserList.BonsaID=" + ddlBonsa.SelectedValue;

        BindBubonsa(int.Parse(ddlBonsa.SelectedValue));
        ddlBubonsa.SelectedIndex = 0;

        BindChongpan(int.Parse(ddlBonsa.SelectedValue), int.Parse(ddlBubonsa.SelectedValue));
        ddlChongPan.SelectedIndex = 0;

        BindMaejang(int.Parse(ddlBonsa.SelectedValue), int.Parse(ddlBubonsa.SelectedValue), int.Parse(ddlChongPan.SelectedValue));
        ddlMaejang.SelectedIndex = 0;

        BindDataSource();
    }
    protected void ddlBubonsa_SelectedIndexChanged(object sender, EventArgs e)
    {
        검색질문식 = "";
        if (ddlBonsa.SelectedIndex > 0)
            검색질문식 = "TBL_UserList.BonsaID=" + ddlBonsa.SelectedValue;

        if (ddlBubonsa.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "TBL_UserList.BubonsaID=" + ddlBubonsa.SelectedValue;
        }

        BindChongpan(int.Parse(ddlBonsa.SelectedValue), int.Parse(ddlBubonsa.SelectedValue));
        ddlChongPan.SelectedIndex = 0;

        BindMaejang(int.Parse(ddlBonsa.SelectedValue), int.Parse(ddlBubonsa.SelectedValue), int.Parse(ddlChongPan.SelectedValue));
        ddlMaejang.SelectedIndex = 0;

        BindDataSource();
    }
    protected void ddlChongPan_SelectedIndexChanged(object sender, EventArgs e)
    {
        검색질문식 = "";
        if (ddlBonsa.SelectedIndex > 0)
            검색질문식 = "TBL_UserList.BonsaID=" + ddlBonsa.SelectedValue;
        if (ddlBubonsa.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "TBL_UserList.BubonsaID=" + ddlBubonsa.SelectedValue;
        }
        if (ddlChongPan.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "TBL_UserList.ChongpanID=" + ddlChongPan.SelectedValue;
        }

        BindMaejang(int.Parse(ddlBonsa.SelectedValue), int.Parse(ddlBubonsa.SelectedValue), int.Parse(ddlChongPan.SelectedValue));
        ddlMaejang.SelectedIndex = 0;

        BindDataSource();
    }
    protected void ddlMaejang_SelectedIndexChanged(object sender, EventArgs e)
    {
        검색질문식 = "";
        if (ddlBonsa.SelectedIndex > 0)
            검색질문식 = "TBL_UserList.BonsaID=" + ddlBonsa.SelectedValue;

        if (ddlBubonsa.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "TBL_UserList.BubonsaID=" + ddlBubonsa.SelectedValue;
        }
        if (ddlChongPan.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "TBL_UserList.ChongpanID=" + ddlChongPan.SelectedValue;
        }
        if (ddlMaejang.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "TBL_UserList.MaeJangID=" + ddlMaejang.SelectedValue;
        }

        BindDataSource();
        cvResult.Text = 검색질문식;
        cvResult.IsValid = false;
    }

    protected void btnFormat_Click(object sender, EventArgs e)
    {
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_Charge ";
        strQuery += "WHERE TBL_Charge.Status>0 AND TBL_Charge.RegDate >= @StartDate AND TBL_Charge.RegDate <= @EndDate ";
        strQuery += "AND (TBL_Charge.UserID IN (SELECT ID FROM TBL_UserList WHERE BonsaID=@ParentID) OR ";
        strQuery += "TBL_Charge.UserID IN (SELECT ID FROM TBL_UserList WHERE BuBonsaID=@ParentID) OR ";
        strQuery += "TBL_Charge.UserID IN (SELECT ID FROM TBL_UserList WHERE ChongpanID=@ParentID) OR ";
        strQuery += "TBL_Charge.UserID IN (SELECT ID FROM TBL_UserList WHERE MaejangID=@ParentID))";
        strQuery += (검색질문식 != "") ? " AND TBL_Charge.UserID IN (SELECT ID FROM TBL_UserList WHERE " + 검색질문식 + ")" : "";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        dbManager.RunMQuery(cmd);

        strQuery = "DELETE TBL_Withdraw ";
        strQuery += "WHERE TBL_Withdraw.Status>0 AND TBL_Withdraw.RegDate >= @StartDate AND TBL_Withdraw.RegDate <= @EndDate ";
        strQuery += "AND (TBL_Withdraw.UserID IN (SELECT ID FROM TBL_UserList WHERE BonsaID=@ParentID) OR ";
        strQuery += "TBL_Withdraw.UserID IN (SELECT ID FROM TBL_UserList WHERE BuBonsaID=@ParentID) OR ";
        strQuery += "TBL_Withdraw.UserID IN (SELECT ID FROM TBL_UserList WHERE ChongpanID=@ParentID) OR ";
        strQuery += "TBL_Withdraw.UserID IN (SELECT ID FROM TBL_UserList WHERE MaejangID=@ParentID))";
        strQuery += (검색질문식 != "") ? " AND TBL_Withdraw.UserID IN (SELECT ID FROM TBL_UserList WHERE " + 검색질문식 + ")" : "";
        cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        dbManager.RunMQuery(cmd);

        BindDataSource();
    }

    public void btnExcel_OnClick(object sender, EventArgs e)
    {
        ExportExcel(grdList, "회원정산");
    }
}
