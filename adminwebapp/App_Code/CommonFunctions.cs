//----------------------------------------------------------------
// Copyright (C) 2004-2009 Kanshell.
// All rights reserved.
//
//----------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Data;

//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;

//using System.ComponentModel;
//using System.Data.SqlClient;
//using System.Reflection;
using System.Security.Cryptography;

/// <summary>
/// 이 클라스는 체계에서 공통적으로 쓰이는 기초함수들을 제공한다.
/// </summary>
public class CF
{
	/// <summary>
	/// 론리값에 대한 기정값(default value)을 가르킨다. 
	/// 읽이전용이며 기정값은 "false" 이다.
	/// </summary>
	public static bool bNothing 
	{
		get
		{
			return false;
		}
	}

    /// <summary>
    /// Byte형값에 대한 기정값(default value)을 가르킨다.
    /// 읽이전용이며 기정값은 "0" 이다.
    /// </summary>
    public static byte byNothing
    {
        get
        {
            return 0;
        }
    }

	/// <summary>
	/// 옹근수(int)값에 대한 기정값(default value)을 가르킨다.
	/// 읽이전용이며 기정값은 "0" 이다.
	/// </summary>
	public static int iNothing
	{
		get
		{
			return 0;
		}
	}

	/// <summary>
	/// 옹근수(long)값에 대한 기정값(default value)을 가르킨다.
	/// 읽이전용이며 기정값은 "0" 이다.
	/// </summary>
	public static long lNothing
	{
		get
		{
			return 0;
		}
	}

	/// <summary>
	/// 류동수(double)값에 대한 기정값(default value)을 가르킨다.
	/// 읽이전용이며 기정값은 "0.0" 이다.
	/// </summary>
	public static double dNothing
	{
		get
		{
			return 0.0;
		}
	}

	/// <summary>
	/// 문자렬(string)값에 대한 기정값(default value)을 가르킨다.
	/// 읽이전용이며 기정값은 "" 이다.
	/// </summary>
	public static string sNothing		
	{
		get
		{
			return "";
		}
	}
	
	/// <summary>
	/// 오브젝트(objcjet)값에 대한 기정값(default value)을 가르킨다.
	/// 읽이전용이며 기정값은 "null" 이다.
	/// </summary>
	public static object oNothing
	{
		get
		{
			return null;
		}
	}

	/// <summary>
	/// 날자(DateTime)값에 대한 기정값(default value)을 가르킨다.
	/// 읽이전용이며 기정값은 "0001/01/01 1:1:1" 이다.
	/// </summary>
	public static DateTime dtNothing
	{
		get
		{
			return DateTime.MinValue;
		}
	}

	/// <summary>
	/// 서력날자를 주체년표기로 전환한다.
	/// </summary>
	/// <param name="dtEroup">날자형: 서력날자를 가르킨다.</param>
	/// <returns>주체년표기문자렬: 양식: "주체91(2004)년 1월 12일"</returns>
    public static string ConvJuCheaDate(DateTime dtEroup)
    {
        if (dtEroup == dtNothing) return "";
        if (dtEroup.Year < 1911)
            return dtEroup.ToString("yyyy.MM.dd");

        DateTime dtJuChea = dtEroup.AddYears(-1911);
        return dtJuChea.ToString("주체yy") + dtEroup.ToString("(yyyy)년 M월 d일");
    }

	/// <summary>
	/// 해당 문자렬을 날자시간으로 바꾼다.
	/// </summary>
	/// <param name="szDateTime">날자시간문자렬</param>
	/// <returns>얻어진 날자시간</returns>
	public static DateTime DateTime_ParseEx(string szDateTime)
	{
		try
		{
			if ((szDateTime = szDateTime.Trim()) == "") return dtNothing;
			szDateTime = szDateTime.Replace(".", "/").Replace(" ", "").Replace("-", "/");
			szDateTime = szDateTime.Replace("년", "/").Replace("월", "/").Replace("일", "");
			szDateTime = szDateTime.Replace("시", ":").Replace("분", ":").Replace("초", "");
			szDateTime = szDateTime.Replace("오전", "AM").Replace("오후", "PM");
			return DateTime.Parse(szDateTime);
		}
		catch
		{
			return dtNothing;
		}
	}

	/// <summary>
	/// 날자시간문자렬이 유효한 문자렬인가를 판정한다.
	/// </summary>
	/// <param name="szDateTime">날자시간문자렬</param>
	/// <returns>표준양식에 맞으면 true, 틀리면 false</returns>
	public static bool IsValidDateTime(string szDateTime)
	{
		return (DateTime_ParseEx(szDateTime) != dtNothing);
	}

