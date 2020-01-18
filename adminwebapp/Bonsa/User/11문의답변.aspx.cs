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

public partial class 게시글관리_11문의답변 : PageBase
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
            string strQuery = "SELECT * FROM QUESTION WHERE ID=" + strID;
            SqlCommand cmd = new SqlCommand(strQuery);
            DataSet dsNotice = dbManager.RunAllinSelectQuery(cmd);
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

        tbxTitle.Text = dsNotice.Tables[0].Rows[0]["title"].ToString();
        tbxContent.Text = dsNotice.Tables[0].Rows[0]["ncontent"].ToString();
        tbxAnswer.Text = dsNotice.Tables[0].Rows[0]["answer"].ToString();
    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("11문의.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strId = hdnID.Value;
        string strAnswer = tbxAnswer.Text.Trim();


        DBManager dbManager = new DBManager();
        string strQuery = "UPDATE QUESTION SET ";
        strQuery += "answer=@Content, ";
        strQuery += "replied=1 ";
        strQuery += "WHERE ID=" + strId;
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add(new SqlParameter("@Content", strAnswer));
        try
        {
            dbManager.RunAllinQuery(cmd);
            cvResult.ErrorMessage = "1:1문의 답변을 하였습니다.";
            Response.Redirect("11문의.aspx");
        }
        catch (Exception ex)
        {
            cvResult.ErrorMessage = "1:1문의 답변에서 오류가 발생하였습니다. " + ex.ToString();
        }
        cvResult.IsValid = false;
    }

    
}
