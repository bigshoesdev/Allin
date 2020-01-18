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
            string strQuery = "SELECT * FROM TBL_Enterprise WHERE ID=" + strID;
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
            lblMoney.Text = dsPartner.Tables[0].Rows[0]["money"].ToString();

            if (!User_IsInRoles(PageBase.ROLES_MANAGER))
            {
                rvMoney.ErrorMessage = "충전해줄수 있는 머니는 " + 인증회원.Tables[0].Rows[0]["money"].ToString() + "까지입니다.";
                rvMoney.MaximumValue = 인증회원.Tables[0].Rows[0]["money"].ToString();
            }
            else
            {
                rvMoney.MaximumValue = long.MaxValue.ToString();
            }
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
            int nAddMoney = int.Parse(tbxAddMoney.Text);
            if (nAddMoney <= 0) 
            {
                return;
            }
            long nMoney = long.Parse(lblMoney.Text);
            int id = int.Parse(hdnID.Value);

            DBManager dbManager = new DBManager();
            string strQuery = "";
            // 1. 본사관리자가 아닌경우 보유코인삭감
            if (!User_IsInRoles(PageBase.ROLES_MANAGER))
            {
                long nAdminMoney = long.Parse(인증회원.Tables[0].Rows[0]["money"].ToString());
                // 1. 본사가 아니면 자신의 코인상태를 검사한다.
                if (nAdminMoney < nAddMoney)
                {
                    throw new Exception("보유코인이 부족합니다..");
                }
                // 2. 코인삭감
                strQuery = "UPDATE TBL_Enterprise SET money=money-" + nAddMoney + " WHERE id=" + 인증회원번호.ToString();
                dbManager.RunMQuery(new SqlCommand(strQuery));
                인증회원.Tables[0].Rows[0]["money"] = nAdminMoney - nAddMoney;
            }

            // 2. 입금체계에 충전리력을 남긴다.
            string admin = "업체코인충전";
            string name = 인증회원.Tables[0].Rows[0]["name"].ToString();
            string memo = "";
            strQuery = "INSERT INTO TBL_ECharge(EnterpriseID, BankInfo, BankName, Money, Memo, State) ";
            strQuery += "VALUES(@EnterpriseID, @BankInfo, @BankName, @Money, @Memo, 2)";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@EnterpriseID", id));
            cmd.Parameters.Add(new SqlParameter("@BankInfo", admin));
            cmd.Parameters.Add(new SqlParameter("@BankName", name));
            cmd.Parameters.Add(new SqlParameter("@Money", nAddMoney));
            cmd.Parameters.Add(new SqlParameter("@Memo", memo)); ;
            dbManager.RunMQuery(cmd);

            strQuery = "UPDATE TBL_Enterprise SET money=money+" + nAddMoney + " WHERE id=" + id.ToString();
            dbManager.RunMQuery(new SqlCommand(strQuery));

            lblMoney.Text = (nMoney + nAddMoney).ToString();
            lblResult.Text = "머니충전에 성공하였습니다.";
            tblMailInfo.Visible = false;
            Response.Write("<script>alert('머니충전에 성공하였습니다.');window.opener.location.href = window.opener.location.href;window.setTimeout('window.close()', 1000);</script>");
        }
        catch(Exception ex)
        {
            cvResult.ErrorMessage = ex.Message;
            cvResult.IsValid = false;
        }
    }
}
