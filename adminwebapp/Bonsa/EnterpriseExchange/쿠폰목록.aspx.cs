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
using System.IO;

public partial class 업체입출금관리_쿠폰목록 : PageBase
{
    public int 쿠폰목록현시방식
    {
        get
        {
            try
            {
                return (int)Session["CouponeListShowMode"];
            }
            catch
            {
                return CF.iNothing;
            }
        }
        set
        {
            if (value == CF.iNothing)
                Session.Remove("CouponeListShowMode");
            else
                Session["CouponeListShowMode"] = value;
        }
    }

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

        KEY_CACHEINFOS = "CACHE:KCCSITE:업체입출금관리:쿠폰목록:검색정보들:";
        KEY_CACHEQUERY = "CACHE:KCCSITE:업체입출금관리:쿠폰목록:검색질문식:";
        KEY_CACHESELINF = "CACHE:KCCSITE:업체입출금관리:쿠폰목록:선택된정보:";
        KEY_CACHESORT = "CACHE:KCCSITE:업체입출금관리:쿠폰목록:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:KCCSITE:업체입출금관리:쿠폰목록:정돈방향:";
        KEY_CACHEFILTER = "CACHE:KCCSITE:업체입출금관리:쿠폰목록:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:KCCSITE:업체입출금관리:쿠폰목록:현재페지위치:";

        grdList.Columns[grdList.Columns.Count - 1].Visible = true;
        if (!IsPostBack)
        {
            쿠폰목록현시방식 = 0; // 0:사용가능 1:불가능 2:전체
        }
        if (GetClass() < 4)
        {
            tdAdmin.Visible = false;
            grdList.Columns[grdList.Columns.Count - 1].Visible = false;
        }

