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

public partial class 게시글관리_공지사항수정 : PageBase
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

            DBManager dbManager = new DBManager();
            string strQuery = "SELECT * FROM TBL_Board WHERE ID=" + strID;
            //string strQuery = "SELECT * FROM TBL_Board WHERE type=1 order by id desc";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsNotice = dbManager.RunMSelectQuery(cmd);
            if (dsNotice.Tables.Count == 0)
            {
                // Response.Redirect("공지사항목록.aspx");
                return;
            }

            if (dsNotice.Tables[0].Rows.Count == 0)
            {
                // Response.Redirect("공지사항목록.aspx");
                return;
            }
            BindInfo(dsNotice);
        }
    }

    void BindInfo(DataSet dsNotice)
    {
        if (dsNotice == null) return;
        if (dsNotice.Tables.Count == 0) return;
        if (dsNotice.Tables[0].Rows.Count == 0) return;

        if (dsNotice.Tables[0].Rows[0]["is_popup"].ToString().Equals("1"))
            chkIsPopup.Checked = true;
        else
            chkIsPopup.Checked = false;

        if (dsNotice.Tables[0].Rows[0]["is_mobile"].ToString().Equals("1"))
            chkIsMobile.Checked = true;
        else
            chkIsMobile.Checked = false;

        tbxTitle.Text = dsNotice.Tables[0].Rows[0]["title"].ToString();
        tbxContent.Text = dsNotice.Tables[0].Rows[0]["TextContent"].ToString();
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("공지사항목록.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strId = hdnID.Value;
        string strTitle = tbxTitle.Text.Trim();
        string strTextContent = tbxContent.Text.Trim();
        string strHTMLContent = tbxContent.Text.Trim();
        string strIsPopup = chkIsPopup.Checked ? "1" : "0";
        string strIsMobile = chkIsMobile.Checked ? "1" : "0";

        if (strId == "0")
        {
            DBManager dbManager = new DBManager();
            string strQuery = "INSERT INTO TBL_Board(userid, title, HTMLContent, TextContent, type, is_popup, is_mobile) ";
            strQuery += "VALUES(@userid, @Title, @Content, @Content, @type, @is_popup, @is_mobile)";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@userid", 1));
            cmd.Parameters.Add(new SqlParameter("@Title", strTitle));
            cmd.Parameters.Add(new SqlParameter("@Content", strTextContent.Replace("\r", "")));
            cmd.Parameters.Add(new SqlParameter("@type", 1));
            cmd.Parameters.Add(new SqlParameter("@is_popup", strIsPopup));
            cmd.Parameters.Add(new SqlParameter("@is_mobile", strIsMobile));

            try
            {
                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "공지사항을 등록하였습니다.";
            }
            catch (Exception ex)
            {
                cvResult.ErrorMessage = "공지사항등록에서 오류가 발생하였습니다. " + ex.ToString();
            }
        }
        else
        {
            DBManager dbManager = new DBManager();
            string strQuery = "UPDATE TBL_Board SET ";
            strQuery += "TextContent=@Content, ";
            strQuery += "HTMLContent=@Content, ";
            strQuery += "Title=@Title, ";
            strQuery += "is_popup=@is_popup, ";
            strQuery += "is_mobile=@is_mobile ";
            strQuery += "WHERE ID=" + strId;
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@Content", strTextContent.Replace("\r", "")));
            cmd.Parameters.Add(new SqlParameter("@Title", strTitle));
            cmd.Parameters.Add(new SqlParameter("@is_popup", strIsPopup));
            cmd.Parameters.Add(new SqlParameter("@is_mobile", strIsMobile));
            try
            {
                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "공지사항을 수정하였습니다.";
            }
            catch (Exception ex)
            {
                cvResult.ErrorMessage = "공지사항수정에서 오류가 발생하였습니다. " + ex.ToString();
            }
        }
        cvResult.IsValid = false;
    }

    
}
