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

public partial class Bonsa_Game_ManualEventMng : PageBase
{
    string id = null;

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
            id = Request.QueryString["id"];

            if(string.IsNullOrEmpty(id) == false)
                BindData();
        }
    }

    protected void BindData()
    {
        
        string strQuery = "SELECT TOP 1 * FROM TBL_MANUALEVENT ";
        DBManager dbManager = new DBManager();
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet ds = dbManager.RunMSelectQuery(cmd);
        if (ds != null)
        {
            if (ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                string strEventName = ds.Tables[0].Rows[0]["event_name"].ToString();
                int iRaceCount = Convert.ToInt32(ds.Tables[0].Rows[0]["race_count"]);
                int iEventMoney = Convert.ToInt32(ds.Tables[0].Rows[0]["give_money"]);
                short sUseYn = Convert.ToInt16(ds.Tables[0].Rows[0]["use_yn"]);

                tbxEventName.Text = strEventName;
                tbxRaceCount.Text = iRaceCount.ToString();
                tbxEventMoney.Text = iEventMoney.ToString();
                if (sUseYn == 0)
                    ddlUseYn.SelectedIndex = 1;
                else
                    ddlUseYn.SelectedIndex = 0;
            }
        }
        
        //vsErrors.ErrorMessage = "관리자정보가 변경되였습니다.";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        string strEventName = tbxEventName.Text.Trim();
        string strRaceCount = tbxRaceCount.Text;
        string strEventMoney = tbxEventMoney.Text.Trim();
        string strUseYn = ddlUseYn.SelectedValue;
        string strEventType = ddlEventType.SelectedValue;

        string strQuery;

        if (string.IsNullOrEmpty(id))
        {
            //strQuery = string.Format("INSERT INTO TBL_MANUALEVENT (event_name, race_count, give_money, use_yn) VALUES('{0}', {1}, {2}, '{3}')", strEventName, strRaceCount, strEventMoney, strUseYn);
            // 2017-04-19 사용여부가 9 는 신규등록상태이다 1: 사용중  0: 정지
            strQuery = string.Format("INSERT INTO TBL_MANUALEVENT (event_name, race_count, give_money, use_yn, event_type) VALUES('{0}', {1}, {2}, '{3}', '{4}')", strEventName, strRaceCount, strEventMoney, "9", strEventType);
            DBManager dm = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            dm.RunMQuery(cmd);
            cvResult.ErrorMessage = "이벤트 등록 되었습니다.";
            cvResult.IsValid = false;
        }
        else
        {
            strQuery = string.Format("UPDATE TBL_MANUALEVENT SET event_name='{0}', race_count={1}, give_money={2}, use_yn='{3}' WHERE id={4}", strEventName, strRaceCount, strEventMoney, strUseYn, id);
            DBManager dm = new DBManager();
            SqlCommand cmd = new SqlCommand(strQuery);
            dm.RunMQuery(cmd);
            cvResult.ErrorMessage = "이벤트 수정 되었습니다.";
            cvResult.IsValid = false;
        }
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManualEventList.aspx");
    }
}
