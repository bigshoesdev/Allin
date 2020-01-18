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

public partial class 게임관리_가상유저상세정보 : PageBase
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
            for (int i = 1; i <= 20; i++)
            {
                ddlAvatar.Items.Add(i.ToString());
            }

            string strID = Request.QueryString["id"];
            try { int.Parse(strID); }
            catch { strID = "0"; }
            hdnID.Value = strID;

            DBManager dbManager = new DBManager();
            string strQuery = "SELECT * FROM TBL_ROBOTLIST WHERE ID=" + strID;
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsRobotList = dbManager.RunMSelectQuery(cmd);
            if (dsRobotList.Tables.Count == 0)
            {
                // Response.Redirect("공지사항목록.aspx");
                return;
            }

            if (dsRobotList.Tables[0].Rows.Count == 0)
            {
                // Response.Redirect("공지사항목록.aspx");
                return;
            }
            BindInfo(dsRobotList);
        }
    }

    void BindInfo(DataSet dsRobotList)
    {
        if (dsRobotList == null) return;
        if (dsRobotList.Tables.Count == 0) return;
        if (dsRobotList.Tables[0].Rows.Count == 0) return;

        hdnID.Value = dsRobotList.Tables[0].Rows[0]["id"].ToString();
        hdnOrgNickname.Value = tbxNickname.Text = dsRobotList.Tables[0].Rows[0]["nickname"].ToString();
        tbxMoney.Text = dsRobotList.Tables[0].Rows[0]["money"].ToString();
        ddlAvatar.SelectedValue = dsRobotList.Tables[0].Rows[0]["avatar"].ToString();
        //rbSexySound1.Checked = dsRobotList.Tables[0].Rows[0]["sexysound"].ToString() == "0";
        tbxAllinWinCount.Text = dsRobotList.Tables[0].Rows[0]["allinwincount"].ToString();
        tbxAllinLoseCount.Text = dsRobotList.Tables[0].Rows[0]["allinlosecount"].ToString();


        tbxMoney.ReadOnly = true;
        lblMoneyMsg.Text = "수정불가. 금액변경은 로붓입출금에서 하십시오";
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("가상유저목록.aspx");
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strId = hdnID.Value;
        string strNickname = tbxNickname.Text.Trim();
        long nMoney = long.Parse(tbxMoney.Text);
        int nAvatar = int.Parse(ddlAvatar.SelectedValue);
        int nSexySound = 0; // rbSexySound1.Checked ? 0 : 1;
        int nAllinWinCount = int.Parse(tbxAllinWinCount.Text);
        int nAllinLoseCount = int.Parse(tbxAllinLoseCount.Text);

        DBManager dbManager = new DBManager();
        if (strId == "0")
        {
            if (nMoney < 0)
            {
                cvResult.ErrorMessage = "초기금액은 0 보다 작을수 없습니다.";
                cvResult.IsValid = false;
                return;
            }

            if ( CheckExistID(strNickname))
            {
                cvResult.ErrorMessage = "이미 등록된 닉네임입니다.";
                cvResult.IsValid = false;
                return;
            }

            // 2017-02-21 로붓의 시작머니도 필요함 
            string strQuery = "INSERT INTO TBL_ROBOTLIST(nickname, money, avatarNo, sexysound, allinwincount, allinlosecount, changedate, start_money) ";
            strQuery += "VALUES(@nickname, @money, @avatarNo, @sexysound, @allinwincount, @allinlosecount, @changedate, @start_money)";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@nickname", strNickname));
            cmd.Parameters.Add(new SqlParameter("@money", nMoney));
            cmd.Parameters.Add(new SqlParameter("@avatarNo", nAvatar));
            cmd.Parameters.Add(new SqlParameter("@sexysound", nSexySound));
            cmd.Parameters.Add(new SqlParameter("@allinwincount", nAllinWinCount));
            cmd.Parameters.Add(new SqlParameter("@allinlosecount", nAllinLoseCount));
            cmd.Parameters.Add(new SqlParameter("@changedate", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@start_money", nMoney));
            try
            {
                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "가상유저정보가 등록되였습니다.";
            }
            catch (Exception ex)
            {
                cvResult.ErrorMessage = "가상유저정보등록에서 오류가 발생하였습니다. " + ex.ToString();
            }
        }
        else
        {
            if (strNickname != hdnOrgNickname.Value && CheckExistID(strNickname))
            {
                cvResult.ErrorMessage = "이미 등록된 닉네임입니다.";
                cvResult.IsValid = false;
                return;
            }

            string strQuery = "UPDATE TBL_ROBOTLIST SET ";
            strQuery += "nickname=@nickname, ";
            strQuery += "money=@money, ";
            strQuery += "avatarNo=@avatarNo, ";
            strQuery += "sexysound=@sexysound, ";
            strQuery += "allinwincount=@allinwincount, ";
            strQuery += "allinlosecount=@allinlosecount, ";
            strQuery += "changedate=@changedate ";
            strQuery += "WHERE id=@id";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@nickname", strNickname));
            cmd.Parameters.Add(new SqlParameter("@money", nMoney));
            cmd.Parameters.Add(new SqlParameter("@avatarNo", nAvatar));
            cmd.Parameters.Add(new SqlParameter("@sexysound", nSexySound));
            cmd.Parameters.Add(new SqlParameter("@allinwincount", nAllinWinCount));
            cmd.Parameters.Add(new SqlParameter("@allinlosecount", nAllinLoseCount));
            cmd.Parameters.Add(new SqlParameter("@changedate", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@id", strId));
            try
            {
                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "가상유저정보가 수정되였습니다.";
            }
            catch (Exception ex)
            {
                cvResult.ErrorMessage = "가상유저정보수정에서 오류가 발생하였습니다. " + ex.ToString();
            }
        }
        cvResult.IsValid = false;
    }

    bool CheckExistID(string strNickname)
    {
        DBManager dbManager = new DBManager();
        // 2017-02-21 사용자 닉네임도 체크 포함
        string strQuery = "SELECT nickname FROM (SELECT nickname FROM TBL_ROBOTLIST UNION ALL SELECT nickname FROM tbl_userlist) AS A WHERE nickname=@nickname";
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        sqlQuery.Parameters.Add("@nickname", SqlDbType.NVarChar);
        sqlQuery.Parameters["@nickname"].Value = strNickname;
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
        else if (dsUser.Tables[0].Rows[0]["nickname"].ToString() == strNickname)
        {
            bExist = true;
        }

        // 사용자
        return bExist;
    }
}
