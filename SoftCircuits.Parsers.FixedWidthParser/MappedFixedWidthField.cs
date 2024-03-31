// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.Parsers.Converters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Describes a fixed-width field that is mapped to a class property or field.
    /// </summary>
    internal class MappedFixedWidthField : FixedWidthField, IFixedWidthField
    {
        private readonly IMember Member;
        private IDataConverter Converter;

        /// <summary>
        /// Constructs a new <see cref="MappedFixedWidthField"/> instance.
        /// </summary>
        /// <param name="member"><see cref="MemberInfo"/> that describes this field. Must be of type
        /// <see cref="FieldInfo"/> or <see cref="PropertyInfo"/>.</param>
        /// <param name="attribute">The field's <see cref="FixedWidthFieldAttribute"/> attribute.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public MappedFixedWidthField(MemberInfo member, FixedWidthFieldAttribute attribute)
            : base(attribute)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            if (member is FieldInfo fieldInfo)
                Member = new FieldMember(fieldInfo);
            else if (member is PropertyInfo propertyInfo)
                Member = new PropertyMember(propertyInfo);
            else
                throw new ArgumentException("Unknown member type", nameof(member));

            Converter = GetConverter(attribute.ConverterType);
        }

        /// <summary>
        /// Constructs a new <see cref="MappedFixedWidthField"/> instance.
        /// </summary>
        /// <param name="member"><see cref="MemberInfo"/> that describes this field. Must be of type
        /// <see cref="FieldInfo"/> or <see cref="PropertyInfo"/>.</param>
        /// <param name="length">This field's length.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public MappedFixedWidthField(MemberInfo member, int length)
            : base(length)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (member is FieldInfo fieldInfo)
                Member = new FieldMember(fieldInfo);
            else if (member is PropertyInfo propertyInfo)
                Member = new PropertyMember(propertyInfo);
            else
                throw new ArgumentException("Unknown member type", nameof(member));

            Converter = GetConverter(null);
        }

        /// <summary>
        /// Gets the value of this member as a string.
        /// </summary>
        /// <param name="item">The instance from which to get this member value.</param>
        /// <returns>The member value.</returns>
        internal override string GetValue(object item)
        {
            if (Member.CanRead)
                return Converter.ConvertToString(Member.GetValue(item));
            return string.Empty;
        }

        /// <summary>
        /// Sets the value of this member from a string.
        /// </summary>
        /// <param name="item">The instance to set this member value.</param>
        /// <param name="s">The string value to set.</param>
        /// <param name="throwExceptionOnInvalidData">If true, a <see cref="FixedWidthDataException"/>
        /// is thrown if <paramref name="s"/> could not be converted to this member type.</param>
        /// <exception cref="FixedWidthDataException"></exception>
        internal override void SetValue(object item, string s, bool throwExceptionOnInvalidData)
        {
            if (Member.CanWrite)
            {
                if (Converter.TryConvertFromString(s, out object? value))
                {
                    Member.SetValue(item, value);
                }
                else
                {
                    // Could not convert field
                    if (throwExceptionOnInvalidData)
                        throw new FixedWidthDataException(Member.Name, s, Member.Type.Name);
                }
            }
        }

        /// <summary>
        /// Gets the converter for this member. If a converter type is specified, that type is instantiated
        /// and returned. Otherwise, the built-in convert for this member's type is returned. If no built-in
        /// converter is found, <see cref="UnsupportedTypeConverter"/> is returned.
        /// </summary>
        /// <param name="converterType">The converter type specified in the <see cref="FixedWidthFieldAttribute"/>.</param>
        /// <returns>A data converter for this member.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private IDataConverter GetConverter(Type? converterType)
        {
            if (converterType != null)
            {
                if (!typeof(IDataConverter).IsAssignableFrom(converterType))
                    throw new InvalidOperationException($"The data converter type specified for member '{Member.Name}' must derive from '{nameof(IDataConverter)}'.");
                if (Activator.CreateInstance(converterType) is not IDataConverter converter)
                    throw new Exception($"Unable to create instance of type '{converterType.FullName}'.");
                return converter;
            }
            return DataConverters.GetConverter(Member.Type);
        }

        #region IFixedWidthField Interface

        public IFixedWidthField SetAlignment(FieldAlignment? alignment)
        {
            Alignment = alignment;
            return this;
        }

        public IFixedWidthField SetPadCharacter(char? padCharacter)
        {
            PadCharacter = padCharacter;
            return this;
        }

        public IFixedWidthField SetTrimField(bool? trimField)
        {
            TrimField = trimField;
            return this;
        }

        public IFixedWidthField SetSkip(int skip)
        {
            Skip = skip;
            return this;
        }

        public IFixedWidthField SetConverterType(Type? converterType)
        {
            Converter = GetConverter(converterType);
            return this;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Builds a list of <see cref="MappedFixedWidthField"/>s from the specified type. Returns an empty collection
        /// if not members have a <see cref="FixedWidthFieldAttribute"/> attribute.
        /// </summary>
        /// <param name="type">The type to build <see cref="MappedFixedWidthField"/>s for.</param>
        /// <returns>A collection of <see cref="MappedFixedWidthField"/>s.</returns>
        public static IEnumerable<FixedWidthField> MapFields(Type type)
        {
            foreach (MemberInfo member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                FixedWidthFieldAttribute? attribute = member.GetCustomAttribute<FixedWidthFieldAttribute>();
                if (attribute != null)
                {
                    FixedWidthField? field = null;

                    if (member is PropertyInfo propertyInfo)
                    {
                        field = new MappedFixedWidthField(propertyInfo, attribute);
                    }
                    else if (member is FieldInfo fieldInfo)
                    {
                        // Ignore compiler-generated fields for backing properties
                        if (!fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute)))
                            field = new MappedFixedWidthField(fieldInfo, attribute);
                    }
                    if (field != null)
                        yield return field;
                }
            }
        }

        /// <summary>
        /// <para>
        /// Maps a field to the specified property. If a map already exists for the property, that map is modified. Otherwise, a new
        /// map is created and appended as the last field.
        /// </para>
        /// <para>
        /// This method is intended to be called after a <see cref="FixedWidthReader{T}"/> or <see cref="FixedWidthWriter{T}"/> has
        /// been created for a type with no <see cref="FixedWidthFieldAttribute"/> attributes to programmatically specify field mapping.
        /// However, it can also be used to modify existing field mappings.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The class type being mapped.</typeparam>
        /// <typeparam name="TMember">The property type being mapped.</typeparam>
        /// <param name="fields">The list of existing field maps. Any new maps will be appended to this collection.</param>
        /// <param name="expression">An expression that specifies the property being mapped.</param>
        /// <param name="length">The number of characters used by this field.</param>
        /// <returns>An <see cref="IFixedWidthField"/> that supports a Fluent interface to set additional mapping properties.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IFixedWidthField MapField<T, TMember>(List<FixedWidthField> fields, Expression<Func<T, TMember>> expression, int length)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            // Get member name from expression
            MemberExpression? member = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
                member = (expression.Body as UnaryExpression)?.Operand as MemberExpression;
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
                member = expression.Body as MemberExpression;

            if (member == null)
                throw new ArgumentException("Unsupported expression type", nameof(expression));

            // Look for existing field with this name
            MappedFixedWidthField? field = null;
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i] is not MappedFixedWidthField mappedField)
                    throw new InvalidOperationException($"Unexpected {nameof(FixedWidthField)} field in generic version of {nameof(FixedWidthReader)}/{nameof(FixedWidthWriter)}.");
                if (string.Equals(mappedField.Member.Name, member.Member.Name, StringComparison.OrdinalIgnoreCase))
                {
                    field = mappedField;
                    field.Length = length;
                    break;
                }
            }

            // Add new field if matching field not found
            if (field == null)
            {
                MemberInfo[] members = typeof(T).GetMember(member.Member.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (members.Length == 0)
                    throw new ArgumentException($"Unsupported expression : Member not found '{member.Member.Name}'", nameof(expression));

                if (members[0] is PropertyInfo propertyInfo)
                {
                    field = new MappedFixedWidthField(propertyInfo, length);
                }
                else if (members[0] is FieldInfo fieldInfo)
                {
                    // Ignore compiler-generated fields for backing properties
                    if (!fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute)))
                        field = new MappedFixedWidthField(fieldInfo, length);
                }

                if (field == null)
                    throw new ArgumentException($"Unsupported expression : Unsupported member type '{members[0].MemberType}'", nameof(expression));

                fields.Add(field);
            }

            return field;
        }

        #endregion

    }
}
