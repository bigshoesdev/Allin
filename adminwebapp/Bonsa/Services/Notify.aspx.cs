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
using System.Text;

public partial class Services_Notify : PageBase
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
        BindDataSource();
    }

    public void BindDataSource()
    {
        if (인증회원 == null)
        {
            return;
        }
        if (HttpContext.Current.User == null)
        {
            return;
        }
        string sndUrl = "";
        DBManager dbManager = new DBManager();
        string strQuery = "";
        if (DataSetUtil.RowStringValue(인증회원, "user_type", 0).Equals("ENT"))
        {
            strQuery = "SELECT *, loginid as ext_id, name as ext_name, 'ENT' as user_type FROM TBL_Enterprise WHERE id=@ParentID";
        }
        else
        {
            strQuery = "SELECT *, loginid as ext_id, name as ext_name, 'EMP' as user_type FROM TBL_Enterprise WHERE id=@ParentID";
        }

        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        인증회원 = dbManager.RunMSelectQuery(cmd);
        lblCoin.Text = String.Format("{0:N0}원", DataSetUtil.RowLongValue(인증회원, "money", 0));

        tdAdmin5.Visible = false;
        if (GetClass() < 4)
        {
            tdAdmin1.Visible = false;
            tdAdmin2.Visible = false;
            tdAdmin3.Visible = false;
            tdAdmin4.Visible = false;
            tdAdmin5.Visible = false;
            tdAdmin6.Visible = false;
            tdAdmin7.Visible = false;
            tdAdmin8.Visible = false;
            tdAdmin9.Visible = false;
            return;
        }

        // 충전요청수
        strQuery = "SELECT TBL_Charge.id FROM TBL_Charge JOIN TBL_UserList ON TBL_UserList.ID = TBL_Charge.UserID WHERE TBL_Charge.status=0 ";
        if( GetClass() < 5 )
            strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID)";
        cmd = new SqlCommand(strQuery);
        if (GetClass() < 5)
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        DataSet dsChargeReqList = dbManager.RunMSelectQuery(cmd);
        int cntChargeCount = DataSetUtil.RowCount(dsChargeReqList);
        for (int i = 0; i < cntChargeCount; i++)
        {
            if (알람정보들.IndexOf("charge" + DataSetUtil.RowStringValue(dsChargeReqList, 0, i)) < 0)
            {
                sndUrl = "../../Images/charge.wav";
                break;
            }
        }

        strQuery = "SELECT * FROM TBL_Charge JOIN TBL_UserList ON TBL_UserList.ID = TBL_Charge.UserID WHERE TBL_Charge.status=3 ";
        if (GetClass() < 5)
            strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID)";
        cmd = new SqlCommand(strQuery);
        if (GetClass() < 5)
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        dsChargeReqList = dbManager.RunMSelectQuery(cmd);
        int cntChargeCount1 = DataSetUtil.RowCount(dsChargeReqList);
        lblTodayCharge.Text = cntChargeCount1.ToString() + "건 완료";
        lblChargeCount.Text = cntChargeCount.ToString() + "건";

        // 환전요청수
        strQuery = "SELECT * FROM TBL_Withdraw JOIN TBL_UserList ON TBL_UserList.ID = TBL_Withdraw.UserID WHERE TBL_Withdraw.status=0 ";
        if (GetClass() < 5)
            strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID)";
        cmd = new SqlCommand(strQuery);
        if (GetClass() < 5)
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        DataSet dsWithdraw = dbManager.RunMSelectQuery(cmd);
        cntChargeCount = DataSetUtil.RowCount(dsWithdraw);
        if (cntChargeCount > 0 && string.IsNullOrEmpty(sndUrl))
        {
            for (int i = 0; i < cntChargeCount; i++)
            {
                if (알람정보들.IndexOf("withdraw" + DataSetUtil.RowStringValue(dsWithdraw, 0, i)) < 0)
                {
                    sndUrl = "../../Images/withdraw.wav";
                    break;
                }
            }
        }

        strQuery = "SELECT * FROM TBL_Withdraw JOIN TBL_UserList ON TBL_UserList.ID = TBL_Withdraw.UserID WHERE TBL_Withdraw.status=3 ";
        if (GetClass() < 5)
            strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID OR MaejangID=@ParentID)";
        cmd = new SqlCommand(strQuery);
        if (GetClass() < 5)
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        dsWithdraw = dbManager.RunMSelectQuery(cmd);
        cntChargeCount1 = DataSetUtil.RowCount(dsWithdraw);
        lblTodayWithdraw.Text = cntChargeCount1.ToString() + "건 완료";
        lblWithdrawCount.Text = cntChargeCount.ToString() + "건";

        // 업체입금요청
        strQuery = "SELECT * FROM TBL_ECharge JOIN TBL_Enterprise ON TBL_Enterprise.ID = TBL_ECharge.enterpriseid WHERE TBL_ECharge.state=0 ";
        if (GetClass() < 5)
            strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID)";
        cmd = new SqlCommand(strQuery);
        if (GetClass() < 5)
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        dsChargeReqList = dbManager.RunMSelectQuery(cmd);
        cntChargeCount = DataSetUtil.RowCount(dsChargeReqList);
        if (cntChargeCount > 0 && string.IsNullOrEmpty(sndUrl))
        {
            for (int i = 0; i < cntChargeCount; i++)
            {
                if (알람정보들.IndexOf("echarge" + DataSetUtil.RowStringValue(dsChargeReqList, 0, i)) < 0)
                {
                    sndUrl = "../../Images/echarge.wav";
                    break;
                }
            }
        }

        strQuery = "SELECT * FROM TBL_ECharge JOIN TBL_Enterprise ON TBL_Enterprise.ID = TBL_ECharge.enterpriseid WHERE TBL_ECharge.state=3 ";
        if (GetClass() < 5)
            strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID)";
        cmd = new SqlCommand(strQuery);
        if (GetClass() < 5)
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        dsChargeReqList = dbManager.RunMSelectQuery(cmd);
        cntChargeCount1 = DataSetUtil.RowCount(dsChargeReqList);
        lblPartnerCharge.Text = cntChargeCount1.ToString() + "건 완료";
        lblPartnerChargeCount.Text = cntChargeCount.ToString() + "건";

        // 업체출금요청
        strQuery = "SELECT * FROM TBL_EWithdraw JOIN TBL_Enterprise ON TBL_Enterprise.ID = TBL_EWithdraw.enterpriseid WHERE TBL_EWithdraw.state=0 ";
        if (GetClass() < 5)
            strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID)";
        cmd = new SqlCommand(strQuery);
        if (GetClass() < 5)
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        dsChargeReqList = dbManager.RunMSelectQuery(cmd);
        cntChargeCount = DataSetUtil.RowCount(dsChargeReqList);
        if (cntChargeCount > 0 && string.IsNullOrEmpty(sndUrl))
        {
            for (int i = 0; i < cntChargeCount; i++)
            {
                if (알람정보들.IndexOf("ewithdraw" + DataSetUtil.RowStringValue(dsChargeReqList, 0, i)) < 0)
                {
                    sndUrl = "../../Images/ewithdraw.wav";
                    break;
                }
            }
        }

        strQuery = "SELECT * FROM TBL_ECharge JOIN TBL_Enterprise ON TBL_Enterprise.ID = TBL_ECharge.enterpriseid WHERE TBL_ECharge.state=3 ";
        if (GetClass() < 5)
            strQuery += " AND (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID)";
        cmd = new SqlCommand(strQuery);
        if (GetClass() < 5)
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        dsChargeReqList = dbManager.RunMSelectQuery(cmd);
        cntChargeCount1 = DataSetUtil.RowCount(dsChargeReqList);
        lblPartnerWithdrawCount.Text = cntChargeCount.ToString() + "건";
        lblPartnerWithdraw.Text = cntChargeCount1.ToString() + "건 완료";

        strQuery = "SELECT * FROM QUESTION WHERE datalength(answer)=0";
        cmd = new SqlCommand(strQuery);
        DataSet dsQuestionList = dbManager.RunAllinSelectQuery(cmd);
        int questionCount = DataSetUtil.RowCount(dsQuestionList);
        lblQuestionCount.Text = questionCount.ToString() + "건";

        if (questionCount > 0 && string.IsNullOrEmpty(sndUrl))
        {
            sndUrl = "../../Images/alarm.mp3";
        }

        strQuery = "SELECT * FROM TBL_USERLIST WHERE ( status=1 and is_new=1 )";
        cmd = new SqlCommand(strQuery);
        DataSet dsUserList = dbManager.RunMSelectQuery(cmd);
        int userCount = DataSetUtil.RowCount(dsUserList);
        lblUserCount.Text = userCount.ToString() + "건";

        if (userCount > 0 && string.IsNullOrEmpty(sndUrl))
        {
            sndUrl = "../../Images/alarm.mp3";
        }

        if (!string.IsNullOrEmpty(sndUrl))
        {
            sndBack.Attributes["src"] = sndUrl;
        }
    }
}
