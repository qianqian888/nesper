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
    /// Static factory for implementations of the <see cref="com.espertech.esper.view.ViewService"/> interface.
    /// </summary>

    public sealed class ViewServiceProvider
	{
		/// <summary> Creates an implementation of the ViewService interface.</summary>
		/// <returns> implementation
		/// </returns>
		public static ViewService NewService()
		{
			return new ViewServiceImpl();
		}
	}
}