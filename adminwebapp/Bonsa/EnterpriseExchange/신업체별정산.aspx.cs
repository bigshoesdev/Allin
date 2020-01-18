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

public partial class Bonsa_EnterpriseExchange_신업체별정산 : PageBase
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
    }

    void BindPartner()
    {
        DBManager dbManager = new DBManager();
        //string strQuery = "SELECT * FROM TBL_Enterprise WHERE ParentID=" + 인증회원번호;
        string strQuery = "SELECT * FROM TBL_Enterprise WHERE id=" + 인증회원번호 + " OR ParentID=" + 인증회원번호;
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsEnterprise = dbManager.RunMSelectQuery(cmd);
        ddlEnterprise.Items.Add(new ListItem("전체", "0:0"));
        if (dsEnterprise.Tables.Count > 0)
        {
            for (int i = 0; i < dsEnterprise.Tables[0].Rows.Count; i++)
            {
                ddlEnterprise.Items.Add(new ListItem(dsEnterprise.Tables[0].Rows[i]["name"].ToString(),
                    dsEnterprise.Tables[0].Rows[i]["loginid"].ToString() ));
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
        string _strquery = "select ent.*,(P.p_service_fee + B.b_service_fee + M.m_service_fee) service_fee, (P.p_ent_service_fee + B.b_ent_service_fee + M.m_ent_service_fee) ent_service_fee, P.p_betMoneySum, B.b_betMoneySum, M.m_betMoneySum, (P.p_betMoneySum+ B.b_betMoneySum+ M.m_betMoneySum) all_betMoneySum "
        + " from tbl_enterprise ent left join "
        + "(SELECT parentid, sum(game_count) p_game_count, sum(betMoneySum) p_betMoneySum, sum(service_fee) p_service_fee, sum(ent_service_fee) p_ent_service_fee FROM F_GET_POKER_GAME_SUMMARY('{0}','{1}') group by parentid) P on p.parentid = ent.id "
        + " left join (SELECT parentid, sum(game_count) b_game_count, sum(betMoneySum) b_betMoneySum, sum(service_fee) b_service_fee, sum(ent_service_fee) b_ent_service_fee FROM F_GET_baduki_GAME_SUMMARY('{0}','{1}') group by parentid) B on B.parentid = ent.id "
        + " left join (SELECT parentid, sum(game_count) m_game_count, sum(betMoneySum) m_betMoneySum, sum(service_fee) m_service_fee, sum(ent_service_fee) m_ent_service_fee FROM F_GET_MATGO_GAME_SUMMARY('{0}','{1}') group by parentid) M on M.parentid = ent.id "
        +" where ent.id={2} or ent.parentid={3} ";
        //string strQuery = string.Format("SELECT * FROM F_GET_ENT_GAME_SUMMARY('{0}', '{1}') WHERE id={2} OR ParentID={3}", 
        string strQuery = string.Format(_strquery,
            첫날자.ToString("yyyy-MM-dd"),
            끝날자.ToString("yyyy-MM-dd"),
             인증회원번호,
              인증회원번호);  //, WHERE ParentID=", + 인증회원번호;
        SqlCommand cmd = new SqlCommand(strQuery);
        //cmd.Parameters.Add(new SqlParameter("@StartDate", 첫날자));
        //cmd.Parameters.Add(new SqlParameter("@EndDate", 끝날자));
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

                long p_betSum = 0, b_betSum = 0, m_betSum = 0;
                long service_fee_sum = 0, ent_service_fee_sum = 0;
                //long benefitSum = 0;
                for (int i = 0; i < src.Tables[0].Rows.Count; i++)
                {
                    p_betSum += (src.Tables[0].Rows[i]["p_betMoneySum"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["p_betMoneySum"]);
                    b_betSum += (src.Tables[0].Rows[i]["b_betMoneySum"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["b_betMoneySum"]);
                    m_betSum += (src.Tables[0].Rows[i]["m_betMoneySum"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["m_betMoneySum"]);
                    //all_betSum += p_betSum + b_betSum + m_betSum;

                    service_fee_sum += (src.Tables[0].Rows[i]["service_fee"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["service_fee"]);
                    ent_service_fee_sum += (src.Tables[0].Rows[i]["ent_service_fee"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["ent_service_fee"]);

                   // e_chargeSum += (src.Tables[0].Rows[i]["e_charge_money"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["e_charge_money"]);
                    //u_chargeSum += (src.Tables[0].Rows[i]["u_charge_money"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["u_charge_money"]);
                   // e_withdrawSum += (src.Tables[0].Rows[i]["e_withdraw_money"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["e_withdraw_money"]);
                   // u_withdrawSum += (src.Tables[0].Rows[i]["u_withdraw_money"] == DBNull.Value) ? 0 : Convert.ToInt64(src.Tables[0].Rows[i]["u_withdraw_money"]);

                    
                }
                //grdList.FooterRow.Cells[4 - 3].Text = moneySum.ToString("N0");

                //grdList.FooterRow.Cells[5 - 3].Text = e_chargeSum.ToString("N0");

                grdList.FooterRow.Cells[1].Text = p_betSum.ToString("N0");
                grdList.FooterRow.Cells[2].Text = b_betSum.ToString("N0");
                grdList.FooterRow.Cells[3].Text = m_betSum.ToString("N0");
                grdList.FooterRow.Cells[4].Text = (p_betSum + b_betSum + m_betSum).ToString("N0");
                grdList.FooterRow.Cells[5].Text = service_fee_sum.ToString("N0");
                grdList.FooterRow.Cells[7].Text = ent_service_fee_sum.ToString("N0");
                //grdList.FooterRow.Cells[7 - 3].Text = e_withdrawSum.ToString("N0");
                //grdList.FooterRow.Cells[8 - 3].Text = u_withdrawSum.ToString("N0");
                //grdList.FooterRow.Cells[9 - 3].Text = gameCount.ToString("N0");
                //grdList.FooterRow.Cells[10 - 3].Text = serviceFeeSum.ToString("N0");
               // grdList.FooterRow.Cells[11 - 3].Text = entServiceFeeSum.ToString("N0");
                
                /*
                if (ddlEnterprise.SelectedIndex > 0)
                {
                    int nClassPercent = int.Parse(ddlEnterprise.SelectedValue.Split(':')[1]);
                    grdList.FooterRow.Cells[7].Text = ((chargeSum - withdrawSum) * nClassPercent / 100).ToString("N0");
                }
                 * */
                //grdList.Columns[7].Visible = false;
                if (GetClass() < 4)
                    grdList.FooterRow.Visible = false;
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
       
    public void btnExcel_OnClick(object sender, EventArgs e)
    {
        ExportExcel(grdList, "업체정산");
    }
}
