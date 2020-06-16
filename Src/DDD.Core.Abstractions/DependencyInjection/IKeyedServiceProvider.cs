namespace DDD.DependencyInjection
{
    public interface IKeyedServiceProvider<in TKey, out TService>
        where TService : class
    {

        #region Methods

        TService GetService(TKey key);

        #endregion Methods

    }
}
