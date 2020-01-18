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

public enum ViewMode
{
    목록보기,
    상세보기,
}

public enum EditMode
{
    보기,
    새로,
    수정
}

/// <summary>
/// Summary description for PageBase
/// </summary>
public abstract class PageBase : System.Web.UI.Page
{
    public int ROOMCOUNT_ROOM = 50;
    public string[] CHANNEL_NAME = {"1채널 100원", "2채널 100원", "3채널 200원", "4채널 200원"};

    public string[] YESI_NAME = {"예시없음", "밤예시", "거북이", "해파리", 
                               "상어", "고래", "미친상어", "미친고래",
                               "황금거북이", "황금고래", "잠수함",
                               "돌발상어", "돌발고래"};

    #region 상수정의부
    //
    // Exception Logging constant
    //
    private const string UNHANDLED_EXCEPTION = "Unhandled Exception:";
    //
    // 오류통보문들
    //
    protected string m_szErrorMsg = "";
    public static string 오류문_정보보존실패 = "정보보존에서 실패하였습니다.";
    public static string 오류문_정보삭제실패 = "정보삭제에서 실패하였습니다.";

    public static string 오류문_선택된정보없음 = "해당 정보를 선택하세요.";
    public static string 오류문_잘못된선택정보 = "선택된 정보가 자료기지에 없습니다.";
    public static string 오류문_검색된자료없음 = "자료가 하나도 없습니다.";
    public static string 알림문_검색된자료없음 = "　* 만일 검색을 진행하셨다면 [갱신/검색취소]단추를 누르세요.";

    //
    // Session Keys : 이 값들은 해당 페지들의 load 사건시 결정된다.
    //    아래의 값들은 무조건 결정되여야 한다.
    //
    public static string KEY_CACHEUSER = "CACHE:NEWBACARA:인증회원:";
    public static string KEY_CACHEINFOS = "CACHE:NEWBACARA:XXX검색정보들:";
    public static string KEY_CACHEQUERY = "CACHE:NEWBACARA:XXX검색질문식:";
    public static string KEY_CACHESELINF = "CACHE:NEWBACARA:XXX선택된정보:";
    public static string KEY_CACHESORT = "CACHE:NEWBACARA:XXX정돈항:";
    public static string KEY_CACHESORTDIRECTION = "CACHE:NEWBACARA:XXX정돈방향:";
    public static string KEY_CACHEFILTER = "CACHE:NEWBACARA:XXX검색인자들:";
    public static string KEY_CACHECURPAGE = "CACHE:NEWBACARA:XXX현재페지위치:";
    public static string KEY_AUTOREFRESH = "CACHE:NEWBACARA:XXX자동갱신:";
    public static string KEY_CACHEALARM = "CACHE:NEWBACARA:XXX알람정보들:";

    public static string KEY_EDITMODE = "CACHE:NEWBACARA:EDITMODE:";
    public static string KEY_VIEWMODE = "CACHE:NEWBACARA:VIEWMODE:";

    public static string KEY_CACHEFIRSTDATE = "CACHE:NEWBACARA:첫날자:";
    public static string KEY_CACHELASTDATE = "CACHE:NEWBACARA:끝날자:";

    #endregion

    //
    // SSL
    //
    protected static string pageSecureUrlBase;
    protected static string pageUrlBase;
    protected static string urlSuffix;

    //
    // 새 정보(레코드)편집시 레코드에 대한 표식부호/값
    //
    private static object NEWRECORDVALUE = "0";

    #region 체계룰정의부
    //
    // 지령실행시 또는 지령의 유(무)효성을 결정할 때 참고되는 회원권한부류들
    // 이 값들은 페지안에 있게되는 콘트롤들의 기초클라스인 ModuleBase 클라스에서도 참고된다.
    // 
    public const string ROLES_SUPERBONSA = "5";
    public const string ROLES_BONSA = "4";
    public const string ROLES_BUBONSA = "3";
    public const string ROLES_CHONGPAN = "2";
    public const string ROLES_MAEJANG = "1";
    public const string ROLES_MEMBER = "0";

    const string ROLES_SUPERBONSA_tmp = "#" + ROLES_SUPERBONSA + "#";
    const string ROLES_BONSA_tmp = "#" + ROLES_BONSA + "#";
    const string ROLES_BUBONSA_tmp = "#" + ROLES_BUBONSA + "#";
    const string ROLES_CHONGPAN_tmp = "#" + ROLES_CHONGPAN + "#";
    const string ROLES_MAEJANG_tmp = "#" + ROLES_MAEJANG + "#";
    const string ROLES_MEMBER_tmp = "#" + ROLES_MEMBER + "#";

