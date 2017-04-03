using System;
using System.Data;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// ����༭
    /// </summary>
    public partial class editannounce : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (DNTRequest.GetString("id") == "")
                {
                    Response.Redirect("global_announcegrid.aspx");
                }
                else
                {
                    LoadAnnounceInf(DNTRequest.GetInt("id", -1));
                    UpdateAnnounceInfo.ValidateForm = true;
                    tbtitle.AddAttributes("maxlength", "200");
                    tbtitle.AddAttributes("rows", "2");
                }
            }
        }

        public void LoadAnnounceInf(int id)
        {
            #region װ�ع�����Ϣ

            DataTable dt = DatabaseProvider.GetInstance().GetAnnouncement(id);
            if (dt.Rows.Count > 0)
            {
                displayorder.Text = dt.Rows[0]["displayorder"].ToString();
                tbtitle.Text = dt.Rows[0]["title"].ToString();
                poster.Text = dt.Rows[0]["poster"].ToString();
                starttime.Text = Utils.GetStandardDateTime(dt.Rows[0]["starttime"].ToString());
                endtime.Text = Utils.GetStandardDateTime(dt.Rows[0]["endtime"].ToString());
                message.Text = dt.Rows[0]["message"].ToString().Trim();
            }

            #endregion
        }

        private void UpdateAnnounceInfo_Click(object sender, EventArgs e)
        {
            #region ���¹��������Ϣ

            if (this.CheckCookie())
            {
                DatabaseProvider.GetInstance().UpdateAnnouncement(DNTRequest.GetInt("id", 0), poster.Text, tbtitle.Text, Utils.StrToInt(displayorder.Text, 0), starttime.Text, endtime.Text, message.Text); 
                //�Ƴ����滺��
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/SimplifiedAnnouncementList");

                //��¼��־
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "���¹���", "���¹���,����Ϊ:" + tbtitle.Text);

                base.RegisterStartupScript( "PAGE", "window.location.href='global_announcegrid.aspx';");
            }

            #endregion
        }

        private void DeleteAnnounce_Click(object sender, EventArgs e)
        {
            #region ɾ������

            if (this.CheckCookie())
            {
                DatabaseProvider.GetInstance().DeleteAnnouncement(DNTRequest.GetInt("id", 0));
                //�Ƴ����滺��
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/SimplifiedAnnouncementList");

                //��¼��־
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "ɾ������", "ɾ������,����Ϊ:" + tbtitle.Text);

                base.RegisterStartupScript( "PAGE", "window.location.href='global_announcegrid.aspx';");
            }

            #endregion
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.UpdateAnnounceInfo.Click += new EventHandler(this.UpdateAnnounceInfo_Click);
            this.DeleteAnnounce.Click += new EventHandler(this.DeleteAnnounce_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion


    }
}