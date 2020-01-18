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
            lblRegDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:MM");
            //rvGameMoney.ErrorMessage = "충전가능한 금액은 " + 인증회원.Tables[0].Rows[0]["money"].ToString() + "원 까지입니다.";
            //rvGameMoney.MaximumValue = 인증회원.Tables[0].Rows[0]["money"].ToString();
            rvGameMoney.MaximumValue = "1000000"; 
        }
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("회원목록.aspx");
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DBManager dbManager = new DBManager();
        string strId = lblID.Text;
        string strLoginID = tbxLoginID.Text.Trim();
        string strPassword = tbxPWD.Text.Trim();
        string strNickName = tbxNickName.Text.Trim();
        string strName = tbxName.Text.Trim();
        string strTel = tbxTelNum.Text;
        string strPartner = "";  // 인증회원.Tables[0].Rows[0]["partner"].ToString();
        long nGameMoney = long.Parse(tbxGameMoney.Text);
        int nNoLogin = int.Parse(ddlNologin.SelectedValue);
        int nStopChat = int.Parse(ddlStopChat.SelectedValue);
        string strMemo = tbxMemo.Text;

        //long nAdminMoney = long.Parse(인증회원.Tables[0].Rows[0]["money"].ToString());
        //// 1. 자신의 코인상태를 검사한다.
        //if (nAdminMoney < nGameMoney)
        //{
        //    throw new Exception("보유코인이 부족합니다..");
        //}
        //// 2. 코인삭감
        //string strQuery = "UPDATE TBL_Enterprise SET money=money-" + nGameMoney + " WHERE id=" + 인증회원번호.ToString();
        //dbManager.RunMQuery(new SqlCommand(strQuery));
        //인증회원.Tables[0].Rows[0]["money"] = nAdminMoney - nGameMoney;

        //// 3. 입금체계에 충전리력을 남긴다.
        //string admin = "회원코인충전";
        //string name = 인증회원.Tables[0].Rows[0]["name"].ToString();
        //string memo = "";
        //strQuery = "INSERT INTO TBL_Charge(UserID, BankInfo, BankName, Money, Memo, State) ";
        //strQuery += "VALUES(@UserID, @BankInfo, @BankName, @Money, @Memo, 2)";
        //SqlCommand cmd = new SqlCommand(strQuery);
        //cmd.Parameters.Add(new SqlParameter("@UserID", id));
        //cmd.Parameters.Add(new SqlParameter("@BankInfo", admin));
        //cmd.Parameters.Add(new SqlParameter("@BankName", name));
        //cmd.Parameters.Add(new SqlParameter("@Money", nGameMoney));
        //cmd.Parameters.Add(new SqlParameter("@Memo", memo)); ;
        //dbManager.RunMQuery(cmd);

        if (CheckExistID(strLoginID))
        {
            cvResult.ErrorMessage = "이미 등록된 아이디입니다.";
            cvResult.IsValid = false;
            return;
        }

        if (CheckExistNickName(strNickName))
        {
            cvResult.ErrorMessage = "이미 등록된 닉네임입니다.";
            cvResult.IsValid = false;
            return;
        }

        int nClass = int.Parse(인증회원.Tables[0].Rows[0]["Class"].ToString());
        string strBonsa = "";
        string strBuBonsa = "";
        string strChongpan = "";
        string strMaejang = "";
        string strBonsaID = "0";
        string strBuBonsaID = "0";
        string strChongpanID = "0";
        string strMaejangID = "0";
        switch (nClass.ToString())
        { 
            case ROLES_BONSA:
                strBonsa = 인증회원.Tables[0].Rows[0]["name"].ToString();
                strBonsaID = 인증회원.Tables[0].Rows[0]["id"].ToString();
                break;
            case ROLES_BUBONSA:
                strBonsa = 인증회원.Tables[0].Rows[0]["bonsa"].ToString();
                strBuBonsa = 인증회원.Tables[0].Rows[0]["name"].ToString();
                strBonsaID = 인증회원.Tables[0].Rows[0]["bonsaid"].ToString();
                strBuBonsaID = 인증회원.Tables[0].Rows[0]["id"].ToString();
                break;
            case ROLES_CHONGPAN:
                strBonsa = 인증회원.Tables[0].Rows[0]["bonsa"].ToString();
                strBuBonsa = 인증회원.Tables[0].Rows[0]["bubonsa"].ToString();
                strChongpan = 인증회원.Tables[0].Rows[0]["name"].ToString();
                strBonsaID = 인증회원.Tables[0].Rows[0]["bonsaid"].ToString();
                strBuBonsaID = 인증회원.Tables[0].Rows[0]["bubonsaid"].ToString();
                strChongpanID = 인증회원.Tables[0].Rows[0]["id"].ToString();
                break;
            case ROLES_MAEJANG:
                strBonsa = 인증회원.Tables[0].Rows[0]["bonsa"].ToString();
                strBuBonsa = 인증회원.Tables[0].Rows[0]["bubonsa"].ToString();
                strChongpan = 인증회원.Tables[0].Rows[0]["chongpan"].ToString();
                strMaejang = 인증회원.Tables[0].Rows[0]["name"].ToString();
                strBonsaID = 인증회원.Tables[0].Rows[0]["bonsaid"].ToString();
                strBuBonsaID = 인증회원.Tables[0].Rows[0]["bubonsaid"].ToString();
                strChongpanID = 인증회원.Tables[0].Rows[0]["chongpanid"].ToString();
                strMaejangID = 인증회원.Tables[0].Rows[0]["id"].ToString();
                break;
        }

        string strQuery = "INSERT INTO TBL_UserList(LoginID, LoginPWD, NickName, Name, TelNum, Partner, NoLogin, StopChat, ChangeDate, Memo, ParentID, Bonsa, Bubonsa, Chongpan, Maejang, BonsaID, BubonsaID, ChongpanID, MaejangID, currencyExPassword) ";
        strQuery += "VALUES(@LoginID, @LoginPWD, @NickName, @Name, @TelNum, @Partner, @NoLogin, @StopChat, @ChangeDate, @Memo, @ParentID, @Bonsa, @Bubonsa, @Chongpan, @Maejang, @BonsaID, @BubonsaID, @ChongpanID, @MaejangID, @currencyExPassword)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@LoginID", strLoginID));
        cmd.Parameters.Add(new SqlParameter("@LoginPWD", strPassword));
        cmd.Parameters.Add(new SqlParameter("@NickName", strNickName));
        cmd.Parameters.Add(new SqlParameter("@Name", strName));
        cmd.Parameters.Add(new SqlParameter("@TelNum", strTel));
        cmd.Parameters.Add(new SqlParameter("@Partner", strPartner));
        cmd.Parameters.Add(new SqlParameter("@NoLogin", nNoLogin));
        cmd.Parameters.Add(new SqlParameter("@StopChat", nStopChat));
        cmd.Parameters.Add(new SqlParameter("@ChangeDate", DateTime.Now));
        cmd.Parameters.Add(new SqlParameter("@Memo", strMemo));
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        cmd.Parameters.Add(new SqlParameter("@Bonsa", strBonsa));
        cmd.Parameters.Add(new SqlParameter("@Bubonsa", strBuBonsa));
        cmd.Parameters.Add(new SqlParameter("@Chongpan", strChongpan));
        cmd.Parameters.Add(new SqlParameter("@Maejang", strMaejang));
        cmd.Parameters.Add(new SqlParameter("@BonsaID", strBonsaID));
        cmd.Parameters.Add(new SqlParameter("@BubonsaID", strBuBonsaID));
        cmd.Parameters.Add(new SqlParameter("@ChongpanID", strChongpanID));
        cmd.Parameters.Add(new SqlParameter("@MaejangID", strMaejangID));
        cmd.Parameters.Add(new SqlParameter("@currencyExPassword", currencyExPassword.Text));
        try
        {
            dbManager.RunMQuery(cmd);
            cvResult.ErrorMessage = "회원정보가 등록되였습니다.";
        }
        catch (Exception ex)
        {
            cvResult.ErrorMessage = "회원정보등록에서 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult.IsValid = false;
    }

    bool CheckExistID(string strLoginID)
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
            bExist = true;
        }
        return bExist;
    }

    bool CheckExistNickName(string strNickName)
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
            bExist = true;
        }
        return bExist;
    }
}
