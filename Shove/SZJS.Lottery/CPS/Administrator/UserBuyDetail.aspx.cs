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

public partial class Cps_Administrator_UserBuyDetail : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //处理请求查看的日期范围
            DateTime fromDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);//默认为本日
            DateTime toDateTime = DateTime.Now;
            if (Request["FromDay"] != null && Request["ToDay"] != null)
            {
                fromDateTime = Shove._Convert.StrToDateTime(Shove._Web.Utility.GetRequest("FromDay") + " 0:0:0", DateTime.Now.ToString());
                toDateTime = Shove._Convert.StrToDateTime(Shove._Web.Utility.GetRequest("ToDay") + " 23:59:59", DateTime.Now.ToString());
            }

            tbBeginTime.Text = fromDateTime.ToString("yyyy-MM-dd");
            tbEndTime.Text = toDateTime.ToString("yyyy-MM-dd");

            long userID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), -1);
            if (userID < 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "参数错误!");
                form1.Visible = false;
                return;
            }

            Users thisUser = new Users(_Site.ID);
            thisUser.ID = userID;

            hfCpsID.Value = thisUser.CpsID.ToString();
            hfUserID.Value = thisUser.ID.ToString();
            lblUserName.Text = thisUser.Name;

            //绑定数据
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/Cps/Administrator/Default.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        //处理传入参数
        long cpsID = Shove._Convert.StrToLong(hfCpsID.Value, -1);
        long userID = Shove._Convert.StrToLong(hfUserID.Value, -1);

        //处理日期
        DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);// 默认查询本日明细
        DateTime toDate = DateTime.Now;
        if (tbBeginTime.Text != "" && tbEndTime.Text != "")
        {
            fromDate = Shove._Convert.StrToDateTime(tbBeginTime.Text.Trim() + " 0:0:0", DateTime.Now.ToString());
            toDate = Shove._Convert.StrToDateTime(tbEndTime.Text.Trim() + " 23:59:59", DateTime.Now.ToString());
        }
        tbBeginTime.Text = fromDate.ToString("yyyy-MM-dd");
        tbEndTime.Text = toDate.ToString("yyyy-MM-dd");

        //获取数据
        DataTable dt = null;
        string key = "Cps_Administrator_UserBuyDetail_" + cpsID + tbBeginTime.Text + tbEndTime.Text;
        dt = Shove._Web.Cache.GetCacheAsDataTable(key);
        if (dt == null)
        {
            int returnValue = -1;
            string returnDescription = "";
            DataSet ds = null;
            int Result = DAL.Procedures.P_GetCpsMemberBuyDetail(ref ds, _Site.ID, cpsID, userID, fromDate, toDate, ref returnValue, ref returnDescription);

            if (Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (returnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);

                return;
            }
            if ((ds == null) || (ds.Tables.Count < 1))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(key, dt, 3600);

        }
        PF.DataGridBindData(g, dt, gPager);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!Shove._String.Valid.isDate(tbBeginTime.Text) || !Shove._String.Valid.isDate(tbEndTime.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输正确的查询日期格式.如 2011-01-01");
            return;
        }
        BindData();
    }
}
