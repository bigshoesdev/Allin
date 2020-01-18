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
///MenuManager 的摘要说明
/// </summary>
public class MenuManager
{
    private ArrayList menu = new ArrayList();
    private SortedDictionary<int, ArrayList> sub_menu = new SortedDictionary<int, ArrayList>();

	public MenuManager()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public ArrayList getMenu()
    {
        return menu;
    }

    public ArrayList getMenu(int nClass, DataSet ds)
    {
        initMenu(nClass, ds);
        return menu;
    }

    public void addMenu(DataRow row)
    {
        // depth2 menu
        if (Convert.IsDBNull(row["depth1"]))
        {
            addSubMenu(row);
        }
        else
        {
            // depth1 menu
            addTopMenu(row);
        }
    }   

    public ArrayList getSubMenu(DataRow row)
    {
        if (sub_menu.Count == 0)
            return null;

        if (!sub_menu.ContainsKey(Convert.ToInt16(row["group_id"])))
            return null;

        return sub_menu[Convert.ToInt16(row["group_id"])];
    }

    private void addSubMenu(DataRow row)
    {
        int group_id = Convert.ToInt16(row["group_id"]);
        if (sub_menu.ContainsKey(group_id))
        {            
            sub_menu[group_id].Add(row);
        }
        else
        {
            ArrayList _rowList = new ArrayList();
            _rowList.Add(row);
            sub_menu.Add(group_id, _rowList);
        }
    }

    

    private void addTopMenu(DataRow row)
    {
        menu.Add(row);
    }

    /// <summary>
    /// 로그인한 class 등급에따라 메뉴 보여준다
    /// </summary>
    /// <param name="entClass"></param>
    private void initMenu(int entClass, DataSet ds)
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_Menu WHERE allow_class<=" + entClass + " ORDER BY group_id, id";
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        DataSet dsMenu = dbManager.RunMSelectQuery(sqlQuery);
        
        string user_id = Convert.ToString(ds.Tables[0].Rows[0]["ext_id"]);
        string user_type = Convert.ToString(ds.Tables[0].Rows[0]["user_type"]);
        PermissionManager pm = new PermissionManager(user_id, user_type);
        foreach (DataRow row in dsMenu.Tables[0].Rows)
        {
            string menu_id = Convert.ToString(row["id"]);

            // 서브메뉴만 추가해줄지 않줄지 판단
            if (Convert.IsDBNull(row["depth1"]))
                if (pm.getPermissionByUserType(menu_id, user_type) == 0)
                    continue;

            addMenu(row);
        }
    }
}




