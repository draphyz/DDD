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

        protected ContactInformation() { }

        #endregion Constructors

        #region Properties

        public string FaxNumber { get; private set; }

        public PostalAddress PostalAddress { get; private set; }

        public EmailAddress PrimaryEmailAddress { get; private set; }

        public string PrimaryTelephoneNumber { get; private set; }

        public EmailAddress SecondaryEmailAddress { get; private set; }

        public string SecondaryTelephoneNumber { get; private set; }

        public Uri WebSite { get; private set; }

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
