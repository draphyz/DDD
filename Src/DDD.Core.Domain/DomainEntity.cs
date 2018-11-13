using Conditions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Core.Domain
{
    using Collections;

    /// <summary>
    /// Base class for entities of the Domain Model.
    /// </summary>
    public abstract class DomainEntity : IEquatable<DomainEntity>
    {
        #region Fields

        private readonly List<IDomainEvent> events = new List<IDomainEvent>();

        #endregion Fields

        #region Constructors

        protected DomainEntity(EntityState entityState = EntityState.Added, IEnumerable<IDomainEvent> events = null)
        {
            this.EntityState = entityState;
            if (events != null)
                this.events.AddRange(events);
        }

        #endregion Constructors

        #region Properties

        protected EntityState EntityState { get; private set; }

        #endregion Properties

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
            return this.IdentityComponents().SequenceEqual(other.IdentityComponents());
        }

        public override bool Equals(object other) => this.Equals(other as DomainEntity);

        public override int GetHashCode() => this.IdentityComponents().CombineHashCodes();

        public virtual string IdentityAsString()
        {
            return string.Join("/", this.IdentityComponents().Select(c => c.EqualityComponents().Single()));
        }

        public abstract IEnumerable<ComparableValueObject> IdentityComponents();

        protected void AddEvent(IDomainEvent @event)
        {
            Condition.Requires(@event, nameof(@event)).IsNotNull();
            this.events.Add(@event);
        }

        protected void MarkAsModified()
        {
            if (this.EntityState != EntityState.Added)
                this.EntityState = EntityState.Modified;
        }

        #endregion Methods
    }
}