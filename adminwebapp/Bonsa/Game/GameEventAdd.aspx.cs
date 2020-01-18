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

public partial class Bonsa_Game_GameEventAdd : PageBase
{
    DataSet dsEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (인증회원 == null)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            //Logout();
            Logout();
            Response.End();
            return;
        }
        if (!IsPostBack)
        {
            string EventID = Request.QueryString["EventID"];
            try
            {
                int.Parse(EventID);
            }
            catch
            {
                EventID = "0";
            }         

            DBManager dbManager = new DBManager();
            string strQuery = "SELECT * FROM TBL_Event WHERE EventID=" + EventID;
            SqlCommand cmd = new SqlCommand(strQuery);
            dsEvent = dbManager.RunMSelectQuery(cmd);
            if (dsEvent.Tables.Count == 0)
            {
                // Response.Redirect("공지사항목록.aspx");
                return;
            }

            if (dsEvent.Tables[0].Rows.Count == 0)
            {
                // Response.Redirect("공지사항목록.aspx");
                return;
            }
            BindInfo(dsEvent);
        }
    }

    void BindInfo(DataSet dsPartner)
    {
        
        if (dsEvent == null) return;
        if (dsEvent.Tables.Count == 0) return;
        if (dsEvent.Tables[0].Rows.Count == 0) return;

        hidEventID.Value = dsEvent.Tables[0].Rows[0]["EventID"].ToString().Trim();
        tbxEventName.Text = dsEvent.Tables[0].Rows[0]["EventName"].ToString().Trim();
        //tbxImageUrl.Text = dsEvent.Tables[0].Rows[0]["ImageDir"].ToString();
        tbxLinkUrl.Text = dsEvent.Tables[0].Rows[0]["LinkUrl"].ToString().Trim();
        ddlIsActive.Text = dsEvent.Tables[0].Rows[0]["IsActive"].ToString().Trim();
       
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("GameEventList.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strEventName = tbxEventName.Text.Trim();
       // string strImageUrl = tbxImageUrl.Text.Trim();

        string strLinkUrl = tbxLinkUrl.Text.Trim();
        string strIsActive = ddlIsActive.Text;
        string strEventID = hidEventID.Value;

        string sitePath = HttpContext.Current.Request.PhysicalApplicationPath;
        string middlePath = @"\Images\Event\";
        string middleUrl = @"/Images/Event/";
        string host = "http://" + HttpContext.Current.Request.Url.Host;
        //Response.Write("host:" + host); Response.End();
     
        string fileName = "";

        // new event
        if (strEventID.Equals("0"))
        {
            //int nClass = int.Parse(인증회원.Tables[0].Rows[0]["Class"].ToString());
            if (CheckExistID(strEventName, strEventID))
            {
                cvResult.ErrorMessage = "이미 등록된 이벤트명 입니다.";
                cvResult.IsValid = false;
                return;
            }

            if (tbxImageFile.HasFile)
            {
                fileName = tbxImageFile.FileName;
                string savePath = sitePath + middlePath + fileName;
                tbxImageFile.SaveAs(savePath);
            }

            DBManager dbManager = new DBManager();
            string strQuery = "INSERT INTO TBL_Event(EventName, ImageDir, ImagePhisicalDir, LinkUrl, IsActive) ";
            strQuery += "VALUES(@EventName, @ImageDir, @PhisicalDir, @LinkUrl, @IsActive)";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@EventName", strEventName));
            cmd.Parameters.Add(new SqlParameter("@ImageDir", host + middleUrl + fileName));
            cmd.Parameters.Add(new SqlParameter("@PhisicalDir", middlePath + fileName));
            cmd.Parameters.Add(new SqlParameter("@LinkUrl", strLinkUrl));
            cmd.Parameters.Add(new SqlParameter("@IsActive", strIsActive));
            try
            {
                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "게임이벤트정보가 등록되였습니다.";
            }
            catch (Exception ex)
            {
                cvResult.ErrorMessage = "게임이벤트 등록에서 오류가 발생하였습니다. " + ex.ToString();
            }
        }
        else
        {
            if (CheckExistID(strEventName, strEventID))
            {
                cvResult.ErrorMessage = "이미 등록된 이벤트명 입니다.";
                cvResult.IsValid = false;
                return;
            }

            if (tbxImageFile.HasFile)
            {
                fileName = tbxImageFile.FileName;
                string savePath = sitePath + middlePath + fileName;
                tbxImageFile.SaveAs(savePath);
            }

            DBManager dbManager = new DBManager();
            string strQuery;
            if(tbxImageFile.HasFile)
                strQuery = "UPDATE TBL_Event SET EventName=@EventName, ImageDir=@ImageDir, ImagePhisicalDir=@PhisicalDir, LinkUrl=@LinkUrl, IsActive=@IsActive, RegTime=GetDate()";
            else
                strQuery = "UPDATE TBL_Event SET EventName=@EventName, LinkUrl=@LinkUrl, IsActive=@IsActive, RegTime=GetDate()";
            strQuery += "WHERE EventID=" + strEventID;
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@EventName", strEventName));

            if (tbxImageFile.HasFile)
            {
                cmd.Parameters.Add(new SqlParameter("@ImageDir", host + middleUrl + fileName));
                cmd.Parameters.Add(new SqlParameter("@PhisicalDir", middlePath + fileName));
            }
            cmd.Parameters.Add(new SqlParameter("@LinkUrl", strLinkUrl));
            cmd.Parameters.Add(new SqlParameter("@IsActive", strIsActive));
            try
            {
                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "게임이벤트정보가 수정되였습니다.";
            }
            catch (Exception ex)
            {
                cvResult.ErrorMessage = "게임이벤트정보 수정에서 오류가 발생하였습니다. " + ex.ToString();
            }
        }
        cvResult.IsValid = false;
    }

    bool CheckExistID(string event_name, string event_id)
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_Event WHERE EventName=@EventName And EventID <>" + event_id;
        //Response.Write(strQuery); Response.End();
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        sqlQuery.Parameters.Add("@EventName", SqlDbType.NVarChar);
        sqlQuery.Parameters["@EventName"].Value = event_name;
        DataSet dsEvent = dbManager.RunMSelectQuery(sqlQuery);

        bool bExist = false;
        if (dsEvent.Tables.Count == 0)
        {
            bExist = false;
        }
        else if (dsEvent.Tables[0].Rows.Count == 0)
        {
            bExist = false;
        }
        else
        {
            bExist = true;
        }
        return bExist;
    }
}
