///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;

namespace com.espertech.esper.support.bean
{
    [Serializable]
	public class SupportRFIDEvent
	{
        public string LocationReportId { get; private set; }

        public string Mac { get; private set; }

        public string ZoneID { get; private set; }

        public SupportRFIDEvent(String mac, String zoneID)
	        : this(null, mac, zoneID)
	    {
	    }

	    public SupportRFIDEvent(String locationReportId, String mac, String zoneID)
	    {
	        this.LocationReportId = locationReportId;
	        this.Mac = mac;
	        this.ZoneID = zoneID;
	    }
	}
} // End of namespace
