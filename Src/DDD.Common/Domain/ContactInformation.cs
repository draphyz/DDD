using System;
using System.Collections.Generic;

namespace DDD.Common.Domain
{
    using Core.Domain;

    public class ContactInformation : ValueObject, IStateObjectConvertible<ContactInformationState>
    {

        #region Constructors

        public ContactInformation(PostalAddress postalAddress = null,
                                  string primaryTelephoneNumber = null,
                                  string secondaryTelephoneNumber = null,
                                  string faxNumber = null,
                                  EmailAddress primaryEmailAddress = null,
                                  EmailAddress secondaryEmailAddress = null,
                                  Uri webSite = null)
        {
            if (IsEmpty(postalAddress,
                        primaryTelephoneNumber,
                        SecondaryTelephoneNumber,
                        faxNumber,
                        primaryEmailAddress,
                        secondaryEmailAddress,
                        webSite))
                throw new ArgumentException("You must a least specify one contact information.");
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

        public static ContactInformation FromState(ContactInformationState state)
        {
            if (state == null) return null;
            return CreateIfNotEmpty
            (
                PostalAddress.FromState(state.PostalAddress),
                state.PrimaryTelephoneNumber,
                state.SecondaryTelephoneNumber,
                state.FaxNumber,
                EmailAddress.CreateIfNotEmpty(state.PrimaryEmailAddress),
                EmailAddress.CreateIfNotEmpty(state.SecondaryEmailAddress),
                string.IsNullOrWhiteSpace(state.WebSite) ? null : new Uri(state.WebSite)
            );
        }

        public static ContactInformation CreateIfNotEmpty(PostalAddress postalAddress = null,
                                                          string primaryTelephoneNumber = null,
                                                          string secondaryTelephoneNumber = null,
                                                          string faxNumber = null,
                                                          EmailAddress primaryEmailAddress = null,
                                                          EmailAddress secondaryEmailAddress = null,
                                                          Uri webSite = null)
        {
            if (IsEmpty(postalAddress,
                        primaryTelephoneNumber,
                        secondaryTelephoneNumber,
                        faxNumber,
                        primaryEmailAddress,
                        secondaryEmailAddress,
                        webSite))
                return null;
            return new ContactInformation(postalAddress,
                                          primaryTelephoneNumber,
                                          secondaryTelephoneNumber,
                                          faxNumber,
                                          primaryEmailAddress,
                                          secondaryEmailAddress,
                                          webSite);
        }

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

        public ContactInformationState ToState()
        {
            return new ContactInformationState
            {
                PostalAddress = this.PostalAddress == null ? new PostalAddressState() : this.PostalAddress.ToState(),
                PrimaryTelephoneNumber = this.PrimaryTelephoneNumber,
                SecondaryTelephoneNumber = this.SecondaryTelephoneNumber,
                FaxNumber = this.FaxNumber,
                PrimaryEmailAddress = this.PrimaryEmailAddress?.Value,
                SecondaryEmailAddress = this.SecondaryEmailAddress?.Value,
                WebSite = this.WebSite?.AbsoluteUri
            };
        }
        public override string ToString()
        {
            var format = "{0} [postalAddress={1}, primaryTelephoneNumber={2}, secondaryTelephoneNumber={3}, faxNumber={4}, primaryEmailAddress={5}, secondaryEmailAddress={6}, webSite={7}]";
            return string.Format(format, this.GetType().Name, this.PostalAddress, this.PrimaryTelephoneNumber, this.SecondaryTelephoneNumber, this.FaxNumber, this.PrimaryEmailAddress, this.SecondaryEmailAddress, this.WebSite);
        }

        private static bool IsEmpty(PostalAddress postalAddress = null,
                                    string primaryTelephoneNumber = null,
                                    string secondaryTelephoneNumber = null,
                                    string faxNumber = null,
                                    EmailAddress primaryEmailAddress = null,
                                    EmailAddress secondaryEmailAddress = null,
                                    Uri webSite = null)
        {
            if (postalAddress != null) return false;
            if (!string.IsNullOrWhiteSpace(primaryTelephoneNumber)) return false;
            if (!string.IsNullOrWhiteSpace(secondaryTelephoneNumber)) return false;
            if (!string.IsNullOrWhiteSpace(faxNumber)) return false;
            if (primaryEmailAddress != null) return false;
            if (secondaryEmailAddress != null) return false;
            if (webSite != null) return false;
            return true;
        }

        #endregion Methods

    }
}
