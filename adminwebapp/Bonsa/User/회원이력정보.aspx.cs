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

public partial class 회원관리_회원상세정보 : PageBase
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
        if (!IsPostBack)
        {
            KEY_AUTOREFRESH = "CACHE:NEWBACARA:슈퍼본사:회원관리:회원이력:자동갱신:";
            KEY_CACHEINFOS = "CACHE:NEWBACARA:슈퍼본사:회원관리:회원이력:검색정보들:";
            KEY_CACHEQUERY = "CACHE:NEWBACARA:슈퍼본사:회원관리:회원이력:검색질문식:";
            KEY_CACHESELINF = "CACHE:NEWBACARA:슈퍼본사:회원관리:회원이력:선택된정보:";
            KEY_CACHESORT = "CACHE:NEWBACARA:슈퍼본사:회원관리:회원이력:정돈항:";
            KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:슈퍼본사:회원관리:회원이력:정돈방향:";
            KEY_CACHEFILTER = "CACHE:NEWBACARA:슈퍼본사:회원관리:회원이력:검색인자들:";
            KEY_CACHECURPAGE = "CACHE:NEWBACARA:슈퍼본사:회원관리:회원이력:현재페지위치:";
            KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:슈퍼본사:회원관리:회원이력:첫날자:";
            KEY_CACHELASTDATE = "CACHE:NEWBACARA:슈퍼본사:회원관리:회원이력:끝날자:";

            int n = 0;
            string strID = Request.QueryString["id"];
            try { n = int.Parse(strID); }
            catch { }

            선택정보번호 = n;

            DBManager dbManager = new DBManager();
            string strQuery = "SELECT TBL_UserList.* FROM TBL_UserList WHERE TBL_UserList.ID=" + strID;
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsUser = dbManager.RunMSelectQuery(cmd);
            if (dsUser.Tables.Count == 0)
            {
                Response.Redirect("회원목록.aspx");
                return;
            }

            if (dsUser.Tables[0].Rows.Count == 0)
            {
                Response.Redirect("회원목록.aspx");
                return;
            }

            BindUserInfo(dsUser);

            검색질문식 = dsUser.Tables[0].Rows[0]["NickName"].ToString();
            lblNickName1.Text = 검색질문식;
            lblNickName2.Text = 검색질문식;
            lblNickName3.Text = 검색질문식;
            lblNickName4.Text = 검색질문식;

            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            

            int funcChargePermission = pm.getFuncPermission("USER_CHARGE", 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            int funcViewerPermission = pm.getFuncPermission("GAME_VIEWER", 인증회원.Tables[0].Rows[0]["user_type"].ToString());

            if (funcChargePermission < 1)
            {
                pnlChargeWithdraw.Visible = false;
                pnlGift.Visible = false;                
            }
            //lblMessage.Text = funcChargePermission.ToString();
            grdLoginHist.Columns[grdLoginHist.Columns.Count - 1].Visible = false;
            grdChargeHist.Columns[grdChargeHist.Columns.Count - 1].Visible = false;
            grdWithdrawHist.Columns[grdWithdrawHist.Columns.Count - 1].Visible = false;
            
            grdChargeHist.Columns[4].Visible = false;
            grdChargeHist.Columns[3].Visible = false;
            grdWithdrawHist.Columns[4].Visible = false;
            grdWithdrawHist.Columns[5].Visible = false;
            
            if (GetClass() < 4)
            {
                pnlChargeWithdraw.Visible = false;
                pnlGift.Visible = false;
                lblTelNum.Visible = false;
                pnlTelNum.Visible = false;

               
            }
        }
    }

    void BindUserInfo(DataSet dsUSer)
    {
        if (dsUSer == null) return;
        if (dsUSer.Tables.Count == 0) return;
        if (dsUSer.Tables[0].Rows.Count == 0) return;

        lblID.Text = dsUSer.Tables[0].Rows[0]["ID"].ToString();
        lblRegDate.Text = ((DateTime)dsUSer.Tables[0].Rows[0]["RegDate"]).ToString("yyyy년 M월 d일 H시 m분");
        lblLoginID.Text = dsUSer.Tables[0].Rows[0]["LoginID"].ToString();
        lblPWD.Text = dsUSer.Tables[0].Rows[0]["LoginPWD"].ToString();
        lblTitleNickName.Text = lblNickName.Text = dsUSer.Tables[0].Rows[0]["Nickname"].ToString();
        lblName.Text = dsUSer.Tables[0].Rows[0]["Name"].ToString();
        lblGameMoney.Text = ((long)dsUSer.Tables[0].Rows[0]["GameMoney"]).ToString("C0");
        lblGameMoney1.Text = ((long)dsUSer.Tables[0].Rows[0]["GameMoney"]).ToString("C0");
        lblReelBonus.Text = ((long)dsUSer.Tables[0].Rows[0]["reelbonus"]).ToString("C0");
        lblWalletMoney.Text = ((long)dsUSer.Tables[0].Rows[0]["wallet_money"]).ToString("C0");
        lblTelNum.Text = dsUSer.Tables[0].Rows[0]["TelNum"].ToString();
        lblPartner.Text = dsUSer.Tables[0].Rows[0]["Partner"].ToString();
        lblNoLogin.Text = dsUSer.Tables[0].Rows[0]["status"].ToString() == "0" ? "허용" : "중지";
        lblStopChat.Text = dsUSer.Tables[0].Rows[0]["StopChat"].ToString() == "0" ? "허용" : "중지";
        lblMemo.Text = dsUSer.Tables[0].Rows[0]["Memo"].ToString();
        bankAccountNumber.Text = dsUSer.Tables[0].Rows[0]["bankAccountNumber"].ToString();
        bankName.Text = dsUSer.Tables[0].Rows[0]["bankName"].ToString();
        depositHolder.Text = dsUSer.Tables[0].Rows[0]["depositHolder"].ToString();
        currencyExPassword.Text = dsUSer.Tables[0].Rows[0]["currencyExPassword"].ToString();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 현재페지위치;
        //lblText.Text = 현재페지위치.ToString();
        switch (현재페지위치)
        {
            case 0:
                BindLoginHist();
                break;
            case 1:
                BindChargeHist();
                break;
            case 2:
                BindWithdrawHist();
                break;
            case 3:
                BindAllinBettingHist();
                break;
        }
    }

    void BindLoginHist()
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_LoginHist JOIN ";
        strQuery += "TBL_UserList ON TBL_UserList.ID=TBL_LoginHist.UserID ";
        strQuery += "WHERE TBL_LoginHist.UserID=" + 선택정보번호 + " ORDER BY TBL_LoginHist.ID DESC";
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsLoginHist = dbManager.RunMSelectQuery(cmd);

        //if (dsLoginHist == null) return;
        //if (dsLoginHist.Tables.Count == 0) return;
        //if (dsLoginHist.Tables[0].Rows.Count == 0) return;

        grdLoginHist.DataSource = dsLoginHist;
        grdLoginHist.DataBind();
    }
    void BindChargeHist()
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT *, Ent.name EntName FROM TBL_Charge JOIN ";
        strQuery += "TBL_UserList ON TBL_Charge.UserID=TBL_UserList.ID ";
        strQuery += " Left join TBL_Enterprise Ent ON Ent.id=TBL_UserList.parentid ";
        strQuery += "WHERE TBL_Charge.UserID=" + 선택정보번호.ToString() + " ORDER BY TBL_Charge.ID DESC";
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsChargeHist = dbManager.RunMSelectQuery(cmd);

        grdChargeHist.DataSource = dsChargeHist;
        grdChargeHist.DataBind();
    }
    void BindWithdrawHist()
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT *, Ent.name EntName FROM TBL_Withdraw JOIN ";
        strQuery += "TBL_UserList ON TBL_Withdraw.UserID=TBL_UserList.ID ";
        strQuery += " Left join TBL_Enterprise Ent ON Ent.id=TBL_UserList.parentid ";
        strQuery += "WHERE TBL_Withdraw.UserID = " + 선택정보번호.ToString() + " ORDER BY TBL_Withdraw.ID DESC";
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsWithdrawHist = dbManager.RunMSelectQuery(cmd);

        grdWithdrawHist.DataSource = dsWithdrawHist;
        grdWithdrawHist.DataBind();
    }

    void BindAllinBettingHist()
    {
        DBManager dbManager = new DBManager();
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string strQuery = "SELECT * FROM dbo.F_GET_ALLIN_GAME_HIST(" + 선택정보번호 + ",'" + today + "') ORDER BY regdate desc";
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsBettingHist = dbManager.RunMSelectQuery(cmd);

        if (dsBettingHist == null) return;
        if (dsBettingHist.Tables.Count == 0) return;
        if (dsBettingHist.Tables[0].Rows.Count == 0) return;

        grdBettingHist.DataSource = dsBettingHist;
        grdBettingHist.DataBind();
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        string strReturnUrl = "회원목록.aspx";
        try
        {
            strReturnUrl = Request.QueryString["ReturnUrl"];
        }
        catch
        { 
        
        }
        Response.Redirect(strReturnUrl);
    }

    protected void grdLoginHist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdLoginHist.PageIndex = e.NewPageIndex;
    }
    protected void grdLoginHist_RowDataBound(object sender, GridViewRowEventArgs e)
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

            lnkNo.Text = (grdLoginHist.PageIndex * grdLoginHist.PageSize + e.Row.RowIndex + 1).ToString();
        }
    }
    protected void grdLoginHist_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int rowIndex = e.RowIndex;
        LinkButton lnkNo = (LinkButton)grdLoginHist.Rows[e.RowIndex].FindControl("lnkNo");
        if (lnkNo == null)
        {
            return;
        }

        string strId = lnkNo.CommandArgument;
        string strRoomID = lnkNo.ToolTip;
        if (strId == "") return;
        // 삭제처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_LoginHist WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        dbManager.RunMQuery(cmd);
    }
    protected void grdChargeHist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdChargeHist.PageIndex = e.NewPageIndex;
    }
    protected void grdChargeHist_RowDataBound(object sender, GridViewRowEventArgs e)
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
            //e.Row.Attributes["onmouseover"] = "javascript:prevBGColor=this.style.backgroundColor; this.style.backgroundColor='#D1DDF1';this.style.cursor='hand';";
            //e.Row.Attributes["onmouseout"] = "javascript:this.style.backgroundColor=prevBGColor;this.style.cursor='default';";
            //e.Row.Attributes["mouseover"] = "cursor:hand";

            lnkNo.Text = (grdChargeHist.PageIndex * grdChargeHist.PageSize + e.Row.RowIndex + 1).ToString();
        }
    }
    protected void grdChargeHist_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int rowIndex = e.RowIndex;
        LinkButton lnkNo = (LinkButton)grdChargeHist.Rows[e.RowIndex].FindControl("lnkNo");
        if (lnkNo == null)
        {
            return;
        }

        string strId = lnkNo.CommandArgument;
        string strRoomID = lnkNo.ToolTip;
        if (strId == "") return;
        // 삭제처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_PokerGameLog WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        dbManager.RunMQuery(cmd);
    }
    protected void grdWithdrawHist_RowDataBound(object sender, GridViewRowEventArgs e)
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

            lnkNo.Text = (grdWithdrawHist.PageIndex * grdWithdrawHist.PageSize + e.Row.RowIndex + 1).ToString();
        }

    }
    protected void grdWithdrawHist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdWithdrawHist.PageIndex = e.NewPageIndex;
    }
    protected void grdWithdrawHist_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int rowIndex = e.RowIndex;
        LinkButton lnkNo = (LinkButton)grdWithdrawHist.Rows[e.RowIndex].FindControl("lnkNo");
        if (lnkNo == null)
        {
            return;
        }

        string strId = lnkNo.CommandArgument;
        string strRoomID = lnkNo.ToolTip;
        if (strId == "") return;
        // 삭제처리 진행
        DBManager dbManager = new DBManager();
        string strQuery = "DELETE TBL_PokerJackHist WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        dbManager.RunMQuery(cmd);
        BindWithdrawHist();
    }
    protected void grdBettingHist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdBettingHist.PageIndex = e.NewPageIndex;
    }
    protected void grdBettingHist_RowDataBound(object sender, GridViewRowEventArgs e)
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
            //e.Row.Attributes["onmouseover"] = "javascript:prevBGColor=this.style.backgroundColor; this.style.backgroundColor='#D1DDF1';this.style.cursor='hand';";
            //e.Row.Attributes["onmouseout"] = "javascript:this.style.backgroundColor=prevBGColor;this.style.cursor='default';";
            //e.Row.Attributes["mouseover"] = "cursor:hand";

            lnkNo.Text = (grdBettingHist.PageIndex * grdBettingHist.PageSize + e.Row.RowIndex + 1).ToString();
        }
    }
    protected void OnTypeClick(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        if (lnk != null)
        {
            현재페지위치 = int.Parse(lnk.CommandArgument);
        }
    }
}
