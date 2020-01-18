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

public partial class 업체입출금관리_코인충전 : PageBase
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

        KEY_CACHEINFOS = "CACHE:KCCSITE:업체입출금관리:코인충전:검색정보들:";
        KEY_CACHEQUERY = "CACHE:KCCSITE:업체입출금관리:코인충전:검색질문식:";
        KEY_CACHESELINF = "CACHE:KCCSITE:업체입출금관리:코인충전:선택된정보:";
        KEY_CACHESORT = "CACHE:KCCSITE:업체입출금관리:코인충전:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:KCCSITE:업체입출금관리:코인충전:정돈방향:";
        KEY_CACHEFILTER = "CACHE:KCCSITE:업체입출금관리:코인충전:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:KCCSITE:업체입출금관리:코인충전:현재페지위치:";

        if (!IsPostBack)
        {
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        GridDataBind();
    }

    /// <summary>
    ///		목록 및 양식 대장의 현재 페지위치를 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    /// </summary>
    public int 현재페지위치1
    {
        get
        {
            try
            {
                return (int)Session["CoinWithDrawPage"];
            }
            catch
            {
                return CF.iNothing;
            }
        }
        set
        {
            if (value == CF.iNothing)
                Session.Remove("CoinWithDrawPage");
            else
                Session["CoinWithDrawPage"] = value;
        }
    }

    void GridDataBind()
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT TBL_ECharge.*, (SELECT bankname FROM TBL_Enterprise where class=4) as bankname, (SELECT top(1) banknum FROM TBL_Enterprise where class=4) as banknum, (SELECT top(1) mastername FROM TBL_Enterprise where class=4) as mastername FROM TBL_ECharge JOIN TBL_Enterprise ON TBL_Enterprise.ID = TBL_ECharge.EnterpriseID AND TBL_Enterprise.ID=" + 인증회원번호;
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        DataSet dsChargeList = dbManager.RunMSelectQuery(cmd);
        if (dsChargeList != null)
        {
            DataView dv = dsChargeList.Tables[0].DefaultView;
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

        strQuery = "SELECT * FROM TBL_EWithdraw JOIN TBL_Enterprise ON TBL_Enterprise.ID = TBL_EWithdraw.EnterpriseID AND TBL_Enterprise.ID=" + 인증회원번호;
        cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        DataSet dsWithdrawList = dbManager.RunMSelectQuery(cmd);
        if (dsWithdrawList != null)
        {
            DataView dv1 = dsWithdrawList.Tables[0].DefaultView;
            string strTmpSort1 = (정돈방향 == SortDirection.Ascending) ? 정돈항 : 정돈항 + " DESC";
            dv1.Sort = strTmpSort1;
            grdWithdrawList.DataSource = dv1;
        }
        else
        {
            grdWithdrawList.DataSource = null;
        }
        grdWithdrawList.PageSize = 페지당현시개수;
        grdWithdrawList.PageIndex = 현재페지위치1;
        grdWithdrawList.DataBind();
    }

    #region 그리드사건처리부
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
    protected void grdLisTBL_RowEditing(object sender, GridViewEditEventArgs e)
    {
        
    }
    protected void grdLisTBL_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void grdWithdrawLisTBL_RowDataBound(object sender, GridViewRowEventArgs e)
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

            lnkNo.Text = (grdWithdrawList.PageIndex * grdWithdrawList.PageSize + e.Row.RowIndex + 1).ToString();
        }

    }
    protected void grdWithdrawLisTBL_Sorting(object sender, GridViewSortEventArgs e)
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
    protected void grdWithdrawLisTBL_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        현재페지위치1 = e.NewPageIndex;
    }
    protected void grdWithdrawLisTBL_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void grdWithdrawLisTBL_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion
    
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
    }
    protected void btnAllDelete_Click(object sender, EventArgs e)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            
        }
    }
    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        grdList.SelectedIndex = 현재페지위치 % 페지당현시개수;
        현재페지위치 = 현재페지위치 / 페지당현시개수;

        PageViewMode = ViewMode.목록보기;
        PageEditMode = DetailsViewMode.ReadOnly;
    }

    protected void btnCharge_Click(object sender, EventArgs e)
    {
        string admin = "업체코인충전";
        string name = 인증회원.Tables[0].Rows[0]["name"].ToString();
        string memo = "";
        Int64 money = int.Parse(tbxChargeMoney.Text);
        if (money < 0) return;

        DBManager dbManager = new DBManager();
        string strQuery = "INSERT INTO TBL_ECharge(EnterpriseID, Money, Memo, State) ";
        strQuery += "VALUES(@EnterpriseID, @Money, @Memo, 0)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@EnterpriseID", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@Money", money));
        cmd.Parameters.Add(new SqlParameter("@Memo", memo));
        try
        {
            dbManager.RunMQuery(cmd);
            cvChargeResult.ErrorMessage = "코인충전요청완료";
        }
        catch (Exception ex)
        {
            cvChargeResult.ErrorMessage = "코인충전실패." + ex.ToString();
        }
        cvChargeResult.IsValid = false;
        tbxChargeMoney.Text = "";
    }

    protected void btnWithdraw_Click(object sender, EventArgs e)
    {
        string admin = "업체코인환전";
        string name = 인증회원.Tables[0].Rows[0]["name"].ToString(); ;
        string memo = "";
        Int64 money = int.Parse(tbxWithdrawMoney.Text);
        if (money < 0) return;

        if (long.Parse(인증회원.Tables[0].Rows[0]["money"].ToString()) < money)
        {
            cvWithdrawResult.ErrorMessage = "캐롯부족";
            cvWithdrawResult.IsValid = false;
            return;
        }

        DBManager dbManager = new DBManager();

        // 1. 업체머니 감소
        string strQuery = "UPDATE TBL_Enterprise SET money=money-@money WHERE id=" + 인증회원.Tables[0].Rows[0]["id"].ToString();
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add("@money", SqlDbType.BigInt);
        cmd.Parameters["@money"].Value = money;
        try
        {
            dbManager.RunMQuery(cmd);
        }
        catch
        {
            cvWithdrawResult.ErrorMessage = "코인환전실패.";
            cvWithdrawResult.IsValid = false;
            return;
        }

        인증회원.Tables[0].Rows[0]["money"] = long.Parse(인증회원.Tables[0].Rows[0]["money"].ToString()) - money;
        
        strQuery = "INSERT INTO TBL_EWithdraw(EnterpriseID, Name, Money, Memo, State) ";
        strQuery += "VALUES(@EnterpriseID, @Name, @Money, @Memo, 0)";
        cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@EnterpriseID", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@Name", name));
        cmd.Parameters.Add(new SqlParameter("@Money", money));
        cmd.Parameters.Add(new SqlParameter("@Memo", memo));
        try
        {
            dbManager.RunMQuery(cmd);
            cvWithdrawResult.ErrorMessage = "코인환전요청완료";
        }
        catch (Exception ex)
        {
            cvWithdrawResult.ErrorMessage = "환전에 실패하였습니다." + ex.ToString();
        }
        cvWithdrawResult.IsValid = false;
        tbxWithdrawMoney.Text = "";
    }
}
