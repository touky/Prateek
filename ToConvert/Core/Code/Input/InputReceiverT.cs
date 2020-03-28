namespace Mayfair.Core.Code.Input
{
    using System;
    using Mayfair.Core.Code.Input.InputLayers;

    public abstract class InputReceiver<T> : InputService.InputReceiver
        where T : InputLayer, new()
    {
        #region Properties
        public override Type LayerType
        {
            get { return typeof(T); }
        }
        #endregion

        #region Class Methods
        public override InputLayer GetNewLayerInstance()
        {
            return new T();
        }
        #endregion

        ~InputReceiver()
        {
            Enabled = false;
        }
    }
}
