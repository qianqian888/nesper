///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

using com.espertech.esper.client;

namespace com.espertech.esper.events
{
    /// <summary>For events that are maps of properties. </summary>
    public interface MappedEventBean : EventBean
    {
        /// <summary>Returns property map. </summary>
        /// <value>properties</value>
        IDictionary<string, object> Properties { get; }
    }
}
