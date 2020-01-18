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

public partial class 게임관리_회원별정산 : PageBase
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

        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:게임관리:회원별정산:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:게임관리:회원별정산:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:게임관리:회원별정산:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:게임관리:회원별정산:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:게임관리:회원별정산:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:게임관리:회원별정산:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:게임관리:회원별정산:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:게임관리:회원별정산:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:게임관리:정산처리:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:게임관리:정산처리:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";

            string loginid = Request.QueryString["loginid"];
            if (string.IsNullOrEmpty(loginid) == false)
            {
                tbxUserId.Text = loginid;
                검색질문식 += " loginid ='" + loginid + "'";
                GridDataBind();
            }

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
        string s_date = 첫날자.ToString("yyyy-MM-dd");
        string e_date = 끝날자.ToString("yyyy-MM-dd");
        string strQuery = "SELECT *, (select name from tbl_enterprise where id=TBL_UserList.parentid) as EntName, ";
        strQuery += "(select sum(betMoneySum) from dbo.F_GET_POKER_GAME_SUMMARY('" + s_date + "', '" + e_date + "')  where id=TBL_UserList.ID) as poker_bet,";
        strQuery += "(select sum(betMoneySum) from dbo.F_GET_BADUKI_GAME_SUMMARY('" + s_date + "', '" + e_date + "') where id=TBL_UserList.ID) as baduki_bet,";
        strQuery += "(select sum(betMoneySum) from dbo.F_GET_MATGO_GAME_SUMMARY('" + s_date + "', '" + e_date + "')  where id=TBL_UserList.ID) as matgo_bet,";

        strQuery += "(select classpercent from tbl_enterprise where id=TBL_UserList.parentid) as class_percent, ";
        strQuery += "(SELECT COUNT(TBL_Charge.id) FROM TBL_Charge WHERE UserID=TBL_UserList.ID AND TBL_Charge.RegDate >= @StartDate AND TBL_Charge.RegDate <= @EndDate AND TBL_Charge.Status>0) AS ChargeCount, ";
        strQuery += "(SELECT SUM(TBL_Charge.Money) FROM TBL_Charge WHERE UserID=TBL_UserList.ID AND TBL_Charge.RegDate >= @StartDate AND TBL_Charge.RegDate <= @EndDate AND TBL_Charge.Status>0) AS ChargeMoney, ";
        strQuery += "(SELECT COUNT(TBL_Withdraw.id) FROM TBL_Withdraw WHERE UserID=TBL_UserList.ID AND TBL_Withdraw.RegDate >= @StartDate AND TBL_Withdraw.RegDate <= @EndDate AND TBL_Withdraw.Status>0 ) AS WithdrawCount, ";
        strQuery += "(SELECT SUM(TBL_Withdraw.Money) FROM TBL_Withdraw WHERE UserID=TBL_UserList.ID AND TBL_Withdraw.RegDate >= @StartDate AND TBL_Withdraw.RegDate <= @EndDate AND TBL_Withdraw.Status>0 ) AS WithdrawMoney ";
        strQuery += "FROM TBL_UserList  WHERE ";
        strQuery += "(BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@Partner", 인증회원.Tables[0].Rows[0]["Partner"].ToString()));
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

        DataSet src = 검색정보얻기(검색질문식);
        if (src != null)
        {
            DataView dv = src.Tables[0].DefaultView;
            string strTmpSort = (정돈방향 == SortDirection.Ascending) ? 정돈항 : 정돈항 + " DESC";
            dv.Sort = strTmpSort;
            grdList.DataSource = dv;
        }
        else
        {
            grdList.DataSource = null;
        }
        
        grdList.PageSize = 페지당현시개수;
        grdList.PageIndex = 현재페지위치;
        grdList.DataBind();
        // 합계계산
        if (src != null)
        {
            if (grdList.FooterRow != null)
            {
                grdList.FooterRow.Cells[0].Text = "합계 전체회원:" + src.Tables[0].Rows.Count.ToString() + "명";
                long moneySum = 0;
                long chargeCount = 0;
                long chargeSum = 0;
                long withdrawSum = 0;
                long withdrawCount = 0;
                long poker_betSum = 0;
                long baduki_betSum = 0;
                long matgo_betSum = 0;

                for (int i = 0; i < src.Tables[0].Rows.Count; i++)
                {
                    moneySum += (src.Tables[0].Rows[i]["gamemoney"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["gamemoney"]);
                    chargeSum += (src.Tables[0].Rows[i]["ChargeMoney"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["ChargeMoney"]);
                    chargeCount += (src.Tables[0].Rows[i]["ChargeCount"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["ChargeCount"]);
                    withdrawSum += (src.Tables[0].Rows[i]["WithdrawMoney"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["WithdrawMoney"]);
                    withdrawCount += (src.Tables[0].Rows[i]["WithdrawCount"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["WithdrawCount"]);
                    poker_betSum += (src.Tables[0].Rows[i]["poker_bet"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["poker_bet"]);
                    baduki_betSum += (src.Tables[0].Rows[i]["baduki_bet"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["baduki_bet"]);
                    matgo_betSum += (src.Tables[0].Rows[i]["matgo_bet"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["matgo_bet"]);

                }
                int idx = 0;
                grdList.FooterRow.Cells[idx + 2].Text = poker_betSum.ToString("N0");
                grdList.FooterRow.Cells[idx + 3].Text = baduki_betSum.ToString("N0");
                grdList.FooterRow.Cells[idx + 4].Text = matgo_betSum.ToString("N0");

                grdList.FooterRow.Cells[idx + 5].Text = moneySum.ToString("N0");
                grdList.FooterRow.Cells[idx + 6].Text = chargeSum.ToString("N0");
                grdList.FooterRow.Cells[idx + 7].Text = chargeCount.ToString("N0") + "회";
                grdList.FooterRow.Cells[idx + 8].Text = withdrawSum.ToString("N0");
                grdList.FooterRow.Cells[idx + 9].Text = withdrawCount.ToString("N0") + "회";
                grdList.FooterRow.Cells[idx + 11].Text = (chargeSum - withdrawSum - moneySum).ToString("N0");
                //if (ddlEnterprise.SelectedIndex > 0)
                //{
                //    int nClassPercent = int.Parse(ddlEnterprise.SelectedValue.Split(':')[1]);
                //    grdList.FooterRow.Cells[7].Text = ((chargeSum - withdrawSum) * nClassPercent / 100).ToString("C0");
                //}
            }
        }
    }
    void UpdateButton()
    {
        //grdList.Columns[grdList.Columns.Count - 1].Visible = (ddlEnterprise.SelectedIndex > 0);
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
    #endregion

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        검색질문식 = "";

        string user_id = tbxUserId.Text.Trim();

        if (string.IsNullOrEmpty(user_id) == false)
        {
            검색질문식 += " loginid like '%" + user_id + "%'";
            //BindDataSource();
        }

        if (tbxStartDate.Text != "" && tbxEndDate.Text != "")
        {
            첫날자 = DateTime.Parse(tbxStartDate.Text + " 00:00:00");
            끝날자 = DateTime.Parse(tbxEndDate.Text + " 23:59:59");
            //BindDataSource();
        }
        BindDataSource();
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
        Response.Redirect("정산처리.aspx");
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
            검색질문식 = "parentid=" + ddlBonsa.SelectedValue;
            //검색질문식 = "BonsaID=" + ddlBonsa.SelectedValue;
        else
            검색질문식 = "";

        BindBubonsa(int.Parse(ddlBonsa.SelectedValue));
        ddlBubonsa.SelectedIndex = 0;

        BindChongpan(int.Parse(ddlBonsa.SelectedValue), int.Parse(ddlBubonsa.SelectedValue));
        ddlChongPan.SelectedIndex = 0;

        BindMaejang(int.Parse(ddlBonsa.SelectedValue), int.Parse(ddlBubonsa.SelectedValue), int.Parse(ddlChongPan.SelectedValue));
        ddlMaejang.SelectedIndex = 0;
    }
    protected void ddlBubonsa_SelectedIndexChanged(object sender, EventArgs e)
    {
        검색질문식 = "";
        if (ddlBonsa.SelectedIndex > 0)
            검색질문식 = "parentid=" + ddlBonsa.SelectedValue;
            //검색질문식 = "BonsaID=" + ddlBonsa.SelectedValue;

        if (ddlBubonsa.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "parentid=" + ddlBubonsa.SelectedValue;
            /*
             * 검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "BubonsaID=" + ddlBubonsa.SelectedValue; */
        }

        BindChongpan(int.Parse(ddlBonsa.SelectedValue), int.Parse(ddlBubonsa.SelectedValue));
        ddlChongPan.SelectedIndex = 0;

        BindMaejang(int.Parse(ddlBonsa.SelectedValue), int.Parse(ddlBubonsa.SelectedValue), int.Parse(ddlChongPan.SelectedValue));
        ddlMaejang.SelectedIndex = 0;
    }
    protected void ddlChongPan_SelectedIndexChanged(object sender, EventArgs e)
    {
        검색질문식 = "";
        if (ddlBonsa.SelectedIndex > 0)
            검색질문식 = "BonsaID=" + ddlBonsa.SelectedValue;
        if (ddlBubonsa.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "BubonsaID=" + ddlBubonsa.SelectedValue;
        }
        if (ddlChongPan.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "ChongpanID=" + ddlChongPan.SelectedValue;
        }

        BindMaejang(int.Parse(ddlBonsa.SelectedValue), int.Parse(ddlBubonsa.SelectedValue), int.Parse(ddlChongPan.SelectedValue));
        ddlMaejang.SelectedIndex = 0;
    }
    protected void ddlMaejang_SelectedIndexChanged(object sender, EventArgs e)
    {
        검색질문식 = "";
        if (ddlBonsa.SelectedIndex > 0)
            검색질문식 = "BonsaID=" + ddlBonsa.SelectedValue;

        if (ddlBubonsa.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "BubonsaID=" + ddlBubonsa.SelectedValue;
        }
        if (ddlChongPan.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "ChongpanID=" + ddlChongPan.SelectedValue;
        }
        if (ddlMaejang.SelectedIndex > 0)
        {
            검색질문식 += 검색질문식 != "" ? " AND " : 검색질문식;
            검색질문식 += "MaeJangID=" + ddlMaejang.SelectedValue;
        }
    }

    protected void btnExcel_OnClick(object sender, EventArgs e)
    {
        ExportExcel(grdList, "회원별정산");
    }
}
