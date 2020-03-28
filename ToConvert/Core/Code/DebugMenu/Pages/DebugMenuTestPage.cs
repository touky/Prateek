namespace Mayfair.Core.Code.DebugMenu.Pages
{
    using Mayfair.Core.Code.DebugMenu.Fields;
    using UnityEngine;

    /// <summary>
    /// The only point of this class is to serve as testing grounds
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

            this.container.Draw(context, "Debug page");
            if (this.container.ShowContent)
            {
                this.testTitle.Draw(context);
                if (this.testTitle.Visible)
                {
                    this.titleField.Draw(context);
                    if (this.titleField.ShowContent)
                    {
                        this.titleEntry.Draw(context, "This title is working");
                    }
                }

                this.testCategory.Draw(context);
                if (this.testCategory.Visible)
                {
                    this.categoryField.Draw(context);
                    if (this.categoryField.ShowContent)
                    {
                        this.categoryEntry.Draw(context, "This category is working");
                    }
                }

                this.testToggle.Draw(context);
                if (this.testToggle.Visible)
                {
                    this.toggle.Draw(context);
                    if (this.toggle.Toggled)
                    {
                        this.toggleEntry.Draw(context, "This toggle is ON");
                    }
                }

                this.testFloatSlider.Draw(context);
                if (this.testFloatSlider.Visible)
                {
                    this.floatSlider.Draw(context);
                }

                this.testIntSlider.Draw(context);
                if (this.testIntSlider.Visible)
                {
                    this.intSlider.Draw(context);
                }

                this.showTextureDebug.Draw(context);
                if (this.showTextureDebug.Visible)
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
                get { return this.visible; }
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
                this.visible = GUILayout.Toggle(this.visible, this.text);
            }
            #endregion
        }
        #endregion
    }
}
