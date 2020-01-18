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

        
        /* 2017-02-25 본사 아닌경우 충전,출금 기능 사용못함 */
        if (GetClass() < 4)
        {
            lnkCharge.Visible = false;
            lnkWithdraw.Visible = false;
            //btnSave.Visible = false;
            
        }

        if (!IsPostBack)
        {
            string strID = Request.QueryString["id"];
            try{int.Parse(strID);}catch{
                strID = "0";
            }
            int n = 0;

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

            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());
                       
            int funcChargePermission = pm.getFuncPermission("USER_CHARGE", 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            int funcViewerPermission = pm.getFuncPermission("GAME_VIEWER", 인증회원.Tables[0].Rows[0]["user_type"].ToString());

            if (funcChargePermission < 1)
            {
                lnkCharge.Visible = false;
                lnkWithdraw.Visible = false;
            }
            lblMessage.Text = funcChargePermission.ToString();
        }
    }

    void BindUserInfo(DataSet dsUSer)
    {
        if (dsUSer == null) return;
        if (dsUSer.Tables.Count == 0) return;
        if (dsUSer.Tables[0].Rows.Count == 0) return;

        lnkCharge.NavigateUrl = "javascript:openNewWindow(\"usercharge.aspx\", " + dsUSer.Tables[0].Rows[0]["ID"].ToString() + ")";
        lnkWithdraw.NavigateUrl = "javascript:openNewWindow(\"userwithdraw.aspx\", " + dsUSer.Tables[0].Rows[0]["ID"].ToString() + ")";

        trPhone.Visible = 인증회원.Tables[0].Rows[0]["CanSeePhone"].ToString() == "1";

        lblID.Text = dsUSer.Tables[0].Rows[0]["ID"].ToString();
        lblRegDate.Text = ((DateTime)dsUSer.Tables[0].Rows[0]["RegDate"]).ToString("yyyy년 M월 d일 H시 m분");
        lblLoginID.Text = dsUSer.Tables[0].Rows[0]["LoginID"].ToString();
        tbxPWD.Text = dsUSer.Tables[0].Rows[0]["LoginPWD"].ToString();
        lblTitleNickName.Text = tbxNickName.Text = dsUSer.Tables[0].Rows[0]["Nickname"].ToString();
        tbxName.Text = dsUSer.Tables[0].Rows[0]["Name"].ToString();
        lblGameMoney.Text = ((long)dsUSer.Tables[0].Rows[0]["GameMoney"]).ToString("C0");
        lblWalletMoney.Text = ((long)dsUSer.Tables[0].Rows[0]["wallet_money"]).ToString("C0");
        tbxTelNum.Text = dsUSer.Tables[0].Rows[0]["TelNum"].ToString();
        tbxPartner.Text = dsUSer.Tables[0].Rows[0]["Partner"].ToString();
        ddlNologin.SelectedIndex = (byte)dsUSer.Tables[0].Rows[0]["status"];
        ddlStopChat.SelectedIndex = (byte)dsUSer.Tables[0].Rows[0]["StopChat"];
        tbxMemo.Text = dsUSer.Tables[0].Rows[0]["Memo"].ToString();
        lblDealMoney.Text = dsUSer.Tables[0].Rows[0]["dealmoney"].ToString();
        tbxDealPercent.Text = dsUSer.Tables[0].Rows[0]["dealpercent"].ToString();
        currencyExPassword.Text = dsUSer.Tables[0].Rows[0]["currencyExPassword"].ToString();
        bankAccountNumber.Text = dsUSer.Tables[0].Rows[0]["bankAccountNumber"].ToString();
        bankName.Text = dsUSer.Tables[0].Rows[0]["bankName"].ToString();
        depositHolder.Text = dsUSer.Tables[0].Rows[0]["depositHolder"].ToString();
        currencyExPassword.Text = dsUSer.Tables[0].Rows[0]["currencyExPassword"].ToString();

        ddlIsNew.Text = dsUSer.Tables[0].Rows[0]["is_new"].ToString();

        //딜비체크
        //유저의 상위업체 딜비 얻기
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT TBL_USERLIST.parentid, TBL_Enterprise.class, TBL_Enterprise.classpercent FROM TBL_USERLIST INNER JOIN TBL_Enterprise ON TBL_USERLIST.parentid = TBL_Enterprise.id WHERE TBL_USERLIST.id=@id";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@id", lblID.Text));
        DataSet dsUser = dbManager.RunMSelectQuery(cmd);
        if (DataSetUtil.IsNullOrEmpty(dsUser))
        {
            return;
        }
        else
        {
            int nClass = DataSetUtil.RowIntValue(dsUser, "class", 0);
            float fPer = DataSetUtil.RowFloatValue(dsUser, "classpercent", 0);
            lblPartnerPer.Text = string.Format("{0:N1}% 이하", fPer);
        }

    }
    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("회원목록.aspx");
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strId = lblID.Text;
        string strPassword = tbxPWD.Text.Trim();
        string strNickName = tbxNickName.Text.Trim();
        string strName = tbxName.Text.Trim();
        string strTel = tbxTelNum.Text;
        string strPartner = tbxPartner.Text.Trim();
        string currencyExPasswordText = currencyExPassword.Text.Trim();
        string bankNameText = bankName.Text.Trim();
        string bankAccountNumberText = bankAccountNumber.Text.Trim();
        string depositHolderText = depositHolder.Text.Trim();
        int nNoLogin = int.Parse(ddlNologin.SelectedValue);
        int nStopChat = int.Parse(ddlStopChat.SelectedValue);
        string strMemo = tbxMemo.Text;
        float fDealPercent = float.Parse(tbxDealPercent.Text);

        int isNew = int.Parse(ddlIsNew.Text);

        if (isNew == 1 && nNoLogin == 0)
        {
            isNew = 0;

            ddlIsNew.Text = isNew.ToString();
        }
            
        if (CheckExistID(lblLoginID.Text, strId))
        {
            cvResult.ErrorMessage = "이미 등록된 아이디입니다.";
            cvResult.IsValid = false;
            return;
        }

        if (CheckExistNickName(strNickName, strId))
        {
            cvResult.ErrorMessage = "이미 등록된 닉네임입니다.";
            cvResult.IsValid = false;
            return;
        }

        DBManager dbManager = new DBManager();
        string strQuery = "";
        SqlCommand cmd = null;
        //딜비체크
        //유저의 상위업체 딜비 얻기
        strQuery = "SELECT TBL_USERLIST.parentid, TBL_Enterprise.class, TBL_Enterprise.classpercent FROM TBL_USERLIST INNER JOIN TBL_Enterprise ON TBL_USERLIST.parentid = TBL_Enterprise.id WHERE TBL_USERLIST.id=@id";
        cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@id", strId));
        DataSet dsUser = dbManager.RunMSelectQuery(cmd);
        if (DataSetUtil.IsNullOrEmpty(dsUser))
        {
            return;
        }
        else
        {
            int nClass = DataSetUtil.RowIntValue(dsUser, "class", 0);
            float fPer = DataSetUtil.RowFloatValue(dsUser, "classpercent", 0);
            if (fDealPercent > fPer)
            {
                cvResult.ErrorMessage = string.Format("딜러비를 0~{0:N1}%사이로 입력하세요.", fPer);
                cvResult.IsValid = false;
                return;
            }
        }

        strQuery = "UPDATE TBL_UserList SET ";
        strQuery += "LoginPWD=@LoginPWD, ";
        strQuery += "NickName=@NickName, ";
        strQuery += "Name=@Name, ";
        strQuery += "TelNum=@TelNum, ";
        strQuery += "Partner=@Partner, ";
        strQuery += "status=@NoLogin, ";
        strQuery += "StopChat=@StopChat, ";
        strQuery += "ChangeDate=@ChangeDate, ";
        strQuery += "Memo=@Memo,";
        strQuery += "currencyExPassword=@currencyExPassword,";
        strQuery += "bankName=@bankName,";
        strQuery += "bankAccountNumber=@bankAccountNumber,";
        strQuery += "depositHolder=@depositHolder,";
        strQuery += "dealpercent=@dealpercent, ";
        strQuery += "is_new=@is_new ";
        strQuery += "WHERE ID=" + strId;
        cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@LoginPWD", strPassword));
        cmd.Parameters.Add(new SqlParameter("@NickName", strNickName));
        cmd.Parameters.Add(new SqlParameter("@Name", strName));
        cmd.Parameters.Add(new SqlParameter("@TelNum", strTel));
        cmd.Parameters.Add(new SqlParameter("@Partner", strPartner));
        cmd.Parameters.Add(new SqlParameter("@NoLogin", nNoLogin));
        cmd.Parameters.Add(new SqlParameter("@StopChat", nStopChat));
        cmd.Parameters.Add(new SqlParameter("@ChangeDate", DateTime.Now));
        cmd.Parameters.Add(new SqlParameter("@Memo", strMemo));
        cmd.Parameters.Add(new SqlParameter("@currencyExPassword", currencyExPasswordText));
        cmd.Parameters.Add(new SqlParameter("@bankName", bankNameText));
        cmd.Parameters.Add(new SqlParameter("@bankAccountNumber", bankAccountNumberText));
        cmd.Parameters.Add(new SqlParameter("@depositHolder", depositHolderText));
        cmd.Parameters.Add(new SqlParameter("@dealpercent", fDealPercent));
        cmd.Parameters.Add(new SqlParameter("@is_new", isNew));
        try
        {
            dbManager.RunMQuery(cmd);
            cvResult.ErrorMessage = "회원정보가 수정되였습니다.";
        }
        catch (Exception ex)
        {
            cvResult.ErrorMessage = "회원정보수정에서 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult.IsValid = false;
    }

    bool CheckExistID(string strLoginID, string strId)
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_UserList WHERE LoginID=@LoginID";
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        sqlQuery.Parameters.Add("@LoginID", SqlDbType.NVarChar);
        sqlQuery.Parameters["@LoginID"].Value = strLoginID;
        DataSet dsUser = dbManager.RunMSelectQuery(sqlQuery);

        bool bExist = false;
        if (dsUser.Tables.Count == 0)
        {
            bExist = false;
        }
        else if (dsUser.Tables[0].Rows.Count == 0)
        {
            bExist = false;
        }
        else if (dsUser.Tables[0].Rows[0]["LoginID"].ToString() == strLoginID)
        {
            bExist = (dsUser.Tables[0].Rows[0]["ID"].ToString() != strId);
        }
        return bExist;
    }

    bool CheckExistNickName(string strNickName, string strId)
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_UserList WHERE NickName=@NickName";
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        sqlQuery.Parameters.Add("@NickName", SqlDbType.NVarChar);
        sqlQuery.Parameters["@NickName"].Value = strNickName;
        DataSet dsUser = dbManager.RunMSelectQuery(sqlQuery);

        bool bExist = false;
        if (dsUser.Tables.Count == 0)
        {
            bExist = false;
        }
        else if (dsUser.Tables[0].Rows.Count == 0)
        {
            bExist = false;
        }
        else if (dsUser.Tables[0].Rows[0]["NickName"].ToString() == strNickName)
        {
            bExist = (dsUser.Tables[0].Rows[0]["ID"].ToString() != strId);
        }
        return bExist;
    }


    protected void OnTypeClick(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        if (lnk != null)
        {
            현재페지위치 = int.Parse(lnk.CommandArgument);
        }
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

    protected void Page_PreRender(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 현재페지위치;
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

}
