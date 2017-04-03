﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_PersonagesAdd : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindLotteryType();
        }
    }

    private void BindLotteryType()
    {
        string LotteryID = Shove._Web.Utility.GetRequest("LotteryID");

        string CacheKey = "dtLotteriesUseLotteryList";
        DataTable dtLotteries = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtLotteries == null)
        {
            dtLotteries = new DAL.Tables.T_Lotteries().Open("[ID], [Name], [Code]", "[ID] in(" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[ID]");

            if (dtLotteries == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(-46)");

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dtLotteries, 6000);
        }

        ddlLotteries.DataSource = dtLotteries;
        ddlLotteries.DataTextField = "Name";
        ddlLotteries.DataValueField = "ID";
        ddlLotteries.DataBind();

        if (ddlLotteries.Items.FindByValue(LotteryID) != null)
        {
            ddlLotteries.SelectedValue = LotteryID;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.FillContent);

        base.OnInit(e);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string UserName = tbName.Text.Trim();

        if (UserName == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入名人用户名！");

            return;
        }

        int order = Shove._Convert.StrToInt(tbOrder.Text.Trim(), -1);

        if (order < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "顺序输入非法！");

            return;
        }

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name='" + UserName + "'", "");

        if (dt == null || dt.Rows.Count == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "不存在"+UserName+"用户！");

            return;
        }

        dt = new DAL.Tables.T_Personages().Open("ID", "UserName='" + UserName + "' and LotteryID=" + Shove._Web.Utility.FilteSqlInfusion(ddlLotteries.SelectedValue) + "", "");

        if (dt != null && dt.Rows.Count > 0)
        {
            Shove._Web.JavaScript.Alert(this.Page,UserName+"已经是"+ddlLotteries.SelectedItem.Text+"的名人了！");

            return;
        }

        DAL.Tables.T_Personages p = new DAL.Tables.T_Personages();

        p.Order.Value = order;
        p.UserName.Value = UserName;
        p.LotteryID.Value = ddlLotteries.SelectedValue;
        p.IsShow.Value = cbisShow.Checked;

        long l = p.Insert();

        if (l > 0)
        {
            Shove._Web.Cache.ClearCache("Admin_Personages");
            Shove._Web.JavaScript.Alert(this, "添加成功", "Personages.aspx?LotteryID=" + ddlLotteries.SelectedValue);
        }
        else
        {
            Shove._Web.JavaScript.Alert(this, "添加失败");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Personages.aspx?LotteryID=" + ddlLotteries.SelectedValue, true);
    }
}
