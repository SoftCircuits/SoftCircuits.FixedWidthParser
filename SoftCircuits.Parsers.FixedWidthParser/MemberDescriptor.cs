// Copyright (c) 2020-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.Parsers.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Describes a class member to fixed-width field mapping.
    /// </summary>
    internal class MemberDescriptor : FixedWidthField
    {
        private readonly IClassMember Member;
        private readonly IDataConverter Converter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="property">The information for this property.</param>
        /// <param name="attribute">The property's <see cref="FixedWidthFieldAttribute"/> attribute.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MemberDescriptor(PropertyInfo property, FixedWidthFieldAttribute attribute)
            : base(attribute)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            Member = new PropertyMember(property);
            Converter = GetConverter(attribute.ConverterType);
            Debug.Assert(Converter != null);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="field">The information for this field.</param>
        /// <param name="attribute">The field's <see cref="FixedWidthFieldAttribute"/> attribute.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MemberDescriptor(FieldInfo field, FixedWidthFieldAttribute attribute)
            : base(attribute)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            Member = new FieldMember(field);
            Converter = GetConverter(attribute.ConverterType);
            Debug.Assert(Converter != null);
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
                    throw new Exception($"Unable to create instance of type {converterType.FullName}.");
                return converter;
            }
            return DataConverters.GetConverter(Member.Type);
        }

        /// <summary>
        /// Gets the value of this member as a string.
        /// </summary>
        /// <param name="item">The instance from which to get this member value.</param>
        /// <returns>The member value.</returns>
        public string GetValue(object item)
        {
            Debug.Assert(Member != null);
            Debug.Assert(Converter != null);

            return Member.CanRead ? Converter.ConvertToString(Member.GetValue(item)) : string.Empty;
        }

        /// <summary>
        /// Sets the value of this member from a string.
        /// </summary>
        /// <param name="item">The instance to set this member value.</param>
        /// <param name="s">The string value to set.</param>
        /// <param name="throwExceptionOnInvalidData">If true, a <see cref="FixedWidthDataException"/>
        /// is thrown if <paramref name="s"/> could not be converted to this member type.</param>
        /// <exception cref="FixedWidthDataException"></exception>
        public void SetValue(object item, string s, bool throwExceptionOnInvalidData)
        {
            Debug.Assert(Member != null);
            Debug.Assert(Converter != null);

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

        #region Static methods

        public static List<MemberDescriptor> GetMemberDescriptors(Type type)
        {
            List<MemberDescriptor> descriptors = new();

            foreach (MemberInfo member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                FixedWidthFieldAttribute? attribute = member.GetCustomAttribute<FixedWidthFieldAttribute>();
                if (attribute != null)
                {
                    MemberDescriptor? field = null;

                    if (member is PropertyInfo propertyInfo)
                    {
                        field = new MemberDescriptor(propertyInfo, attribute);
                    }
                    else if (member is FieldInfo fieldInfo)
                    {
                        // Ignore compiler-generated fiedls for backing properties
                        if (!fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute)))
                        {
                            field = new MemberDescriptor(fieldInfo, attribute);
                        }
                    }

                    if (field != null)
                        descriptors.Add(field);
                }
            }
            return descriptors;
        }

        #endregion

    }
}
