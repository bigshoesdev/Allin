<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Security" %>
<%@ Import Namespace="System.Security.Principal" %>
<%@ Import Namespace="System.Runtime.Remoting" %>
<%@ Import Namespace="System.Web.SessionState" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }

    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {
        if (HttpContext.Current.User != null)
        {
            if (HttpContext.Current.User.Identity.AuthenticationType == "Forms")
            {
                System.Web.Security.FormsIdentity Identity;
                Identity = (System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity;

                //'그 회원이 어떤 role 에 속하는가를 알아낸다.
                DBManager dbManager = new DBManager();
                string strQuery = "SELECT * FROM TBL_Enterprise WHERE LoginID=@LoginID";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(strQuery);
                //cmd.Parameters.Add("@ID", System.Data.SqlDbType.NVarChar);
                cmd.Parameters.Add("@LoginID", System.Data.SqlDbType.NVarChar);
                cmd.Parameters["@LoginID"].Value = Identity.Name;
                System.Data.DataSet dsUser = dbManager.RunMSelectQuery(cmd);
                string strRole = "";
                if (dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
                    strRole = dsUser.Tables[0].Rows[0]["Class"].ToString();

                //'role 이름을 가지고 있는 GenericPrincipal 을 만든다.
                //' 그리고 그것을 다시 현재 Request에 배치한다.
                bool bNoHasRole = (strRole == null || strRole == "");
                if (!bNoHasRole)
                    HttpContext.Current.User = new GenericPrincipal(Identity, new string[] { strRole });
            }
        }
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
