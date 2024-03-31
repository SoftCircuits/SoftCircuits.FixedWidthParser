// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Interface for accessing class members.
    /// </summary>
    internal interface IMember
    {
        string Name { get; }
        Type Type { get; }
        bool CanRead { get; }
        bool CanWrite { get; }
        void SetValue(object item, object value);
        object? GetValue(object item);
    }
}
