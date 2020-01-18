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

public partial class 입출금관리_회원별정산 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:업체페지:입출금관리:회원별정산:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:업체페지:입출금관리:회원별정산:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:업체페지:입출금관리:회원별정산:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:업체페지:입출금관리:회원별정산:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:업체페지:입출금관리:회원별정산:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:업체페지:입출금관리:회원별정산:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:업체페지:입출금관리:회원별정산:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:업체페지:입출금관리:회원별정산:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:업체페지:입출금관리:정산처리:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:업체페지:입출금관리:정산처리:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";

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

        string strQuery = "SELECT TBL_UserList.ID, (SELECT LoginID FROM TBL_UserList AS tmp WHERE ID=TBL_UserList.ID) AS LoginID, ";
        strQuery += "(SELECT GameMoney FROM TBL_UserList AS tmp WHERE ID=TBL_UserList.ID) AS GameMoney, ";
        strQuery += "(SELECT COUNT(TBL_Charge.id) FROM TBL_Charge WHERE UserID=TBL_UserList.ID AND TBL_Charge.RegDate >= @StartDate AND TBL_Charge.RegDate <= @EndDate AND TBL_Charge.Status>0) AS ChargeCount, ";
        strQuery += "(SELECT SUM(TBL_Charge.Money) FROM TBL_Charge WHERE UserID=TBL_UserList.ID AND TBL_Charge.RegDate >= @StartDate AND TBL_Charge.RegDate <= @EndDate AND TBL_Charge.Status>0) AS ChargeMoney, ";
        strQuery += "(SELECT COUNT(TBL_Withdraw.id) FROM TBL_Withdraw WHERE UserID=TBL_UserList.ID AND TBL_Withdraw.RegDate >= @StartDate AND TBL_Withdraw.RegDate <= @EndDate AND TBL_Withdraw.Status>0) AS WithdrawCount, ";
        strQuery += "(SELECT SUM(TBL_Withdraw.Money) FROM TBL_Withdraw WHERE UserID=TBL_UserList.ID AND TBL_Withdraw.RegDate >= @StartDate AND TBL_Withdraw.RegDate <= @EndDate AND TBL_Withdraw.Status>0) AS WithdrawMoney ";
        strQuery += "FROM TBL_UserList  GROUP BY ALL ID, ParentID HAVING ParentID=" + 인증회원번호.ToString();

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
        페지당현시개수 = 20;
        grdList.PageSize = 페지당현시개수;
        grdList.PageIndex = 현재페지위치;
        grdList.DataBind();
        // 합계계산
        if (검색정보들 != null)
        {
            if (grdList.FooterRow != null)
            {
                grdList.FooterRow.Cells[0].Text = "합계 전체회원:" + 검색정보들.Tables[0].Rows.Count.ToString() + "명";
                long GameMoneySum = 0;
                long chargeCount = 0;
                long chargeSum = 0;
                long withdrawSum = 0;
                long withdrawCount = 0;
                long benefitSum = 0;
                for (int i = 0; i < 검색정보들.Tables[0].Rows.Count; i++)
                {
                    GameMoneySum += (검색정보들.Tables[0].Rows[i]["GameMoney"] == DBNull.Value) ? 0 : Convert.ToInt64(검색정보들.Tables[0].Rows[i]["GameMoney"]);
                    chargeCount += (검색정보들.Tables[0].Rows[i]["ChargeCount"] == DBNull.Value) ? 0 : Convert.ToInt64(검색정보들.Tables[0].Rows[i]["ChargeCount"]);
                    chargeSum += (검색정보들.Tables[0].Rows[i]["ChargeMoney"] == DBNull.Value) ? 0 : Convert.ToInt64(검색정보들.Tables[0].Rows[i]["ChargeMoney"]);
                    withdrawCount += (검색정보들.Tables[0].Rows[i]["WithdrawCount"] == DBNull.Value) ? 0 : Convert.ToInt64(검색정보들.Tables[0].Rows[i]["WithdrawCount"]);
                    withdrawSum += (검색정보들.Tables[0].Rows[i]["WithdrawMoney"] == DBNull.Value) ? 0 : Convert.ToInt64(검색정보들.Tables[0].Rows[i]["WithdrawMoney"]);
                    benefitSum += chargeSum - withdrawSum;
                }
                grdList.FooterRow.Cells[1].Text = GameMoneySum.ToString("N0") + "알";
                grdList.FooterRow.Cells[2].Text = chargeSum.ToString("N0") + "";
                grdList.FooterRow.Cells[3].Text = chargeCount.ToString("N0") + "회";
                grdList.FooterRow.Cells[4].Text = withdrawSum.ToString("N0") + "";
                grdList.FooterRow.Cells[5].Text = withdrawCount.ToString("N0") + "회";
                grdList.FooterRow.Cells[6].Text = benefitSum.ToString("N0") + "";
                grdList.FooterRow.Cells[7].Text = (benefitSum * double.Parse(인증회원.Tables[0].Rows[0]["ClassPercent"].ToString()) / 100) .ToString("N0") + "";
            }
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
        string strSearchKey = "LoginID";
        string strSearchValue = tbxSearchValue.Text;

        검색질문식 = strSearchKey + " LIKE '%" + strSearchValue + "%'";

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
        tbxSearchValue.Text = "";
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

        BindDataSource();
    }
}
