namespace DDD.Common.Domain
{
    public class ContactInformationState
    {

        #region Properties

        public string FaxNumber { get; set; }

        public PostalAddressState PostalAddress { get; set; }

        public string PrimaryEmailAddress { get; set; }

        public string PrimaryTelephoneNumber { get; set; }

        public string SecondaryEmailAddress { get; set; }

        public string SecondaryTelephoneNumber { get; set; }

        public string WebSite { get; set; }

        #endregion Properties

    }
}
