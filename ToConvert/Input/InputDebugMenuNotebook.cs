namespace Assets.Prateek.ToConvert.Input
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.DebugMenu;
    using Assets.Prateek.ToConvert.DebugMenu.Content;
    using Assets.Prateek.ToConvert.Input.Providers;
    using Assets.Prateek.ToConvert.Reflection;
    using UnityEngine;

    internal class InputDebugMenuNotebook : DebugMenuNotebook
    {
        #region Fields
        private ReflectedField<List<Touch>> touches = "touches";
        private ReflectedField<List<Touch>> fakeTouches = "fakeTouches";
        private ReflectedField<List<InputServiceProvider>> providers = "providers";

        private Texture2D touchCircle;

        private InputService owner;
        #endregion

        #region Constructors
        public InputDebugMenuNotebook(InputService owner, string title) : base(title)
        {
            this.owner = owner;
            touches.Init(this.owner);
            providers.Init(this.owner);

            Init();
        }

        public InputDebugMenuNotebook(InputService owner, string shortTitle, string title) : base(shortTitle, title)
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

        public override void Draw(DebugMenuContext context)
        {
            base.Draw(context);

            foreach (InputServiceProvider provider in providers.Value)
            {
                if (!fakeTouches.TryInit(provider))
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

            foreach (var touch in touches)
            {
                var sizeWH = Mathf.Max(Screen.width, Screen.height);
                var size   = Vector2.one * (sizeWH / 16f) * (isFake ? 1.3f : 1f);
                var pos    = touch.position;
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
