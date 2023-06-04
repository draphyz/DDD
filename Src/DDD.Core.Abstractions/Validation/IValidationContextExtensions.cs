using EnsureThat;
using System.Threading;

namespace DDD.Validation
{
    using Collections;

    public static class IValidationContextExtensions
    {

        #region Methods

        public static void AddRuleSets(this IValidationContext context, params string[] ruleSets)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            Ensure.That(ruleSets, nameof(ruleSets)).IsNotNull();
            context.Add(ValidationContextInfo.RuleSets, ruleSets);
        }

        public static void AddCancellationToken(this IValidationContext context, CancellationToken cancellationToken)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            context.Add(ValidationContextInfo.CancellationToken, cancellationToken);
        }

        public static CancellationToken CancellationToken(this IValidationContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue(ValidationContextInfo.CancellationToken, out CancellationToken cancellationToken);
            return cancellationToken;
        }

        public static string[] RuleSets(this IValidationContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            context.TryGetValue(ValidationContextInfo.RuleSets, out string[] ruleSets);
            return ruleSets;
        }


        #endregion Methods

    }
}
