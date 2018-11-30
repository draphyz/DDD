using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DDD.Threading
{
    /// <summary>
    /// Removes synchronization context within an asynchronous method.
    /// See <a href="https://blogs.msdn.microsoft.com/benwilli/2017/02/09/an-alternative-to-configureawaitfalse-everywhere/">An alternative to ConfigureAwait(false) everywhere</a>.
    /// </summary>
    public class SynchronizationContextRemover : INotifyCompletion
    {

        #region Properties

        public bool IsCompleted => SynchronizationContext.Current == null;

        #endregion Properties

        #region Methods

        public SynchronizationContextRemover GetAwaiter() => this;

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            var previousContext = SynchronizationContext.Current;
            try
            {
                SynchronizationContext.SetSynchronizationContext(null);
                continuation();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        #endregion Methods

    }
}
