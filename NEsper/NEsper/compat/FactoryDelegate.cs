///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////


namespace com.espertech.esper.compat
{
    /// <summary>
    /// Factory pattern delegate that creates an object of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public delegate T FactoryDelegate<T>();
}
