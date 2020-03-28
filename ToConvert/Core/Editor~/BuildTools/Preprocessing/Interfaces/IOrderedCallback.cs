namespace Mayfair.Core.Editor.BuildTools.Preprocessing.Interfaces
{
    public interface IOrderedCallback
    {
        /// <summary>
        /// Used to determine the priority order of calls to the implementing class.
        /// THe lower value instances are called first.
        /// </summary>
        int CallbackOrder { get; }
    }
}