///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

using com.espertech.esper.client;

namespace com.espertech.esper.pattern
{
    /// <summary>
    /// Interface for a root state node accepting a callback to use to indicate pattern results.
    /// </summary>
    public interface EvalRootState : PatternStopCallback
    {
        /// <summary>Accept callback to indicate pattern results. </summary>
        /// <value>is a pattern result call</value>
        PatternMatchCallback Callback { set; }

        void StartRecoverable(bool startRecoverable, MatchedEventMap beginState);
    
        void Accept(EvalStateNodeVisitor visitor);
    
        void RemoveMatch(ISet<EventBean> matchEvent);
    }
}
