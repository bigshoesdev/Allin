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

public partial class 업체입출금관리_업체별정산 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:업체입출금관리:업체별정산:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:업체입출금관리:업체별정산:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:업체입출금관리:업체별정산:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:업체입출금관리:업체별정산:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:업체입출금관리:업체별정산:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:업체입출금관리:업체별정산:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:업체입출금관리:업체별정산:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:업체입출금관리:업체별정산:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:업체입출금관리:정산처리:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:업체입출금관리:정산처리:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";

            BindDataSource();
            BindPartner();
        }

        btnViewByDate.Visible = false;
    }

    void BindPartner()
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_Enterprise WHERE ParentID=" + 인증회원번호 + " or id=" + 인증회원번호;
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsEnterprise = dbManager.RunMSelectQuery(cmd);
        ddlEnterprise.Items.Add(new ListItem("전체", "0:0"));
        if (dsEnterprise.Tables.Count > 0)
        {
            for (int i = 0; i < dsEnterprise.Tables[0].Rows.Count; i++)
            {
                ddlEnterprise.Items.Add(new ListItem(dsEnterprise.Tables[0].Rows[i]["name"].ToString(),
                    dsEnterprise.Tables[0].Rows[i]["loginid"].ToString() + ":" + dsEnterprise.Tables[0].Rows[i]["ClassPercent"].ToString()));
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

        /*
        string strQuery = "SELECT TBL_Enterprise.ID, LoginID, name, money, TBL_Enterprise.classpercent, ";
        strQuery += "(SELECT SUM(gamemoney) FROM TBL_USERLIST WHERE parentid=TBL_Enterprise.id) AS game_money, ";
		strQuery += "(SELECT COUNT(TBL_Charge.id) FROM TBL_Charge WHERE EnterpriseID=TBL_Enterprise.ID AND TBL_Charge.RegDate >= @StartDate AND TBL_Charge.RegDate <= @EndDate AND TBL_Charge.State>0) AS ChargeCount, ";
		strQuery += "(SELECT SUM(TBL_Charge.Money) FROM TBL_Charge WHERE EnterpriseID=TBL_Enterprise.ID AND TBL_Charge.RegDate >= @StartDate AND TBL_Charge.RegDate <= @EndDate AND TBL_Charge.State>0) AS ChargeMoney, ";
		strQuery += "(SELECT COUNT(TBL_Withdraw.id) FROM TBL_Withdraw WHERE EnterpriseID=TBL_Enterprise.ID AND TBL_Withdraw.RegDate >= @StartDate AND TBL_Withdraw.RegDate <= @EndDate AND TBL_Withdraw.State>0 ) AS WithdrawCount, ";
		strQuery += "(SELECT SUM(TBL_Withdraw.Money) FROM TBL_Withdraw WHERE EnterpriseID=TBL_Enterprise.ID AND TBL_Withdraw.RegDate >= @StartDate AND TBL_Withdraw.RegDate <= @EndDate AND TBL_Withdraw.State>0 ) AS WithdrawMoney ";
        strQuery += "FROM TBL_Enterprise WHERE class<=4 ";
         * */
        string strQuery = "select ent.id, ent.[name], isnull(Charge.chargeSum, 0) c_sum, isnull(Charge.chargeCount, 0) c_count, "
                //+" (SELECT SUM(gamemoney) FROM TBL_USERLIST WHERE parentid=ent.id) AS game_money, "
                +" isnull(Withdraw.withdrawSum, 0) w_sum , isnull(Withdraw.withdrawCount, 0) w_count, ent.classpercent,"
                +" isnull(Charge.chargeSum, 0) - isnull(Withdraw.withdrawSum, 0) as fee,"
                +" (isnull(Charge.chargeSum, 0) - isnull(Withdraw.withdrawSum, 0)) * ent.classpercent/100 ent_fee,"
                + " isnull(Charge.chargeSum, 0) - isnull(Withdraw.withdrawSum, 0) - ((isnull(Charge.chargeSum, 0) - isnull(Withdraw.withdrawSum, 0)) * ent.classpercent/100) real_fee"
                +" from tbl_enterprise ent "
                +" left join "
                +" ("
                +"  select sum(isnull(C.[money],0)) chargeSum, count(*) chargeCount, U.parentid  as parentid"
                +"  from tbl_charge C"
                +"  left join TBL_USERLIST U on C.userid = U.id"
                +"  where C.status > 0 and C.update_time between @StartDate and @EndDate "  
                +"  group by U.parentid  "
                +" ) Charge on Charge.parentid = ent.id"
                +" left join "
                +" ("
                +"  select sum(isnull(W.[money],0)) withdrawSum, count(*) withdrawCount, U.parentid as parentid"
                +"  from tbl_withdraw W"
                +"  left join TBL_USERLIST U on W.userid = U.id"
                + "  where W.status > 0 and W.update_time between @StartDate and @EndDate " 
                + "  group by U.parentid"
                +") Withdraw on ent.id = Withdraw.parentid WHERE ent.class<=4 ";
        if(GetClass() < 4)
            strQuery += " and (ent.ParentID=" + 인증회원번호 + " or ent.id=" + 인증회원번호 + ")";

        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
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
                grdList.FooterRow.Cells[0].Text = "합계 전체업체:" + src.Tables[0].Rows.Count.ToString() + "명";
                long chargeCount = 0;
                long chargeSum = 0;
                long withdrawSum = 0;
                long withdrawCount = 0;
                long ent_fee_sum = 0;
                long real_fee_sum = 0;
                for (int i = 0; i < src.Tables[0].Rows.Count; i++)
                {
                    //moneySum += (src.Tables[0].Rows[i]["game_money"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["game_money"]);
                    chargeSum += (src.Tables[0].Rows[i]["c_sum"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["c_sum"]);
                    chargeCount += (src.Tables[0].Rows[i]["c_count"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["c_count"]);
                    withdrawSum += (src.Tables[0].Rows[i]["w_sum"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["w_sum"]);
                    withdrawCount += (src.Tables[0].Rows[i]["w_count"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["w_count"]);
                    ent_fee_sum += (src.Tables[0].Rows[i]["ent_fee"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["ent_fee"]);
                    real_fee_sum += (src.Tables[0].Rows[i]["real_fee"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["real_fee"]);
                }
                //grdList.FooterRow.Cells[1].Text = moneySum.ToString("N0");
                grdList.FooterRow.Cells[1].Text = chargeSum.ToString("N0");
                grdList.FooterRow.Cells[2].Text = chargeCount.ToString("N0") + "회";
                grdList.FooterRow.Cells[3].Text = withdrawSum.ToString("N0");
                grdList.FooterRow.Cells[4].Text = withdrawCount.ToString("N0") + "회";
                grdList.FooterRow.Cells[5].Text = (chargeSum - withdrawSum).ToString("N0");
                grdList.FooterRow.Cells[7].Text = (ent_fee_sum).ToString("N0");
                grdList.FooterRow.Cells[8].Text = (real_fee_sum).ToString("N0");
                
            }

            //grdList.Columns[1].Visible = false;
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
        BindDataSource();
    }
    protected void btnViewByUser_Click(object sender, EventArgs e)
    {
        Response.Redirect("업체정산처리.aspx");
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
    protected void ddlEnterprise_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEnterprise.SelectedIndex == 0)
        {
            검색질문식 = "";
        }
        else
        {
            검색질문식 = "LoginID='" + ddlEnterprise.SelectedItem.Text + "'";
        }
        BindDataSource();
    }
}
