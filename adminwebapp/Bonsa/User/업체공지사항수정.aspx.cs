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

public partial class 회원관리_업체공지사항수정 : PageBase
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
            string strID = Request.QueryString["id"];
            try { int.Parse(strID); }
            catch { strID = "0"; }
            hdnID.Value = strID;
            BindPartner();

            DBManager dbManager = new DBManager();
            string strQuery = "SELECT * FROM TBL_AdminBoard WHERE ID=" + strID;
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsNotice = dbManager.RunMSelectQuery(cmd);
            if (dsNotice.Tables.Count == 0)
            {
                // Response.Redirect("업체공지사항목록.aspx");
                return;
            }

            if (dsNotice.Tables[0].Rows.Count == 0)
            {
                // Response.Redirect("업체공지사항목록.aspx");
                return;
            }
            BindInfo(dsNotice);
        }
    }

    void BindPartner()
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_Enterprise ";
        strQuery += "WHERE (BonsaID=@ParentID OR BuBonsaID=@ParentID OR ChongpanID=@ParentID)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
        DataSet dsEnterprise = dbManager.RunMSelectQuery(cmd);
        ddlEnterprise.DataSource = dsEnterprise;
        ddlEnterprise.DataTextField = "loginid";
        ddlEnterprise.DataValueField = "ID";
        ddlEnterprise.DataBind();
        ddlEnterprise.Items.Insert(0, new ListItem("전체", "0"));
    }

    void BindInfo(DataSet dsNotice)
    {
        if (dsNotice == null) return;
        if (dsNotice.Tables.Count == 0) return;
        if (dsNotice.Tables[0].Rows.Count == 0) return;

        tbxTitle.Text = dsNotice.Tables[0].Rows[0]["title"].ToString();
        tbxContent.Text = dsNotice.Tables[0].Rows[0]["TextContent"].ToString();
        for (int i = 0; i < ddlEnterprise.Items.Count; i++)
        {
            if (ddlEnterprise.Items[i].Value == dsNotice.Tables[0].Rows[0]["receiverid"].ToString())
            {
                ddlEnterprise.SelectedIndex = i; break;
            }
        }
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("업체공지사항목록.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strId = hdnID.Value;
        string strReceiver = ddlEnterprise.SelectedValue;
        string strTitle = tbxTitle.Text.Trim();
        string strTextContent = tbxContent.Text.Trim();
        string strHTMLContent = tbxContent.Text.Trim();

        if (strId == "0")
        {
            DBManager dbManager = new DBManager();
            string strQuery = "INSERT INTO TBL_AdminBoard(senderid, class, receiverid, title, HTMLContent, TextContent) ";
            strQuery += "VALUES(@senderid, @class, @receiverid, @Title, @Content, @Content)";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@senderid", 인증회원번호));
            cmd.Parameters.Add(new SqlParameter("@class", 인증회원.Tables[0].Rows[0]["class"]));
            cmd.Parameters.Add(new SqlParameter("@receiverid", strReceiver));
            cmd.Parameters.Add(new SqlParameter("@Title", strTitle));
            cmd.Parameters.Add(new SqlParameter("@Content", strTextContent));
            try
            {
                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "업체공지사항을 등록하였습니다.";
            }
            catch (Exception ex)
            {
                cvResult.ErrorMessage = "업체공지사항등록에서 오류가 발생하였습니다. " + ex.ToString();
            }
        }
        else
        {
            DBManager dbManager = new DBManager();
            string strQuery = "UPDATE TBL_AdminBoard SET ";
            strQuery += "Title=@Title, ";
            strQuery += "TextContent=@Content, ";
            strQuery += "HTMLContent=@Content ";
            strQuery += "WHERE ID=" + strId;
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@Content", strTextContent));
            cmd.Parameters.Add(new SqlParameter("@Title", strTitle));
            try
            {
                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "업체공지사항을 수정하였습니다.";
            }
            catch (Exception ex)
            {
                cvResult.ErrorMessage = "업체공지사항수정에서 오류가 발생하였습니다. " + ex.ToString();
            }
        }
        cvResult.IsValid = false;
    }
}
