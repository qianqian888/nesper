///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2015 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;

using com.espertech.esper.client;
using com.espertech.esper.events.arr;
using com.espertech.esper.events.bean;
using com.espertech.esper.events.map;
using com.espertech.esper.events.xml;

namespace com.espertech.esper.events.property
{
    using DataMap = IDictionary<string, object>;

    /// <summary>
    /// Represents a dynamic indexed property of a given name.
    /// <para/>
    /// Dynamic properties always exist, have an Object type and are resolved to a
    /// method during runtime.
    /// </summary>
    public class DynamicIndexedProperty 
        : PropertyBase
        , DynamicProperty
    {
        private readonly int _index;
    
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="propertyName">is the property name</param>
        /// <param name="index">is the index of the array or indexed property</param>
        public DynamicIndexedProperty(String propertyName, int index)
            : base(propertyName)
        {
            _index = index;
        }

        public override bool IsDynamic
        {
            get { return true; }
        }

        public override String[] ToPropertyArray()
        {
            return new[] {PropertyNameAtomic};
        }
    
        public override EventPropertyGetter GetGetter(BeanEventType eventType, EventAdapterService eventAdapterService)
        {
            return new DynamicIndexedPropertyGetter(PropertyNameAtomic, _index, eventAdapterService);
        }
    
        public override Type GetPropertyType(BeanEventType eventType, EventAdapterService eventAdapterService)
        {
            return typeof(Object);
        }
    
        public override GenericPropertyDesc GetPropertyTypeGeneric(BeanEventType beanEventType, EventAdapterService eventAdapterService)
        {
            return GenericPropertyDesc.ObjectGeneric;
        }
    
        public override Type GetPropertyTypeMap(DataMap optionalMapPropTypes, EventAdapterService eventAdapterService)
        {
            return typeof(Object);
        }

        public override MapEventPropertyGetter GetGetterMap(DataMap optionalMapPropTypes, EventAdapterService eventAdapterService)
        {
            return new MapIndexedPropertyGetter(PropertyNameAtomic, _index);
        }


        public override ObjectArrayEventPropertyGetter GetGetterObjectArray(
            IDictionary<string, int> indexPerProperty, 
            IDictionary<String, Object> nestableTypes, 
            EventAdapterService eventAdapterService)
        {
            int propertyIndex;
            
            if (indexPerProperty.TryGetValue(PropertyNameAtomic, out propertyIndex))
                return new ObjectArrayIndexedPropertyGetter(propertyIndex, _index);

            return null;
        }

        public override void ToPropertyEPL(TextWriter writer)
        {
            writer.Write(PropertyNameAtomic);
            writer.Write('[');
            writer.Write(Convert.ToString(_index));
            writer.Write(']');
            writer.Write('?');
        }
    
        public override EventPropertyGetter GetGetterDOM(SchemaElementComplex complexProperty, EventAdapterService eventAdapterService, BaseXMLEventType eventType, String propertyExpression)
        {
            return new DOMIndexedGetter(PropertyNameAtomic, _index, null);
        }
    
        public override SchemaItem GetPropertyTypeSchema(SchemaElementComplex complexProperty, EventAdapterService eventAdapterService)
        {
            return null;  // dynamic properties always return Node
        }
    
        public override EventPropertyGetter GetGetterDOM()
        {
            return new DOMIndexedGetter(PropertyNameAtomic, _index, null);
        }

        public int Index
        {
            get { return _index; }
        }
    }
}
