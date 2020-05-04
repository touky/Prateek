namespace Mayfair.Core.Editor.Database.ExcelToDatabaseContent
{
    using System.Collections.Concurrent;
    using System.Text;
    using Mayfair.Core.Code.GUIExt;
    using Mayfair.Plugins.AtDbMayfair.Editor.ErrorSystem;

    public class ErrorGUILogger : GUILogger
    {
        #region Fields
        private readonly StringBuilder stringBuilder = new StringBuilder();
        private ConcurrentQueue<NoticeContainer> notices = new ConcurrentQueue<NoticeContainer>();
        #endregion

        #region Class Methods
        public void PrintNotices(TintPrefix tintError, TintPrefix tintWarning, TintPrefix tintTask)
        {
            while (!notices.IsEmpty)
            {
                if (notices.TryDequeue(out NoticeContainer notice))
                {
                    string message = FormatAndAddLocation(notice);
                    switch (notice.noticeType)
                    {
                        case NoticeType.Default:
                        {
                            Log(message);
                            break;
                        }
                        case NoticeType.Task:
                        {
                            Log(tintTask, message);
                            break;
                        }
                        case NoticeType.Warning:
                        {
                            Log(tintWarning, message);
                            break;
                        }
                        case NoticeType.Error:
                        {
                            Log(tintError, message);
                            break;
                        }
                    }
                }
            }
        }

        public void LogNotices(params IErrorLogger[] loggers)
        {
            foreach (IErrorLogger logger in loggers)
            {
                logger.ErrorLogger.TransferNotices(notices);
            }
        }

        private string FormatAndAddLocation(NoticeContainer notice)
        {
            string format = notice.baseMessage;
            object[] args = notice.args;
            string location = notice.GetLocation();
            stringBuilder.Clear();
            stringBuilder.AppendFormat(format, args);

            stringBuilder.Append(location);
            string item = stringBuilder.ToString();
            return item;
        }
        #endregion
    }
}
