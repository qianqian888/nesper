///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

namespace com.espertech.esper.view
{
    /// <summary>
    /// Marker interface for use with view factories that create data window views that 
    /// are asymetric in posting insert and remove stream data: Data windows that post 
    /// only a partial insert and remove stream as output when compared to the insert 
    /// and remove stream received. 
    /// <para/>
    /// Please <see cref="com.espertech.esper.view.DataWindowView" /> for details on 
    /// views that meet data window requirements.
    /// </summary>
    public interface AsymetricDataWindowViewFactory : DataWindowViewFactory
    {
    }
}
