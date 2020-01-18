using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class MngMember_usercharge : PageBase
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

        //try
        //{
        string strID = Request.QueryString["id"];

        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_UserList WHERE ID=" + strID;
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsPartner = dbManager.RunMSelectQuery(cmd);
        if (dsPartner.Tables.Count == 0)
        {
            // Response.Redirect("공지사항목록.aspx");
            return;
        }

        if (dsPartner.Tables[0].Rows.Count == 0)
        {
            // Response.Redirect("공지사항목록.aspx");
            return;
        }

        hdnID.Value = dsPartner.Tables[0].Rows[0]["id"].ToString();
        lblUserNickname.Text = dsPartner.Tables[0].Rows[0]["loginid"].ToString();
        lblUserID.Text = dsPartner.Tables[0].Rows[0]["loginid"].ToString();
        //lblMoney.Text = Convert.ToInt32(dsPartner.Tables[0].Rows[0]["gamemoney"]).ToString("N0");
        lblMoney.Text = dsPartner.Tables[0].Rows[0]["gamemoney"].ToString();

        int gamemoney = Convert.ToInt32(dsPartner.Tables[0].Rows[0]["gamemoney"]);
        rvMoney.ErrorMessage = "출금가능한 머니는 " + gamemoney.ToString("N0") + "까지입니다.";
        rvMoney.MaximumValue = Convert.ToInt32(gamemoney).ToString();
        //}
        //catch
        //{
        //    Response.Write("<script>window.close();</script>");
        //}
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            int nAddMoney = Convert.ToInt32(tbxAddMoney.Text);
            if (nAddMoney <= 0)
            {
                return;
            }
            long nMoney = long.Parse(lblMoney.Text);
            int id = Convert.ToInt32(hdnID.Value);

            DBManager dbManager = new DBManager();
            string strQuery = "";
            SqlCommand cmd = null;
            // 1. 본사관리자가 아닌경우 보유코인증가
            if (!User_IsInRoles(PageBase.ROLES_MANAGER))
            {
                // 회원보유머니 검사
                strQuery = "SELECT gamemoney FROM TBL_USERLIST WHERE id=@userid AND gamemoney>=@reqmoney";
                cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add(new SqlParameter("@userid", id));
                cmd.Parameters.Add(new SqlParameter("@reqmoney", nAddMoney));
                DataSet ds = dbManager.RunMSelectQuery(cmd);
                if (DataSetUtil.IsNullOrEmpty(ds))
                {
                    ShowMessageBox("요청머니가 회원의 보유머니를 넘어납니다.");
                    return;
                }
                // 업체보유머니 증가
                strQuery = "UPDATE TBL_Enterprise set money=money+@reqmoney where id=@id";
                cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add(new SqlParameter("@id", 인증회원번호));
                cmd.Parameters.Add(new SqlParameter("@reqmoney", nAddMoney));
                dbManager.RunMQuery(cmd);
            }

            // 2. 출금체계에 충전리력을 남긴다.
            string admin = "운영자출금";
            string name = 인증회원.Tables[0].Rows[0]["name"].ToString();
            string memo = "";
            strQuery = "INSERT INTO TBL_Withdraw(UserID, BankInfo, Name, Money, RealMoney, Status, reg_userid, reg_username, update_time) ";
            strQuery += "VALUES(@UserID, @BankInfo, @Name, @Money, @RealMoney, 2, @reg_userid, @reg_username, GetDate())";
            cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@UserID", id));
            cmd.Parameters.Add(new SqlParameter("@BankInfo", admin));
            cmd.Parameters.Add(new SqlParameter("@Name", name));
            cmd.Parameters.Add(new SqlParameter("@Money", nAddMoney));
            cmd.Parameters.Add(new SqlParameter("@RealMoney", nAddMoney));
            cmd.Parameters.Add(new SqlParameter("@reg_userid", Convert.ToString(인증회원.Tables[0].Rows[0]["ext_id"])));
            cmd.Parameters.Add(new SqlParameter("@reg_username", 인증회원.Tables[0].Rows[0]["ext_name"].ToString()));
            dbManager.RunMQuery(cmd);


            // 3. 회원캐롯을 감소시킨다.
            strQuery = "UPDATE TBL_UserList SET gamemoney=gamemoney-" + nAddMoney + ", ChangeDate=getDate() WHERE id=" + id.ToString();
            dbManager.RunMQuery(new SqlCommand(strQuery));

            lblMoney.Text = (nMoney + nAddMoney).ToString();
            lblResult.Text = "머니출금에 성공하였습니다.";
            tblMailInfo.Visible = false;
            Response.Write("<script>alert('머니출금에 성공하였습니다.');window.opener.location.href = window.opener.location.href;window.setTimeout('window.close()', 1000);</script>");
        }
        catch(Exception ex)
        {
            cvResult.ErrorMessage = ex.StackTrace;
            cvResult.IsValid = false;
        }
    }
}
