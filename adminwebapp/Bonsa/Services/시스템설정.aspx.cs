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

public partial class 게임관리_환경설정 : PageBase
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
            DBManager dbManager = new DBManager();
            //2017-02-13
            string strQuery = "SELECT A.*, Env.EnvValue FROM TBL_AdminInfo A, (SELECT EnvValue FROM TBL_SYSENV WHERE EnvCode='MobileConfirm') Env";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsAdminInfo = dbManager.RunMSelectQuery(cmd);
            if (dsAdminInfo.Tables.Count > 0 && dsAdminInfo.Tables[0].Rows.Count > 0)
            {
                BindInfo(dsAdminInfo);
            }

            string menu_id = Request.QueryString["mid"];
            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            if (permission <= 1)
            {
                //btnNew.Visible = false;
                //btnNew.Visible = false;
                btnSave.Visible = false;
                //grdList.Columns[grdList.Columns.Count - 1].Visible = false;
                //grdList.Columns[grdList.Columns.Count - 2].Visible = false;
                //grdList.Columns[grdList.Columns.Count - 3].Visible = false;
                //grdList.Columns[grdList.Columns.Count - 4].Visible = false;
            }
        }
    }

    void BindInfo(DataSet dsAdminInfo)
    {
        if (dsAdminInfo == null) return;
        if (dsAdminInfo.Tables.Count == 0) return;
        if (dsAdminInfo.Tables[0].Rows.Count == 0) return;

        tbxSubtract.Text = dsAdminInfo.Tables[0].Rows[0]["subtract"].ToString();
        tbxCardSubtract.Text = dsAdminInfo.Tables[0].Rows[0]["cardsubtract"].ToString();
        tbxStartMoney.Text = dsAdminInfo.Tables[0].Rows[0]["startmoney"].ToString();
        cbxDupIPCheck.Checked = dsAdminInfo.Tables[0].Rows[0]["dupliplogin"].ToString() == "1";
        tbxExchangeRatio.Text = dsAdminInfo.Tables[0].Rows[0]["ExchangeRatio"].ToString();
        tbxMinChargeMoney.Text = dsAdminInfo.Tables[0].Rows[0]["MinChargeMoney"].ToString();
        tbxMinCardChargeMoney.Text = dsAdminInfo.Tables[0].Rows[0]["MinCardChargeMoney"].ToString();
        tbxMinWithdrawMoney.Text = dsAdminInfo.Tables[0].Rows[0]["MinWithdrawMoney"].ToString();
        tbxMinCardWithdrawMoney.Text = dsAdminInfo.Tables[0].Rows[0]["MinCardWithdrawMoney"].ToString();
        cbxAllowCardCharge.Checked = dsAdminInfo.Tables[0].Rows[0]["AllowCardCharge"].ToString() == "1";
        cbxAllStopChat.Checked = dsAdminInfo.Tables[0].Rows[0]["allstopchat"].ToString() == "1";
        cbxMobileConfirm.Checked = dsAdminInfo.Tables[0].Rows[0]["EnvValue"].ToString() == "1";
        rbEnableInvokeOn.Checked = dsAdminInfo.Tables[0].Rows[0]["enableinvoke"].ToString() == "1";
        rbEnableInvokeOff.Checked = dsAdminInfo.Tables[0].Rows[0]["enableinvoke"].ToString() == "0";
        tbxRecommendPercent.Text = dsAdminInfo.Tables[0].Rows[0]["recommend_percent"].ToString();
        tbxNoLoginDays.Text = dsAdminInfo.Tables[0].Rows[0]["nologin_days"].ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int nSubtract = int.Parse(tbxSubtract.Text);
            int nCardSubtract = int.Parse(tbxCardSubtract.Text);
            int nStartMoney = int.Parse(tbxStartMoney.Text);
            int nDuplIpBlock = cbxDupIPCheck.Checked ? 1 : 0;
            double nExchangeRatio = double.Parse(tbxExchangeRatio.Text);
            int nMinChargeMoney = int.Parse(tbxMinChargeMoney.Text);
            int nMinCardChargeMoney = int.Parse(tbxMinCardChargeMoney.Text);
            int nMinWithdrawMoney = int.Parse(tbxMinWithdrawMoney.Text);
            int nMinCardWithdrawMoney = int.Parse(tbxMinCardWithdrawMoney.Text);
            int nAllowCardCharge = cbxAllowCardCharge.Checked ? 1 : 0;
            int nAllStopChat = cbxAllStopChat.Checked ? 1 : 0;
            int nEnableInvoke = rbEnableInvokeOn.Checked ? 1 : 0;
            string strMobileConfirm = cbxMobileConfirm.Checked ? "1" : "0";
            int nRecommendPercent = Convert.ToInt16(tbxRecommendPercent.Text);
            int nNologinDays = Convert.ToInt16(tbxNoLoginDays.Text);

            string strQuery = "UPDATE TBL_AdminInfo SET ";
            strQuery += "subtract=@subtract, ";
            strQuery += "cardsubtract=@cardsubtract, ";
            strQuery += "dupliplogin=@dupliplogin, ";
            strQuery += "startmoney=@startmoney, ";
            strQuery += "exchangeratio=@exchangeratio, ";
            strQuery += "minchargemoney=@minchargemoney, ";
            strQuery += "mincardchargemoney=@mincardchargemoney, ";
            strQuery += "minwithdrawmoney=@minwithdrawmoney, ";
            strQuery += "mincardwithdrawmoney=@mincardwithdrawmoney, ";
            strQuery += "allowcardcharge=@allowcardcharge, ";
            strQuery += "allstopchat=@allstopchat, ";
            strQuery += "enableinvoke=@enableinvoke, ";
            strQuery += "recommend_percent=@recommend_percent, ";
            strQuery += "changedate=getDate(), ";
            strQuery += "nologin_days=@nologin_days";
            DBManager dbManager = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@subtract", nSubtract));
            cmd.Parameters.Add(new SqlParameter("@cardsubtract", nCardSubtract));
            cmd.Parameters.Add(new SqlParameter("@dupliplogin", nDuplIpBlock));
            cmd.Parameters.Add(new SqlParameter("@startmoney", nStartMoney));
            cmd.Parameters.Add(new SqlParameter("@exchangeratio", nExchangeRatio));
            cmd.Parameters.Add(new SqlParameter("@minchargemoney", nMinChargeMoney));
            cmd.Parameters.Add(new SqlParameter("@mincardchargemoney", nMinCardChargeMoney));
            cmd.Parameters.Add(new SqlParameter("@minwithdrawmoney", nMinWithdrawMoney));
            cmd.Parameters.Add(new SqlParameter("@mincardwithdrawmoney", nMinCardWithdrawMoney));
            cmd.Parameters.Add(new SqlParameter("@allowcardcharge", nAllowCardCharge));
            cmd.Parameters.Add(new SqlParameter("@allstopchat", nAllStopChat));
            cmd.Parameters.Add(new SqlParameter("@enableinvoke", nEnableInvoke));
            cmd.Parameters.Add(new SqlParameter("@recommend_percent", nRecommendPercent));
            cmd.Parameters.Add(new SqlParameter("@nologin_days", nNologinDays));
            dbManager.RunMQuery(cmd);
            // 2017-02-13
            strQuery = "UPDATE TBL_SYSENV SET EnvValue=@envvalue WHERE EnvCode='MobileConfirm'";
            SqlCommand cmd2 = new SqlCommand(strQuery);
            cmd2.Parameters.Add(new SqlParameter("@envvalue", strMobileConfirm));
            dbManager.RunMQuery(cmd2);
            cvResult2.ErrorMessage = "관리자정보가 변경되였습니다.";
        }
        catch (Exception ex)
        {
            cvResult2.ErrorMessage = "관리자정보변경에 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult2.IsValid = false;
    }
    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        try
        {
            string strPassword = tbxPassword.Text.Trim();
            string strQuery = "UPDATE TBL_Enterprise SET ";
            strQuery += "loginpwd=@loginpwd WHERE id=@id";
            DBManager dbManager = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@loginpwd", strPassword));
            cmd.Parameters.Add(new SqlParameter("@id", 인증회원번호));
            dbManager.RunMQuery(cmd);
            cvResult1.ErrorMessage = "관리자비밀번호가 변경되였습니다.";
        }
        catch(Exception ex)
        {
            cvResult1.ErrorMessage = "관리자비밀번호변경에 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult1.IsValid = false;
    }
}
