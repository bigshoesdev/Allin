using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.SqlClient;

public partial class Manager : ModuleBase
{
    public MenuManager menuManager;
    public ArrayList menu;
    public ArrayList icons = new ArrayList();
    
    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLoginStatus();
        if (!IsPostBack)
        {
            //int nClass = int.Parse(인증회원.Tables[0].Rows[0]["Class"].ToString());
            
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        CheckLoginStatus();

        if (인증회원 != null)
        {
            int nClass = int.Parse(인증회원.Tables[0].Rows[0]["Class"].ToString());
            icons.Add("fa fa-user");
            icons.Add("fa fa fa-cog");
            icons.Add("fa fa-credit-card");
            icons.Add("fa fa-calculator");
            icons.Add("fa fa-gamepad");
            icons.Add("fa fa-gamepad");
            icons.Add("fa fa-gamepad");
            icons.Add("fa fa-bullhorn");
            icons.Add("fa fa-cogs");
            icons.Add("fa fa-creative-commons");
            icons.Add("fa fa-recycle");
            icons.Add("fa fa-snowflake-o");

            initMenu(nClass);

            menu = menuManager.getMenu();
            
        }

       
    }

    protected void CheckLoginStatus()
    {
        if (인증회원 == null)
        {
            Session.Clear();
            Session.Abandon(); 
            FormsAuthentication.SignOut();
            //FormsAuthentication.RedirectToLoginPage();
            logout();
            Response.End();
        }
    }

    protected void lnkLogouTBL_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        FormsAuthentication.SignOut();
        //FormsAuthentication.RedirectToLoginPage();
        logout();
        Response.End();
    }

    protected void logout()
    {
        Response.Write("<script>top.location.href='../../Confirm/로그인.aspx'; </script>");
    }

    private void initMenu(int entClass)
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_Menu WHERE allow_class<=" + entClass + " ORDER BY group_id, id";
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        DataSet dsMenu = dbManager.RunMSelectQuery(sqlQuery);
        
        menuManager = new MenuManager();
        string user_id = Convert.ToString(인증회원.Tables[0].Rows[0]["ext_id"]);
        string user_type = Convert.ToString(인증회원.Tables[0].Rows[0]["user_type"]);
        PermissionManager pm = new PermissionManager(user_id, user_type);
        foreach (DataRow row in dsMenu.Tables[0].Rows)
        {
            string menu_id = Convert.ToString(row["id"]);

            // 서브메뉴만 추가해줄지 않줄지 판단
            if (Convert.IsDBNull(row["depth1"]))
                if (pm.getPermissionByUserType(menu_id, user_type) == 0)
                    continue;

            menuManager.addMenu(row);
        }
    }
}
