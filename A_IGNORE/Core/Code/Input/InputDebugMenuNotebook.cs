namespace Mayfair.Core.Code.Input
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.GUIExt;
    using Mayfair.Core.Code.Input.Providers;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using UnityEngine;

    internal class InputDebugMenuNotebook : DebugMenuNotebook
    {
        #region Fields
        private ReflectedField<List<Touch>> touches = "touches";
        private ReflectedField<List<Touch>> fakeTouches = "fakeTouches";
        private ReflectedField<List<InputServant>> providers = "providers";

        private Texture2D touchCircle;

        private InputDaemon owner;
        #endregion

        #region Constructors
        public InputDebugMenuNotebook(InputDaemon owner, string title) : base(title)
        {
            this.owner = owner;
            touches.Init(this.owner);
            providers.Init(this.owner);

            Init();
        }

        public InputDebugMenuNotebook(InputDaemon owner, string shortTitle, string title) : base(shortTitle, title)
        {
            this.owner = owner;
            touches.Init(this.owner);
            providers.Init(this.owner);

            Init();
        }
        #endregion

        #region Class Methods
        private void Init()
        {
            if (touchCircle == null)
            {
                touchCircle = Resources.Load<Texture2D>("UI_Debug_TouchCircle");
            }
        }

        public override void OnGUI()
        {
            foreach (InputServant servant in providers.Value)
            {
                if (!fakeTouches.TryInit(servant))
                {
                    continue;
                }

                DrawTouches(fakeTouches.Value, true);
            }

            DrawTouches(touches.Value);
        }

        private void DrawTouches(List<Touch> touches, bool isFake = false)
        {
            Init();

            foreach (Touch touch in touches)
            {
                int sizeWH = Mathf.Max(Screen.width, Screen.height);
                Vector2 size = Vector2.one * (sizeWH / 16f) * (isFake ? 1.3f : 1f);
                Vector2 pos = touch.position;
                pos.y = Screen.height - pos.y;
                pos -= size * 0.5f;
                using (new ColorScope(isFake ? Color.grey : Color.red))
                {
                    GUI.DrawTexture(new Rect(pos, size), touchCircle);
                }
            }
        }
        #endregion
    }
}