	/// <summary>
	///		지정된 자료을 지정된 자료렬에 대응시킨다.
	///		int, double, long, string, datetime형의 값들에 대한 null값 검사를 진행한다.
	/// </summary>
	/// <param name="oRow">지정된 자료렬</param>
	/// <param name="szFieldName">지정된 자료의 마당이름</param>
	/// <param name="oValue">지정된 자료</param>
	public static void SetDataIntoRow(DataRow oRow, string szFieldName, object oValue)
	{
		//
		// 주의: 이 함수는 Web층의 PageBase.vb & ModuleBase.vb 
		//       BusinessFacade  의 일반체계.vb 에 반영되여 있다.
		//       따라서 변경된 내용을 우의 모쥴들에 반영할것! 
		//
		if (oValue == CF.oNothing)
		{
			oRow[szFieldName] = DBNull.Value;
			return; 
		}
		System.Type TypeOfValue = oValue.GetType();
		if (TypeOfValue.Equals(typeof(int)))
			if ((int)oValue == CF.iNothing) oValue = null;
		if (TypeOfValue.Equals(typeof(long)))
			if ((long)oValue == CF.lNothing) oValue = null;
		if (TypeOfValue.Equals(typeof(double)))
			if ((double)oValue == CF.dNothing) oValue = null;
		if (TypeOfValue.Equals(typeof(string)))
			if ((string)oValue == CF.sNothing) oValue = null;
		if (TypeOfValue.Equals(typeof(DateTime)))
			if ((DateTime)oValue == CF.dtNothing) oValue = null;
		if (oValue == null)
			oRow[szFieldName] = DBNull.Value;
		else
			oRow[szFieldName] = oValue;
	}

	/// <summary>
	/// 자료렬에서 해당 값(문자렬)을 얻는다.
	/// </summary>
	/// <param name="objData"></param>
	/// <returns></returns>
    public static string GetStrDataFromRow(object objData)
	{
		try 
		{
			return ((objData == DBNull.Value) ? sNothing : (string)objData); }
		catch 
		{
			return sNothing; }
	}
	/// <summary>
	/// 자료렬에서 해당 값(수값)을 얻는다.
	/// </summary>
	/// <param name="objData"></param>
	/// <returns></returns>
    public static int GetIntDataFromRow(object objData)
	{
		try 
		{
			return ((objData == DBNull.Value) ? iNothing : (int)objData); }
		catch 
		{
			return iNothing; }
    }

	/// <summary>
	/// 자료렬에서 해당 값(수값)을 얻는다.
	/// </summary>
	/// <param name="objData"></param>
	/// <returns></returns>
    public static double GetDblDataFromRow(object objData)
	{
		try 
		{
			return ((objData == DBNull.Value) ? dNothing : (double)objData); }
		catch 
		{
			return dNothing; }
	}

	/// <summary>
	/// 자료렬에서 해당 값(날자)을 얻는다.
	/// </summary>
	/// <param name="objData"></param>
	/// <returns></returns>
    public static DateTime GetDateDataFromRow(object objData)
	{
		try 
		{
			return ((objData == DBNull.Value) ? dtNothing : (DateTime)objData); }
		catch 
		{
			return dtNothing; }
	}

	/// <summary>
	/// 자료렬에서 해당 값(날자)을 얻는다.
	/// </summary>
	/// <param name="objData"></param>
	/// <returns></returns>
	public static string GetDateDataFromRow(object objData, string szFormat)
	{
		DateTime dt = GetDateDataFromRow(objData);
		if ((dt == dtNothing) || (szFormat == null))
			return "";
		return (szFormat == "") ? dt.ToString() : dt.ToString(szFormat);
	}

	/// <summary>
	/// 자료렬에서 해당 값(론리값(거짓?참))을 얻는다.
	/// </summary>
	/// <param name="objData"></param>
	/// <returns></returns>
    public static bool GetBoolDataFromRow(object objData)
    {
		try 
		{
			return ((objData == DBNull.Value) ? bNothing : (bool)objData); }
		catch 
		{
			return bNothing; }
	}

	/// <summary>
	/// 자료렬에서 해당 값(수값)을 얻는다.
	/// </summary>
	/// <param name="objData"></param>
	/// <returns></returns>
    public static long GetlongDataFromRow(object objData)
	{
		try 
		{
			return ((objData == DBNull.Value) ? lNothing : (long)objData); }
		catch 
		{
			return lNothing; }
	}

	/// <summary>
	/// 바이트크기의 수값을 단위포마트로 변환한다.
	/// </summary>
	/// <param name="nValue"></param>
	/// <param name="szUnit"></param>
	/// <param name="szFormat"></param>
	/// <returns></returns>
    public static string ConvByteUnit(ref int nValue, string szUnit, string szFormat)
	{
        int nUnit = 1;
        double dValue = nValue;
        if (szUnit == null)
		{
            if (nValue > 1024 * 1024 * 1024)
			{
                nUnit = (1024 * 1024 * 1024);
                szUnit = "Ｇ";
			}
            else if (nValue > 1024 * 1024)
			{
                nUnit = (1024 * 1024);
                szUnit = "Ｍ";
			}
            else if (nValue > 1024)
			{
                nUnit = 1024;
                szUnit = "Ｋ";
			}
            else
			{
                nUnit = 1;
                szUnit = "";
			}
        }
        else
		{
            switch (szUnit.ToLower())
			{	
                case "g": nUnit = 1024 * 1024 * 1024; break;
                case "m": nUnit = 1024 * 1024; break;
                case "k": nUnit = 1024; break;
                case "b": nUnit = 1; break;
                default: nUnit = 1; break;
            }
        }

        dValue /= nUnit;
        nValue = (int)(dValue);
		return (((szFormat == null) ? dValue.ToString("#,##0") : dValue.ToString(szFormat)) + szUnit);
	}