        grdList.Columns[4].Visible = true;
        if (쿠폰목록현시방식 == 0)
        {
            btnShowUse.Enabled = false;
            grdList.Columns[4].Visible = false;
        }
        else if (쿠폰목록현시방식 == 1)
        {
            btnShowUnUse.Enabled = false;
            grdList.Columns[grdList.Columns.Count-1].Visible = false;
        }
        else
            btnShowAll.Enabled = false;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        GridDataBind();
    }

    void GridDataBind()
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_COUPON WHERE 1=1 ";
        if (GetClass() < 4)
            strQuery += " AND enterpriseid=@id";


        if (쿠폰목록현시방식 == 0)
        {
            strQuery += " AND userid is null";
        }
        else if (쿠폰목록현시방식 == 1)
        {
            strQuery += " AND userid is not null";
        }

        SqlCommand cmd = new SqlCommand(strQuery);
        if (GetClass() < 4)
            cmd.Parameters.Add(new SqlParameter("@id", 인증회원번호));

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
        DataRowView dr = (DataRowView)e.Row.DataItem;
        if (dr != null)
        {
            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            if (lnkDelete != null)
            {
                if (!string.IsNullOrEmpty(dr["userid"].ToString()))
                    lnkDelete.Enabled = false;
            }
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
        int rowIndex = e.RowIndex;
        LinkButton lnkNo = (LinkButton)grdList.Rows[e.RowIndex].FindControl("lnkNo");
        if (lnkNo == null)
        {
            return;
        }

        string strId = lnkNo.CommandArgument;
        if (strId == "") return;

        try
        {
            DBManager dbManager = new DBManager();
            string strQuery = "Delete FROM TBL_COUPON WHERE id=@id AND userid is null";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@id", strId));
            dbManager.RunMQuery(cmd);
        }
        catch (Exception ex)
        {
            cvResult.ErrorMessage = "쿠폰 삭제과정에 오류가 발생하였습니다." + ex.Message;
            cvResult.IsValid = false;
        }
    }
    #endregion
    
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        GridDataBind();
    }
    protected void btnAllDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DBManager dbManager = new DBManager();
            string strQuery = "Delete FROM TBL_COUPON WHERE userid is null";
            SqlCommand cmd = new SqlCommand(strQuery);
            dbManager.RunMQuery(cmd);
        }
        catch (Exception ex)
        {
            cvResult.ErrorMessage = "쿠폰목록 삭제과정에 오류가 발생하였습니다." + ex.Message;
            cvResult.IsValid = false;
        }
    }
    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        grdList.SelectedIndex = 현재페지위치 % 페지당현시개수;
        현재페지위치 = 현재페지위치 / 페지당현시개수;

        PageViewMode = ViewMode.목록보기;
        PageEditMode = DetailsViewMode.ReadOnly;
    }

    protected void btnRegCoupon_Click(object sender, EventArgs e)
    {
        try
        {
            string strCouponName = tbxCouponName.Text.Trim();
            string money = ddlCouponMoney.SelectedValue;
            DBManager dbManager = new DBManager();

            string strQuery = "INSERT INTO TBL_COUPON(name, money) VALUES(@name, @money)";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
            cmd.Parameters.Add(new SqlParameter("@Money", money));
            dbManager.RunMQuery(cmd);
        }
        catch (Exception ex)
        {
            cvRegCouponResult.ErrorMessage = "쿠폰등록실패." + ex.ToString();
        }
        cvRegCouponResult.IsValid = false;
    }

    protected void btn10KList_Click(object sender, EventArgs e)
    {
        try
        {
            string strPath = Server.MapPath("10000.txt");
            if (!File.Exists(strPath))
            {
                cvRegCouponResult.ErrorMessage = "10000.txt 파일을 찾을수 없습니다.";
                cvRegCouponResult.IsValid = false;
                return;
            }

            StreamReader sr = new StreamReader(strPath, System.Text.Encoding.UTF8);
            string[] strCoupons = sr.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            sr.Close();
            int cntCoupons = strCoupons.Length;
            int recordCoupon = 0;
            for (int i = 0; i < cntCoupons; i++)
            {
                string strCouponName = strCoupons[i].Trim();
                if (!string.IsNullOrEmpty(strCouponName))
                {
                    DBManager dbManager = new DBManager();
                    string strQuery = "SELECT count(*) FROM TBL_COUPON WHERE name=@name";
                    SqlCommand cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                    DataSet dsResult = dbManager.RunMSelectQuery(cmd);

                    if(DataSetUtil.RowIntValue(dsResult, 0, 0) == 0)
                    {
                        strQuery = "INSERT INTO TBL_COUPON(name, money) VALUES(@name, 10000)";
                        cmd = new SqlCommand(strQuery);
                        cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                        dbManager.RunMQuery(cmd);
                        recordCoupon++;
                    }
                }
            }
            cvRegCouponResult.ErrorMessage = string.Format("{0}개중 {1}개등록", cntCoupons, recordCoupon);
        }
        catch (Exception ex)
        {
            cvRegCouponResult.ErrorMessage = "쿠폰등록실패." + ex.ToString();
        }
        cvRegCouponResult.IsValid = false;
    }
    protected void btn20KList_Click(object sender, EventArgs e)
    {
        try
        {
            string strPath = Server.MapPath("20000.txt");
            if (!File.Exists(strPath))
            {
                cvRegCouponResult.ErrorMessage = "20000.txt 파일을 찾을수 없습니다.";
                cvRegCouponResult.IsValid = false;
                return;
            }

            StreamReader sr = new StreamReader(strPath, System.Text.Encoding.UTF8);
            string[] strCoupons = sr.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            sr.Close();
            int cntCoupons = strCoupons.Length;
            int recordCoupon = 0;
            for (int i = 0; i < cntCoupons; i++)
            {
                string strCouponName = strCoupons[i].Trim();
                if (!string.IsNullOrEmpty(strCouponName))
                {
                    DBManager dbManager = new DBManager();
                    string strQuery = "SELECT count(*) FROM TBL_COUPON WHERE name=@name";
                    SqlCommand cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                    DataSet dsResult = dbManager.RunMSelectQuery(cmd);

                    if (DataSetUtil.RowIntValue(dsResult, 0, 0) == 0)
                    {
                        strQuery = "INSERT INTO TBL_COUPON(name, money) VALUES(@name, 10000)";
                        cmd = new SqlCommand(strQuery);
                        cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                        dbManager.RunMQuery(cmd);
                        recordCoupon++;
                    }
                }
            }
            cvRegCouponResult.ErrorMessage = string.Format("{0}개중 {1}개등록", cntCoupons, recordCoupon);
        }
        catch (Exception ex)
        {
            cvRegCouponResult.ErrorMessage = "쿠폰등록실패." + ex.ToString();
        }
        cvRegCouponResult.IsValid = false;
    }
    protected void btn50KList_Click(object sender, EventArgs e)
    {
        try
        {
            string strPath = Server.MapPath("50000.txt");
            if (!File.Exists(strPath))
            {
                cvRegCouponResult.ErrorMessage = "50000.txt 파일을 찾을수 없습니다.";
                cvRegCouponResult.IsValid = false;
                return;
            }

            StreamReader sr = new StreamReader(strPath, System.Text.Encoding.UTF8);
            string[] strCoupons = sr.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            sr.Close();
            int cntCoupons = strCoupons.Length;
            int recordCoupon = 0;
            for (int i = 0; i < cntCoupons; i++)
            {
                string strCouponName = strCoupons[i].Trim();
                if (!string.IsNullOrEmpty(strCouponName))
                {
                    DBManager dbManager = new DBManager();
                    string strQuery = "SELECT count(*) FROM TBL_COUPON WHERE name=@name";
                    SqlCommand cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                    DataSet dsResult = dbManager.RunMSelectQuery(cmd);

                    if (DataSetUtil.RowIntValue(dsResult, 0, 0) == 0)
                    {
                        strQuery = "INSERT INTO TBL_COUPON(name, money) VALUES(@name, 10000)";
                        cmd = new SqlCommand(strQuery);
                        cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                        dbManager.RunMQuery(cmd);
                        recordCoupon++;
                    }
                }
            }
            cvRegCouponResult.ErrorMessage = string.Format("{0}개중 {1}개등록", cntCoupons, recordCoupon);
        }
        catch (Exception ex)
        {
            cvRegCouponResult.ErrorMessage = "쿠폰등록실패." + ex.ToString();
        }
        cvRegCouponResult.IsValid = false;
    }
    protected void btn100KList_Click(object sender, EventArgs e)
    {
        try
        {
            string strPath = Server.MapPath("100000.txt");
            if (!File.Exists(strPath))
            {
                cvRegCouponResult.ErrorMessage = "100000.txt 파일을 찾을수 없습니다.";
                cvRegCouponResult.IsValid = false;
                return;
            }

            StreamReader sr = new StreamReader(strPath, System.Text.Encoding.UTF8);
            string[] strCoupons = sr.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            sr.Close();
            int cntCoupons = strCoupons.Length;
            int recordCoupon = 0;
            for (int i = 0; i < cntCoupons; i++)
            {
                string strCouponName = strCoupons[i].Trim();
                if (!string.IsNullOrEmpty(strCouponName))
                {
                    DBManager dbManager = new DBManager();
                    string strQuery = "SELECT count(*) FROM TBL_COUPON WHERE name=@name";
                    SqlCommand cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                    DataSet dsResult = dbManager.RunMSelectQuery(cmd);

                    if (DataSetUtil.RowIntValue(dsResult, 0, 0) == 0)
                    {
                        strQuery = "INSERT INTO TBL_COUPON(name, money) VALUES(@name, 10000)";
                        cmd = new SqlCommand(strQuery);
                        cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                        dbManager.RunMQuery(cmd);
                        recordCoupon++;
                    }
                }
            }
            cvRegCouponResult.ErrorMessage = string.Format("{0}개중 {1}개등록", cntCoupons, recordCoupon);
        }
        catch (Exception ex)
        {
            cvRegCouponResult.ErrorMessage = "쿠폰등록실패." + ex.ToString();
        }
        cvRegCouponResult.IsValid = false;
    }
    protected void btn500KList_Click(object sender, EventArgs e)
    {
        try
        {
            string strPath = Server.MapPath("500000.txt");
            if (!File.Exists(strPath))
            {
                cvRegCouponResult.ErrorMessage = "500000.txt 파일을 찾을수 없습니다.";
                cvRegCouponResult.IsValid = false;
                return;
            }

            StreamReader sr = new StreamReader(strPath, System.Text.Encoding.UTF8);
            string[] strCoupons = sr.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            sr.Close();
            int cntCoupons = strCoupons.Length;
            int recordCoupon = 0;
            for (int i = 0; i < cntCoupons; i++)
            {
                string strCouponName = strCoupons[i].Trim();
                if (!string.IsNullOrEmpty(strCouponName))
                {
                    DBManager dbManager = new DBManager();
                    string strQuery = "SELECT count(*) FROM TBL_COUPON WHERE name=@name";
                    SqlCommand cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                    DataSet dsResult = dbManager.RunMSelectQuery(cmd);

                    if (DataSetUtil.RowIntValue(dsResult, 0, 0) == 0)
                    {
                        strQuery = "INSERT INTO TBL_COUPON(name, money) VALUES(@name, 10000)";
                        cmd = new SqlCommand(strQuery);
                        cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                        dbManager.RunMQuery(cmd);
                        recordCoupon++;
                    }
                }
            }
            cvRegCouponResult.ErrorMessage = string.Format("{0}개중 {1}개등록", cntCoupons, recordCoupon);
        }
        catch (Exception ex)
        {
            cvRegCouponResult.ErrorMessage = "쿠폰등록실패." + ex.ToString();
        }
        cvRegCouponResult.IsValid = false;
    }
    protected void btn1MList_Click(object sender, EventArgs e)
    {
        try
        {
            string strPath = Server.MapPath("1000000.txt");
            if (!File.Exists(strPath))
            {
                cvRegCouponResult.ErrorMessage = "1000000.txt 파일을 찾을수 없습니다.";
                cvRegCouponResult.IsValid = false;
                return;
            }

            StreamReader sr = new StreamReader(strPath, System.Text.Encoding.UTF8);
            string[] strCoupons = sr.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            sr.Close();
            int cntCoupons = strCoupons.Length;
            int recordCoupon = 0;
            for (int i = 0; i < cntCoupons; i++)
            {
                string strCouponName = strCoupons[i].Trim();
                if (!string.IsNullOrEmpty(strCouponName))
                {
                    DBManager dbManager = new DBManager();
                    string strQuery = "SELECT count(*) FROM TBL_COUPON WHERE name=@name";
                    SqlCommand cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                    DataSet dsResult = dbManager.RunMSelectQuery(cmd);

                    if (DataSetUtil.RowIntValue(dsResult, 0, 0) == 0)
                    {
                        strQuery = "INSERT INTO TBL_COUPON(name, money) VALUES(@name, 10000)";
                        cmd = new SqlCommand(strQuery);
                        cmd.Parameters.Add(new SqlParameter("@name", strCouponName));
                        dbManager.RunMQuery(cmd);
                        recordCoupon++;
                    }
                }
            }
            cvRegCouponResult.ErrorMessage = string.Format("{0}개중 {1}개등록", cntCoupons, recordCoupon);
        }
        catch (Exception ex)
        {
            cvRegCouponResult.ErrorMessage = "쿠폰등록실패." + ex.ToString();
        }
        cvRegCouponResult.IsValid = false;
    }
    protected void btnShowUse_Click(object sender, EventArgs e)
    {
        btnShowUse.Enabled = true;
        btnShowUnUse.Enabled = true;
        btnShowAll.Enabled = true;
        쿠폰목록현시방식 = 0;
        GridDataBind();
    }
    protected void btnShowUnUse_Click(object sender, EventArgs e)
    {
        btnShowUse.Enabled = true;
        btnShowUnUse.Enabled = true;
        btnShowAll.Enabled = true;
        쿠폰목록현시방식 = 1;
        GridDataBind();
    }
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        btnShowUse.Enabled = true;
        btnShowUnUse.Enabled = true;
        btnShowAll.Enabled = true;
        쿠폰목록현시방식 = 2;
        GridDataBind();
    }
}
