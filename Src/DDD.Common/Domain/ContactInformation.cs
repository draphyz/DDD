using System;
using System.Collections.Generic;

namespace DDD.Common.Domain
{
    using Core.Domain;

    public class ContactInformation : ValueObject
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
                throw new ArgumentException("You must a least specify a contact information.");
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

        public override string ToString()
        {
            var s = $"{GetType().Name} [{nameof(PostalAddress)}={PostalAddress}, {nameof(PrimaryTelephoneNumber)}={PrimaryTelephoneNumber}, {nameof(SecondaryTelephoneNumber)}={SecondaryTelephoneNumber}, ";
            s += $"{nameof(FaxNumber)}={FaxNumber}, {nameof(PrimaryEmailAddress)}={PrimaryEmailAddress}, {nameof(SecondaryEmailAddress)}={SecondaryEmailAddress}, {nameof(WebSite)}={WebSite}]";
            return s;
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
