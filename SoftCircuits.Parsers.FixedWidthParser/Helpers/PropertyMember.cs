// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Reflection;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Class for accessing class properties.
    /// </summary>
    internal class PropertyMember : IMember
    {
        private readonly PropertyInfo Property;

        public PropertyMember(PropertyInfo property)
        {
            Property = property;
        }

        public string Name => Property.Name;

        public Type Type => Property.PropertyType;

        public bool CanRead => Property.CanRead;

        public bool CanWrite => Property.CanWrite;

        public void SetValue(object item, object value) => Property.SetValue(item, value);

        public object? GetValue(object item) => Property.GetValue(item);
    }
}
