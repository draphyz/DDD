using System;
using System.Collections.Generic;

namespace DDD.Common.Domain
{
    using Core.Domain;

    public class ContactInformation : ValueObject
    {

        #region Constructors

        public ContactInformation(PostalAddress postalAddress,
                                  string primaryTelephoneNumber,
                                  string secondaryTelephoneNumber,
                                  string faxNumber,
                                  EmailAddress primaryEmailAddress,
                                  EmailAddress secondaryEmailAddress,
                                  Uri webSite)
        {
            this.PostalAddress = postalAddress;
            if (!string.IsNullOrWhiteSpace(primaryTelephoneNumber))
                this.PrimaryTelephoneNumber = primaryTelephoneNumber;
            if (!string.IsNullOrWhiteSpace(secondaryTelephoneNumber))
                this.SecondaryTelephoneNumber = secondaryTelephoneNumber;
            if (!string.IsNullOrWhiteSpace(faxNumber))
                this.FaxNumber = faxNumber;
            this.PrimaryEmailAddress = primaryEmailAddress;
            this.SecondaryEmailAddress = secondaryEmailAddress;
            this.WebSite = webSite;
        }

        #endregion Constructors

        #region Properties

        public string FaxNumber { get; }

        public PostalAddress PostalAddress { get; }

        public EmailAddress PrimaryEmailAddress { get; }

        public string PrimaryTelephoneNumber { get; }

        public EmailAddress SecondaryEmailAddress { get; }

        public string SecondaryTelephoneNumber { get; }

        public Uri WebSite { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.PostalAddress;
            yield return this.PrimaryTelephoneNumber;
            yield return this.SecondaryTelephoneNumber;
            yield return this.FaxNumber;
            yield return this.PrimaryEmailAddress;
            yield return this.SecondaryEmailAddress;
            yield return this.WebSite;
        }

        public override string ToString()
        {
            var format = "{0} [postalAddress={1}, primaryTelephoneNumber={2}, secondaryTelephoneNumber={3}, faxNumber={4}, primaryEmailAddress={5}, secondaryEmailAddress={6}, webSite={7}]";
            return string.Format(format, this.GetType().Name, this.PostalAddress, this.PrimaryTelephoneNumber, this.SecondaryTelephoneNumber, this.FaxNumber, this.PrimaryEmailAddress, this.SecondaryEmailAddress, this.WebSite);
        }

        #endregion Methods

    }
}
