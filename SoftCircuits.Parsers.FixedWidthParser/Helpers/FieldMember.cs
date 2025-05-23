﻿// Copyright (c) 2020-2025 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Reflection;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Class for accessing field properties.
    /// </summary>
    internal class FieldMember(FieldInfo field) : IMember
    {
        private readonly FieldInfo Field = field;

        public string Name => Field.Name;

        public Type Type => Field.FieldType;

        public bool CanRead => true;

        public bool CanWrite => true;

        public void SetValue(object item, object value) => Field.SetValue(item, value);

        public object? GetValue(object item) => Field.GetValue(item);
    }
}
