using EnsureThat;
using System.Collections.Generic;

namespace DDD.Common.Domain
{
    using Core.Domain;

    public class PostalAddress : ValueObject
    {

        #region Constructors

        public PostalAddress(string street,
                             string city,
                             string postalCode = null,
                             Alpha2CountryCode countryCode = null,
                             string houseNumber = null,
                             string boxNumber = null)
        {
            Ensure.That(street, nameof(street)).IsNotNullOrWhiteSpace();
            Ensure.That(city, nameof(city)).IsNotNullOrWhiteSpace();
            this.Street = street.ToTitleCase();
            this.City = city.ToTitleCase();
            if (!string.IsNullOrWhiteSpace(postalCode))
                this.PostalCode = postalCode.ToUpper();
            this.CountryCode = countryCode;
            if (!string.IsNullOrWhiteSpace(houseNumber))
                this.HouseNumber = houseNumber.ToUpper();
            if (!string.IsNullOrWhiteSpace(boxNumber))
                this.BoxNumber = boxNumber.ToUpper();
        }

        protected PostalAddress() { }

        #endregion Constructors

        #region Properties

        public string BoxNumber { get; private set; }

        public string City { get; private set; }

        public Alpha2CountryCode CountryCode { get; private set; }

        public string HouseNumber { get; private set; }

        public string PostalCode { get; private set; }

        public string Street { get; private set; }

        #endregion Properties

        #region Methods

        public static PostalAddress CreateIfNotEmpty(string street,
                                                     string city,
                                                     string postalCode = null,
                                                     Alpha2CountryCode countryCode = null,
                                                     string houseNumber = null,
                                                     string boxNumber = null)
        {
            if (string.IsNullOrWhiteSpace(street)) return null;
            if (string.IsNullOrWhiteSpace(city)) return null;
            return new PostalAddress
            (
                street,
                city,
                postalCode,
                countryCode,
                houseNumber,
                boxNumber
            );
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Street;
            yield return this.City;
            yield return this.PostalCode;
            yield return this.CountryCode;
            yield return this.HouseNumber;
            yield return this.BoxNumber;
        }

        public override string ToString()
            => $"{GetType().Name} [{nameof(Street)}={Street}, {nameof(HouseNumber)}={HouseNumber}, {nameof(BoxNumber)}{BoxNumber}, {nameof(PostalCode)}={PostalCode}, {nameof(City)}={City}, {nameof(CountryCode)}={CountryCode}]";

        #endregion Methods

    }
}