﻿using DDD.DependencyInjection;
using SimpleInjector;
using EnsureThat;
using System.Collections.Generic;
using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    public class KeyedServiceProvider<TKey, TService> : IKeyedServiceProvider<TKey, TService>
        where TService : class
    {

        #region Fields

        private readonly Container container;
        private readonly Dictionary<TKey, InstanceProducer<TService>> producers;

        #endregion Fields

        #region Constructors

        public KeyedServiceProvider(Container container, IEqualityComparer<TKey> keyComparer = null)
        {
            Ensure.That(container, nameof(container)).IsNotNull();
            this.container = container;
            this.producers = new Dictionary<TKey, InstanceProducer<TService>>(keyComparer);
        }

        #endregion Constructors

        #region Methods

        public TService GetService(TKey key)
        {
            if (this.producers.TryGetValue(key, out var producer))
                return producer.GetInstance();
            throw new InvalidOperationException($"No registration for key {key}.");
        }

        public void Register<TImplementation>(TKey key, Lifestyle lifestyle)
            where TImplementation : class, TService
        {
            Ensure.That(lifestyle, nameof(lifestyle)).IsNotNull();
            var producer = lifestyle.CreateProducer<TService, TImplementation>(container);
            this.producers.Add(key, producer);
        }

        public void Register(TKey key, Func<TService> instanceCreator, Lifestyle lifestyle)
        {
            Ensure.That(instanceCreator, nameof(instanceCreator)).IsNotNull();
            Ensure.That(lifestyle, nameof(lifestyle)).IsNotNull();
            var producer = lifestyle.CreateProducer<TService>(instanceCreator, container);
            this.producers.Add(key, producer);
        }

        #endregion Methods

    }
}