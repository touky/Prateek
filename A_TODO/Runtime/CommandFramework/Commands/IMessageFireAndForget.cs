namespace Prateek.A_TODO.Runtime.CommandFramework.Commands
{
    public interface IMessageFireAndForget<T0>
    {
        #region Class Methods
        void Initialize(T0 p0);
        #endregion
    }

    public interface IMessageFireAndForget<T0, T1>
    {
        #region Class Methods
        void Initialize(T0 p0, T1 p1);
        #endregion
    }

    public interface IMessageFireAndForget<T0, T1, T2>
    {
        #region Class Methods
        void Initialize(T0 p0, T1 p1, T2 p2);
        #endregion
    }

    public interface IMessageFireAndForget<T0, T1, T2, T3>
    {
        #region Class Methods
        void Initialize(T0 p0, T1 p1, T2 p2, T3 p3);
        #endregion
    }

    public interface IMessageFireAndForget<T0, T1, T2, T3, T4>
    {
        #region Class Methods
        void Initialize(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4);
        #endregion
    }
}