    public const string ROLES_MANAGER = ROLES_SUPERBONSA_tmp;
    public const string ROLES_ENTERPRISE = ROLES_BONSA_tmp + ROLES_BUBONSA_tmp + ROLES_CHONGPAN_tmp + ROLES_MAEJANG_tmp;

    public const string ROLES_ADMINPAGE = ROLES_BONSA_tmp + ROLES_BUBONSA_tmp + ROLES_CHONGPAN_tmp + ROLES_MAEJANG_tmp;
    public const string ROLES_MEMBERPAGE = ROLES_MEMBER_tmp;

    //
    // 추가 삭제 갱신 보기 등을 할수 있는 권한그릅을 설정한다.
    // 체계전반의 보안정책을 제공한다.
    //
    public static string ROLES_SPC_DELETABLE = ROLES_ENTERPRISE;
    public static string ROLES_NOR_DELETABLE = ROLES_ENTERPRISE;
    public static string ROLES_DEF_DELETABLE = ROLES_ENTERPRISE;

    public static string ROLES_SPC_EDITABLE = ROLES_ENTERPRISE;
    public static string ROLES_NOR_EDITABLE = ROLES_ENTERPRISE;
    public static string ROLES_DEF_EDITABLE = ROLES_ENTERPRISE;

    public static string ROLES_SPC_NEWABLE = ROLES_ENTERPRISE;
    public static string ROLES_NOR_NEWABLE = ROLES_ENTERPRISE;
    public static string ROLES_DEF_NEWABLE = ROLES_ENTERPRISE;

    #endregion

    /// <summary>
    ///		구축자, 국부변수들을 초기화한다.
    /// </summary>
    public PageBase()
    {
        try
        {
            urlSuffix = Context.Request.Url.Host + Context.Request.ApplicationPath;
            pageUrlBase = "http://" + urlSuffix;
        }
        catch
        { }
    }

    /// <summary>
    ///		URL에 대한 앞붙이를 얻는다.
    /// </summary>
    public static string UrlBase
    {
        get
        {
            return pageUrlBase;
        }
    }

