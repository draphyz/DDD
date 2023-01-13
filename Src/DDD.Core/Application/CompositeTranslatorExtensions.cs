using System;
using Conditions;

namespace DDD.Core.Application
{
    using Mapping;
    using Collections;

    public static class CompositeTranslatorExtensions
    {

        #region Methods

        public static void RegisterFallback(this CompositeTranslator<Exception, CommandException> translator)
        {
            Condition.Requires(translator, nameof(translator)).IsNotNull();
            translator.Register<Exception>((exception, context) =>
            {
                ICommand command = null;
                context?.TryGetValue("Command", out command);
                return new CommandException(isTransient: false, command, exception);
            });
        }

        public static void RegisterFallback(this CompositeTranslator<Exception, QueryException> translator)
        {
            Condition.Requires(translator, nameof(translator)).IsNotNull();
            translator.Register<Exception>((exception, context) =>
            {
                IQuery query = null;
                context?.TryGetValue("Query", out query);
                return new QueryException(isTransient: false, query, exception);
            });
        }

        #endregion Methods

    }
}