	/// <summary>
	/// 수값을 조문단위포마트로 변환한다.
	/// </summary>
	/// <param name="nValue"></param>
	/// <param name="szUnit"></param>
	/// <param name="szFormat"></param>
	/// <returns></returns>
    public static string ConvNumberUnit(ref int nValue, string szUnit, string szFormat)
	{
        int nUnit = 1;
        double dValue = nValue;
        if (szUnit == null)
		{
            if (nValue > 1000000)
			{
                nUnit = 1000000;
                szUnit = "백만";
			}
            else if (nValue > 10000)
			{
                nUnit = 10000;
                szUnit = "만";
			}
            else
			{
                nUnit = 1;
                szUnit = "";
            }
		}
        else
		{
            switch (szUnit)
			{
                case "백만": nUnit = 1000000; break;
                case "십만": nUnit = 100000; break;
                case "만":	 nUnit = 10000; break;
                case "천":   nUnit = 1000; break;
                case "백":   nUnit = 100; break;
                case "십":   nUnit = 10; break;
                case "단":   nUnit = 1; break;
                default:     nUnit = 1; break;
            }
        }

        dValue /= nUnit;
        nValue = (int)(dValue);
		return (((szFormat == null) ? dValue.ToString("#,##0") : dValue.ToString(szFormat)) + szUnit);
    }

	/// <summary>
	/// 날자시간단위문자(조문)를 얻는다.
	/// </summary>
	/// <param name="nIndex">날자시간의 단위번호(0:년, 1:월, 2:일, 3:시, 4:분, 5:초, 6:m초)</param>
	/// <returns>날자시간단위의 문자</returns>
    public static string GetDateTimeLabel(int nIndex)
	{
        switch (nIndex)
		{
            case 0 : return "년";
            case 1 : return "월";
            case 2 : return "일";
            case 3 : return "시";
            case 4 : return "분";
            case 5 : return "초";
            case 6 : return "m초";
            default: return "모름";
		}
    }

	/// <summary>
	///   우리말문자렬을 UTF8의 16진수 문자렬(%XX%XX%XX..)로 변환한다.
	//    이것은 말단에서 Get방식으로 우리말을 보낼 때 써버에서 우리말을 정확히 받기 위해서이다.
	/// </summary>
	/// <param name="szURI">변환하려는 문자렬</param>
	/// <returns>변환된 문자렬</returns>
    public static string encodeURI(string szURI)
    {
        if ((szURI == null) || (szURI == "")) return "";
        System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
        Byte[] bytes = utf8.GetBytes(szURI);
        StringBuilder szEncode = new StringBuilder("");
        foreach (Byte b in bytes)
            szEncode.AppendFormat("%{0:X}", b);
		return szEncode.ToString();
    }

    /// <summary>
    ///		어떤 문자렬자료에 대한 암호화자료생성모듈
    /// </summary>
    /// <param name="strItem">암호화할 자료</param>
    /// <returns>암호화된 2진자료</returns>
    public static byte[] 암호화(string strItem)
    {
        return System.Text.ASCIIEncoding.UTF8.GetBytes(strItem);
        //if (WebConfiguration.Default.EncryptPassword)
        //{
        //    SHA256 sha = SHA256.Create();
        //    return sha.ComputeHash(System.Text.ASCIIEncoding.UTF8.GetBytes(strItem));
        //}
        //else
        //{
        //    return System.Text.ASCIIEncoding.UTF8.GetBytes(strItem);
        //}
    }

    /// <summary>
    ///		어떤 암호화된 바이트배렬의 복호화모듈
    /// </summary>
    /// <param name="strItem">암호화할 자료</param>
    /// <returns>암호화된 2진자료</returns>
    public static string 복호화(byte[] bytItem)
    {
        string strTmp = System.Text.ASCIIEncoding.UTF8.GetString(bytItem);
        int nTmp = strTmp.IndexOf('\0');
        if (nTmp > -1)
        {
            strTmp = strTmp.Substring(0, nTmp);
        }
        else
        {
            strTmp = "";
        }
        return strTmp;
        //if (WebConfiguration.Default.EncryptPassword)
        //{
        //    return "";
        //}
        //else
        //{
        //    string strTmp = System.Text.ASCIIEncoding.UTF8.GetString(bytItem);
        //    int nTmp = strTmp.IndexOf('\0');
        //    if (nTmp > -1)
        //    {
        //        strTmp = strTmp.Substring(0, nTmp);
        //    }
        //    else
        //    {
        //        strTmp = "";
        //    }
        //    return strTmp;
        //}
    }

} //class 'CF