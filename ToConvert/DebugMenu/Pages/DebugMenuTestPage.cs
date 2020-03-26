namespace Assets.Prateek.ToConvert.DebugMenu.Pages
{
    using Assets.Prateek.ToConvert.DebugMenu.Fields;
    using UnityEngine;

    /// <summary>
    ///     The only point of this class is to serve as testing grounds
    /// </summary>
    public class DebugMenuTestPage : DebugMenuPage
    {
        #region Fields
        private CategoryField container = new CategoryField();

        private TestField testTitle = new TestField("Test title");
        private TitleField titleField = new TitleField();
        private LabelField titleEntry = new LabelField();

        private TestField testCategory = new TestField("Test category");
        private CategoryField categoryField = new CategoryField();
        private LabelField categoryEntry = new LabelField();

        private TestField testToggle = new TestField("Test toggle");
        private ToggleField toggle = new ToggleField("This test toggle");
        private LabelField toggleEntry = new LabelField();

        private TestField testFloatSlider = new TestField("Test float slider");
        private FloatSliderField floatSlider = new FloatSliderField("Float slider", new Vector2(-10, 10));

        private TestField testIntSlider = new TestField("Test int slider");
        private IntSliderField intSlider = new IntSliderField("Int slider", new Vector2Int(-10, 10));

        private TestField showTextureDebug = new TestField("Show texture debug");
        #endregion

        #region Constructors
        public DebugMenuTestPage(string title) : base(title) { }
        #endregion

        #region Class Methods
        public override void Draw(DebugMenuContext context)
        {
            base.Draw(context);

            container.Draw(context, "Debug page");
            if (container.ShowContent)
            {
                testTitle.Draw(context);
                if (testTitle.Visible)
                {
                    titleField.Draw(context);
                    if (titleField.ShowContent)
                    {
                        titleEntry.Draw(context, "This title is working");
                    }
                }

                testCategory.Draw(context);
                if (testCategory.Visible)
                {
                    categoryField.Draw(context);
                    if (categoryField.ShowContent)
                    {
                        categoryEntry.Draw(context, "This category is working");
                    }
                }

                testToggle.Draw(context);
                if (testToggle.Visible)
                {
                    toggle.Draw(context);
                    if (toggle.Toggled)
                    {
                        toggleEntry.Draw(context, "This toggle is ON");
                    }
                }

                testFloatSlider.Draw(context);
                if (testFloatSlider.Visible)
                {
                    floatSlider.Draw(context);
                }

                testIntSlider.Draw(context);
                if (testIntSlider.Visible)
                {
                    intSlider.Draw(context);
                }

                showTextureDebug.Draw(context);
                if (showTextureDebug.Visible)
                {
                    DebugMenuContext.DrawTextureDebug();

                    //test = GUILayout.HorizontalSlider(test, 0, 10);
                    //test = GUILayout.HorizontalSlider(test, 0, 10, this.context.Sliders[0], this.context.Sliders[1]);

                    //DebugMenuContext.DrawStyleDebug(this.context.Sliders[1]);
                }
            }
        }
        #endregion

        #region Nested type: TestField
        private class TestField : DebugField
        {
            #region Fields
            private bool visible = false;
            private string text;
            #endregion

            #region Properties
            public bool Visible
            {
                get { return visible; }
            }
            #endregion

            #region Constructors
            public TestField(string text)
            {
                this.text = text;
            }
            #endregion

            #region Unity EditorOnly Methods
            public override void OnGUI(DebugMenuContext context)
            {
                visible = GUILayout.Toggle(visible, text);
            }
            #endregion
        }
        #endregion
    }
}
