using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
/// <summary>
///PermissionManager 的摘要说明
/// </summary>
public class PermissionManager
{
    // key: menu_id   value:permission
    private Dictionary<string, int> menuPermission = new Dictionary<string, int>();
    private Dictionary<string, int> funcPermission = new Dictionary<string, int>();

	public PermissionManager(string user_id, string user_type)
	{
        if(user_type.Equals("ENT"))
            return; // 업체관리자아면 

        DBManager dbManager = new DBManager();
        string sql = "SELECT * FROM TBL_EmpPermission WHERE emp_id=(select id from tbl_employee where loginid='" + user_id + "')";
        SqlCommand sqlQuery = new SqlCommand(sql);
        DataSet dsPermission = dbManager.RunMSelectQuery(sqlQuery);
        DataTable table = dsPermission.Tables[0];
        for (int i = 0; i < table.Rows.Count; i++)
        {
            menuPermission.Add(Convert.ToString(table.Rows[i]["menu_id"]), Convert.ToInt32(table.Rows[i]["permission"]));
        }


        string sql2 = "SELECT *, (select func_code from tbl_funcpermission where id=TS.func_id) func_code FROM TBL_FuncPermissionSet TS WHERE emp_id=(select id from tbl_employee where loginid='" + user_id + "')";
        SqlCommand sqlQuery2 = new SqlCommand(sql2);
        DataSet dsPermission2 = dbManager.RunMSelectQuery(sqlQuery2);
        DataTable table2 = dsPermission2.Tables[0];
        for (int i = 0; i < table2.Rows.Count; i++)
        {
            funcPermission.Add(Convert.ToString(table2.Rows[i]["func_code"]), Convert.ToInt32(table2.Rows[i]["permission"]));
        }
	}

    /// <summary>
    /// 메뉴에 따른 사원 권한뺀다, 관리자면 무조건 허용
    /// </summary>
    /// <param name="menu_id"></param>
    /// <param name="user_type"></param>
    /// <returns></returns>
    public int getPermissionByUserType(string menu_id, string user_type)
    {
        if(user_type.Equals("ENT"))
            return 2; // all permission

        if(menu_id != null)
        {
            if (menuPermission.ContainsKey(menu_id))
                return menuPermission[menu_id];
        }else
        {
            return 0;
        }

        return 0;

    }

    /// <summary>
    /// 메뉴에 따른 사원 권한뺀다, 관리자면 무조건 허용
    /// </summary>
    /// <param name="menu_id"></param>
    /// <param name="user_type"></param>
    /// <returns></returns>
    public int getFuncPermission(string func_code, string user_type)
    {
        if (user_type.Equals("ENT"))
            return 1; // all permission

        if (funcPermission.ContainsKey(func_code))
            return funcPermission[func_code];

        return 0;

    }


}
