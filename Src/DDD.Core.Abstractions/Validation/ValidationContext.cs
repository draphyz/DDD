using System.Collections.Generic;

namespace DDD.Validation
{
    using Collections;
    using System.Threading;

    public class ValidationContext : Dictionary<string, object>, IValidationContext
    {

        #region Constructors

        public ValidationContext() 
        { 
        }

        public ValidationContext(IDictionary<string, object> dictionary) : base(dictionary)
        {
        }

        #endregion Constructors

        #region Methods

        public static IValidationContext CancellableContext(CancellationToken cancellationToken)
        {
            var context = new ValidationContext();
            context.AddCancellationToken(cancellationToken);
            return context;
        }

        public static IValidationContext WithRuleSets(params string[] ruleSets)
        {
            var context = new ValidationContext();
            context.AddRuleSets(ruleSets);
            return context;
        }

        public static IValidationContext FromObject(object context)
        {
            var validationContext = new ValidationContext();
            if (context != null)
                validationContext.AddObject(context);
            return validationContext;
        }

        #endregion Methods

    }
}
