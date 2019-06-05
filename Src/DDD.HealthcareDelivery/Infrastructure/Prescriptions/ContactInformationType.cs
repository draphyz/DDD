using NHibernate;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Core.Infrastructure.Data;

    public class ContactInformationType : CompositeUserType<ContactInformation>
    {

        public ContactInformationType()
        {
            this.Mutable(false);
            this.Property(i => i.PrimaryTelephoneNumber, NHibernateUtil.AnsiString);
            this.Property(i => i.SecondaryTelephoneNumber, NHibernateUtil.AnsiString);
            //this.Property(i => i.PrimaryEmailAddress, NHibernateUtil.AnsiString);
            //this.Property(i => i.SecondaryEmailAddress, NHibernateUtil.AnsiString);
            //this.Property(i => i.WebSite, NHibernateUtil.AnsiString);
        }


        //        this.Property(p => p.Prescriber.ContactInformation.PrimaryTelephoneNumber)
        //            .HasColumnName(ToCasingConvention("PrescriberPhone1"))
        //            .HasColumnOrder(15)
        //            .IsUnicode(false)
        //            .HasMaxLength(20);
        //        this.Property(p => p.Prescriber.ContactInformation.SecondaryTelephoneNumber)
        //            .HasColumnName(ToCasingConvention("PrescriberPhone2"))
        //            .HasColumnOrder(16)
        //            .IsUnicode(false)
        //            .HasMaxLength(20);
        //        this.Property(p => p.Prescriber.ContactInformation.PrimaryEmailAddress)
        //            .HasColumnName(ToCasingConvention("PrescriberEmail1"))
        //            .HasColumnOrder(17)
        //            .IsUnicode(false)
        //            .HasMaxLength(50);
        //        this.Property(p => p.Prescriber.ContactInformation.SecondaryEmailAddress)
        //            .HasColumnName(ToCasingConvention("PrescriberEmail2"))
        //            .HasColumnOrder(18)
        //            .IsUnicode(false)
        //            .HasMaxLength(50);
        //        this.Property(p => p.Prescriber.ContactInformation.WebSite)
        //            .HasColumnName(ToCasingConvention("PrescriberWebSite"))
        //            .HasColumnOrder(19)
        //            .IsUnicode(false)
        //            .HasMaxLength(255);

    }
}
