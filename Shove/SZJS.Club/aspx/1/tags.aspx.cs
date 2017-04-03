using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Common;
using System;

namespace Discuz.Web
{
    /// <summary>
    /// ��ǩ�б�ҳ
    /// </summary>
    public partial class tags : PageBase
    {
        #region ҳ�����

        /// <summary>
        /// ʹ����ָ����ǩ�������б�
        /// </summary>
        public List<MyTopicInfo> topiclist;
        /// <summary>
        /// �����������TagId
        /// </summary>
        public int tagid;
        /// <summary>
        /// Tag����ϸ��Ϣ
        /// </summary>
        public TagInfo tag;
        /// <summary>
        /// ҳ��
        /// </summary>
        public int pageid = 1;
        /// <summary>
        /// ������
        /// </summary>
        public int topiccount = 0;
        /// <summary>
        /// ��־��
        /// </summary>
        public int spacepostcount = 0;
        /// <summary>
        /// ͼƬ��
        /// </summary>
        public int photocount = 0;
        /// <summary>
        /// ҳ��
        /// </summary>
        public int pagecount = 1;
        /// <summary>
        /// ��ҳҳ������
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// ��ǰ�б�����,topic=�����б�,spacepost=��־�б�,photo=ͼƬ�б�
        /// </summary>
        public string listtype;
        /// <summary>
        /// ��ǩ����
        /// </summary>
        public TagInfo[] taglist;
        #endregion

        protected override void ShowPage()
        {
            
            if (config.Enabletag != 1)
            {
                AddErrLine("û������Tag����");
                return;
            }

            tagid = DNTRequest.GetInt("tagid", 0);

            if (tagid > 0)
            {
                tag = Tags.GetTagInfo(tagid);
                if (tag == null)
                {
                    AddErrLine("ָ���ı�ǩ������");
                    return;
                }

                if (tag.Orderid < 0)
                {
                    AddErrLine("ָ���ı�ǩ�ѱ��ر�");
                }

                if (IsErr())
                {
                    return;
                }

                listtype = DNTRequest.GetString("t");

                pageid = DNTRequest.GetInt("page", 1);
                if (pageid < 1)
                {
                    pageid = 1;
                }
                pagetitle = tag.Tagname;
                if (listtype.Equals(""))
                {
                    listtype = "topic";
                
                }
                switch (listtype)
                {

                    case "topic":
                        topiccount = Topics.GetTopicsCountWithSameTag(tagid);
                        pagecount = topiccount % config.Tpp == 0 ? topiccount / config.Tpp : topiccount / config.Tpp + 1;

                        if (pagecount == 0)
                        {
                            pagecount = 1;
                        }
                        if (pageid > pagecount)
                        {
                            pageid = pagecount;
                        }

                        if (topiccount > 0)
                        {
                            topiclist = Topics.GetTopicsWithSameTag(tagid, pageid, config.Tpp);
                            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "tags.aspx?t=topic&tagid=" + tagid, 8);
                        }
                        else
                        {
                            AddMsgLine("�ñ�ǩ����������");
                        }
                        break;


                }
            }
            else
            {
                pagetitle = "��ǩ";

                taglist = ForumTags.GetCachedHotForumTags(100);

            }
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:50.
                ��ҳ�������Discuz!NTģ������������ 2008/10/13 15:54:50. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("var aspxrewrite = " + config.Aspxrewrite.ToString().Trim() + ";\r\n");
            templateBuilder.Append("</" + "script>\r\n");
            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\"><a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; <a href=\"tags.aspx\">��ǩ</a> &raquo; \r\n");

            if (page_err == 0 && tagid > 0)
            {

                templateBuilder.Append("" + tag.Tagname.ToString().Trim() + "\r\n");

            }	//end if

            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</div>\r\n");

