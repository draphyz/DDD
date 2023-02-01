﻿using System;
using System.Collections.Generic;
using EnsureThat;

namespace DDD.Mapping
{
    /// <summary>
    /// Adapter that converts a delegate into an object that implements the interface IObjectTranslator.
    /// </summary>
    public class DelegatingTranslator<TSource, TDestination> : ObjectTranslator<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {

        #region Fields

        private readonly Func<TSource, IDictionary<string, object>, TDestination> translator;

        #endregion Fields

        #region Constructors

        public DelegatingTranslator(Func<TSource, IDictionary<string, object>, TDestination> translator)
        {
            Ensure.That(translator).IsNotNull();
            this.translator = translator;
        }

        #endregion Constructors

        #region Methods

        public override TDestination Translate(TSource source, IDictionary<string, object> context = null)
        {
            Ensure.That(source).IsNotNull();
            return this.translator(source, context);
        }

        #endregion Methods

    }
}
