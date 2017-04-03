﻿using System;
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

public partial class CPS_Admin_LinkList : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hBeginTime.Value = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-01";
            tbBeginTime.Text = hBeginTime.Value;

            hEndTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
            tbEndTime.Text = hEndTime.Value;

            BindLinkList();

            lbUserName.InnerHtml = _User.Name + ",你好!";
            if (_User.cps.Type == 2)
            {
                lbUserType.InnerHtml = "代理商";
                trPromoterCPS.Visible = false;
                trPromoter.Visible = true;
            }
        }
    }
    protected void lbExit_Click(object sender, EventArgs e)
    {
        if (_User != null)
        {
            string ReturnDescription = "";
            _User.Logout(ref ReturnDescription);
        }

        this.Response.Redirect("../index.aspx");
    }


    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isCpsRequestLogin = true;
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Admin/Default.aspx?SubPage=LinkList.aspx";

        base.OnInit(e);
    }

    #endregion

    private void BindLinkList()
    {
        DateTime StartTime = Shove._Convert.StrToDateTime(tbBeginTime.Text.Trim() + " 0:0:0", DateTime.Now.ToString());
        DateTime EndTime = Shove._Convert.StrToDateTime(tbEndTime.Text.Trim() + " 23:59:59", DateTime.Now.ToString());

        if (EndTime.CompareTo(StartTime) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "开始时间不能大于截止时间。");

            return;
        }

        string CacheKey = "Cps_Admin_LinkList" + _User.cps.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            DataSet ds = null;

            int ReturnValue = -1;
            string ReturnDescprtion = "";

            int Result = DAL.Procedures.P_GetCpsLinkList(ref ds, _User.cps.ID, StartTime, EndTime, ref ReturnValue, ref ReturnDescprtion);

            if (Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (ReturnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescprtion);

                return;
            }

            if (ds == null)
            {
                PF.GoError(ErrorNumber.NoData, "没有数据！", this.GetType().BaseType.FullName);

                return;
            }

            dt = ds.Tables[0];

            Shove._Web.Cache.SetCache(CacheKey, dt, 3600);
        }

        DataTable dtTemp = GetTotal(dt);
        PF.DataGridBindData(g, dtTemp, gPager);
        gPager.Visible = g.PageCount > 1;
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindLinkList();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindLinkList();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindLinkList();
    }

    protected DataTable GetTotal(DataTable dt)
    {
        if (dt.Columns.IndexOf("SiteMoney") == -1)
            dt.Columns.Add(new DataColumn("SiteMoney", typeof(decimal)));

        double scale = _Site.SiteOptions["BonusScale"].ToDouble(0.02);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string pid = dt.Rows[i]["PID"].ToString();
            DataTable dtSite = new DAL.Tables.T_UnionLinkScale().Open("BonusScale", "UnionID=" + _User.ID + " and SiteLinkPID='" + pid + "'", "BonusScale");

            if (dtSite != null && dtSite.Rows.Count > 0)
                scale = Shove._Convert.StrToDouble(dtSite.Rows[0][0].ToString(), 0);

            double tradeMoney = 0;
            if (!double.TryParse(dt.Rows[i]["TradeMoney"].ToString(), out tradeMoney))
            {
                tradeMoney = 0;
            }

            dt.Rows[i]["SiteMoney"] = tradeMoney * scale;
        }

        return dt;
    }
}
