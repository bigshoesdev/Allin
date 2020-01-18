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

public partial class 게임관리_올인방목록 : PageBase
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

        KEY_AUTOREFRESH = "CACHE:MTOURNAMENT:게임관리:올인방목록:자동갱신:";
        KEY_CACHEINFOS = "CACHE:MTOURNAMENT:게임관리:올인방목록:검색정보들:";
        KEY_CACHEQUERY = "CACHE:MTOURNAMENT:게임관리:올인방목록:검색질문식:";
        KEY_CACHESELINF = "CACHE:MTOURNAMENT:게임관리:올인방목록:선택된정보:";
        KEY_CACHESORT = "CACHE:MTOURNAMENT:게임관리:올인방목록:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:MTOURNAMENT:게임관리:올인방목록:정돈방향:";
        KEY_CACHEFILTER = "CACHE:MTOURNAMENT:게임관리:올인방목록:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:MTOURNAMENT:게임관리:올인방목록:현재페지위치:";
        KEY_CACHEFIRSTDATE = "CACHE:MTOURNAMENT:게임관리:바둑이방목록:첫날자:";
        KEY_CACHELASTDATE = "CACHE:MTOURNAMENT:게임관리:바둑이방목록:끝날자:";


        if (!IsPostBack)
        {
            string menu_id = Request.QueryString["mid"];
            PermissionManager pm = new PermissionManager(인증회원.Tables[0].Rows[0]["ext_id"].ToString(),
                                                        인증회원.Tables[0].Rows[0]["user_type"].ToString());

            int permission = pm.getPermissionByUserType(menu_id, 인증회원.Tables[0].Rows[0]["user_type"].ToString());
            if (permission <= 1)
            {
                btnNew.Visible = false;
            }

            int funcViewerPermission = pm.getFuncPermission("ALLIN_ROOM_SHOW", 인증회원.Tables[0].Rows[0]["user_type"].ToString());

            if(funcViewerPermission < 1)
            {
                roomView.Value = "0";
            }
            else
            {
                roomView.Value = "1";
            }
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("올인방등록.aspx");
    }
}
