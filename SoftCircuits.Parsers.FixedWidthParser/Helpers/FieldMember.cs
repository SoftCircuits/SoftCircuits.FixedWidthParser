// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SoftCircuits.Parsers;

/// <summary>
/// Class for accessing class fields.
/// </summary>
internal class FieldMember(FieldInfo field) : IMember
{
    private readonly FieldInfo Field = field;
    private Func<object, object?>? Getter;
    private Action<object, object?>? Setter;

    public string Name => Field.Name;
    public Type Type => Field.FieldType;
    public bool CanRead => true;
    public bool CanWrite => !Field.IsInitOnly && !Field.IsLiteral;

    public object? GetValue(object item)
    {
        Getter ??= BuildGetter(Field);
        return Getter(item);
    }

    public void SetValue(object item, object value)
    {
        Setter ??= BuildSetter(Field);
        Setter(item, value);
    }

    private static Func<object, object?> BuildGetter(FieldInfo field)
    {
        ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
        UnaryExpression cast = Expression.Convert(instance, field.DeclaringType!);
        MemberExpression access = Expression.Field(cast, field);
        UnaryExpression result = Expression.Convert(access, typeof(object));
        return Expression.Lambda<Func<object, object?>>(result, instance).Compile();
    }

    private static Action<object, object?> BuildSetter(FieldInfo field)
    {
        ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
        ParameterExpression value = Expression.Parameter(typeof(object), "value");
        UnaryExpression castInstance = Expression.Convert(instance, field.DeclaringType!);
        UnaryExpression castValue = Expression.Convert(value, field.FieldType);
        MemberExpression access = Expression.Field(castInstance, field);
        BinaryExpression assign = Expression.Assign(access, castValue);
        return Expression.Lambda<Action<object, object?>>(assign, instance, value).Compile();
    }
}
