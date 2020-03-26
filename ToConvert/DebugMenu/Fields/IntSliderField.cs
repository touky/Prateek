namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    using UnityEngine;

    /// <summary>
    ///     An int slider field, draws a slider with the given limits
    /// </summary>
    public class IntSliderField : FloatSliderField
    {
        #region Constructors
        public IntSliderField(string title, Vector2Int minMax, int initialValue = 0) : base(title, minMax, initialValue)
        {
            format = string.Empty;

            RefreshMinMaxContent();
        }
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            base.OnGUI(context);

            Value = Mathf.RoundToInt(Value);
        }
        #endregion
    }
}