            if (page_err == 0)
            {


                if (tagid > 0)
                {

                    templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("			function changeTab(obj)\r\n");
                    templateBuilder.Append("			{\r\n");
                    templateBuilder.Append("				if (obj.className == 'currenttab')\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					obj.className = '';\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("				else\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					obj.className = 'currenttab';\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("			}\r\n");
                    templateBuilder.Append("		</" + "script>\r\n");
                    templateBuilder.Append("		<div class=\"searchtab\">\r\n");
                    templateBuilder.Append("			<a id=\"tab_forum\" \r\n");

                    if (listtype == "topic")
                    {

                        templateBuilder.Append("class=\"currenttab\" \r\n");

                    }
                    else
                    {

                        templateBuilder.Append(" onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\" href=\"\r\n");

                        if (config.Aspxrewrite == 1)
                        {

                            templateBuilder.Append("topictag-" + tagid.ToString() + ".aspx\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("tags.aspx?tagid=" + tagid.ToString() + "\r\n");

                        }	//end if

                        templateBuilder.Append("\"\r\n");

                    }	//end if

                    templateBuilder.Append(">����</a>\r\n");

                    templateBuilder.Append("		</div>\r\n");

                    if (listtype == "topic")
                    {


                        if (topiccount == 0)
                        {


                            templateBuilder.Append("<div class=\"box message\">\r\n");
                            templateBuilder.Append("	<h1>������ʾ��Ϣ</h1>\r\n");
                            templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");

                            if (msgbox_url != "")
                            {

                                templateBuilder.Append("	<p><a href=\"" + msgbox_url.ToString() + "\">��������û��ת��, ��������.</a></p>\r\n");

                            }	//end if

                            templateBuilder.Append("</div>\r\n");
                            templateBuilder.Append("</div>\r\n");



                        }
                        else
                        {

                            templateBuilder.Append("				<DIV class=\"mainbox forumlist\">\r\n");
                            templateBuilder.Append("				<TABLE cellSpacing=\"0\" cellPadding=\"0\" summary=\"�����ǩ���\">\r\n");
                            templateBuilder.Append("					<thead>\r\n");
                            templateBuilder.Append("					<tr>\r\n");
                            templateBuilder.Append("						<th>����</th>\r\n");
                            templateBuilder.Append("						<th>���ڰ��</th>\r\n");
                            templateBuilder.Append("						<td>����</td>\r\n");
                            templateBuilder.Append("						<td class=\"nums\">�ظ�</td>\r\n");
                            templateBuilder.Append("						<td class=\"nums\">�鿴</td>\r\n");
                            templateBuilder.Append("						<td>��󷢱�</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("					</thead>\r\n");

                            int topic__loop__id = 0;
                            foreach (MyTopicInfo topic in topiclist)
                            {
                                topic__loop__id++;

                                templateBuilder.Append("					<tbody>\r\n");
                                templateBuilder.Append("						<tr>\r\n");
                                templateBuilder.Append("							<td>\r\n");
                                aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid, 0);

                                templateBuilder.Append("								<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + topic.Title.ToString().Trim() + "</a></td>\r\n");
                                templateBuilder.Append("							<td>\r\n");
                                aspxrewriteurl = this.ShowForumAspxRewrite(topic.Fid, 0);

                                templateBuilder.Append("								<a href=\"" + aspxrewriteurl.ToString() + "\">" + topic.Forumname.ToString().Trim() + "</a>\r\n");
                                templateBuilder.Append("							</td>\r\n");
                                templateBuilder.Append("							<td>\r\n");
                                templateBuilder.Append("								<p>\r\n");

                                if (Utils.StrToInt(topic.Posterid, 0) == -1)
                                {

                                    templateBuilder.Append("									�ο�\r\n");

                                }
                                else
                                {

                                    aspxrewriteurl = this.UserInfoAspxRewrite(topic.Posterid);

                                    templateBuilder.Append("									<a href=\"" + aspxrewriteurl.ToString() + "\">" + topic.Poster.ToString().Trim() + "</a>\r\n");

                                }	//end if

                                templateBuilder.Append("</p>\r\n");
                                templateBuilder.Append("								<em>\r\n");
                                templateBuilder.Append(Convert.ToDateTime(topic.Postdatetime).ToString("yyyy.MM.dd HH:mm"));
                                templateBuilder.Append("</em>\r\n");
                                templateBuilder.Append("							</td>\r\n");
                                templateBuilder.Append("							<td class=\"nums\">" + topic.Replies.ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("							<td class=\"nums\">" + topic.Views.ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("							<td>\r\n");
                                templateBuilder.Append("									<em><a href=\"showtopic.aspx?topicid=" + topic.Tid.ToString().Trim() + "&page=end\" target=\"_blank\">\r\n");
                                templateBuilder.Append(Convert.ToDateTime(topic.Lastpost).ToString("yyyy.MM.dd HH:mm"));
                                templateBuilder.Append("</a></em>\r\n");
                                templateBuilder.Append("									<cite>by\r\n");

                                if (topic.Lastposterid == -1)
                                {

                                    templateBuilder.Append("										�ο�\r\n");

                                }
                                else
                                {

                                    templateBuilder.Append("										<a href=\"" + UserInfoAspxRewrite(topic.Lastposterid).ToString().Trim() + "\" target=\"_blank\">" + topic.Lastposter.ToString().Trim() + "</a>\r\n");

                                }	//end if

                                templateBuilder.Append("									</cite>\r\n");
                                templateBuilder.Append("							</td>\r\n");
                                templateBuilder.Append("						</tr>\r\n");
                                templateBuilder.Append("					</tbody>\r\n");

                            }	//end loop

                            templateBuilder.Append("					</table>			\r\n");
                            templateBuilder.Append("				</div>\r\n");
                            templateBuilder.Append("				<div class=\"pages_btns\">\r\n");
                            templateBuilder.Append("					<div class=\"pages\">\r\n");
                            templateBuilder.Append("						<em>" + pageid.ToString() + "/" + pagecount.ToString() + "ҳ</em>" + pagenumbers.ToString() + "\r\n");
                            templateBuilder.Append("						<kbd>��ת<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) { window.location='tags.aspx?t=topic&tagid=" + tagid.ToString() + "&page=' + (parseInt(this.value) > 0 ? parseInt(this.value) : 1);}\" size=\"4\" maxlength=\"9\" class=\"colorblue2\"/>ҳ</kbd>\r\n");
                            templateBuilder.Append("					</div>\r\n");
                            templateBuilder.Append("				</div>\r\n");

                        }	//end if


                    }

                }
                else
                {

                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"cache/tag/closedtags.txt\"></" + "script>\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"cache/tag/colorfultags.txt\"></" + "script>\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"javascript/template_showtopic.js\"></" + "script>	\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"javascript/template_tags.js\"></" + "script>	\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"javascript/ajax.js\"></" + "script>\r\n");
                    templateBuilder.Append("		<div class=\"mainbox\">\r\n");
                    templateBuilder.Append("			<h3>��̳���ű�ǩ</h3>\r\n");
                    templateBuilder.Append("			<ul id=\"forumhottags\" class=\"taglist\">\r\n");

                    int tag__loop__id = 0;
                    foreach (TagInfo tag in taglist)
                    {
                        tag__loop__id++;

                        templateBuilder.Append("					<li><a \r\n");

                        if (config.Aspxrewrite == 1)
                        {

                            templateBuilder.Append("					href=\"topictag-" + tag.Tagid.ToString().Trim() + ".aspx\" \r\n");

                        }
                        else
                        {

                            templateBuilder.Append("					href=\"tags.aspx?t=topic&tagid=" + tag.Tagid.ToString().Trim() + "\" \r\n");

                        }	//end if


                        if (tag.Color != "")
                        {

                            templateBuilder.Append("					style=\"color: #" + tag.Color.ToString().Trim() + ";\"\r\n");

                        }	//end if

                        templateBuilder.Append("					title=\"" + tag.Fcount.ToString().Trim() + "\">" + tag.Tagname.ToString().Trim() + "</a></li>\r\n");

                    }	//end loop

                    templateBuilder.Append("			</ul>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if


            }
            else
            {


                templateBuilder.Append("<div class=\"box message\">\r\n");
                templateBuilder.Append("	<h1>������" + page_err.ToString() + "������</h1>\r\n");
                templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");
                templateBuilder.Append("	<p class=\"errorback\">\r\n");
                templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("			if(" + msgbox_showbacklink.ToString() + ")\r\n");
                templateBuilder.Append("			{\r\n");
                templateBuilder.Append("				document.write(\"<a href=\\\"" + msgbox_backlink.ToString() + "\\\">������һ��</a> &nbsp; &nbsp;|&nbsp; &nbsp  \");\r\n");
                templateBuilder.Append("			}\r\n");
                templateBuilder.Append("		</" + "script>\r\n");
                templateBuilder.Append("		<a href=\"forumindex.aspx\">��̳��ҳ</a>\r\n");

                if (usergroupid == 7)
                {

                    templateBuilder.Append("		 &nbsp; &nbsp|&nbsp; &nbsp; <a href=\"register.aspx\">ע��</a>\r\n");

                }	//end if

                templateBuilder.Append("	</p>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("</div>\r\n");



            }	//end if

            templateBuilder.Append("	</div>\r\n");


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

            templateBuilder.Append("	<li class=\"popuser\"><a href=\"" + aspxrewriteurl.ToString() + "\">�ҵ�����</a></li>\r\n");


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
