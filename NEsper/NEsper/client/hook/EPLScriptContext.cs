///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

namespace com.espertech.esper.client.hook
{
    /// <summary>Available when using JSR-223 scripts or MVEL, for access of script attributes. </summary>
    public interface EPLScriptContext
    {
        /// <summary>
        /// Set a script attributed.
        /// </summary>
        /// <param name="attribute">name to use</param>
        /// <param name="value">value to set</param>
        void SetScriptAttribute(string attribute, object value);

        /// <summary>
        /// Return a script attribute value.
        /// </summary>
        /// <param name="attribute">name to retrieve value for</param>
        /// <returns>attribute value or null if undefined</returns>
        object GetScriptAttribute(string attribute);
    }
}