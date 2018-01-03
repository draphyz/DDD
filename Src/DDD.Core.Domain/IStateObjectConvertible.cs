namespace DDD.Core.Domain
{
    public interface IStateObjectConvertible<out TState>
        where TState : class, new()
    {

        #region Methods

        TState ToState();

        #endregion Methods

    }
}