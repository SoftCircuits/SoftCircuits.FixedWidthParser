// Copyright (c) 2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Reflection;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Interface for accessing class members.
    /// </summary>
    interface IClassMember
    {
        string Name { get; }
        Type Type { get; }
        bool CanRead { get; }
        bool CanWrite { get; }
        void SetValue(object item, object value);
        object GetValue(object item);
    }

    /// <summary>
    /// Class for accessing field properties.
    /// </summary>
    internal class ClassFieldMember : IClassMember
    {
        private readonly FieldInfo Field;

        public ClassFieldMember(FieldInfo field)
        {
            Field = field;
        }

        public string Name => Field.Name;

        public Type Type => Field.FieldType;

        public bool CanRead => true;

        public bool CanWrite => true;

        public void SetValue(object item, object value) => Field.SetValue(item, value);

        public object GetValue(object item) => Field.GetValue(item);
    }

    /// <summary>
    /// Class for accessing class properties.
    /// </summary>
    internal class ClassPropertyAccess : IClassMember
    {
        private readonly PropertyInfo Property;

        public ClassPropertyAccess(PropertyInfo property)
        {
            Property = property;
        }

        public string Name => Property.Name;

        public Type Type => Property.PropertyType;

        public bool CanRead => Property.CanRead;

        public bool CanWrite => Property.CanWrite;

        public void SetValue(object item, object value) => Property.SetValue(item, value);

        public object GetValue(object item) => Property.GetValue(item);
    }
}
