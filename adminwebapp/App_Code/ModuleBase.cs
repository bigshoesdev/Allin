//////////////////////////////////////////////////////////////////
// Copyright (C) 2003-2004 GPSH WR, Inc.
// All rights reserved.
//
//////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

public enum 운영자생성모드
{ 
    본사생성,
    부본사생성,
    총판생성,
    매장생성
}

/// <summary>
/// Summary description for PageBase.
/// </summary>
public class ModuleBase : System.Web.UI.MasterPage
{
    public static string KEY_CACHEUSER = PageBase.KEY_CACHEUSER;
    public static string KEY_CACHEALARM = PageBase.KEY_CACHEALARM;

    public ModuleBase()
    {
        try
        {
            //if (GoldenConfiguration.EnableSsl)
            //{
            //    string szPath = Context.Request.Url.ToString().ToLower();
            //    // URL에 "/Secure/", "/UserMngSys/"가 있으면 https통신을 하게 하여주고 
            //    // 그렇지 않으면  http로 주소변환한다.
            //    if ((szPath.IndexOf("/secure/") > -1))
            //    {
            //        if (!Context.Request.IsSecureConnection)
            //        {
            //            //Context.Response.Redirect(szPath.Replace("http:", "https:"), true);
            //            Context.Response.Redirect(szPath.Insert(4, "s"));
            //        }
            //    }
            //    else
            //    {
            //        if (Context.Request.IsSecureConnection)
            //            Context.Response.Redirect(szPath.Replace("https:", "http:"), true);
            //    }
            //}
        }
        catch
        {
            // for design time
        }
    }

    /// <summary>
    ///		확인된 회원에 대한 자료를 처리(보관/얻기)한다.
    ///		<remark>쎄션에 보관된다.</remark>
    /// </summary>
    public DataSet 인증회원
    {
        get
        {
            DataSet ds = null;
            try
            {
                ds = (DataSet)Session[KEY_CACHEUSER];
            }
            catch
            {
            }

            return ds;
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
        //if (인증회원 == null) return false;
        //string UserRole = (string)(인증회원.Tables[0].Rows[0][회원자료.권한]);
        //return ((UserRole != "") && (szRoles.IndexOf("#" + UserRole + "#") >= 0));
        return true;
    }

    public void ShowMessageBox(string strMsg, string strUrl)
    {
        Response.Write("<script>alert('" + strMsg + "');location.href='" + strUrl + "';</script>");
    }

    public void ShowMessageBox(string strMsg)
    {
        Response.Write("<script>alert('" + strMsg + "');</script>");
    }
}
