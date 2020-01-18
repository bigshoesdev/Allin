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

public partial class Bonsa_PBM_RobotWithdraw : PageBase
{
    DataSet dsRobot;

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
        //string strQuery = "SELECT * FROM TBL_Enterprise WHERE ID=" + strID;
        string strQuery = "SELECT * FROM TBL_RobotList WHERE ID=" + strID;
        SqlCommand cmd = new SqlCommand(strQuery);
        dsRobot = dbManager.RunMSelectQuery(cmd);
        if (dsRobot.Tables.Count == 0)
        {
            // Response.Redirect("공지사항목록.aspx");
            return;
        }

        if (dsRobot.Tables[0].Rows.Count == 0)
        {
            // Response.Redirect("공지사항목록.aspx");
            return;
        }

        hdnID.Value = dsRobot.Tables[0].Rows[0]["id"].ToString();
        //lblRobotID.Text = 
        lblRobotNickname.Text = dsRobot.Tables[0].Rows[0]["nickname"].ToString();
        lblMoney.Text = Convert.ToInt32(dsRobot.Tables[0].Rows[0]["money"]).ToString();

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



            // 2. 입금체계에 충전리력을 남긴다.
            //string admin = "업체코인충전";
            string robot_id = dsRobot.Tables[0].Rows[0]["id"].ToString();
            string robot_nickname = dsRobot.Tables[0].Rows[0]["nickname"].ToString();

            string name = 인증회원.Tables[0].Rows[0]["name"].ToString();
            string user_id = 인증회원.Tables[0].Rows[0]["loginid"].ToString();
            string user_db_id = 인증회원.Tables[0].Rows[0]["id"].ToString();
            string memo = tbxMemo.Text;

            strQuery = "select money from tbl_robotlist where id=" + robot_id;
            SqlCommand chkCmd = new SqlCommand(strQuery);
            DataSet chkDs = dbManager.RunMSelectQuery(chkCmd);
            if (chkDs == null || chkDs.Tables[0] == null || chkDs.Tables[0].Rows == null || chkDs.Tables[0].Rows.Count == 0)
            {
                Response.Write("<script>alert('로붓 데이터에 이상이 있습니다.'); window.opener.location.reload(); window.close();</script>");
                Response.End();
                return;
            }

            long robotMoney = Convert.ToInt64(chkDs.Tables[0].Rows[0][0]);
            if (nAddMoney > robotMoney)
            {
                lblMoney.Text = robotMoney.ToString();
                lblResult.Text = "보유머니가 출금액보다 적습니다.";
                return;
            }

            strQuery = "INSERT INTO TBL_robot_charge(robot_id, robot_nickname, charge_money, memo ,reg_user_name, reg_userid, reg_user_db_id) ";
            strQuery += "VALUES(@robot_id, @robot_nickname, @charge_money, @memo, @reg_user_name, @reg_userid, @reg_user_db_id)";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@robot_id", id));
            cmd.Parameters.Add(new SqlParameter("@robot_nickname", robot_nickname));
            cmd.Parameters.Add(new SqlParameter("@charge_money", -nAddMoney)); // 금액뺀다 2017-04-18
            cmd.Parameters.Add(new SqlParameter("@memo", memo));
            cmd.Parameters.Add(new SqlParameter("@reg_user_name", name));
            cmd.Parameters.Add(new SqlParameter("@reg_userid", user_id));
            cmd.Parameters.Add(new SqlParameter("@reg_user_db_id", user_db_id));
            dbManager.RunMQuery(cmd);

            // 금액뺀다 2017-04-18
            strQuery = "UPDATE TBL_robotlist SET money = money - " + nAddMoney + " WHERE id=" + robot_id;
            dbManager.RunMQuery(new SqlCommand(strQuery));

            lblMoney.Text = (nMoney - nAddMoney).ToString("N0");
            lblResult.Text = "로붓머니 출금에 성공하였습니다.";
            tblMailInfo.Visible = false;
            Response.Write("<script>alert('머니 출금에 성공하였습니다. '); window.opener.location.reload(); window.close();</script>");
        }
        catch (Exception ex)
        {
            cvResult.ErrorMessage = ex.Message;
            cvResult.IsValid = false;
        }
    }
}
