namespace Mayfair.Core.Code.GameAction
{
    using System.Collections.Generic;
    using Prateek.KeynameFramework;
    using Prateek.NoticeFramework.Notices.Core;

    /// <summary>
    /// GameActionMessage is the standard way of sending a notice to signify that a specific action has happened.
    /// It is meant to be a general broadcasting notice, not a communication tool
    /// </summary>
    public class GameActionNotice : BroadcastNotice
    {
        #region Fields
        protected List<Keyname> tags = new List<Keyname>();
        protected float targetValue = 1;
        #endregion

        #region Properties
        public override long NoticeID
        {
            get { return ConvertToId(typeof(GameActionNotice)); }
        }

        public Keyname Verb
        {
            get { return tags.Count == 0 ? (Keyname) string.Empty : tags[0]; }
        }

        public Keyname this[int index]
        {
            get { return index + 1 >= tags.Count ? (Keyname) string.Empty : tags[index + 1]; }
        }

        public List<Keyname> Tags
        {
            get { return tags; }
        }

        public float TargetValue
        {
            get { return targetValue; }
        }
        #endregion

        #region Class Methods
        public void Add(Keyname keyname)
        {
            tags.Add(keyname);
        }
        #endregion
    }
}
