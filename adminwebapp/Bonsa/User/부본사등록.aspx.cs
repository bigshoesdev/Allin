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

public partial class 회원관리_부본사상세정보 : PageBase
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
        if (!IsPostBack)
        {
            string strID = Request.QueryString["id"];
            try { int.Parse(strID); }
            catch { strID = "0"; }
            hdnID.Value = strID;
            tbxName.ReadOnly = false;
            fntChange.Visible = false;
            rvClassPercent.MaximumValue = 인증회원.Tables[0].Rows[0]["ClassPercent"].ToString();
            rvClassPercent.ErrorMessage = "부본사지분율은 0부터 " + rvClassPercent.MaximumValue + "사이의 값이어야 합니다.";
            lblClassPercent.Text = 인증회원.Tables[0].Rows[0]["ClassPercent"].ToString() + "%이하";
            
            if (strID == "0")   // 새로 추가하는경우
            {
                tblChangeMoney.Visible = false;
            }

            if (this.GetClass() == 4)
            {
                tblChangeMoney.Style["display"] = "none";
            }

            BindInfo();
        }
    }
    void BindInfo()
    {
        DBManager dbManager = new DBManager();
        string strQuery;
        string strID = hdnID.Value;
        strQuery = "SELECT * FROM TBL_Enterprise WHERE ID=" + strID;
        SqlCommand cmd = new SqlCommand(strQuery);
        DataSet dsPartner = dbManager.RunMSelectQuery(cmd);
        if (DataSetUtil.IsNullOrEmpty(dsPartner))
            return;

        tbxName.ReadOnly = true;
        fntChange.Visible = true;
        hdnLoginID.Value = tbxLoginID.Text = dsPartner.Tables[0].Rows[0]["LoginID"].ToString();
        tbxPWD.Text = dsPartner.Tables[0].Rows[0]["LoginPWD"].ToString();
        tbxName.Text = dsPartner.Tables[0].Rows[0]["name"].ToString();
        tbxPartner.Text = dsPartner.Tables[0].Rows[0]["partner"].ToString();
        //tbxMoney.Text = dsPartner.Tables[0].Rows[0]["money"].ToString();
        tbxMoney.Text = Convert.ToDouble(dsPartner.Tables[0].Rows[0]["money"]).ToString();
        tbxClassPercent.Text = dsPartner.Tables[0].Rows[0]["ClassPercent"].ToString();
        tbxBankName.Text = dsPartner.Tables[0].Rows[0]["bankname"].ToString();
        tbxBankNum.Text = dsPartner.Tables[0].Rows[0]["banknum"].ToString();
        tbxMaster.Text = dsPartner.Tables[0].Rows[0]["mastername"].ToString();

        if (dsPartner.Tables[0].Rows[0]["use_yn"].ToString().Equals("1"))
            rdoUseYn.SelectedIndex = 0;
        else
            rdoUseYn.SelectedIndex = 1;

        /*if (dsPartner.Tables[0].Rows[0]["bill_kind"].ToString().Equals("GAME"))
            rdoKind.SelectedIndex = 0;
        else
            rdoKind.SelectedIndex = 1;*/


    }

    protected void btnLisTBL_Click(object sender, EventArgs e)
    {
        Response.Redirect("부본사목록.aspx");
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strOrgLoginID = hdnLoginID.Value;
        string strId = hdnID.Value;
        string strLoginID = tbxLoginID.Text.Trim();
        string strPassword = tbxPWD.Text.Trim();
        string strName = tbxName.Text.Trim();
        string strPartner = tbxPartner.Text.Trim();
        double nMoney = Convert.ToDouble(tbxMoney.Text);
        float nClassPercent = float.Parse(tbxClassPercent.Text);
        string strBillKind = "0";// rdoKind.SelectedValue;
        string use_yn = rdoUseYn.SelectedValue;
        string strBankName = tbxBankName.Text.Trim();
        string strMaster = tbxMaster.Text.Trim();
        string strBankNum = tbxBankNum.Text.Trim();

        if (CheckExistID(strId, strLoginID))
        {
            cvResult.ErrorMessage = "이미 등록된 아이디입니다.";
            cvResult.IsValid = false;
            return;
        }

        if (CheckExistPartner(strId, strPartner))
        {
            cvResult.ErrorMessage = "이미 등록된 파트너코드입니다.";
            cvResult.IsValid = false;
            return;
        }
        DBManager dbManager = new DBManager();
        if (strId == "0")
        {
            int nClass = int.Parse(인증회원.Tables[0].Rows[0]["Class"].ToString());
            string strBonsa = "";
            string strBonsaID = "0";
            switch (nClass.ToString())
            {
                case ROLES_BONSA:
                    strBonsa = 인증회원.Tables[0].Rows[0]["name"].ToString();
                    strBonsaID = 인증회원.Tables[0].Rows[0]["id"].ToString();
                    break;
            }

            string strQuery = "INSERT INTO TBL_Enterprise(LoginID, LoginPWD, Name, Money, ClassPercent, Class, ParentID, Partner, Bonsa, BonsaID, Bill_Kind, Use_Yn, bankname, banknum, mastername) ";
            strQuery += "VALUES(@LoginID, @LoginPWD, @Name, @Money, @ClassPercent, @Class, @ParentID, @Partner, @Bonsa, @BonsaID, @Bill_Kind, @Use_Yn, @bankname, @banknum, @mastername);UPDATE tbl_enterprise SET bubonsa=@bubonsa, bubonsaid=@@IDENTITY WHERE id=@@IDENTITY;";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@LoginID", strLoginID));
            cmd.Parameters.Add(new SqlParameter("@LoginPWD", strPassword));
            cmd.Parameters.Add(new SqlParameter("@Name", strName));
            cmd.Parameters.Add(new SqlParameter("@Money", nMoney));
            cmd.Parameters.Add(new SqlParameter("@ClassPercent", nClassPercent));
            cmd.Parameters.Add(new SqlParameter("@Class", ROLES_BUBONSA));
            cmd.Parameters.Add(new SqlParameter("@ParentID", 인증회원번호));
            cmd.Parameters.Add(new SqlParameter("@Partner", strPartner));
            cmd.Parameters.Add(new SqlParameter("@Bonsa", strBonsa));
            cmd.Parameters.Add(new SqlParameter("@BonsaID", strBonsaID));
            cmd.Parameters.Add(new SqlParameter("@bubonsa", strName));
            cmd.Parameters.Add(new SqlParameter("@Bill_Kind", strBillKind));
            cmd.Parameters.Add(new SqlParameter("@Use_Yn", use_yn));
            cmd.Parameters.Add(new SqlParameter("@bankname", strBankName));
            cmd.Parameters.Add(new SqlParameter("@banknum", strBankNum));
            cmd.Parameters.Add(new SqlParameter("@mastername", strMaster)); 
            
            try
            {
                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "부본사사정보가 등록되였습니다.";
            }
            catch (Exception ex)
            {
                cvResult.ErrorMessage = "부본사사정보등록에서 오류가 발생하였습니다. " + ex.ToString();
            }
        }
        else
        {
            // 하층업체 최대 지분율찾아서 체크한다 2017-04-19 
            DataSet maxClassPercentDS = dbManager.RunMSelectQuery(new SqlCommand("select isnull(max(classpercent), 0) from tbl_enterprise where parentid=" + strId));
            if (maxClassPercentDS.Tables.Count > 0 || maxClassPercentDS.Tables[0].Rows.Count > 0)
            {
                float maxClassPercent = Convert.ToSingle(maxClassPercentDS.Tables[0].Rows[0][0]);
                if (nClassPercent <= maxClassPercent)
                {
                    cvResult.ErrorMessage = "지분율이 " + maxClassPercent + "% 인 하층업체가 있습니다.";
                    cvResult.IsValid = false;
                    return;
                }
            }

            string strQuery = "UPDATE TBL_Enterprise SET LoginID=@LoginID, LoginPWD=@LoginPWD, Name=@Name, Money=@Money, Partner=@Partner, ClassPercent=@ClassPercent, Bill_Kind=@Bill_Kind, Use_Yn=@Use_Yn, bankname=@bankname, banknum=@banknum, mastername=@mastername, Change_Time=GetDate() ";
            strQuery += "WHERE id=@id";
            SqlCommand cmd = new SqlCommand(strQuery);
            cmd.Parameters.Add(new SqlParameter("@LoginID", strLoginID));
            cmd.Parameters.Add(new SqlParameter("@LoginPWD", strPassword));
            cmd.Parameters.Add(new SqlParameter("@Name", strName));
            cmd.Parameters.Add(new SqlParameter("@Money", nMoney));
            cmd.Parameters.Add(new SqlParameter("@Partner", strPartner));
            cmd.Parameters.Add(new SqlParameter("@ClassPercent", nClassPercent));
            cmd.Parameters.Add(new SqlParameter("@id", strId));
            cmd.Parameters.Add(new SqlParameter("@Bill_Kind", strBillKind));
            cmd.Parameters.Add(new SqlParameter("@Use_Yn", use_yn));
            cmd.Parameters.Add(new SqlParameter("@bankname", strBankName));
            cmd.Parameters.Add(new SqlParameter("@banknum", strBankNum));
            cmd.Parameters.Add(new SqlParameter("@mastername", strMaster)); 
            
            try
            {
                dbManager.RunMQuery(cmd);
                cvResult.ErrorMessage = "부본사사정보가 수정되였습니다.";
            }
            catch (Exception ex)
            {
                cvResult.ErrorMessage = "부본사사정보수정에서 오류가 발생하였습니다. " + ex.ToString();
            }
        }
        cvResult.IsValid = false;
    }

    bool CheckExistID(string id, string strLoginID)
    {
        DBManager dbManager = new DBManager();
        //string strQuery = "SELECT * FROM TBL_Enterprise WHERE LoginID=@LoginID AND id <>" + id;

        // 2017-02-26 등록시 사원아이디도 통합체크한다.
        string strQuery = "SELECT * FROM V_ENT_EMP_USERS WHERE LoginID=@LoginID AND id <>" + id;
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        sqlQuery.Parameters.Add("@LoginID", SqlDbType.NVarChar);
        sqlQuery.Parameters["@LoginID"].Value = strLoginID;
        DataSet dsUser = dbManager.RunMSelectQuery(sqlQuery);

        bool bExist = false;
        if (dsUser.Tables.Count == 0)
        {
            bExist = false;
        }
        else if (dsUser.Tables[0].Rows.Count == 0)
        {
            bExist = false;
        }
        else
        {
            bExist = true;
        }
        return bExist;
    }

    bool CheckExistPartner(string id, string strPartner)
    {
        DBManager dbManager = new DBManager();
        string strQuery = "SELECT * FROM TBL_Enterprise WHERE Partner=@Partner AND id <> " + id.ToString();
        SqlCommand sqlQuery = new SqlCommand(strQuery);
        sqlQuery.Parameters.Add("@Partner", SqlDbType.NVarChar);
        sqlQuery.Parameters["@Partner"].Value = strPartner;
        DataSet dsUser = dbManager.RunMSelectQuery(sqlQuery);

        bool bExist = false;
        if (dsUser.Tables.Count == 0)
        {
            bExist = false;
        }
        else if (dsUser.Tables[0].Rows.Count == 0)
        {
            bExist = false;
        }
        else
        {
            bExist = true;
        }
        return bExist;
    }
    protected void btnUpdateGameMoney_Click(object sender, EventArgs e)
    {
        cvUpdateMoney.IsValid = true;
        long nUpdateMoney = 0;
        try
        {
            nUpdateMoney = long.Parse(tbxUpdateGameMoney.Text);
        }
        catch
        {
            cvUpdateMoney.ErrorMessage = "수값을 입력하세요.";
            goto lblReturn;
        }
        if (nUpdateMoney < 0)
        {
            cvUpdateMoney.ErrorMessage = "비정상적인 요청을 하였습니다.";
            goto lblReturn;
        }

        try
        {
            DBManager dbManager = new DBManager();
            SqlCommand cmd;
            string strQuery = "";
            string strId = hdnID.Value;
            string strName = tbxName.Text.Trim();
            if (optGameMoneyUpdate.SelectedValue == "+")
            {
                //현재 업체의 보유머니
                strQuery = "Select money FROM TBL_Enterprise WHERE id=@id And money>=@reqmoney";
                cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add(new SqlParameter("@id", 인증회원번호));
                cmd.Parameters.Add(new SqlParameter("@reqmoney", nUpdateMoney));
                DataSet dsResult = dbManager.RunMSelectQuery(cmd);
                if (DataSetUtil.IsNullOrEmpty(dsResult))
                {
                    cvUpdateMoney.ErrorMessage = "강제충전머니가 업체보유머니를 넘어납니다.";
                    goto lblReturn;
                }
                else
                {
                    // 현재 업체의 보유머니 감소
                    strQuery = "UPDATE TBL_Enterprise set money=money-@reqmoney where id=@id";
                    cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@id", 인증회원번호));
                    cmd.Parameters.Add(new SqlParameter("@reqmoney", nUpdateMoney));
                    dbManager.RunMQuery(cmd);

                    // 요청 업체의 보유머니 증가
                    strQuery = "UPDATE TBL_Enterprise set money=money+@reqmoney where id=@id";
                    cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@id", strId));
                    cmd.Parameters.Add(new SqlParameter("@reqmoney", nUpdateMoney));
                    dbManager.RunMQuery(cmd);

                    // 업체충전테이블에 추가
                    strQuery = "INSERT INTO TBL_ECharge(EnterpriseID, Money, Memo, State) ";
                    strQuery += "VALUES(@EnterpriseID, @Money, @Memo, 2)";
                    cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@EnterpriseID", strId));
                    cmd.Parameters.Add(new SqlParameter("@Money", nUpdateMoney));
                    cmd.Parameters.Add(new SqlParameter("@Memo", DataSetUtil.RowStringValue(인증회원, "name", 0) + "님이 강제충전"));
                    dbManager.RunMQuery(cmd);
                }
            }
            else
            {
                // 요청업체의 보유머니
                strQuery = "Select money FROM TBL_Enterprise WHERE id=@id And money>@reqmoney";
                cmd = new SqlCommand(strQuery);
                cmd.Parameters.Add(new SqlParameter("@id", strId));
                cmd.Parameters.Add(new SqlParameter("@reqmoney", nUpdateMoney));
                DataSet dsResult = dbManager.RunMSelectQuery(cmd);
                if (DataSetUtil.IsNullOrEmpty(dsResult))
                {
                    cvUpdateMoney.ErrorMessage = "강제환전머니가 해당 업체보유머니를 넘어납니다.";
                    goto lblReturn;
                }
                else
                {
                    // 현재 업체의 보유머니 증가
                    strQuery = "UPDATE TBL_Enterprise set money=money+@reqmoney where id=@id";
                    cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@id", 인증회원번호));
                    cmd.Parameters.Add(new SqlParameter("@reqmoney", nUpdateMoney));
                    dbManager.RunMQuery(cmd);

                    // 요청 업체의 보유머니 감소
                    strQuery = "UPDATE TBL_Enterprise set money=money-@reqmoney where id=@id";
                    cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@id", strId));
                    cmd.Parameters.Add(new SqlParameter("@reqmoney", nUpdateMoney));
                    dbManager.RunMQuery(cmd);

                    strQuery = "INSERT INTO TBL_EWithdraw(EnterpriseID, Money, Memo, State) ";
                    strQuery += "VALUES(@EnterpriseID, @Money, @Memo, 2)";
                    cmd = new SqlCommand(strQuery);
                    cmd.Parameters.Add(new SqlParameter("@EnterpriseID", strId));
                    cmd.Parameters.Add(new SqlParameter("@Money", nUpdateMoney));
                    cmd.Parameters.Add(new SqlParameter("@Memo", DataSetUtil.RowStringValue(인증회원, "name", 0) + "님이 강제환전"));
                    dbManager.RunMQuery(cmd);
                }
            }
            BindInfo();
            return;
        }
        catch (Exception ex)
        {
            cvUpdateMoney.ErrorMessage = "회원 강제충전 혹은 환전처리에서 오류가 발생하였습니다." + ex.ToString();
        }
    lblReturn:
        cvUpdateMoney.IsValid = false;

    }
}
