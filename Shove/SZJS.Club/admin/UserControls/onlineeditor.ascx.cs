using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Discuz.Control;

namespace Discuz.Web.Admin
{

    /// <summary>
    ///	���߱༭�ؼ�
    /// </summary>
    public partial class OnlineEditor : UserControl
    {
        public string controlname;
        public int postminchars = 0;
        public int postmaxchars = 200;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { }
        }


        public string Text
        {
            set { DataTextarea.InnerText = value; }
            get { return DataTextarea.InnerText.Replace("'", "''"); }
        }

        #region Web ������������ɵĴ���

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}