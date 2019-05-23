using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Base class for entities of the Domain Model.
    /// </summary>
    public abstract class DomainEntity : IEquatable<DomainEntity>
    {

        #region Fields

        private readonly List<IDomainEvent> events = new List<IDomainEvent>();

        #endregion Fields

        #region Constructors

        protected DomainEntity(IEnumerable<IDomainEvent> events = null)
        {
            if (events != null)
                this.events.AddRange(events);
        }

        #endregion Constructors

        #region Methods

        public static bool operator !=(DomainEntity a, DomainEntity b)
        {
            if (ReferenceEquals(a, null)) return !ReferenceEquals(b, null);
            return !a.Equals(b);
        }

        public static bool operator ==(DomainEntity a, DomainEntity b)
        {
            if (ReferenceEquals(a, null)) return ReferenceEquals(b, null);
            return a.Equals(b);
        }

        public virtual IEnumerable<IDomainEvent> AllEvents() => this.events.AsReadOnly();

        public virtual void ClearAllEvents()
        {
            this.events.Clear();
        }

        public bool Equals(DomainEntity other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(other, null)) return false;
            if (this.GetType() != other.GetType()) return false;
            return this.Identity().Equals(other.Identity());
        }

        public override bool Equals(object other) => this.Equals(other as DomainEntity);

        public override int GetHashCode() => this.Identity().GetHashCode();

        public abstract ComparableValueObject Identity();

        public virtual string IdentityAsString()
        {
            return string.Join("/", this.Identity().PrimitiveEqualityComponents());
        }
        protected void AddEvent(IDomainEvent @event)
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            this.events.Add(@event);
        }

        #endregion Methods

    }
}