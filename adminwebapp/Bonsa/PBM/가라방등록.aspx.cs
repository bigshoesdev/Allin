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

public partial class 게임관리_가라방상세정보 : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (인증회원 == null || GetClass() < 4)
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
            string strID = Request.QueryString["id"];
            try { int.Parse(strID); }
            catch { strID = "0"; }
            hdnID.Value = strID;

            BindInfo();
        }
    }

    void BindDDL()
    {
        ddlPingMoney.Items.Clear();
        ddlCurPlayer.Items.Clear();
        switch (ddlGameType.SelectedValue)
        {
            case "1": // 포커이면
                ddlPingMoney.Items.Add(new ListItem("300원방", "300"));
                ddlPingMoney.Items.Add(new ListItem("500원방", "500"));
                ddlPingMoney.Items.Add(new ListItem("1000원방", "1000"));
                ddlPingMoney.Items.Add(new ListItem("2000원방", "2000"));
                ddlPingMoney.Items.Add(new ListItem("5000원방", "5000"));
                ddlPingMoney.Items.Add(new ListItem("10000원방", "10000"));
                ddlCurPlayer.Items.Add(new ListItem("2명", "2"));
                ddlCurPlayer.Items.Add(new ListItem("3명", "3"));
                ddlCurPlayer.Items.Add(new ListItem("4명", "4"));
                ddlCurPlayer.Items.Add(new ListItem("5명", "5"));
                break;
            case "2": // 바둑이이면
                ddlPingMoney.Items.Add(new ListItem("300원방", "300"));
                ddlPingMoney.Items.Add(new ListItem("500원방", "500"));
                ddlPingMoney.Items.Add(new ListItem("1000원방", "1000"));
                ddlPingMoney.Items.Add(new ListItem("2000원방", "2000"));
                ddlPingMoney.Items.Add(new ListItem("5000원방", "5000"));
                ddlPingMoney.Items.Add(new ListItem("10000원방", "10000"));
                ddlCurPlayer.Items.Add(new ListItem("2명", "2"));
                ddlCurPlayer.Items.Add(new ListItem("3명", "3"));
                ddlCurPlayer.Items.Add(new ListItem("4명", "4"));
                ddlCurPlayer.Items.Add(new ListItem("5명", "5"));
                break;
            case "3": // 맞고이면
                ddlPingMoney.Items.Add(new ListItem("300원방", "300"));
                ddlPingMoney.Items.Add(new ListItem("500원방", "500"));
                ddlPingMoney.Items.Add(new ListItem("1000원방", "1000"));
                ddlPingMoney.Items.Add(new ListItem("2000원방", "2000"));
                ddlPingMoney.Items.Add(new ListItem("5000원방", "5000"));
                ddlPingMoney.Items.Add(new ListItem("10000원방", "10000"));
                ddlCurPlayer.Items.Add(new ListItem("2명", "2"));
                break;
        }
    }

    void BindInfo()
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_VROOMINFO WHERE ID=" + hdnID.Value;
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsRobotList = dbManager.RunMSelectQuery(cmd);

        if (DataSetUtil.IsNullOrEmpty(dsRobotList))
        {
            BindDDL();
            return;
        }

        hdnID.Value = dsRobotList.Tables[0].Rows[0]["id"].ToString();
        ddlGameType.SelectedValue = DataSetUtil.RowStringValue(dsRobotList, "gametype", 0);
        BindDDL();
        ddlGameType.Enabled = false;
        ddlPingMoney.SelectedValue = DataSetUtil.RowStringValue(dsRobotList, "bingmoney", 0);
        ddlCurPlayer.SelectedValue = DataSetUtil.RowStringValue(dsRobotList, "curusercount", 0);
        tbxRoomName.Text = DataSetUtil.RowStringValue(dsRobotList, "roomname", 0);

        tbxTime.Text = ((int)(DataSetUtil.RowDateTime(dsRobotList, "expiretime", 0) - DateTime.Now).TotalHours).ToString();
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("가라방목록.aspx");
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strId = hdnID.Value;
        //방형태
        int iGameType = int.Parse(ddlGameType.SelectedValue);
        //방이름
        string strRoomName = tbxRoomName.Text.Trim();
        //삥머니
        int iPingMoney = int.Parse(ddlPingMoney.SelectedValue);
        //방안유저수
        int iCntPlayer = int.Parse(ddlCurPlayer.SelectedValue);
        //유지시간
        int nTime = int.Parse(tbxTime.Text.Trim());


        try
        {
            if (strId == "0")
            {
                string strQuery = "INSERT INTO TBL_VROOMINFO(gametype, roomname,bingmoney,curusercount, expiretime) " +
                    "VALUES(@gametype, @roomname,@bingmoney,@curusercount, DATEADD(hour, @time, GETDATE()))";
                SqlCommand cmd;
                DBManager dbManager = new DBManager();
                cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add(new SqlParameter("@gametype", iGameType));
                cmd.Parameters.Add(new SqlParameter("@roomname", strRoomName));
                cmd.Parameters.Add(new SqlParameter("@bingmoney", iPingMoney));
                cmd.Parameters.Add(new SqlParameter("@curusercount", iCntPlayer));
                cmd.Parameters.Add(new SqlParameter("@time", nTime));

                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "가라방이 등록되였습니다";
            }
            else
            {
                string strQuery = "UPDATE TBL_VROOMINFO SET gametype=@gametype, roomname=@roomname,bingmoney=@bingmoney, curusercount=@curusercount, expiretime=DATEADD(hour, @time, GETDATE()), ischange=1 WHERE id=@id ";
                SqlCommand cmd;
                DBManager dbManager = new DBManager();
                cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add(new SqlParameter("@gametype", iGameType));
                cmd.Parameters.Add(new SqlParameter("@roomname", strRoomName));
                cmd.Parameters.Add(new SqlParameter("@bingmoney", iPingMoney));
                cmd.Parameters.Add(new SqlParameter("@curusercount", iCntPlayer));
                cmd.Parameters.Add(new SqlParameter("@time", nTime));
                cmd.Parameters.Add(new SqlParameter("@id", int.Parse(strId)));

                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "가라방정보가 수정되었습니다.";
            }
        }
        catch (Exception ex)
        {
            cvResult.ErrorMessage = "가라방등록에서 오류가 발생하였습니다." + ex.ToString();
        }
        cvResult.IsValid = false;
    }
    protected void ddlGameType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDDL();
    }
}
