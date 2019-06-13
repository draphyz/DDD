using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Common.Infrastructure.Data;
    using Domain.Prescriptions;
    using Domain.Practitioners;
    using Domain.Facilities;

    internal abstract class PrescriptionMapping<TPractitionerLicenseNumber, TFacilityLicenseNumber, TSocialSecurityNumber, TSex> 
        : ClassMapping<Prescription>
        where TPractitionerLicenseNumber : HealthcarePractitionerLicenseNumber
        where TFacilityLicenseNumber : HealthFacilityLicenseNumber
        where TSocialSecurityNumber : SocialSecurityNumber
        where TSex : Sex
    {

        #region Fields

        private readonly bool useUpperCase;

        #endregion Fields

        #region Constructors

        public PrescriptionMapping(bool useUpperCase)
        {
            this.useUpperCase = useUpperCase;
            this.Lazy(false);
            // Table
            this.Table(ToCasingConvention("Prescription"));
            // Keys
            this.ComponentAsId(p => p.Identifier, m1 =>
            m1.Property(i => i.Value, m2 =>
            {
                m2.Column(ToCasingConvention("PrescriptionId"));
            }));
            // Fields
            this.Discriminator(m =>
            {
                m.Column(ToCasingConvention("PrescriptionType"));
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
                    m3.Name(ToCasingConvention("Language"));
                    m3.SqlType("char(2)");
                });
                m2.Type(NHibernateUtil.AnsiString);
                m2.Length(2);
                m2.NotNullable(true);
            }));

            this.Property(p => p.CreatedOn, m => m.Type(NHibernateUtil.DateTimeNoMs));
            this.Property(p => p.DelivrableAt, m => m.Type(NHibernateUtil.Date));
            // Prescriber
            this.Property(p => p.Prescriber, m =>
            {
                m.Type<HealthcarePractitionerType<TPractitionerLicenseNumber, TSocialSecurityNumber>>();
                m.Columns
                (m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberId"));
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberType"));
                    m1.Length(20);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberLastName"));
                    m1.Length(50);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberFirstName"));
                    m1.Length(50);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberDisplayName"));
                    m1.Length(100);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberLicenseNum"));
                    m1.Length(25);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberSSN"));
                    m1.Length(25);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberSpeciality"));
                    m1.Length(50);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberPhone1"));
                    m1.Length(20);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberPhone2"));
                    m1.Length(20);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberEmail1"));
                    m1.Length(50);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberEmail2"));
                    m1.Length(50);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberWebSite"));
                    m1.Length(255);
                    m1.SqlType("varchar(255)");
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberStreet"));
                    m1.Length(50);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberHouseNum"));
                    m1.Length(10);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberBoxNum"));
                    m1.Length(10);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberPostCode"));
                    m1.Length(10);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberCity"));
                    m1.Length(50);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("PrescriberCountry"));
                    m1.Length(2);
                    m1.SqlType("char(2)");
                });
            });
            // Patient
            this.Component(p => p.Patient, m1 =>
            {
                m1.Property(p => p.Identifier, m2 => m2.Column(ToCasingConvention("PatientId")));
                m1.Component(p => p.FullName, m2 =>
                {
                    m2.Property(n => n.FirstName, m3 =>
                    {
                        m3.Column(ToCasingConvention("PatientFirstName"));
                        m3.Type(NHibernateUtil.AnsiString);
                        m3.Length(50);
                        m3.NotNullable(true);
                    });
                    m2.Property(n => n.LastName, m3 =>
                    {
                        m3.Column(ToCasingConvention("PatientLastName"));
                        m3.Type(NHibernateUtil.AnsiString);
                        m3.Length(50);
                        m3.NotNullable(true);
                    });
                });
                m1.Property(p => p.Sex, m3 =>
                {
                    m3.Column(ToCasingConvention("PatientSex"));
                    m3.Type(new EnumerationCodeType<TSex>());
                    m3.Length(2);
                    m3.NotNullable(true);
                });
                m1.Component(p => p.SocialSecurityNumber, m2 =>
                {
                    m2.Class<TSocialSecurityNumber>();
                    m2.Property(n => n.Value, m3 =>
                    {
                        m3.Column(ToCasingConvention("PatientSSN"));
                        m3.Type(NHibernateUtil.AnsiString);
                        m3.Length(25);
                    });
                });
                m1.Property(p => p.Birthdate, m2 =>
                {
                    m2.Column(ToCasingConvention("PatientBirthdate"));
                    m2.Type(NHibernateUtil.Date);
                });
            });
            // Facility
            this.Property(p => p.HealthFacility, m =>
            {
                m.Type<HealthFacilityType<TFacilityLicenseNumber>>();
                m.Columns
                (m1 =>
                {
                    m1.Name(ToCasingConvention("FacilityId"));
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("FacilityType"));
                    m1.Length(20);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("FacilityName"));
                    m1.Length(100);
                    m1.NotNullable(true);
                },
                m1 =>
                {
                    m1.Name(ToCasingConvention("FacilityLicenseNum"));
                    m1.Length(25);
                });
            });
        }

        #endregion Constructors

        #region Methods

        protected string ToCasingConvention(string name) => this.useUpperCase ? name.ToUpperInvariant() : name;

        #endregion Methods

    }
}
