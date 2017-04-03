using System;
using System.Data;
using Discuz.Common;
#if NET1
#else
using Discuz.Common.Generic;
#endif
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// �ҵ�����
    /// </summary>
    public partial class myposts : PageBase
    {
        #region ҳ�����
#if NET1
		public MyTopicInfoCollection topics;
#else
        /// <summary>
        /// ���������������б�
        /// </summary>
        public List<MyTopicInfo> topics;
#endif
        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public int pageid;
        /// <summary>
        /// ��ҳ��
        /// </summary>
        public int pagecount;
        /// <summary>
        /// ��������
        /// </summary>
        public int topiccount;
        /// <summary>
        /// ��ҳҳ������
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// ��ǰ��¼���û���Ϣ
        /// </summary>
        public UserInfo user = new UserInfo();
        #endregion

        private int pagesize = 16;

        protected override void ShowPage()
        {
            pagetitle = "�û��������";

            if (userid == -1)
            {
                AddErrLine("����δ��¼");
                return;
            }

            user = Discuz.Forum.Users.GetUserInfo(userid);

            //�õ���ǰ�û������ҳ��
            pageid = DNTRequest.GetInt("page", 1);
            //��ȡ��������
            topiccount = Topics.GetTopicsCountbyReplyUserId(this.userid);
            //��ȡ��ҳ��
            pagecount = topiccount%pagesize == 0 ? topiccount/pagesize : topiccount/pagesize + 1;
            if (pagecount == 0)
            {
                pagecount = 1;
            }
            //��������ҳ���п��ܵĴ���
            if (pageid < 1)
            {
                pageid = 1;
            }
            if (pageid > pagecount)
            {
                pageid = pagecount;
            }

            this.topics = Topics.GetTopicsByReplyUserId(this.userid, pageid, pagesize, 600, config.Hottopic);

            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "myposts.aspx", 8);
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:00.
                ��ҳ�������Discuz!NTģ������������ 2008/10/13 15:56:00. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"usercp.aspx\">�û�����</a>  &raquo; <strong>�ҵĻظ�</strong>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<div id=\"headsearch\">\r\n");
            templateBuilder.Append("		<div id=\"search\">\r\n");

            templateBuilder.Append("			<form method=\"post\" action=\"search.aspx\" target=\"_blank\" onsubmit=\"bind_keyword(this);\">\r\n");
            templateBuilder.Append("				<input type=\"hidden\" name=\"poster\" />\r\n");
            templateBuilder.Append("				<input type=\"hidden\" name=\"keyword\" />\r\n");
            templateBuilder.Append("				<input type=\"hidden\" name=\"type\" value=\"\" />\r\n");
            templateBuilder.Append("				<input id=\"keywordtype\" type=\"hidden\" name=\"keywordtype\" value=\"0\"/>\r\n");
            templateBuilder.Append("				<div id=\"searchbar\">\r\n");
            templateBuilder.Append("					<dl>\r\n");
            templateBuilder.Append("						<dt id=\"quicksearch\" class=\"s2\" onclick=\"showMenu(this.id, false);\" onmouseover=\"MouseCursor(this);\">���ӱ���</dt>\r\n");
            templateBuilder.Append("						<dd class=\"textinput\"><input type=\"text\" name=\"keywordf\" value=\"\" class=\"text\"/></dd>\r\n");
            templateBuilder.Append("						<dd><input name=\"searchsubmit\" type=\"submit\" value=\"\" class=\"s3\"/></dd>\r\n");
            templateBuilder.Append("					</dl>\r\n");
            templateBuilder.Append("				</div>\r\n");
            templateBuilder.Append("			</form>\r\n");
            templateBuilder.Append("			<script type=\"text/javascript\">function bind_keyword(form){if(form.keywordtype.value=='8'){form.keyword.value='';form.poster.value=form.keywordf.value; } else { form.poster.value=''; form.keyword.value=form.keywordf.value;if(form.keywordtype.value == '2')form.type.value = 'spacepost';if(form.keywordtype.value == '3')form.type.value = 'album';}}</" + "script>\r\n");


            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!--body-->\r\n");
            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("	function checkCheckBox(form,objtag)\r\n");
            templateBuilder.Append("	{\r\n");
            templateBuilder.Append("		for(var i = 0; i < form.elements.length; i++) \r\n");
            templateBuilder.Append("		{\r\n");
            templateBuilder.Append("			var e = form.elements[i];\r\n");
            templateBuilder.Append("			if(e.name == \"pmitemid\") \r\n");
            templateBuilder.Append("			{\r\n");
            templateBuilder.Append("				e.checked = objtag.checked;\r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("		}\r\n");
            templateBuilder.Append("		objtag.checked = !objtag.checked;\r\n");
            templateBuilder.Append("	}\r\n");
            templateBuilder.Append("</" + "script>\r\n");
            templateBuilder.Append("<div class=\"controlpannel\">\r\n");

            templateBuilder.Append("<div class=\"pannelmenu\">\r\n");

            if (userid > 0)
            {


                if (pagename == "usercptopic.aspx" || pagename == "usercppost.aspx" || pagename == "usercpdigest.aspx" || pagename == "usercpprofile.aspx"


                  || pagename == "usercpnewpassword.aspx" || pagename == "usercppreference.aspx")
                {

                    templateBuilder.Append("	   <a href=\"usercpprofile.aspx\" class=\"current\"><span>��������</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("	   <a href=\"usercpprofile.aspx\">��������</a>\r\n");

                }	//end if


                if (pagename == "usercpinbox.aspx" || pagename == "usercpsentbox.aspx" || pagename == "usercpdraftbox.aspx" || pagename == "usercppostpm.aspx" || pagename == "usercpshowpm.aspx" || pagename == "usercppmset.aspx")
                {

                    templateBuilder.Append("	   <a href=\"usercpinbox.aspx\" class=\"current\"><span>����Ϣ</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("	   <a href=\"usercpinbox.aspx\">����Ϣ</a>\r\n");

                }	//end if


                if (pagename == "usercpsubscribe.aspx")
                {

                    templateBuilder.Append("	   <a href=\"usercpsubscribe.aspx\" class=\"current\"><span>�ղؼ�</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("	   <a href=\"usercpsubscribe.aspx\">�ղؼ�</a>\r\n");

                }	//end if


                if (pagename == "usercpcreditspay.aspx" || pagename == "usercpcreditstransfer.aspx" || pagename == "usercpcreditspayoutlog.aspx" || pagename == "usercpcreditspayinlog.aspx"


                   || pagename == "usercpcreaditstransferlog.aspx")
                {

                    templateBuilder.Append("       <a href=\"usercpcreditspay.aspx\" class=\"current\"><span>��ҽ���</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("       <a href=\"usercpcreditspay.aspx\">��ҽ���</a>\r\n");

                }	//end if


                if (pagename == "usercpforumsetting.aspx")
                {

                    templateBuilder.Append("			<a href=\"usercpforumsetting.aspx\" class=\"current\"><span>��̳����</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("			<a href=\"usercpforumsetting.aspx\">��̳����</a>\r\n");

                }	//end if
            }	//end if

            templateBuilder.Append("	</div>\r\n");


            templateBuilder.Append("	<div class=\"pannelcontent\">\r\n");
            templateBuilder.Append("		<div class=\"pcontent\">\r\n");
            templateBuilder.Append("			<div class=\"panneldetail\">\r\n");
            templateBuilder.Append("				<div class=\"pannelbody\">\r\n");
            templateBuilder.Append("					<div class=\"pannellist\">\r\n");

            if (page_err == 0)
            {

                templateBuilder.Append("						<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n");
                templateBuilder.Append("						<tbody>\r\n");
                templateBuilder.Append("							<tr>\r\n");
                templateBuilder.Append("							<td width=\"2%\">&nbsp;</td>\r\n");
                templateBuilder.Append("							<td width=\"4%\">&nbsp;</td>\r\n");
                templateBuilder.Append("							<td width=\"40%\" style=\"text-align:left;\">��������</td>\r\n");
                templateBuilder.Append("							<td width=\"20%\">���ڰ��</td>\r\n");
                templateBuilder.Append("							<td width=\"10%\">����</td>\r\n");
                templateBuilder.Append("							<td><span class=\"fontfamily\">��󷢱�</span></td>\r\n");
                templateBuilder.Append("							</tr>\r\n");

                int topic__loop__id = 0;
                foreach (MyTopicInfo topic in topics)
                {
                    topic__loop__id++;

                    templateBuilder.Append("							<tr class=\"messagetable\" onmouseover=\"this.className='messagetableon'\" onmouseout=\"this.className='messagetable'\">\r\n");
                    templateBuilder.Append("							<td width=\"2%\">\r\n");

                    if (topic.Folder != "")
                    {

                        aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid, 0);

                        templateBuilder.Append("								<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\"><img src=\"templates/" + templatepath.ToString() + "/images/folder_" + topic.Folder.ToString().Trim() + ".gif\" alt=\"ͼ��\"/></a>\r\n");

                    }	//end if

                    templateBuilder.Append("							</td>\r\n");
                    templateBuilder.Append("							<td width=\"4%\">\r\n");

                    if (topic.Iconid != 0)
                    {

                        templateBuilder.Append("<img src=\"images/posticons/" + topic.Iconid.ToString().Trim() + ".gif\" alt=\"ͼ��\" />\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("&nbsp;\r\n");

                    }	//end if

                    templateBuilder.Append("</td>\r\n");
                    templateBuilder.Append("							<td width=\"40%\" style=\"text-align:left;\">\r\n");
                    aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid, 0);

                    templateBuilder.Append("								<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + topic.Title.ToString().Trim() + "</a></td>\r\n");
                    templateBuilder.Append("							<td width=\"20%\">\r\n");
                    aspxrewriteurl = this.ShowForumAspxRewrite(topic.Fid, 0);

                    templateBuilder.Append("								<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + topic.Forumname.ToString().Trim() + "</a></td>\r\n");
                    templateBuilder.Append("							<td width=\"10%\">\r\n");

                    if (topic.Posterid != -1)
                    {

                        aspxrewriteurl = this.UserInfoAspxRewrite(topic.Posterid);

                        templateBuilder.Append("							<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + topic.Poster.ToString().Trim() + "</a>\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("" + topic.Poster.ToString().Trim() + "\r\n");

                    }	//end if

                    templateBuilder.Append("</td>\r\n");
                    templateBuilder.Append("							<td><span class=\"fontfamily\"><a href=\"showtopic.aspx?topicid=" + topic.Tid.ToString().Trim() + "&page=end\"><script type=\"text/javascript\">document.write(convertdate('" + topic.Lastpost.ToString().Trim() + "'));</" + "script></a> by \r\n");

                    if (topic.Lastposterid != -1)
                    {

                        templateBuilder.Append("<a href=\"userinfo-" + topic.Lastposterid.ToString().Trim() + "" + config.Extname.ToString().Trim() + "\" target=\"_blank\">" + topic.Lastposter.ToString().Trim() + "</a>\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("" + topic.Lastposter.ToString().Trim() + "\r\n");

                    }	//end if

                    templateBuilder.Append("</span></td>\r\n");
                    templateBuilder.Append("							</tr>\r\n");

                }	//end loop

                templateBuilder.Append("						</tbody>\r\n");
                templateBuilder.Append("						</table>\r\n");
                templateBuilder.Append("						</div>						\r\n");
                templateBuilder.Append("						<div class=\"pages_btns\">\r\n");
                templateBuilder.Append("							<div class=\"pages\">\r\n");
                templateBuilder.Append("								<em>" + pageid.ToString() + "/" + pagecount.ToString() + "ҳ</em>" + pagenumbers.ToString() + "\r\n");
                templateBuilder.Append("								<kbd>��ת<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) {\r\n");
                templateBuilder.Append("					window.location='myposts.aspx?page='+this.value;}\"  size=\"4\" maxlength=\"9\" class=\"colorblue2\"/>ҳ</kbd>\r\n");
                templateBuilder.Append("							</div>\r\n");
                templateBuilder.Append("						</div>\r\n");

            }
            else
            {


                templateBuilder.Append("<div class=\"box message\">\r\n");
                templateBuilder.Append("	<h1>������ʾ</h1>\r\n");
                templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");
                templateBuilder.Append("	<p class=\"errorback\">\r\n");
                templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("			if(" + msgbox_showbacklink.ToString() + ")\r\n");
                templateBuilder.Append("			{\r\n");
                templateBuilder.Append("				document.write(\"<a href=\\\"" + msgbox_backlink.ToString() + "\\\">������һ��</a> &nbsp; &nbsp;|  \");\r\n");
                templateBuilder.Append("			}\r\n");
                templateBuilder.Append("		</" + "script>\r\n");
                templateBuilder.Append("		&nbsp; &nbsp; <a href=\"forumindex.aspx\">��̳��ҳ</a>\r\n");

                if (usergroupid == 7)
                {

                    templateBuilder.Append("		 |&nbsp; &nbsp; <a href=\"register.aspx\">ע��</a>\r\n");

                }	//end if

                templateBuilder.Append("	</p>\r\n");
                templateBuilder.Append("</div>\r\n");



            }	//end if

            templateBuilder.Append("			  </div>\r\n");
            templateBuilder.Append("			</div>\r\n");
            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!--body-->\r\n");
            templateBuilder.Append("</div>\r\n");


            if (footerad != "")
            {

                templateBuilder.Append("<!--�ײ������ʾ-->\r\n");
                templateBuilder.Append("<div id=\"ad_footerbanner\">" + footerad.ToString() + "</div>\r\n");
                templateBuilder.Append("<!--�ײ�������-->\r\n");

            }	//end if

            templateBuilder.Append(Discuz.Web.UI.PageElement.Bottom);
            templateBuilder.Append("<ul class=\"popupmenu_popup headermenu_popup\" id=\"stats_menu\" style=\"display: none\">\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx\">����״��</a></li>\r\n");

            if (config.Statstatus == 1)
            {

                templateBuilder.Append("	<li><a href=\"stats.aspx?type=views\">����ͳ��</a></li>\r\n");
                templateBuilder.Append("	<li><a href=\"stats.aspx?type=client\">�ͻ����</a></li>\r\n");

            }	//end if

            templateBuilder.Append("	<li><a href=\"stats.aspx?type=posts\">��������¼</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=forumsrank\">�������</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=topicsrank\">��������</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=postsrank\">��������</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=creditsrank\">�������</a></li>\r\n");

            if (config.Oltimespan > 0)
            {

                templateBuilder.Append("	<li><a href=\"stats.aspx?type=onlinetime\">����ʱ��</a></li>\r\n");

            }	//end if

            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<ul class=\"popupmenu_popup headermenu_popup\" id=\"my_menu\" style=\"display: none\">\r\n");
            templateBuilder.Append("	<li><a href=\"mytopics.aspx\">�ҵ�����</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"myposts.aspx\">�ҵ�����</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"search.aspx?posterid=" + userid.ToString() + "&amp;type=digest\">�ҵľ���</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"myattachment.aspx\">�ҵĸ���</a></li>\r\n");

            if (userid > 0)
            {

                templateBuilder.Append("	<li><a href=\"usercpsubscribe.aspx\">�ҵ��ղ�</a></li>\r\n");

            }	//end if

            templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/mymenu.js\"></" + "script>\r\n");
            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<ul class=\"popupmenu_popup\" id=\"viewpro_menu\" style=\"display: none\">\r\n");

            if (useravatar != "")
            {

                templateBuilder.Append("		<img src=\"" + useravatar.ToString() + "\"/>\r\n");

            }	//end if

            aspxrewriteurl = this.UserInfoAspxRewrite(userid);

            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<div id=\"quicksearch_menu\" class=\"searchmenu\" style=\"display: none;\">\r\n");
            templateBuilder.Append("	<div onclick=\"document.getElementById('keywordtype').value='0';document.getElementById('quicksearch').innerHTML='���ӱ���';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">���ӱ���</div>\r\n");


            templateBuilder.Append("	<div onclick=\"document.getElementById('keywordtype').value='8';document.getElementById('quicksearch').innerHTML='��&nbsp;&nbsp;��';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">��&nbsp;&nbsp;��</div>\r\n");
            templateBuilder.Append("</div>\r\n");



            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
    }
}