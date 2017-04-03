using System;
using System.Text;
using Discuz.Forum.ScheduledEvents;
using Discuz.Forum;

namespace Discuz.Event
{
    /// <summary>
    /// �йر�ǩ�ļƻ�����
    /// </summary>
    public class TagsEvent : IEvent
    {
        #region IEvent ��Ա

        public void Execute(object state)
        {
            ForumTags.WriteHotTagsListForForumCacheFile(60);
            ForumTags.WriteHotTagsListForForumJSONPCacheFile(60);
        }

        #endregion
    }
}
