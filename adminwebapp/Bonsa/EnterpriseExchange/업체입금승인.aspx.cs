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
using System.IO;

public partial class 업체입출금관리_업체입금승인 : PageBase
{
    public int 업체입금승인현시방식
    {
        get
        {
            try
            {
                return (int)Session["CouponeListShowMode"];
            }
            catch
            {
                return CF.iNothing;
            }
        }
        set
        {
            if (value == CF.iNothing)
                Session.Remove("CouponeListShowMode");
            else
                Session["CouponeListShowMode"] = value;
        }
    }

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

        KEY_CACHEINFOS = "CACHE:KCCSITE:업체입출금관리:업체입금승인:검색정보들:";
        KEY_CACHEQUERY = "CACHE:KCCSITE:업체입출금관리:업체입금승인:검색질문식:";
        KEY_CACHESELINF = "CACHE:KCCSITE:업체입출금관리:업체입금승인:선택된정보:";
        KEY_CACHESORT = "CACHE:KCCSITE:업체입출금관리:업체입금승인:정돈항:";
        KEY_CACHESORTDIRECTION = "CACHE:KCCSITE:업체입출금관리:업체입금승인:정돈방향:";
        KEY_CACHEFILTER = "CACHE:KCCSITE:업체입출금관리:업체입금승인:검색인자들:";
        KEY_CACHECURPAGE = "CACHE:KCCSITE:업체입출금관리:업체입금승인:현재페지위치:";
        string strID = Request.QueryString["id"];
        try { int.Parse(strID); }
        catch
        {
            return;
        }
        string strMoney = Request.QueryString["money"];
        try { int.Parse(strMoney); }
        catch
        {
            return;
        }
        lblReqMoney.Text = strMoney;
        BindInfo();
    }

    void BindInfo()
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT COUNT(*) FROM TBL_COUPON WHERE enterpriseid is null AND userid is null and money=10000";
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsInfo = dbManager.RunMSelectQuery(cmd);
        lbl10KCoupon.Text = DataSetUtil.RowStringValue(dsInfo, 0, 0);

        strQuery = "SELECT COUNT(*) FROM TBL_COUPON WHERE enterpriseid is null AND userid is null and money=20000";
        cmd = new SqlCommand(strQuery);
        dsInfo = dbManager.RunMSelectQuery(cmd);
        lbl20KCoupon.Text = DataSetUtil.RowStringValue(dsInfo, 0, 0);

        strQuery = "SELECT COUNT(*) FROM TBL_COUPON WHERE enterpriseid is null AND userid is null and money=50000";
        cmd = new SqlCommand(strQuery);
        dsInfo = dbManager.RunMSelectQuery(cmd);
        lbl50KCoupon.Text = DataSetUtil.RowStringValue(dsInfo, 0, 0);

        strQuery = "SELECT COUNT(*) FROM TBL_COUPON WHERE enterpriseid is null AND userid is null and money=100000";
        cmd = new SqlCommand(strQuery);
        dsInfo = dbManager.RunMSelectQuery(cmd);
        lbl100KCoupon.Text = DataSetUtil.RowStringValue(dsInfo, 0, 0);

        strQuery = "SELECT COUNT(*) FROM TBL_COUPON WHERE enterpriseid is null AND userid is null and money=500000";
        cmd = new SqlCommand(strQuery);
        dsInfo = dbManager.RunMSelectQuery(cmd);
        lbl500KCoupon.Text = DataSetUtil.RowStringValue(dsInfo, 0, 0);

        strQuery = "SELECT COUNT(*) FROM TBL_COUPON WHERE enterpriseid is null AND userid is null and money=1000000";
        cmd = new SqlCommand(strQuery);
        dsInfo = dbManager.RunMSelectQuery(cmd);
        lbl1MCoupon.Text = DataSetUtil.RowStringValue(dsInfo, 0, 0);
    }
    
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        BindInfo();
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try {
            string strID = Request.QueryString["id"];   // 입금요청테블의 아이디
            int totalMoney = 0;
            int cnt10KCoupon = int.Parse(tbx10KCoupon.Text.Trim());
            int cnt20KCoupon = int.Parse(tbx20KCoupon.Text.Trim());
            int cnt50KCoupon = int.Parse(tbx50KCoupon.Text.Trim());
            int cnt100KCoupon = int.Parse(tbx100KCoupon.Text.Trim());
            int cnt500KCoupon = int.Parse(tbx500KCoupon.Text.Trim());
            int cnt1MCoupon = int.Parse(tbx1MCoupon.Text.Trim());
            totalMoney = cnt10KCoupon * 10000 + cnt20KCoupon * 20000 + cnt50KCoupon * 50000 + cnt100KCoupon * 100000 + cnt500KCoupon * 500000 + cnt1MCoupon * 1000000;

            string strMoney = Request.QueryString["money"];
            int reqMoney = int.Parse(strMoney);
            if (totalMoney != reqMoney)
            {
                cvRegCouponResult.ErrorMessage = string.Format("선택된 머니가 요청머니양과 차이납니다. 요청머니:{0}, 선택된머니:{1}", reqMoney, totalMoney);
            }
            else if(totalMoney > 0)
            {
                if (cnt10KCoupon > int.Parse(lbl10KCoupon.Text) || cnt20KCoupon > int.Parse(lbl20KCoupon.Text) || cnt50KCoupon > int.Parse(lbl50KCoupon.Text) || cnt100KCoupon > int.Parse(lbl100KCoupon.Text) || cnt500KCoupon > int.Parse(lbl500KCoupon.Text) || cnt1MCoupon > int.Parse(lbl1MCoupon.Text))
                {
                    cvRegCouponResult.ErrorMessage = string.Format("요청한 쿠폰수가 보유쿠폰수를 넘어납니다.");
                }
                else {
                    DBManager dbManager = new DBManager();
                    // 입금요청정보를 얻어온다.
                    string strQuery = "SELECT * FROM TBL_ECHARGE WHERE id=@id AND state=0";
                    SqlCommand cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@id", strID));
                    DataSet dsPartner = dbManager.RunMSelectQuery(cmd);
                    if (DataSetUtil.IsNullOrEmpty(dsPartner))
                    {
                        cvRegCouponResult.ErrorMessage = "요청한 충전이 존재하지 않습니다.";
                    }
                    else
                    {
                        int enterpriseid = DataSetUtil.RowIntValue(dsPartner, "enterpriseid", 0);
                        int money = DataSetUtil.RowIntValue(dsPartner, "money", 0);

                        if (reqMoney != money)
                        {
                            cvRegCouponResult.ErrorMessage = "비정상적인 요청입니다.";
                        }
                        else
                        {
                            // 업체정보를 얻어온다.
                            strQuery = "SELECT * FROM TBL_ENTERPRISE WHERE id=@enterpriseid";
                            cmd = new SqlCommand(strQuery);
                            cmd.Parameters.Add(new SqlParameter("@enterpriseid", enterpriseid));
                            dsPartner = dbManager.RunMSelectQuery(cmd);
                            if (DataSetUtil.IsNullOrEmpty(dsPartner))
                            {
                                cvRegCouponResult.ErrorMessage = "요청한 업체가 존재하지 않습니다.";
                            }
                            else
                            {
                                string enterprisename = DataSetUtil.RowStringValue(dsPartner, "name", 0);

                                // 쿠폰을 해당업체것으로 등록한다.
                                strQuery = "UPDATE TBL_COUPON SET enterpriseid=@enterpriseid, enterprisename=@enterprisename WHERE id in (SELECT TOP(" + cnt10KCoupon + ") id FROM TBL_COUPON WHERE enterpriseid is null AND userid is null AND money=10000)";
                                cmd = new SqlCommand(strQuery);
                                cmd.Parameters.Add(new SqlParameter("@enterpriseid", enterpriseid));
                                cmd.Parameters.Add(new SqlParameter("@enterprisename", enterprisename));
                                dbManager.RunMQuery(cmd);
                                strQuery = "UPDATE TBL_COUPON SET enterpriseid=@enterpriseid, enterprisename=@enterprisename WHERE id in (SELECT TOP(" + cnt20KCoupon + ") id FROM TBL_COUPON WHERE enterpriseid is null AND userid is null AND money=20000)";
                                cmd = new SqlCommand(strQuery);
                                cmd.Parameters.Add(new SqlParameter("@enterpriseid", enterpriseid));
                                cmd.Parameters.Add(new SqlParameter("@enterprisename", enterprisename));
                                dbManager.RunMQuery(cmd);
                                strQuery = "UPDATE TBL_COUPON SET enterpriseid=@enterpriseid, enterprisename=@enterprisename WHERE id in (SELECT TOP(" + cnt50KCoupon + ") id FROM TBL_COUPON WHERE enterpriseid is null AND userid is null AND money=50000)";
                                cmd = new SqlCommand(strQuery);
                                cmd.Parameters.Add(new SqlParameter("@enterpriseid", enterpriseid));
                                cmd.Parameters.Add(new SqlParameter("@enterprisename", enterprisename));
                                dbManager.RunMQuery(cmd);
                                strQuery = "UPDATE TBL_COUPON SET enterpriseid=@enterpriseid, enterprisename=@enterprisename WHERE id in (SELECT TOP(" + cnt100KCoupon + ") id FROM TBL_COUPON WHERE enterpriseid is null AND userid is null AND money=100000)";
                                cmd = new SqlCommand(strQuery);
                                cmd.Parameters.Add(new SqlParameter("@enterpriseid", enterpriseid));
                                cmd.Parameters.Add(new SqlParameter("@enterprisename", enterprisename));
                                dbManager.RunMQuery(cmd);
                                strQuery = "UPDATE TBL_COUPON SET enterpriseid=@enterpriseid, enterprisename=@enterprisename WHERE id in (SELECT TOP(" + cnt500KCoupon + ") id FROM TBL_COUPON WHERE enterpriseid is null AND userid is null AND money=500000)";
                                cmd = new SqlCommand(strQuery);
                                cmd.Parameters.Add(new SqlParameter("@enterpriseid", enterpriseid));
                                cmd.Parameters.Add(new SqlParameter("@enterprisename", enterprisename));
                                dbManager.RunMQuery(cmd);
                                strQuery = "UPDATE TBL_COUPON SET enterpriseid=@enterpriseid, enterprisename=@enterprisename WHERE id in (SELECT TOP(" + cnt1MCoupon + ") id FROM TBL_COUPON WHERE enterpriseid is null AND userid is null AND money=1000000)";
                                cmd = new SqlCommand(strQuery);
                                cmd.Parameters.Add(new SqlParameter("@enterpriseid", enterpriseid));
                                cmd.Parameters.Add(new SqlParameter("@enterprisename", enterprisename));
                                dbManager.RunMQuery(cmd);

                                // 입금요청을 승인상태로 놓는다.
                                strQuery = "UPDATE tbl_echarge set state=1 WHERE id=@id AND state=0";
                                cmd = new SqlCommand(strQuery);
                                cmd.Parameters.Add(new SqlParameter("@id", strID));
                                dbManager.RunMQuery(cmd);

                                ShowMessageBox("승인이 정상적으로 되었습니다.", "업체입금요청.aspx");
                            }
                        }
                    }
                }
            }
            else {
                cvRegCouponResult.ErrorMessage = "승인할 쿠폰을 선택해주세요.";
            }
        }
        catch (Exception ex)
        {
            cvRegCouponResult.ErrorMessage = "입금승인중 오류가 발생하였습니다." + ex.ToString();
        }

        cvRegCouponResult.IsValid = false;
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("업체입금요청.aspx");
    }
}
