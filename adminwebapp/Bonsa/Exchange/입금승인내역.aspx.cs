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

public partial class 입출금관리_입금승인내역 : PageBase
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
        KEY_AUTOREFRESH = "CACHE:NEWBACARA:본사:업체페지:입출금관리:입금승인내역:자동갱신:";
        KEY_CACHEINFOS = "CACHE:NEWBACARA:본사:업체페지:입출금관리:입금승인내역:검색정보들:";
        KEY_CACHEQUERY = "CACHE:NEWBACARA:본사:업체페지:입출금관리:입금승인내역:검색질문식:";
        KEY_CACHESELINF = "CACHE:NEWBACARA:본사:업체페지:입출금관리:입금승인내역:선택된정보:";
        KEY_CACHESORT = "CACHE:NEWBACARA:본사:업체페지:입출금관리:입금승인내역:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:본사:업체페지:입출금관리:입금승인내역:정돈방향:";
        KEY_CACHEFILTER = "CACHE:NEWBACARA:본사:업체페지:입출금관리:입금승인내역:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:NEWBACARA:본사:업체페지:입출금관리:입금승인내역:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:본사:업체페지:입출금관리:입금승인내역:첫날자:";
        KEY_CACHELASTDATE = "CACHE:NEWBACARA:본사:업체페지:입출금관리:입금승인내역:끝날자:";

        if (!IsPostBack)
        {
            검색질문식 = "";
            정돈항 = "ID";
            정돈방향 = SortDirection.Descending;

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

    public void BindDataSource()
    {
        if (검색정보들 != null)
        {
            검색정보들.Dispose();
            검색정보들 = null;
        }

        
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT TBL_Charge.*, TBL_UserList.*, TBL_USERLIST.name AS UserName,TBL_USERLIST.depositHolder AS DepositHolder, Ent.name EntName FROM TBL_Charge JOIN TBL_UserList ON TBL_UserList.ID = TBL_Charge.UserID ";
        strQuery += " LEFT JOIN TBL_Enterprise Ent ON Ent.id = TBL_UserList.parentid ";
        strQuery += "WHERE TBL_Charge.status>0 AND TBL_Charge.RegDate >= @StartDate AND TBL_Charge.RegDate <= @EndDate ";
        strQuery += " AND (TBL_UserList.BonsaID=@ParentID OR TBL_UserList.BuBonsaID=@ParentID OR TBL_UserList.ChongpanID=@ParentID OR TBL_UserList.MaejangID=@ParentID)";

        /* 2017-03-31 입금유형구분한다 **/
        string status = ddlChargeType.SelectedValue;
        if (string.IsNullOrEmpty(status) == false)
        {
            strQuery += " and TBL_Charge.status=" + status;
        }

        string strEnt = ddlBubonsa.SelectedValue;
        if (string.IsNullOrEmpty(strEnt) == false && strEnt.Equals("0") == false)
        {
            strQuery += " and Ent.id=" + strEnt;
        }
        //lblQuery.Text = "sv:" + ddlBonsa.SelectedValue;
        
        
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@Partner", 인증회원.Tables[0].Rows[0]["Partner"].ToString()));
        DataSet dsChargeList = dbManager.RunMSelectQuery(cmd);
        검색정보들 = dsChargeList;
    }

    void BindBubonsa(int nBonsaID)
    {
        string strQuery = "";
        if (nBonsaID == 0)
        {
            //strQuery = "SELECT * FROM TBL_Enterprise WHERE Class=" + PageBase.ROLES_BUBONSA;
            strQuery = "SELECT * FROM TBL_Enterprise WHERE Class<=4 and use_yn='1'";
        }
        else
        {
            //strQuery = "SELECT * FROM TBL_Enterprise WHERE Class=" + PageBase.ROLES_BUBONSA + " AND BonsaID=" + nBonsaID.ToString();
            strQuery = "SELECT * FROM TBL_Enterprise WHERE Class<=4 and use_yn='1'";
        }
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

        long moneySum = 0;

        DataSet src = 검색정보얻기(검색질문식);
        if (src != null && (src.Tables[0] != null) && (src.Tables[0].Rows != null) && src.Tables[0].Rows.Count > 0)
        {
            DataView dv = src.Tables[0].DefaultView;
            string strTmpSort = (정돈방향 == SortDirection.Ascending) ? 정돈항 : 정돈항 + " DESC";
            dv.Sort = strTmpSort;

            grdList.DataSource = dv;
            lblRowCount.Text = src.Tables[0].Rows.Count.ToString();

            for (int i = 0; i < src.Tables[0].Rows.Count; i++)
            {
                moneySum += Convert.ToInt64(src.Tables[0].Rows[i]["money"]);
            }
        }
        else
        {
            lblRowCount.Text = "0";
            grdList.DataSource = null;
        }
        grdList.PageSize = 페지당현시개수;
        grdList.PageIndex = 현재페지위치;
        grdList.DataBind();

        if (src != null && (src.Tables[0] != null) && (src.Tables[0].Rows != null) && src.Tables[0].Rows.Count > 0)
        {
            grdList.FooterRow.Cells[0].Text = "합계";
            grdList.FooterRow.Cells[6].Text = moneySum.ToString("N0");
        }

        lblSortExpr.Text = "《" + 정돈항 + "》";
        pnlDescSortLbl.Visible = 정돈방향 == SortDirection.Descending;
        pnlAscSortLbl.Visible = 정돈방향 == SortDirection.Ascending;
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
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_Charge WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        int nResult = dbManager.RunMQuery(cmd);
        BindDataSource();

    }
    #endregion

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strSearchKey = ddlSearchKey.SelectedValue;
        string strSearchValue = tbxSearchValue.Text;

        검색질문식 = strSearchKey + " LIKE '%" + strSearchValue + "%'";

       // ddlBonsa.SelectedIndex = 0;
        //ddlBubonsa.SelectedIndex = 0;
        ddlChongPan.SelectedIndex = 0;
        ddlMaejang.SelectedIndex = 0;

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
        ddlSearchKey.SelectedIndex = 0;
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
                string strQuery = "DELETE TBL_Charge WHERE ID=" + 검색정보들.Tables[0].Rows[i]["ID"].ToString();
                SqlCommand cmd = new SqlCommand(strQuery);
                int nResult = dbManager.RunMQuery(cmd);
            }
            BindDataSource();
        }
    }

    protected void ddlBonsa_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbxSearchValue.Text = "";
        검색질문식 = "";
        if (ddlBonsa.SelectedIndex > 0)
            검색질문식 = "TBL_UserList.BonsaID=" + ddlBonsa.SelectedValue;

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
        tbxSearchValue.Text = "";
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
        tbxSearchValue.Text = "";
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
        tbxSearchValue.Text = "";
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
    }

    protected void btnExcel_OnClick(object sender, EventArgs e)
    {
        ExportExcel(grdList, "입금승인내역");
    }
}
