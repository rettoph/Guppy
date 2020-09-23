﻿using Guppy.Events.Delegates;
using Guppy.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Lists.Interfaces
{
    public interface IServiceList : IService, IEnumerable
    {
        Type ListType { get; }

        void TryAdd(Object instance);
        T GetById<T>(Guid id)
            where T : class, IService;
    }

    public interface IServiceList<TService> : IServiceList, IEnumerable<TService>
        where TService : IService
    {
        #region Events
        /// <summary>
        /// Public filter allowing the current list to determin
        /// whether or not a particular item may be added.
        /// </summary>
        event ValidateEventDelegate<IServiceList<TService>, TService> CanAdd;

        /// <summary>
        /// Public filter allowing the current list to determin
        /// whether or not a particular item may be removed.
        /// </summary>
        event ValidateEventDelegate<IServiceList<TService>, TService> CanRemove;

        /// <summary>
        /// Public event invoked when an item is added and 
        /// all internal configuration is complete.
        /// </summary>
        event OnEventDelegate<IServiceList<TService>, TService> OnAdded;

        /// <summary>
        /// Public event invoked when an item is removed and 
        /// all internal configuration is complete.
        /// </summary>
        event OnEventDelegate<IServiceList<TService>, TService> OnRemoved;
        #endregion

        #region Helper Methods
        Boolean TryAdd(TService item);
        Boolean TryRemove(TService item);

        new T GetById<T>(Guid id)
            where T : class, TService;
        #endregion
    }
}
