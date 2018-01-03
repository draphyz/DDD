using System.Data.Entity.Validation;
using System.Linq;
using Conditions;

namespace DDD.Core.Infrastructure.Data
{
    using Collections;

    public static class DbEntityValidationExceptionExtensions
    {

        #region Methods

        /// <summary>
        /// Adds the validation errors associated with the entity in the Data property.
        /// </summary>
        /// <param name="exception">The current exception.</param>
        public static void AddErrorsInData(this DbEntityValidationException exception)
        {
            Condition.Requires(exception, nameof(exception)).IsNotNull();
            exception.EntityValidationErrors
                     .SelectMany(e => e.ValidationErrors)
                     .ForEach((i, e) => exception.AddErrorInData(i, e));
        }

        private static void AddErrorInData(this DbEntityValidationException exception, int index, DbValidationError error)
        {
            var key = $"EntityValidationError{index}";
            if (!exception.Data.Contains(key))
                exception.Data[key] = $"Property : {error.PropertyName}, Error : {error.ErrorMessage}";
        }

        #endregion Methods

    }
}
