using EnsureThat;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DDD.Common.Domain
{
    using Core.Domain;

    public class EmailAddress : ComparableValueObject
    {

        #region Constructors

        public EmailAddress(string value)
        {
            Ensure.That(value, nameof(value)).IsNotNullOrWhiteSpace();
            Ensure.That(value, nameof(value)).Matches("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
            this.Value = value;
        }

        #endregion Constructors

        #region Properties

        public string Value { get; }

        #endregion Properties

        #region Methods

        public static EmailAddress CreateIfNotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return new EmailAddress(value);
        }

        public static EmailAddress FromParts(string username, string domain)
        {
            Ensure.That(username, nameof(username)).IsNotNullOrWhiteSpace();
            Ensure.That(domain, nameof(domain)).IsNotNullOrWhiteSpace();
            return new EmailAddress($"{username}@{domain}");
        }

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Value;
        }

        public string Domain() => this.Parts().Last();

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Value.ToLower();
        }

        public IEnumerable<string> Parts() => this.Value.Split('@');

        public override string ToString() => $"{this.GetType().Name} [value={this.Value}]";

        public string Username() => this.Parts().First();

        public EmailAddress WithDomain(string domain)
        {
            Ensure.That(domain, nameof(domain)).IsNotNullOrWhiteSpace();
            return new EmailAddress($"{this.Username()}@{domain}");
        }

        public EmailAddress WithUsername(string username)
        {
            Ensure.That(username, nameof(username)).IsNotNullOrWhiteSpace();
            return new EmailAddress($"{username}@{this.Domain()}");
        }

        #endregion Methods

    }
}