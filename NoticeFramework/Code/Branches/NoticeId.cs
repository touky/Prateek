namespace Prateek.NoticeFramework.Branches {
    using System;
    using Prateek.NoticeFramework.Notices.Core;
    using Prateek.NoticeFramework.TransmitterReceiver;

    internal struct NoticeId
    {
        private long Id;
        private Type type;
        private NoticeReceiver noticeReceiver;

        public Type Type
        {
            get { return type; }
        }

        public NoticeId(Type type, NoticeReceiver noticeReceiver)
        {
            this.type = type;
            this.noticeReceiver = noticeReceiver;
            Id = 0;
        }

        public static implicit operator NoticeId(Type type)
        {
            return new NoticeId(type, null);
        }

        public static implicit operator NoticeId(long Id)
        {
            return new NoticeId {Id = Id};
        }

        public long GetValidId()
        {
            if (Id != 0)
            {
                return Id;
            }

            if (type.IsSubclassOf(typeof(ResponseNotice)))
            {
                Id = Notice.ConvertToId(type, noticeReceiver);
            }
            else
            {
                Id = Notice.ConvertToId(type);
            }

            return Id;
        }
    }
}