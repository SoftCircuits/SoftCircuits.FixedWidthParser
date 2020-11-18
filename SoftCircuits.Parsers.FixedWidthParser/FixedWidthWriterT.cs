// Copyright (c) 2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SoftCircuits.Parsers
{
    public class FixedWidthWriter<T> : FixedWidthWriter where T : class, new()
    {
        private List<MemberDescriptor> MemberDescriptors;
        private string[] WriteValues;

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to write.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthWriter(string filename, FixedWidthOptions options = null)
            : base(Enumerable.Empty<FixedWidthField>(), filename, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter{T}"/> instance.
        /// </summary>
        /// <param name="filename">File to write.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthWriter(string filename, Encoding encoding, FixedWidthOptions options = null)
            : base(Enumerable.Empty<FixedWidthField>(), filename, encoding, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to write.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthWriter(Stream stream, FixedWidthOptions options = null)
            : base(Enumerable.Empty<FixedWidthField>(), stream, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthWriter{T}"/> instance.
        /// </summary>
        /// <param name="stream">Stream to write.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        public FixedWidthWriter(Stream stream, Encoding encoding, FixedWidthOptions options = null)
            : base(Enumerable.Empty<FixedWidthField>(), stream, encoding, options)
        {
            InitializeMemberDescriptors();
        }

        /// <summary>
        /// Initializes the class member descriptors.
        /// </summary>
        private void InitializeMemberDescriptors()
        {
            MemberDescriptors = MemberDescriptor.GetMemberDescriptors(typeof(T));
            Debug.Assert(Fields.Count == 0);
            Fields.AddRange(MemberDescriptors);
        }

        /// <summary>
        /// Writes the given item to the fixed-width file.
        /// </summary>
        /// <param name="item">The item to write</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Please use Write() instead.")]
        public void WriteItem(T item) => Write(item);

        /// <summary>
        /// Writes the given item to the fixed-width file.
        /// </summary>
        /// <param name="item">The item to write</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public void Write(T item)
        {
            // Ensure write buffers are allocated
            if (WriteValues == null || WriteValues.Length != MemberDescriptors.Count)
                WriteValues = new string[MemberDescriptors.Count];

            // Get values
            for (int i = 0; i < MemberDescriptors.Count; i++)
                WriteValues[i] = MemberDescriptors[i].GetValue(item);

            // Write values
            Write(WriteValues);
        }

        /// <summary>
        /// Writes the given items to the fixed-width file.
        /// </summary>
        /// <param name="items">The items to write</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FixedWidthOverflowException"></exception>
        public void Write(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (T item in items)
                Write(item);
        }
    }
}
