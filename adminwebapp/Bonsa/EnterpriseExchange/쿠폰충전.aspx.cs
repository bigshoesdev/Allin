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

public partial class 업체입출금관리_쿠폰충전 : PageBase
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

        KEY_CACHEINFOS = "CACHE:KCCSITE:업체입출금관리:쿠폰충전:검색정보들:";
        KEY_CACHEQUERY = "CACHE:KCCSITE:업체입출금관리:쿠폰충전:검색질문식:";
        KEY_CACHESELINF = "CACHE:KCCSITE:업체입출금관리:쿠폰충전:선택된정보:";
        KEY_CACHESORT = "CACHE:KCCSITE:업체입출금관리:쿠폰충전:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:KCCSITE:업체입출금관리:쿠폰충전:정돈방향:";
        KEY_CACHEFILTER = "CACHE:KCCSITE:업체입출금관리:쿠폰충전:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:KCCSITE:업체입출금관리:쿠폰충전:현재페지위치:";

        if (!IsPostBack)
        {
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        GridDataBind();
    }

    void GridDataBind()
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT TBL_ECharge.*, (SELECT bankname FROM TBL_Enterprise where class=4) as bankname, (SELECT top(1) banknum FROM TBL_Enterprise where class=4) as banknum, (SELECT top(1) mastername FROM TBL_Enterprise where class=4) as mastername FROM TBL_ECharge JOIN TBL_Enterprise ON TBL_Enterprise.ID = TBL_ECharge.EnterpriseID AND TBL_Enterprise.ID=" + 인증회원번호 + " WHERE TBL_ECharge.chargetype = 1";
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
        string admin = "업체쿠폰충전";
        string name = 인증회원.Tables[0].Rows[0]["name"].ToString();
        string memo = "";
        Int64 money = int.Parse(tbxChargeMoney.Text);
        if (money < 0) return;

        DBManager dbManager = new DBManager();
        string strQuery = "INSERT INTO TBL_ECharge(EnterpriseID, Money, Memo, State, chargetype) ";
        strQuery += "VALUES(@EnterpriseID, @Money, @Memo, 0, 1)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@EnterpriseID", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@Money", money));
        cmd.Parameters.Add(new SqlParameter("@Memo", memo));
        try
        {
            dbManager.RunMQuery(cmd);
            cvChargeResult.ErrorMessage = "쿠폰충전요청완료";
        }
        catch (Exception ex)
        {
            cvChargeResult.ErrorMessage = "쿠폰충전실패." + ex.ToString();
        }
        cvChargeResult.IsValid = false;
        tbxChargeMoney.Text = "";
    }
}
