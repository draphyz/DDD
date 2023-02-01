using System.Threading;
using System.Threading.Tasks;
using NHibernate;

namespace DDD.Core.Infrastructure.Data
{
    using Domain;

    /// <summary>
    /// Defines a factory for creating instances of Nhibernate sessions associated with a specific bounded context.
    /// </summary>
    public interface ISessionFactory<TContext> where TContext : BoundedContext
    {

        #region Properties

        TContext Context { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a new instance of NHibernate session.
        /// </summary>
        ISession CreateSession();

        /// <summary>
        /// Creates asyncronously a new instance of NHibernate session.
        /// </summary>
        Task<ISession> CreateSessionAsync(CancellationToken cancellationToken = default);

        #endregion Methods
    }
}
