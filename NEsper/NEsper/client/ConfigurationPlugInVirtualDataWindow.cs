///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

namespace com.espertech.esper.client
{
    /// <summary>
    /// Configuration information for plugging in a custom view.
    /// </summary>
    [Serializable]
    public class ConfigurationPlugInVirtualDataWindow
    {
        /// <summary>Returns the namespace </summary>
        /// <value>namespace</value>
        public string Namespace { get; set; }

        /// <summary>Returns the view name. </summary>
        /// <value>view name</value>
        public string Name { get; set; }

        /// <summary>Returns the view factory class name. </summary>
        /// <value>factory class name</value>
        public string FactoryClassName { get; set; }

        /// <summary>
        /// Returns any additional configuration passed to the factory as part of the context.
        /// </summary>
        /// <value>The config.</value>
        public object Config { get; set; }
    }
}
