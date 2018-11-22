using Conditions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace DDD.Common.Domain
{
    using Core.Domain;

    public class EmailAddress : ComparableValueObject
    {

        #region Constructors

        public EmailAddress(string address)
        {
            Condition.Requires(address, nameof(address))
                     .IsNotNullOrWhiteSpace()
                     .Evaluate(a => Regex.IsMatch(a, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"));
            this.Address = address;
        }

        #endregion Constructors

        #region Properties

        public string Address { get; }

        #endregion Properties

        #region Methods

        public static EmailAddress CreateIfNotEmpty(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) return null;
            return new EmailAddress(address);
        }

        public static EmailAddress FromParts(string username, string domain)
        {
            Condition.Requires(username, nameof(username)).IsNotNullOrWhiteSpace();
            Condition.Requires(domain, nameof(domain)).IsNotNullOrWhiteSpace();
            return new EmailAddress($"{username}@{domain}");
        }

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Address;
        }

        public string Domain() => this.Parts().Last();

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Address.ToLower();
        }

        public IEnumerable<string> Parts() => this.Address.Split('@');

        public override string ToString() => $"{this.GetType().Name} [address={this.Address}]";

        public string Username() => this.Parts().First();

        public EmailAddress WithDomain(string domain)
        {
            Condition.Requires(domain, nameof(domain)).IsNotNullOrWhiteSpace();
            return new EmailAddress($"{this.Username()}@{domain}");
        }

        public EmailAddress WithUsername(string username)
        {
            Condition.Requires(username, nameof(username)).IsNotNullOrWhiteSpace();
            return new EmailAddress($"{username}@{this.Domain()}");
        }

        #endregion Methods

    }
}