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

public partial class CPS_Admin_BonusScaleView : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            lbUserName.InnerHtml = _User.Name + ",你好!";
            if (_User.cps.Type == 2)
            {
                lbUserType.InnerHtml = "代理商";
                trPromoterCPS.Visible = false;
                trPromoter.Visible = true;
            }
            BindData();
        }
    }
    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isCpsRequestLogin = true;
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Admin/Default.aspx?SubPage=BonusScaleView.aspx";

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        string cacheKeyScaleList = "Cps_Admin_BonusScaleView_ScaleList_" + _User.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKeyScaleList);

        if (dt == null)
        {
            int returnValue = -1;
            string returnDescription = "";
            DataSet ds = null;
            int Result = DAL.Procedures.P_CpsGetUserBonusScaleList(ref ds, _User.ID, ref returnValue, ref returnDescription);

            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "系统出错!");

                return;

            }
            if (ds == null || ds.Tables.Count < 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "获取数据出错!");

                return;
            }
            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(cacheKeyScaleList, dt, 100000);
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
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
}
