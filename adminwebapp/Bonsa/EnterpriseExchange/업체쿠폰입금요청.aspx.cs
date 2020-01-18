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

public partial class 업체입출금관리_업체쿠폰입금요청 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:업체입출금관리:업체쿠폰입금요청목록:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:업체입출금관리:업체쿠폰입금요청목록:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:업체입출금관리:업체쿠폰입금요청목록:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:업체입출금관리:업체쿠폰입금요청목록:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:업체입출금관리:업체쿠폰입금요청목록:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:업체입출금관리:업체쿠폰입금요청목록:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:업체입출금관리:업체쿠폰입금요청목록:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:업체입출금관리:업체쿠폰입금요청목록:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:업체입출금관리:업체쿠폰입금요청목록:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:업체입출금관리:업체쿠폰입금요청목록:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";
            정돈항 = "ID";
            정돈방향 = SortDirection.Descending;

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
        string strQuery = "SELECT TBL_ECharge.*, TBL_Enterprise.loginid, TBL_Enterprise.name, TBL_Enterprise.money as emoney, (SELECT bankname FROM TBL_Enterprise where class=4) as bankname, (SELECT top(1) banknum FROM TBL_Enterprise where class=4) as banknum, (SELECT top(1) mastername FROM TBL_Enterprise where class=4) as mastername FROM TBL_ECharge JOIN TBL_Enterprise ON TBL_Enterprise.ID = TBL_ECharge.EnterpriseID WHERE TBL_ECharge.state=0 AND TBL_ECharge.chargetype=1";// AND TBL_Enterprise.ParentID=" + 인증회원번호;
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsChargeReqList = dbManager.RunMSelectQuery(cmd);
        검색정보들 = dsChargeReqList;
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
        grdList.PageSize = 페지당현시개수;
        grdList.PageIndex = 현재페지위치;
        grdList.DataBind();

        lblSortExpr.Text = "《" + 정돈항 + "》";
        pnlDescSortLbl.Visible = 정돈방향 == SortDirection.Descending;
        pnlAscSortLbl.Visible = 정돈방향 == SortDirection.Ascending;
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
        // 1. 업체쿠폰입금요청레코드를 삭제한다
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_ECharge WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        int nResult = dbManager.RunMQuery(cmd);

        BindDataSource();

    }
    protected void grdLisTBL_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int rowIndex = e.NewEditIndex;
        LinkButton lnkNo = (LinkButton)grdList.Rows[rowIndex].FindControl("lnkNo");
        if (lnkNo == null)
        {
            cvResult.ErrorMessage = "충전처리과정에 오류가 발생하였습니다.";
            cvResult.IsValid = false;
            return;
        }
        
        string strId = lnkNo.CommandArgument;
        if (strId == "") return;

        DBManager dbManager = new DBManager();
        string strQuery = "SELECT money FROM TBL_ECharge WHERE ID=" + strId;
        DataSet dsChargeInfo = dbManager.RunMSelectQuery(new SqlCommand(strQuery));
        if (dsChargeInfo.Tables.Count == 0 || dsChargeInfo.Tables[0].Rows.Count == 0) 
        {
            cvResult.ErrorMessage = "충전요청정보를 찾을수 없습니다.";
            cvResult.IsValid = false;
            return;
        }
        long nReqMoney = (long)dsChargeInfo.Tables[0].Rows[0]["money"];
        Response.Redirect("업체입금승인.aspx?id=" + strId +"&money="+DataSetUtil.RowStringValue(dsChargeInfo, "money", 0));
        return;

        /*if (nReqMoney > (long)인증회원.Tables[0].Rows[0]["money"])
        {
            cvResult.ErrorMessage = "충전가능금액을 초과합니다.";
            cvResult.IsValid = false;
            return;
        }

        // 0. 업체머니를 감소시킨다.
        strQuery = "UPDATE TBL_Enterprise SET ";
        strQuery += "money=money-" + nReqMoney;
        strQuery += " WHERE ID=" + 인증회원번호;
        dbManager.RunMQuery(new SqlCommand(strQuery));
        인증회원.Tables[0].Rows[0]["money"] = (long)인증회원.Tables[0].Rows[0]["money"] - nReqMoney;
        
        // 1. 요청기록을 승인상태로 갱신
        strQuery = "UPDATE TBL_ECharge SET state=1 WHERE ID=" + strId;
        dbManager.RunMQuery(new SqlCommand(strQuery));

        // 2. 회원의 캐쉬를 증가시킨다.
        string strUserID = lnkNo.ToolTip;
        strQuery = "UPDATE TBL_Enterprise SET ";
        strQuery += "money=money+" + nReqMoney;
        strQuery += " WHERE ID = " + strUserID;
        dbManager.RunMQuery(new SqlCommand(strQuery));
        BindDataSource();*/
    }
    #endregion

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        검색질문식 = "";
        BindDataSource();
    }
    protected void btnAllDelete_Click(object sender, EventArgs e)
    {
        if (검색정보들 != null && 검색정보들.Tables.Count > 0)
        {
            DBManager dbManager = new DBManager();
            System.Collections.Generic.List<string> strRoomIDList = new System.Collections.Generic.List<string>();
            for (int i = 0; i < 검색정보들.Tables[0].Rows.Count; i++)
            {
                string strQuery = "DELETE TBL_ECharge WHERE ID=" + 검색정보들.Tables[0].Rows[i]["ID"].ToString();
                SqlCommand cmd = new SqlCommand(strQuery);
                int nResult = dbManager.RunMQuery(cmd);
            }
            BindDataSource();
        }
    }
}
