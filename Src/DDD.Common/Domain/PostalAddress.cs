﻿using System.Collections.Generic;
using EnsureThat;

namespace DDD.Common.Domain
{
    using Core.Domain;

    public class PostalAddress : ValueObject, IStateObjectConvertible<PostalAddressState>
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

        #endregion Constructors

        #region Properties

        public string BoxNumber { get; }

        public string City { get; }

        public Alpha2CountryCode CountryCode { get; }

        public string HouseNumber { get; }

        public string PostalCode { get; }

        public string Street { get; }

        #endregion Properties

        #region Methods

        public static PostalAddress FromState(PostalAddressState state)
        {
            if (state == null) return null;
            return CreateIfNotEmpty
            (
                state.Street,
                state.City,
                state.PostalCode,
                Alpha2CountryCode.CreateIfNotEmpty(state.CountryCode),
                state.HouseNumber,
                state.BoxNumber
            );
        }

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
        public PostalAddressState ToState()
        {
            return new PostalAddressState
            {
                Street = this.Street,
                HouseNumber = this.HouseNumber,
                BoxNumber = this.BoxNumber,
                PostalCode = this.PostalCode,
                City = this.City,
                CountryCode = this.CountryCode?.Value
            };
        }

        public override string ToString()
            => $"{GetType().Name} [{nameof(Street)}={Street}, {nameof(HouseNumber)}={HouseNumber}, {nameof(BoxNumber)}{BoxNumber}, {nameof(PostalCode)}={PostalCode}, {nameof(City)}={City}, {nameof(CountryCode)}={CountryCode}]";

        #endregion Methods

    }
}