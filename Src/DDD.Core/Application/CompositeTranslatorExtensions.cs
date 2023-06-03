using System;
using EnsureThat;

namespace DDD.Core.Application
{
    using Mapping;
    using Collections;

    public static class CompositeTranslatorExtensions
    {

        #region Methods

        public static void RegisterFallback(this CompositeTranslator<Exception, CommandException> translator)
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            translator.Register<Exception>((exception, context) =>
            {
                context.TryGetValue("Command", out ICommand command);
                return new CommandException(isTransient: false, command, exception);
            });
        }

        public static void RegisterFallback(this CompositeTranslator<Exception, QueryException> translator)
        {
            Ensure.That(translator, nameof(translator)).IsNotNull();
            translator.Register<Exception>((exception, context) =>
            {
                context.TryGetValue("Query", out IQuery query);
                return new QueryException(isTransient: false, query, exception);
            });
        }

        #endregion Methods

    }
}
