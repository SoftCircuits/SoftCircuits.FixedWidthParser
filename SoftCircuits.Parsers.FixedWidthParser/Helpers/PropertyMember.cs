// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SoftCircuits.Parsers;

/// <summary>
/// Class for accessing class properties.
/// </summary>
internal class PropertyMember(PropertyInfo property) : IMember
{
    private readonly PropertyInfo Property = property;
    private Func<object, object?>? Getter;
    private Action<object, object?>? Setter;

    public string Name => Property.Name;
    public Type Type => Property.PropertyType;
    public bool CanRead => Property.CanRead;
    public bool CanWrite => Property.CanWrite;

    public object? GetValue(object item)
    {
        Getter ??= BuildGetter(Property);
        return Getter(item);
    }

    public void SetValue(object item, object value)
    {
        Setter ??= BuildSetter(Property);
        Setter(item, value);
    }

    private static Func<object, object?> BuildGetter(PropertyInfo property)
    {
        ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
        UnaryExpression cast = Expression.Convert(instance, property.DeclaringType!);
        MemberExpression access = Expression.Property(cast, property);
        UnaryExpression result = Expression.Convert(access, typeof(object));
        return Expression.Lambda<Func<object, object?>>(result, instance).Compile();
    }

    private static Action<object, object?> BuildSetter(PropertyInfo property)
    {
        ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
        ParameterExpression value = Expression.Parameter(typeof(object), "value");
        UnaryExpression castInstance = Expression.Convert(instance, property.DeclaringType!);
        UnaryExpression castValue = Expression.Convert(value, property.PropertyType);
        MemberExpression access = Expression.Property(castInstance, property);
        BinaryExpression assign = Expression.Assign(access, castValue);
        return Expression.Lambda<Action<object, object?>>(assign, instance, value).Compile();
    }
}
