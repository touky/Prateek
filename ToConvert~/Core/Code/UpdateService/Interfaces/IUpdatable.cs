namespace Mayfair.Core.Code.UpdateService.Interfaces
{
    /// <summary>
    /// This should not be used as an example of Service/Provider/Messaging interaction
    /// as it violates the asynchronicity goals of our systems.
    /// </summary>
    public interface IUpdatable
    {
        void OnUpdate();
    }
}