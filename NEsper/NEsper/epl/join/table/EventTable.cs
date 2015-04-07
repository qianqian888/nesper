///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using com.espertech.esper.client;

namespace com.espertech.esper.epl.join.table
{
    /// <summary>
    /// Table of events allowing add and remove. Lookup in table is coordinated through 
    /// the underlying implementation.
    /// </summary>
    public interface EventTable : IEnumerable<EventBean>
    {
        /// <summary>
        /// Add and remove events from table. <para/>It is up to the index to decide whether to add first and then 
        /// remove, or whether to remove and then add. <para/>It is important to note that a given event can be in 
        /// both the removed and the added events. This means that unique indexes probably need to remove first 
        /// and then add. Most other non-unique indexes will add first and then remove since the an event can be 
        /// both in the add and the remove stream.
        /// </summary>
        /// <param name="newData">to add</param>
        /// <param name="oldData">to remove</param>
        void AddRemove(EventBean[] newData, EventBean[] oldData);

        /// <summary>
        /// Add events to table.
        /// </summary>
        /// <param name="events">to add</param>
        void Add(EventBean[] events);

        /// <summary>
        /// Remove events from table.
        /// </summary>
        /// <param name="events">to remove</param>
        void Remove(EventBean[] events);

        /// <summary>
        /// Adds the specified event to the table.
        /// </summary>
        /// <param name="event">The event.</param>
        void Add(EventBean @event);

        /// <summary>
        /// Removes the specified event to the table.
        /// </summary>
        /// <param name="event">The event.</param>
        void Remove(EventBean @event);

        /// <summary>
        /// Returns true if the index is empty, or false if not
        /// </summary>
        /// <returns>true for empty index</returns>
        bool IsEmpty();

        /// <summary>Clear out index. </summary>
        void Clear();
    
        String ToQueryPlan();

        /// <summary>
        /// If the number of events is readily available, an implementation will return that number or it may 
        /// return null to indicate that the count is not readily available.
        /// </summary>
        /// <value>number of events</value>
        int? NumberOfEvents { get; }

        /// <summary>
        /// If the index retains events using some key-based Organization this returns the number of keys, and 
        /// may return null to indicate that either the number of keys is not available or costly to obtain. 
        /// <para/>
        /// The number returned can be an estimate and may not be accurate.
        /// </summary>
        /// <value>number of events</value>
        int NumKeys { get; }

        /// <summary>
        /// Return the index object itself, or an object-array for multiple index structures.
        /// <para/>
        /// May return null if the information is not readily available, i.e. externally maintained index
        /// </summary>
        /// <value>index object</value>
        object Index { get; }

        EventTableOrganization Organization { get; }
    }
}
