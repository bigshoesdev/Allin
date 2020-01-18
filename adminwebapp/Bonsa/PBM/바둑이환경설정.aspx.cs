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

public partial class 게임관리_바둑이환경설정 : PageBase
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
            string strQuery = "SELECT * FROM TBL_SETTINGINFO";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsSettingInfo = dbManager.RunBadukiSelectQuery(cmd);
            if (dsSettingInfo.Tables.Count > 0 && dsSettingInfo.Tables[0].Rows.Count > 0)
            {
                BindInfo(dsSettingInfo);
            }

            string menu_id = Request.QueryString["mid"];
            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            if (permission <= 1)
            {
                btnSave.Visible = false;
                //btnNew.Visible = false;
                //btnNew.Visible = false;
                //btnSave.Visible = false;
                //grdList.Columns[grdList.Columns.Count - 1].Visible = false;
                //grdList.Columns[grdList.Columns.Count - 2].Visible = false;
                //grdList.Columns[5].Visible = false;
                //grdList.Columns[6].Visible = false;
            }
        }
    }

    void BindInfo(DataSet dsSettingInfo)
    {
        if (dsSettingInfo == null) return;
        if (dsSettingInfo.Tables.Count == 0) return;
        if (dsSettingInfo.Tables[0].Rows.Count == 0) return;

        tbxMngSubtract.Text = dsSettingInfo.Tables[0].Rows[0]["mngsubtract"].ToString();
        tbxJackSubtract.Text = dsSettingInfo.Tables[0].Rows[0]["jacksubtract"].ToString();
        cbxEventFlag.Checked = dsSettingInfo.Tables[0].Rows[0]["EventFlag"].ToString() != "0";
        tbxEventMoney.Text = dsSettingInfo.Tables[0].Rows[0]["EventMoney"].ToString();
        cbxAllowDuplIP.Checked = dsSettingInfo.Tables[0].Rows[0]["allowduplip"].ToString() != "0";

        tbxTotalJackMoney.Text = ((long)dsSettingInfo.Tables[0].Rows[0]["TotalJackMoney"]).ToString();
        rbJackMode1.Checked = dsSettingInfo.Tables[0].Rows[0]["JackMode"].ToString() == "1";
        rbJackMode2.Checked = dsSettingInfo.Tables[0].Rows[0]["JackMode"].ToString() == "0";
        tbxJackRatio1.Text = dsSettingInfo.Tables[0].Rows[0]["JackRatio1"].ToString();
        tbxJackRatio2.Text = dsSettingInfo.Tables[0].Rows[0]["JackRatio2"].ToString();
        tbxJackRatio3.Text = dsSettingInfo.Tables[0].Rows[0]["JackRatio3"].ToString();
        tbxJackMoney1.Text = dsSettingInfo.Tables[0].Rows[0]["JackMoney1"].ToString();
        tbxJackMoney2.Text = dsSettingInfo.Tables[0].Rows[0]["JackMoney2"].ToString();
        tbxJackMoney3.Text = dsSettingInfo.Tables[0].Rows[0]["JackMoney3"].ToString();

        tbxGolfRate.Text = dsSettingInfo.Tables[0].Rows[0]["GolfRate"].ToString();
        tbxSecondRate.Text = dsSettingInfo.Tables[0].Rows[0]["SecondRate"].ToString();
        tbxThirdRate.Text = dsSettingInfo.Tables[0].Rows[0]["ThirdRate"].ToString();
        tbxMade5Rate.Text = dsSettingInfo.Tables[0].Rows[0]["Made5Rate"].ToString();
        tbxMade6Rate.Text = dsSettingInfo.Tables[0].Rows[0]["Made6Rate"].ToString();
        tbxMade7Rate.Text = dsSettingInfo.Tables[0].Rows[0]["Made7Rate"].ToString();
        tbxMade8Rate.Text = dsSettingInfo.Tables[0].Rows[0]["Made8Rate"].ToString();
        tbxMade9Rate.Text = dsSettingInfo.Tables[0].Rows[0]["Made9Rate"].ToString();
        tbxMade10Rate.Text = dsSettingInfo.Tables[0].Rows[0]["Made10Rate"].ToString();
        tbxMade11Rate.Text = dsSettingInfo.Tables[0].Rows[0]["Made11Rate"].ToString();
        tbxMade12Rate.Text = dsSettingInfo.Tables[0].Rows[0]["Made12Rate"].ToString();
        tbxMade13Rate.Text = dsSettingInfo.Tables[0].Rows[0]["Made13Rate"].ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            double nMngSubtract = double.Parse(tbxMngSubtract.Text);
            double nJackSubtract = double.Parse(tbxJackSubtract.Text);
            byte bytEventFlag = cbxEventFlag.Checked ? (byte)1 : (byte)0;
            int nEventMoney = int.Parse(tbxEventMoney.Text);
            byte bytAllowDuplIP = cbxAllowDuplIP.Checked ? (byte)1 : (byte)0;

            long nTotalJackMoney = long.Parse(tbxTotalJackMoney.Text);
            byte bytJackMode = rbJackMode1.Checked ? (byte)1 : (byte)0;
            double dblJackRatio1 = double.Parse(tbxJackRatio1.Text);
            double dblJackRatio2 = double.Parse(tbxJackRatio2.Text);
            double dblJackRatio3 = double.Parse(tbxJackRatio3.Text);
            int nJackMoney1 = int.Parse(tbxJackMoney1.Text);
            int nJackMoney2 = int.Parse(tbxJackMoney2.Text);
            int nJackMoney3 = int.Parse(tbxJackMoney3.Text);

            double dblGolfRate = double.Parse(tbxGolfRate.Text);
            double dblSecondRate = double.Parse(tbxSecondRate.Text);
            double dblThirdRate = double.Parse(tbxThirdRate.Text);
            double dblMade5Rate = double.Parse(tbxMade5Rate.Text);
            double dblMade6Rate = double.Parse(tbxMade6Rate.Text);
            double dblMade7Rate = double.Parse(tbxMade7Rate.Text);
            double dblMade8Rate = double.Parse(tbxMade8Rate.Text);
            double dblMade9Rate = double.Parse(tbxMade9Rate.Text);
            double dblMade10Rate = double.Parse(tbxMade10Rate.Text);
            double dblMade11Rate = double.Parse(tbxMade11Rate.Text);
            double dblMade12Rate = double.Parse(tbxMade12Rate.Text);
            double dblMade13Rate = double.Parse(tbxMade13Rate.Text);

            string strQuery = "UPDATE TBL_SETTINGINFO SET ";
            strQuery += "mngsubtract=@mngsubtract, ";
            strQuery += "jacksubtract=@jacksubtract, ";
            strQuery += "EventFlag=@EventFlag, ";
            strQuery += "EventMoney=@EventMoney, ";
            strQuery += "allowduplip=@allowduplip, ";
            strQuery += "TotalJackMoney=@TotalJackMoney, ";
            strQuery += "JackMode=@JackMode, ";
            strQuery += "JackRatio1=@JackRatio1, ";
            strQuery += "JackRatio2=@JackRatio2, ";
            strQuery += "JackRatio3=@JackRatio3, ";
            strQuery += "JackMoney1=@JackMoney1, ";
            strQuery += "JackMoney2=@JackMoney2, ";
            strQuery += "JackMoney3=@JackMoney3, ";
            strQuery += "GolfRate=@GolfRate, ";
            strQuery += "SecondRate=@SecondRate, ";
            strQuery += "ThirdRate=@ThirdRate, ";
            strQuery += "Made5Rate=@Made5Rate, ";
            strQuery += "Made6Rate=@Made6Rate, ";
            strQuery += "Made7Rate=@Made7Rate, ";
            strQuery += "Made8Rate=@Made8Rate, ";
            strQuery += "Made9Rate=@Made9Rate, ";
            strQuery += "Made10Rate=@Made10Rate, ";
            strQuery += "Made11Rate=@Made11Rate, ";
            strQuery += "Made12Rate=@Made12Rate, ";
            strQuery += "Made13Rate=@Made13Rate, ";
            strQuery += "changedate=getDate() ";
            DBManager dbManager = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@mngsubtract", nMngSubtract));
            cmd.Parameters.Add(new SqlParameter("@jacksubtract", nJackSubtract));
            cmd.Parameters.Add(new SqlParameter("@EventFlag", bytEventFlag));
            cmd.Parameters.Add(new SqlParameter("@EventMoney", nEventMoney));
            cmd.Parameters.Add(new SqlParameter("@allowduplip", bytAllowDuplIP));
            cmd.Parameters.Add(new SqlParameter("@TotalJackMoney", nTotalJackMoney));
            cmd.Parameters.Add(new SqlParameter("@JackMode", bytJackMode));
            cmd.Parameters.Add(new SqlParameter("@JackRatio1", dblJackRatio1));
            cmd.Parameters.Add(new SqlParameter("@JackRatio2", dblJackRatio2));
            cmd.Parameters.Add(new SqlParameter("@JackRatio3", dblJackRatio3));
            cmd.Parameters.Add(new SqlParameter("@JackMoney1", nJackMoney1));
            cmd.Parameters.Add(new SqlParameter("@JackMoney2", nJackMoney2));
            cmd.Parameters.Add(new SqlParameter("@JackMoney3", nJackMoney3));
            cmd.Parameters.Add(new SqlParameter("@GolfRate", dblGolfRate));
            cmd.Parameters.Add(new SqlParameter("@SecondRate", dblSecondRate));
            cmd.Parameters.Add(new SqlParameter("@ThirdRate", dblThirdRate));
            cmd.Parameters.Add(new SqlParameter("@Made5Rate", dblMade5Rate));
            cmd.Parameters.Add(new SqlParameter("@Made6Rate", dblMade6Rate));
            cmd.Parameters.Add(new SqlParameter("@Made7Rate", dblMade7Rate));
            cmd.Parameters.Add(new SqlParameter("@Made8Rate", dblMade8Rate));
            cmd.Parameters.Add(new SqlParameter("@Made9Rate", dblMade9Rate));
            cmd.Parameters.Add(new SqlParameter("@Made10Rate", dblMade10Rate));
            cmd.Parameters.Add(new SqlParameter("@Made11Rate", dblMade11Rate));
            cmd.Parameters.Add(new SqlParameter("@Made12Rate", dblMade12Rate));
            cmd.Parameters.Add(new SqlParameter("@Made13Rate", dblMade13Rate));
            dbManager.RunBadukiQuery(cmd);

            /** 2017-04-18 포커 맞고도 같이 수정 **/
            strQuery = "UPDATE TBL_SETTINGINFO SET mngsubtract=" + nMngSubtract;
            dbManager.RunPokerQuery(new SqlCommand(strQuery));
            dbManager.RunMatgoQuery(new SqlCommand(strQuery));
            // 바둑이 수수료율을 본사 지분율로 본다
            dbManager.RunMQuery(new SqlCommand("update tbl_enterprise set classpercent=" + nMngSubtract + " where class=4"));
            
            cvResult2.ErrorMessage = "환경설정정보가 변경되였습니다.";
        }
        catch (Exception ex)
        {
            cvResult2.ErrorMessage = "환경설정정보변경에 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult2.IsValid = false;
    }
}
