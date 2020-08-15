using NHibernate;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DDD.Core.Infrastructure.Data
{
    using NHibernate.Mapping.ByCode;

    /// <remarks>
    ///  This class must be unit tested.
    /// </remarks>
    public abstract class CompositeUserType<TComponent> : ICompositeUserType
    {

        #region Fields

        public const string NotNullDiscriminatorValue = "not null";
        public const string NullDiscriminatorValue = "null";
        private readonly Type returnedClass;
        private readonly Dictionary<Type, ClassMapping> classes;
        private bool isMutable;
        private readonly List<PropertyMapping> properties;
        private string[] propertyNames;
        private IType[] propertyTypes;

        #endregion Fields

        #region Constructors

        protected CompositeUserType()
        {
            this.properties = new List<PropertyMapping>();
            this.classes = new Dictionary<Type, ClassMapping>();
            this.isMutable = true;
            this.returnedClass = typeof(TComponent);
            this.classes[this.returnedClass] = ClassMapping.FromMappedClass(this.returnedClass);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Are objects of this type mutable?
        /// </summary>
        bool ICompositeUserType.IsMutable => this.isMutable;

        /// <summary>
        /// Get the "property names" that may be used in a query.
        /// </summary>
        string[] ICompositeUserType.PropertyNames
        {
            get
            {
                if (this.propertyNames == null)
                {
                    this.propertyNames = this.properties.Select(p => p.Name).ToArray();
                }
                return this.propertyNames;
            }
        }

        /// <summary>
        /// Get the corresponding "property types".
        /// </summary>
        IType[] ICompositeUserType.PropertyTypes
        {
            get
            {
                if (this.propertyTypes == null)
                {
                    this.propertyTypes = this.properties.Select(p => p.PersistentType).ToArray();
                }
                return this.propertyTypes;
            }
        }

        /// <summary>
        /// The class returned by NullSafeGet().
        /// </summary>
        Type ICompositeUserType.ReturnedClass => this.returnedClass;

        int PropertySpan => this.properties.Count;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Reconstruct an object from the cacheable representation.
        /// At the very least this method should perform a deep copy. (optional operation)
        /// </summary>
        object ICompositeUserType.Assemble(object cached, ISessionImplementor session, object owner)
        {
            if (cached == null) return null;
            var values = (object[])cached;
            var assembled = new object[values.Length];
            var mappedClass = this.returnedClass;
            for (var i = 0; i < values.Length; i++)
            {
                assembled[i] = this.AsUserType().PropertyTypes[i].Assemble(values[i], session, owner);
                if (this.properties[i].IsDriscriminator)
                    mappedClass = this.GetMappedClass(assembled[i]);
            }
            var result = this.Instantiate(mappedClass);
            this.SetPropertyValues(result, assembled);
            return result;
        }

        /// <summary>
        /// Return a deep copy of the persistent state, stopping at entities and at collections.
        /// </summary>
        object ICompositeUserType.DeepCopy(object component)
        {
            if (component == null) return null;
            var values = this.GetPropertyValues(component);
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = this.AsUserType().PropertyTypes[i].DeepCopy(values[i], null);
            }
            var result = this.Instantiate(component.GetType());
            this.SetPropertyValues(result, values);
            return result;
        }

        /// <summary>
        /// Transform the object into its cacheable representation.
        /// At the very least this method should perform a deep copy.
        /// That may not be enough for some implementations, method should perform a deep copy. That may not be enough for some implementations, however; for example, associations must be cached as identifier values. (optional operation)
        /// </summary>
        object ICompositeUserType.Disassemble(object component, ISessionImplementor session)
        {
            if (component == null) return null;
            var values = this.GetPropertyValues(component);
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = this.AsUserType().PropertyTypes[i].Disassemble(values[i], session, null);
            }
            return values;
        }

        /// <summary>
        /// Compare two instances of the class mapped by this type for persistence
        /// "equality", ie. equality of persistent state.
        /// </summary>
        bool ICompositeUserType.Equals(object x, object y)    // TODO : change component
        {
            var isEqualFast = IsEqualFast(x, y);
            if (isEqualFast.HasValue)
                return isEqualFast.Value;

            var xvalues = GetPropertyValues(x);
            var yvalues = GetPropertyValues(y);
            for (var i = 0; i < this.PropertySpan; i++)
            {
                if (!propertyTypes[i].IsEqual(xvalues[i], yvalues[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get a hashcode for the instance, consistent with persistence "equality"
        /// </summary>
        int ICompositeUserType.GetHashCode(object x)
        {
            if (this.GetClassMapping(x.GetType()).OverridesGetHashCode)
                return x.GetHashCode();
            return this.GetHashCode(x);
        }

        /// <summary>
        /// Get the value of a property.
        /// </summary>
        object ICompositeUserType.GetPropertyValue(object component, int property)
        {
            if (component == null) return null;
            var propertyMapping = this.properties[property];
            if (propertyMapping.IsDriscriminator)
                return this.GetClassMapping(component.GetType()).DiscriminatorValue;
            if (!propertyMapping.MemberInfo.DeclaringType.IsAssignableFrom(component.GetType())) return null;
            return propertyMapping.MemberInfo.GetPropertyOrFieldValue(component);
        }

        /// <summary>
        /// Retrieve an instance of the mapped class from a DbDataReader. Implementors
        /// should handle possibility of null values.
        /// </summary>
        object ICompositeUserType.NullSafeGet(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
        {
            var value = this.Hydrate(dr, names, session, owner);
            if (value == null) return null;
            var values = (object[])value;
            var mappedClass = this.returnedClass;
            if (this.HasSubComponents())
            {
                var discriminatorColumn = names.First(n => n.StartsWith(this.DiscriminatorName(), StringComparison.OrdinalIgnoreCase));
                var discriminatorValue = dr[discriminatorColumn];
                mappedClass = this.GetMappedClass(discriminatorValue);
            }
            var result = this.Instantiate(mappedClass);
            this.SetPropertyValues(result, values);
            return result;
        }

        /// <summary>
        /// Write an instance of the mapped class to a prepared statement.
        /// Implementors should handle possibility of null values.
        /// A multi-column type should be written to parameters starting from index.
        /// If a property is not settable, skip it and don't increment the index.
        /// </summary>
        void ICompositeUserType.NullSafeSet(DbCommand cmd, object value, int begin, bool[] settable, ISessionImplementor session)
        {
            var values = NullSafeGetValues(value);
            var loc = 0;
            for (var i = 0; i < values.Length; i++)
            {
                var length = this.AsUserType().PropertyTypes[i].GetColumnSpan(session.Factory);
                switch (length)
                {
                    case 0:
                        //noop
                        break;
                    case 1:
                        if (settable[loc])
                        {
                            this.AsUserType().PropertyTypes[i].NullSafeSet(cmd, values[i], begin, session);
                            begin++;
                        }
                        break;
                    default:
                        var subsettable = new bool[length];
                        Array.Copy(settable, loc, subsettable, 0, length);
                        this.AsUserType().PropertyTypes[i].NullSafeSet(cmd, values[i], begin, subsettable, session);
                        begin += ArrayHelper.CountTrue(subsettable);
                        break;
                }
                loc += length;
            }
        }

        /// <summary>
        /// During merge, replace the existing (target) value in the entity we are merging to
        /// with a new (original) value from the detached entity we are merging. For immutable
        /// objects, or null values, it is safe to simply return the first parameter. For
        /// mutable objects, it is safe to return a copy of the first parameter. However, since
        /// composite user types often define component values, it might make sense to recursively
        /// replace component values in the target object.
        /// </summary>
        object ICompositeUserType.Replace(object original, object target, ISessionImplementor session, object owner)
        {
            if (!this.isMutable || original == null) return original;
            var result = target ?? this.Instantiate(original.GetType());
            var values = TypeHelper.Replace(this.GetPropertyValues(original), this.GetPropertyValues(result), this.AsUserType().PropertyTypes, session, owner, null);
            this.SetPropertyValues(result, values);
            return result;
        }

        /// <summary>
        /// Set the value of a property.
        /// </summary>
        void ICompositeUserType.SetPropertyValue(object component, int property, object value)
        {
            if (component == null) return;
            var propertyMapping = this.properties[property];
            if (propertyMapping.IsDriscriminator) return;
            if (!propertyMapping.MemberInfo.DeclaringType.IsAssignableFrom(component.GetType())) return;
            propertyMapping.MemberInfo.SetPropertyOrFieldValue(component, value);
        }

        protected void Discriminator(string column, IType persistentType = null)
        {
            var propertyMapping = new PropertyMapping
            {
                Name = column,
                PersistentType = persistentType ?? NHibernateUtil.AnsiString,
                IsDriscriminator = true
            };
            this.properties.Add(propertyMapping);
        }

        protected void DiscriminatorValue(object value)
        {
            this.classes[this.returnedClass].DiscriminatorValue = value != null ? value.ToString() : NullDiscriminatorValue;
        }

        /// <summary>
        /// Indicates if the component is mutable.
        /// </summary>
        protected void Mutable(bool isMutable)
        {
            this.isMutable = isMutable;
        }

        /// <summary>
        /// Maps a property of the component.
        /// </summary>
        protected void Property(string notVisiblePropertyOrFieldName, IType persistentType = null)
        {
            var memberInfo = this.returnedClass.GetPropertyOrFieldMatchingName(notVisiblePropertyOrFieldName);
            if (memberInfo == null)
                throw new MappingException($"Member not found. The member '{notVisiblePropertyOrFieldName}' does not exists in type {this.returnedClass.FullName}");
            var propertyMapping = PropertyMapping.FromMappedProperty(memberInfo);
            if (persistentType != null)
                propertyMapping.PersistentType = persistentType;
            this.properties.Add(propertyMapping);
        }

        /// <summary>
        /// Maps a property of the component.
        /// </summary>
        protected void Property<TProperty>(Expression<Func<TComponent, TProperty>> property, IType persistentType = null)
        {
            var memberInfo = TypeExtensions.DecodeMemberAccessExpressionOf(property);
            var propertyMapping = PropertyMapping.FromMappedProperty(memberInfo);
            if (persistentType != null)
                propertyMapping.PersistentType = persistentType;
            this.properties.Add(propertyMapping);
        }

        /// <summary>
        /// Maps a subcomponent.
        /// </summary>
        protected void Subclass<TSubclass>(Action<ISubcomponentMapper<TSubclass>> mapping = null) where TSubclass : TComponent
        {
            var mapper = new SubcomponentMapper<TSubclass>(this);
            mapping?.Invoke(mapper);
        }

        private ICompositeUserType AsUserType() => this;

        private string DiscriminatorName()
        {
            var propertyMapping = this.properties.FirstOrDefault(p => p.IsDriscriminator);
            if (propertyMapping == null)
                throw new InvalidOperationException("No discriminator found.");
            return propertyMapping.Name;
        }
        private ClassMapping GetClassMapping(Type mappedClass)
        {
            if (!this.classes.ContainsKey(mappedClass))
                throw new ArgumentException(nameof(mappedClass), $"The class '{mappedClass.Name}' is not mapped.");
            return this.classes[mappedClass];
        }

        private int GetHashCode(object x)
        {
            var result = 17;
            var values = this.GetPropertyValues(x);
            unchecked
            {
                for (var i = 0; i < values.Length; i++)
                {
                    var y = values[i];
                    result *= 37;
                    if (y != null)
                        result += propertyTypes[i].GetHashCode(y);
                }
            }
            return result;
        }

        private Type GetMappedClass(object discriminatorValue)
        {
            var value = discriminatorValue != null ? discriminatorValue.ToString() : NullDiscriminatorValue;
            var mappedClass = this.classes.FirstOrDefault(c => c.Value.DiscriminatorValue == value).Key;
            if (mappedClass == null)
            {
                if (value != NullDiscriminatorValue)
                    mappedClass = this.classes.FirstOrDefault(c => c.Value.DiscriminatorValue == NotNullDiscriminatorValue).Key;
            }
            if (mappedClass != null) return mappedClass;
            throw new ArgumentOutOfRangeException(nameof(discriminatorValue), $"No mapped class found for discriminator value '{value}'");
        }

        private object[] GetPropertyValues(object component)
        {
            var values = new object[this.PropertySpan];
            if (component != null)
            {
                for (var i = 0; i < values.Length; i++)
                    values[i] = this.AsUserType().GetPropertyValue(component, i);
            }
            return values;
        }

        private bool HasSubComponents() => this.classes.Count() > 1;

        private object Hydrate(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var begin = 0;
            var notNull = false;
            var values = new object[this.PropertySpan];
            for (var i = 0; i < values.Length; i++)
            {
                var length = this.AsUserType().PropertyTypes[i].GetColumnSpan(session.Factory);
                var range = ArrayHelper.Slice(names, begin, length); //cache this
                var value = this.AsUserType().PropertyTypes[i].Hydrate(rs, range, session, owner);
                if (value != null) notNull = true;
                values[i] = value;
                begin += length;
            }
            if (this.returnedClass.IsValueType)
                return values;
            else
                return notNull ? values : null;
        }

        private object Instantiate(Type mappedClass)
        {
            var mapping = this.classes[mappedClass];
            if (mapping.IsAbstract)
                throw new InstantiationException("Cannot instantiate abstract class or interface: ", mappedClass);
            var constructor = ReflectHelper.GetDefaultConstructor(mappedClass);
            if (constructor == null)
                throw new InstantiationException($"No default constructor for component: ", mappedClass);
            return constructor.Invoke(null);
        }

        private bool? IsEqualFast(object x, object y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            var componentType = this.AsUserType().ReturnedClass;
            if (!componentType.IsInstanceOfType(x) || !componentType.IsInstanceOfType(y)) return false;
            return null;
        }

        private object[] NullSafeGetValues(object value)
        {
            if (value == null) return new object[this.PropertySpan];
            return this.GetPropertyValues(value);
        }

        private void SetPropertyValues(object component, object[] values)
        {
            for (var i = 0; i < values.Length; i++)
                this.AsUserType().SetPropertyValue(component, i, values[i]);
        }

        #endregion Methods

        #region Classes

        private class ClassMapping
        {

            #region Properties

            public string DiscriminatorValue { get; set; }

            public bool IsAbstract { get; set; }

            public bool OverridesGetHashCode { get; set; }

            #endregion Properties

            #region Methods

            public static ClassMapping FromMappedClass(Type mappedClass)
            {
                return new ClassMapping
                {
                    OverridesGetHashCode = ReflectHelper.OverridesGetHashCode(mappedClass),
                    IsAbstract = ReflectHelper.IsAbstractClass(mappedClass),
                    DiscriminatorValue = mappedClass.Name
                };
            }

            #endregion Methods

        }

        private class PropertyMapping
        {

            #region Properties

            public bool IsDriscriminator { get; set; }

            public MemberInfo MemberInfo { get; set; }

            public string Name { get; set; }

            public IType PersistentType { get; set; }

            #endregion Properties

            #region Methods

            public static PropertyMapping FromMappedProperty(MemberInfo mappedProperty)
            {
                return new PropertyMapping
                {
                    MemberInfo = mappedProperty,
                    Name = mappedProperty.Name,
                    PersistentType = NHibernateUtil.GuessType(mappedProperty.GetPropertyOrFieldType()),
                    IsDriscriminator = false
                };
            }

            #endregion Methods

        }

        private class SubcomponentMapper<TSubComponent> : ISubcomponentMapper<TSubComponent> where TSubComponent : TComponent
        {

            #region Fields

            private readonly CompositeUserType<TComponent> compositeUserType;

            #endregion Fields

            #region Constructors

            public SubcomponentMapper(CompositeUserType<TComponent> compositeUserType)
            {
                this.compositeUserType = compositeUserType;
                this.compositeUserType.classes[typeof(TSubComponent)] = ClassMapping.FromMappedClass(typeof(TSubComponent));
            }

            #endregion Constructors

            #region Methods

            public void DiscriminatorValue(object value)
            {
                this.compositeUserType.classes[typeof(TSubComponent)].DiscriminatorValue = value != null ? value.ToString() : NullDiscriminatorValue;
            }

            /// <summary>
            /// Maps a property of the subcomponent.
            /// </summary>
            public void Property(string notVisiblePropertyOrFieldName, IType persistentType = null)
            {
                this.compositeUserType.Property(notVisiblePropertyOrFieldName, persistentType);
            }

            /// <summary>
            /// Maps a property of the subcomponent.
            /// </summary>
            public void Property<TProperty>(Expression<Func<TSubComponent, TProperty>> property, IType persistentType = null)
            {
                var memberInfo = TypeExtensions.DecodeMemberAccessExpressionOf(property);
                var propertyMapping = PropertyMapping.FromMappedProperty(memberInfo);
                if (persistentType != null)
                    propertyMapping.PersistentType = persistentType;
                this.compositeUserType.properties.Add(propertyMapping);
            }

            #endregion Methods

        }

        #endregion Classes

    }

}