    /// <summary>
    ///		확인된 회원에 대한 자료를 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    /// </summary>
    public virtual DataSet 인증회원
    {
        get
        {
            try
            {
                DataSet ds = (DataSet)Session[KEY_CACHEUSER];
                if (ds == null) return null;
                if (ds.Tables.Count == 0) return null;
                if (ds.Tables[0].Rows.Count != 1) return null;
                return ds;
            }
            catch{
            }
            return null;
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHEUSER);
            else
                Session[KEY_CACHEUSER] = value;
        }
    }

    /// <summary>
    ///		확인된 회원에 대한 자료를 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    /// </summary>
    public virtual int 인증회원번호
    {
        get
        {
            try
            {
                DataSet ds = (DataSet)Session[KEY_CACHEUSER];
                if (ds == null) return 0;
                if (ds.Tables.Count == 0) return 0;
                if (ds.Tables[0].Rows.Count != 1) return 0;
                return int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
            }
            catch
            {
            }
            return 0;
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHEUSER);
            else
                Session[KEY_CACHEUSER] = value;
        }
    }

    /// <summary>
    ///		확인된 회원에 대한 자료를 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    /// </summary>
    public virtual string 파트너코드
    {
        get
        {
            try
            {
                DataSet ds = (DataSet)Session[KEY_CACHEUSER];
                if (ds == null) return "-";
                if (ds.Tables.Count == 0) return "-";
                if (ds.Tables[0].Rows.Count != 1) return "-";
                return ds.Tables[0].Rows[0]["PartnerCode"].ToString();
            }
            catch
            {
            }
            return "-";
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHEUSER);
            else
                Session[KEY_CACHEUSER] = value;
        }
    }

    public DataSet 검색정보들
    {
        get
        {
            try
            {
                DataSet retValue = (DataSet)Session[KEY_CACHEINFOS];
                return retValue;
            }
            catch
            {
                return null;
            }
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHEINFOS);
            else
                Session[KEY_CACHEINFOS] = value;
        }
    }

    /// <summary>
    ///			매개 정보들에 대한 검색질문식들을 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    ///		<remark>매개 상회정보페지들에서 (검색질문식작성후) 이용된다.</remark>
    /// </summary>
    public string 검색질문식
    {
        get
        {
            try
            {
                return (string)Session[KEY_CACHEQUERY];
            }
            catch
            {
                return null;
            }
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHEQUERY);
            else
                Session[KEY_CACHEQUERY] = value;
        }
    }

    /// <summary>
    ///		검색인자들에 대한 자료를 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    /// </summary>
    public object 검색인자들
    {
        get
        {
            try
            {
                return Session[KEY_CACHEFILTER];
            }
            catch
            {
                return null;
            }
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHEFILTER);
            else
                Session[KEY_CACHEFILTER] = value;
        }
    }

    /// <summary>
    ///		매개 정보페지들에서 현시할 대상(양식 ? 대장 ? 검색)을 처리(보관/얻기)한다.
    ///		<remark>뷰스테트에 보관된다.</remark>
    /// </summary>
    protected string 현시대상
    {
        get
        {
            return (string)this.ViewState["현시대상"];
        }
        set
        {
            this.ViewState["현시대상"] = value;
        }
    }

    /// <summary>
    ///		해당 동작(확인 및 등록)을 진행하고 
    ///		귀환할 대상(이 페지를 호출한 대상)을 처리(보관/얻기)한다.
    ///		<remark>뷰스테트에 보관된다.</remark>
    /// </summary>
    protected string 귀환대상
    {
        get
        {
            return (string)this.ViewState["귀환대상"];
        }
        set
        {
            this.ViewState["귀환대상"] = value;
        }
    }

    /// <summary>
    ///		매개 정보페지들에서 검색창을 호출한 주인(양식 ? 대장 ? 검색)을 처리(보관/얻기)한다.
    ///		<remark>뷰스테트에 보관된다.</remark>
    /// </summary>
    protected string 검색주인
    {
        get
        {
            return (string)this.ViewState["검색주인"];
        }
        set
        {
            this.ViewState["검색주인"] = value;
        }
    }

    /// <summary>
    ///		양식대장과 목록대장 현시에서 이용되는 정돈항을 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    /// </summary>
    public string 정돈항
    {
        get
        {
            try
            {
                if (Session[KEY_CACHESORT].ToString() == "")
                    return "ID";
                return (string)Session[KEY_CACHESORT];
            }
            catch
            {
                return "ID";
            }
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHESORT);
            else
                Session[KEY_CACHESORT] = value;
        }
    }

    /// <summary>
    /// 양식보기에서 검색을 위한 첫 날자정보를 보관한다.
    /// </summary>
    public DateTime 첫날자
    {
        get
        {
            try
            {
                return (DateTime)Session[KEY_CACHEFIRSTDATE];
            }
            catch
            {
                //return DateTime.Today.AddDays(-10);
                return new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, 1);
            }
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHEFIRSTDATE);
            else
            {
                try
                {
                    Session[KEY_CACHEFIRSTDATE] = DateTime.Parse(value.ToString("yyyy-MM-dd 00:00:00"));
                }
                catch (Exception de)
                {
                    Session[KEY_CACHEFIRSTDATE] = new DateTime(DateTime.Now.Year, 1, 1);
                }
            }
        }
    }

    /// <summary>
    /// 양식보기에서 검색을 위한 마지막날자정보를 보관한다.
    /// </summary>
    public DateTime 끝날자
    {
        get
        {
            try
            {
                return (DateTime)Session[KEY_CACHELASTDATE];
            }
            catch
            {
                
                return DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd 23:59:59"));
            }
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHELASTDATE);
            else
            {
                Session[KEY_CACHELASTDATE] = DateTime.Parse(value.ToString("yyyy-MM-dd 23:59:59"));
            }
        }
    }

    /// <summary>
    ///		양식대장과 목록대장 현시에서 이용되는 정돈방향을 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    /// </summary>
    public SortDirection 정돈방향
    {
        get
        {
            try
            {
                return (SortDirection)Session[KEY_CACHESORTDIRECTION];
            }
            catch
            {
                return SortDirection.Descending;
            }
        }
        set
        {
            Session[KEY_CACHESORTDIRECTION] = value;
        }
    }

    /// <summary>
    ///		목록대장에서 선택된 정보를 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    ///		<remark>이 정보는 양식대장에서 이용된다.</remark>
    /// </summary>
    public DataRow 선택된정보
    {
        get
        {
            try
            {
                return (DataRow)Session[KEY_CACHESELINF];
            }
            catch
            {
                return null;
            }
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHESELINF);
            else
                Session[KEY_CACHESELINF] = value;
        }
    }

    /// <summary>
    ///		목록 및 양식 대장의 현재 페지위치를 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    /// </summary>
    public int 현재페지위치
    {
        get
        {
            try
            {
                return (int)Session[KEY_CACHECURPAGE];
            }
            catch
            {
                return CF.iNothing;
            }
        }
        set
        {
            if (value == CF.iNothing)
                Session.Remove(KEY_CACHECURPAGE);
            else
                Session[KEY_CACHECURPAGE] = value;
        }
    }

    /// <summary>
    ///		목록 및 양식 대장의 페지당 현시 개수를  처리(보관/얻기)한다.
    ///		<remark>뷰스테스에 보관된다.</remark>
    /// </summary>
    protected int 페지당현시개수
    {
        get
        {
            try
            {
                return 20;
                return (int)this.Session["페지당현시개수"];
            }
            catch
            {
                return 10;
            }
        }
        set
        {
            this.Session["페지당현시개수"] = value;
        }
    }

    /// <summary>
    ///		목록 및 양식 대장의 페지당 현시 개수를  처리(보관/얻기)한다.
    ///		<remark>뷰스테스에 보관된다.</remark>
    /// </summary>
    protected bool 자동갱신
    {
        get
        {
            try
            {
                return (bool)this.Session[KEY_AUTOREFRESH];
            }
            catch
            {
                return false;
            }
        }
        set
        {
            this.Session[KEY_AUTOREFRESH] = value;
        }
    }

    /// <summary>
    ///		편집상태를 처리(보관/얻기)한다.
    ///		<remark>뷰스테트에 보관된다.</remark>
    ///		<remark>상태가 새편집인 경우: "new",  갱신(수정)인 경우: "edit"</remark>
    /// </summary>
    public DetailsViewMode PageEditMode
    {
        get
        {
            try
            {
                return (DetailsViewMode)this.Session[KEY_EDITMODE];
            }
            catch
            {
                return DetailsViewMode.ReadOnly;
            }
        }
        set
        {
            자동갱신 = (value == DetailsViewMode.ReadOnly);
            this.Session[KEY_EDITMODE] = value;
        }
    }

    /// <summary>
    ///		뷰상태를 처리(보관/얻기)한다.
    ///		<remark>뷰스테트에 보관된다."list": 목록보기, "detail": 상세보기, "edit": 편집상태보기</remark>
    /// </summary>
    public ViewMode PageViewMode
    {
        get
        {
            try
            {
                return (ViewMode)this.Session[KEY_VIEWMODE];
            }
            catch
            {
                return ViewMode.목록보기;
            }
        }
        set
        {
            this.Session[KEY_VIEWMODE] = value;
        }
    }

    /// <summary>
    ///		자료표(대장 및 양식)의 총 페지수를 처리(보관/얻기)한다.
    ///		<remark>뷰스테트에 보관된다.</remark>
    /// </summary>
    public int 총페지수
    {
        get
        {
            return (int)this.ViewState["총페지수"];
        }
        set
        {
            this.ViewState["총페지수"] = value;
        }
    }

    /// <summary>
    ///		정보목록에서 선택된 정보의 등록번호를 처리(보관/얻기)한다.
    ///		<remark>뷰스테트에 보관된다.</remark>
    /// </summary>
    public int 선택정보번호
    {
        get
        {
            return (int)this.ViewState["선택정보번호"];
        }
        set
        {
            this.ViewState["선택정보번호"] = value;
        }
    }

    /// <summary>
    ///		검색된 정보의 개수를 처리(보관/얻기)한다.
    ///		<remark>뷰스테트에 보관된다.</remark>
    /// </summary>
    public int 검색정보개수
    {
        get
        {
            return (int)this.ViewState["검색정보개수"];
        }
        set
        {
            this.ViewState["검색정보개수"] = value;
        }
    }

    /// <summary>
    ///		알람을 울릴 대상들을 선정한다. 
    ///		<remark>쎄션에 보관된다.</remark>
    ///		<remark>1:1게시판, 충전, 환전체계들에서 이용한다.</remark>
    /// </summary>
    public string 알람정보들
    {
        get
        {
            try
            {
                return Session[KEY_CACHEALARM].ToString();
            }
            catch
            {
                return "";
            }
        }
        set
        {
            if (value == null)
                Session.Remove(KEY_CACHEALARM);
            else
                Session[KEY_CACHEALARM] = value;
        }
    }

    /// <summary>
    ///		확인된 회원의 권한이 정의된 권한범위안에 있는가 없는가를 판정한다.
    /// </summary>
    /// <param name="szRoles">정의된 권한범위</param>
    /// <returns>있으면 tru; 없으면 false</returns>
    public virtual bool User_IsInRoles(string szRoles)
    {
        if (인증회원 == null) return false;
        string UserRole = 인증회원.Tables[0].Rows[0]["Class"].ToString();
        return ((UserRole != "") && (szRoles.IndexOf("#" + UserRole + "#") >= 0));
        return false;
    }

    /// <summary>
    /// 검색된 정보로부터 질문식을 실행한 자료를 얻는다
    /// </summary>
    /// <param name="질문식">검색질의식, ""이면 전체 검색자료를 리턴한다.</param>
    /// <param name="자료표명">검색된정보의 테블명 null 혹은 ""이면 0번 테블을 참조한다.</param>
    /// <returns>질문식을 실행한 결과 혹은 null</returns>
    public virtual DataSet 검색정보얻기(string 질문식, string 자료표명)
    {
        if (검색정보들 == null)
            return null;

        if (검색정보들.Tables[자료표명] == null)
            return null;

        DataSet dsNewDataSet = 검색정보들.Clone();
        try
        {
            if (질문식 == null || 질문식 == "")
            {
                return 검색정보들;
            }
            else
            {
                DataRow[] rows = 검색정보들.Tables[자료표명].Select(질문식);
                foreach (DataRow row in rows)
                    dsNewDataSet.Tables[자료표명].Rows.Add(row.ItemArray);
            }
        }
        catch
        { 
            
        }
        return dsNewDataSet;
    }

    /// <summary>
    /// 검색된 정보로부터 질문식을 실행한 자료를 얻는다
    /// </summary>
    /// <param name="질문식">검색질의식, ""이면 전체 검색자료를 리턴한다.</param>
    /// <param name="자료표명">검색된정보의 테블명 null 혹은 ""이면 0번 테블을 참조한다.</param>
    /// <returns>질문식을 실행한 결과 혹은 null</returns>
    public virtual DataSet 검색정보얻기(string 질문식)
    {
        if (검색정보들 == null)
            return null;

        if (검색정보들.Tables.Count == 0)
            return null;

        if (검색정보들.Tables[0] == null)
            return null;

        DataSet dsNewDataSet = 검색정보들.Clone();
        try
        {
            if (질문식 == null || 질문식 == "")
            {
                return 검색정보들;
            }
            else
            {
                DataRow[] rows = 검색정보들.Tables[0].Select(질문식);
                foreach (DataRow row in rows)
                    dsNewDataSet.Tables[0].Rows.Add(row.ItemArray);
            }
        }
        catch
        {

        }
        return dsNewDataSet;
    }

    /// <summary>
    /// 관리자 계정등급 2017-02-25
    /// </summary>
    /// <returns>Class</returns>
    public int GetClass()
    {
        if (인증회원 == null)
            return -1;

        return Convert.ToInt16(인증회원.Tables[0].Rows[0]["Class"]);
    }

   
    /// <summary>
    /// 엑셀출력하기 위한 2017-02-28 이거 없으면 오류생김
    /// </summary>
    /// <param name="control"></param>
    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time. 
    }
    public void ExportExcel(GridView gv, string fileName)
    {
        if (gv.Rows.Count == 0)
            return;

       
        //gv.DataBind();

        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "UTF-8";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName +"_"+ DateTime.Now.ToString("yyyy-MM-dd") + ".xls"); //这里的FileName.xls可以用变量动态替换
        // 如果设置为 GetEncoding("GB2312");导出的文件将会出现乱码！！！
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
       
        //gv.EnableViewState = false;
        //gv.AllowSorting = false;
        gv.AllowPaging = false;
        //gv.AllowSorting = false;
       
        //gv.DataBind();
        gv.RenderControl(oHtmlTextWriter);
        Response.Output.Write(oStringWriter.ToString());
        Response.Flush();
        Response.End();
    }

    public void ShowMessageBox(string strMsg, string strUrl)
    {
        Response.Write("<script>alert('" + strMsg + "');location.href='" + strUrl + "';</script>");
    }

    public void ShowMessageBox(string strMsg)
    {
        Response.Write("<script>alert('" + strMsg + "');</script>");
    }

    public void Logout()
    {
        Response.Write("<script>top.location.href='../../Confirm/로그인.aspx';</script>");
    }
    public void HeaderLogout()
    {
        Response.Write("<script>top.location.href='../Confirm/로그인.aspx';</script>");
    }


    


}
