using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Core.Infrastructure.Data;

    public class PostalAddressType : CompositeUserType<PostalAddress>
    {
        #region Constructors

        public PostalAddressType()
        {
            this.Mutable(false);
            this.Property(a => a.Street, NHibernateUtil.AnsiString);
            this.Property(a => a.HouseNumber, NHibernateUtil.AnsiString);
            this.Property(a => a.BoxNumber, NHibernateUtil.AnsiString);
            this.Property(a => a.PostalCode, NHibernateUtil.AnsiString);
            this.Property(a => a.City, NHibernateUtil.AnsiString);
            this.Property(a => a.CountryCode, NHibernateUtil.AnsiString);
        }

        #endregion Constructors

        //        this.Property(p => p.Prescriber.ContactInformation.PostalAddress.Street)
        //            .HasColumnName(ToCasingConvention("PrescriberStreet"))
        //            .HasColumnOrder(20)
        //            .IsUnicode(false)
        //            .HasMaxLength(50);
        //        this.Property(p => p.Prescriber.ContactInformation.PostalAddress.HouseNumber)
        //            .HasColumnName(ToCasingConvention("PrescriberHouseNum"))
        //            .HasColumnOrder(21)
        //            .IsUnicode(false)
        //            .HasMaxLength(10);
        //        this.Property(p => p.Prescriber.ContactInformation.PostalAddress.BoxNumber)
        //            .HasColumnName(ToCasingConvention("PrescriberBoxNum"))
        //            .HasColumnOrder(22)
        //            .IsUnicode(false)
        //            .HasMaxLength(10);
        //        this.Property(p => p.Prescriber.ContactInformation.PostalAddress.PostalCode)
        //            .HasColumnName(ToCasingConvention("PrescriberPostCode"))
        //            .HasColumnOrder(23)
        //            .IsUnicode(false)
        //            .HasMaxLength(10);
        //        this.Property(p => p.Prescriber.ContactInformation.PostalAddress.City)
        //            .HasColumnName(ToCasingConvention("PrescriberCity"))
        //            .HasColumnOrder(24)
        //            .IsUnicode(false)
        //            .HasMaxLength(50);
        //        this.Property(p => p.Prescriber.ContactInformation.PostalAddress.CountryCode)
        //            .HasColumnName(ToCasingConvention("PrescriberCountry"))
        //            .HasColumnOrder(25)
        //            .IsUnicode(false)
        //            .HasMaxLength(2)
        //            .IsFixedLength();
    }
}
