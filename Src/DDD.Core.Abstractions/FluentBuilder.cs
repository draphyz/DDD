using System;
using System.ComponentModel;

namespace DDD
{
    /// <summary>
    /// Base class for building an object in a fluent way.
    /// </summary>
    public abstract class FluentBuilder<T> : IObjectBuilder<T>
        where T : class
    {

        #region Methods

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => base.Equals(obj);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => base.GetHashCode();

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() => base.ToString();

        /// <summary>Gets the <see cref="System.Type"/> of the current instance.</summary>
        /// <returns>The <see cref="System.Type"/> instance that represents the exact runtime
        /// type of the current instance.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType() => base.GetType();

        T IObjectBuilder<T>.Build() => this.Build();

        protected abstract T Build();

        #endregion Methods

    }
}
