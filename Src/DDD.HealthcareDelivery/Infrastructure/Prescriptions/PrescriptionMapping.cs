using NHibernate;
using ByCode = NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Common.Infrastructure.Data;
    using Domain.Prescriptions;
    using Domain.Practitioners;


    internal abstract class PrescriptionMapping<TPractitionerLicenseNumber, TSocialSecurityNumber, TSex> 
        : ClassMapping<Prescription>
        where TPractitionerLicenseNumber : HealthcarePractitionerLicenseNumber
        where TSocialSecurityNumber : SocialSecurityNumber
        where TSex : Sex
    {

        #region Constructors

        protected PrescriptionMapping()
        {
            this.Lazy(false);
            this.SchemaAction(ByCode.SchemaAction.None);
            // Table
            this.Table("Prescription");
            // Keys
            this.ComponentAsId(p => p.Identifier, m1 => 
            m1.Property(i => i.Value, m2 =>
            {
                m2.Column("PrescriptionId");
            }));
            // Fields
            this.Discriminator(m =>
            {
                m.Column("PrescriptionType");
                m.Length(5);
                m.NotNullable(true);
            });
            this.Property(p => p.Status, m =>
            {
                m.Type(new EnumerationCodeType<PrescriptionStatus>());
                m.Length(3);
                m.NotNullable(true);
                m.Column(m1 => m1.SqlType("char(3)"));
            });
            this.Component(p => p.LanguageCode, m1 =>
            m1.Property(l => l.Value, m2 =>
            {
                m2.Column(m3 =>
                {
                    m3.Name("Language");
                    m3.SqlType("char(2)");
                });
                m2.Type(NHibernateUtil.AnsiString);
                m2.Length(2);
                m2.NotNullable(true);
            }));
            this.Property(p => p.CreatedOn, m =>
            {
                m.Type(NHibernateUtil.Date);
                m.NotNullable(true);
            });
            this.Property(p => p.DeliverableAt, m => m.Type(NHibernateUtil.Date));
            // Prescriber
            this.Property(p => p.Prescriber, m =>
            {
                m.Type<HealthcarePractitionerType<TPractitionerLicenseNumber, TSocialSecurityNumber>>();
                m.Columns
                (m1 =>
                {
                    m1.Name("PrescriberId");
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name("PrescriberType");
                    m1.Length(20);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name("PrescriberLastName");
                    m1.Length(50);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name("PrescriberFirstName");
                    m1.Length(50);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name("PrescriberDisplayName");
                    m1.Length(100);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name("PrescriberLicenseNum");
                    m1.Length(25);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name("PrescriberSSN");
                    m1.Length(25);
                },
                m1 =>
                {
                    m1.Name("PrescriberSpeciality");
                    m1.Length(50);
                },
                m1 =>
                {
                    m1.Name("PrescriberPhone1");
                    m1.Length(20);
                },
                m1 =>
                {
                    m1.Name("PrescriberPhone2");
                    m1.Length(20);
                },
                m1 =>
                {
                    m1.Name("PrescriberEmail1");
                    m1.Length(50);
                },
                m1 =>
                {
                    m1.Name("PrescriberEmail2");
                    m1.Length(50);
                },
                m1 =>
                {
                    m1.Name("PrescriberWebSite");
                    m1.Length(255);
                },
                m1 =>
                {
                    m1.Name("PrescriberStreet");
                    m1.Length(50);
                },
                m1 =>
                {
                    m1.Name("PrescriberHouseNum");
                    m1.Length(10);
                },
                m1 =>
                {
                    m1.Name("PrescriberBoxNum");
                    m1.Length(10);
                },
                m1 =>
                {
                    m1.Name("PrescriberPostCode");
                    m1.Length(10);
                },
                m1 =>
                {
                    m1.Name("PrescriberCity");
                    m1.Length(50);
                },
                m1 =>
                {
                    m1.Name("PrescriberCountry");
                    m1.Length(2);
                    m1.SqlType("char(2)");
                });
            });
            // Patient
            this.Component(p => p.Patient, m1 =>
            {
                m1.Property(p => p.Identifier, m2 =>
                {
                    m2.Column("PatientId");
                    m2.NotNullable(true);
                });
                m1.Component(p => p.FullName, m2 =>
                {
                    m2.Property(n => n.FirstName, m3 =>
                    {
                        m3.Column("PatientFirstName");
                        m3.Type(NHibernateUtil.AnsiString);
                        m3.Length(50);
                        m3.NotNullable(true);
                    });
                    m2.Property(n => n.LastName, m3 =>
                    {
                        m3.Column("PatientLastName");
                        m3.Type(NHibernateUtil.AnsiString);
                        m3.Length(50);
                        m3.NotNullable(true);
                    });
                });
                m1.Property(p => p.Sex, m3 =>
                {
                    m3.Column("PatientSex");
                    m3.Type(new EnumerationCodeType<TSex>());
                    m3.Length(2);
                    m3.NotNullable(true);
                });
                m1.Component(p => p.SocialSecurityNumber, m2 =>
                {
                    m2.Class<TSocialSecurityNumber>();
                    m2.Property(n => n.Value, m3 =>
                    {
                        m3.Column("PatientSSN");
                        m3.Type(NHibernateUtil.AnsiString);
                        m3.Length(25);
                    });
                });
                m1.Property(p => p.Birthdate, m2 =>
                {
                    m2.Column("PatientBirthdate");
                    m2.Type(NHibernateUtil.Date);
                });
            });
            // Encounter
            this.Component(p => p.EncounterIdentifier, m1 =>
            m1.Property(i => i.Value, m2 =>
            {
                m2.Column("EncounterId");
            }));
        }

        #endregion Constructors

    }
}
