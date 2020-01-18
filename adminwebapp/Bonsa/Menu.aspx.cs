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


public partial class Bonsa_Menu : PageBase
{
    public int nClass;

    public MenuManager menuManager = new MenuManager();
    public ArrayList menu;

    public bool isQuestionAlert;
    public ArrayList icons = new ArrayList();
  
    protected void Page_Load(object sender, EventArgs e)
    {
        
        CheckLoginStatus();
        if (!IsPostBack)
        {
           

        }
        //initMenu(GetClass());

        menu = menuManager.getMenu(GetClass(), 인증회원);

        DBManager dbManager = new DBManager();
        string strQuery = "SELECT count(*) as count FROM ALARM WHERE kind='question'";
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        DataSet dsCount = dbManager.RunAllinSelectQuery(sqlQuery);

        int count = Convert.ToInt32(dsCount.Tables[0].Rows[0]["count"]);

        if (count > 0)
            isQuestionAlert = true;
        else
            isQuestionAlert = false;
    }

    

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // CheckLoginStatus();
        icons.Add("fa fa-user");
        icons.Add("fa fa fa-cog");
        icons.Add("fa fa-credit-card");
        icons.Add("fa fa-calculator");
        icons.Add("fa fa-gamepad");
        icons.Add("fa fa-bullhorn");
        icons.Add("fa fa-cogs");
        icons.Add("fa fa-creative-commons");
        icons.Add("fa fa-recycle");
        icons.Add("fa fa-snowflake-o");
       
    }

    void CheckLoginStatus()
    {
        if (인증회원 == null)
        {
            
            Session.Clear();
            Session.Abandon();
           
            FormsAuthentication.SignOut();
            
            //Logout();
            HeaderLogout();
            Response.End();
        }
    }

    protected void lnkLogouTBL_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        FormsAuthentication.SignOut();
        //Logout();
        HeaderLogout();
        Response.End();
    }

    /// <summary>
    /// 로그인한 class 등급에따라 메뉴 보여준다
    /// </summary>
    /// <param name="entClass"></param>
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
