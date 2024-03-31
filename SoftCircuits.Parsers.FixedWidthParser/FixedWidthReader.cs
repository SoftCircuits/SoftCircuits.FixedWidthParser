// Copyright (c) 2020-2024 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SoftCircuits.Parsers
{
    /// <summary>
    /// Class to read fixed-width fields from a file.
    /// </summary>
    public class FixedWidthReader : IDisposable
    {
        private readonly StreamReader Reader;

        /// <summary>
        /// Options used by this reader.
        /// </summary>
        protected readonly FixedWidthOptions Options;

        /// <summary>
        /// Gets the list of fixed-width field descriptors.
        /// </summary>
        protected List<FixedWidthField> Fields { get; set; }

        /// <summary>
        /// Returns the original unparsed line of text last read by <see cref="Read()"/> or <see cref="ReadAsync()"/>. Valid only
        /// when <see cref="Read()"/> returns true.
        /// </summary>
        public string? CurrentLine { get; private set; }

        /// <summary>
        /// Gets the column values read by the last call to <see cref="Read()"/> or <see cref="ReadAsync()"/>. Guaranteed not to be null
        /// when these methods return true.
        /// </summary>
        [Obsolete("This property has been deprecated and will be removed in a future version. Please use the Values property instead.")]
        public string[]? Columns => Values;

        /// <summary>
        /// Gets the column values read by the last call to <see cref="Read()"/> or <see cref="ReadAsync()"/>. Guaranteed not to be null
        /// when these methods return true.
        /// </summary>
        public string[]? Values { get; private set; }

        /// <summary>
        /// Returns true if the current file position is at the end of the file.
        /// </summary>
        public bool EndOfFile => Reader.EndOfStream;

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="filename">File to read.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, string filename, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(filename);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="filename">File to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, string filename, Encoding encoding, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(filename, encoding);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="filename">File to read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, string filename, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(filename, detectEncodingFromByteOrderMarks);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="filename">File to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, string filename, Encoding encoding, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(filename, encoding, detectEncodingFromByteOrderMarks);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="stream">Stream to read.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, Stream stream, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(stream);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="stream">Stream to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, Stream stream, Encoding encoding, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(stream, encoding);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="stream">Stream to read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, Stream stream, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(stream, detectEncodingFromByteOrderMarks);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Constructs a new <see cref="FixedWidthReader"/> instance.
        /// </summary>
        /// <param name="fields">List of fixed-width field descriptors.</param>
        /// <param name="stream">Stream to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="options">Library options. Leave as null to use the default options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FixedWidthReader(IEnumerable<FixedWidthField> fields, Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, FixedWidthOptions? options = null)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            Fields = new List<FixedWidthField>(fields);
            Reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks);
            Options = options ?? new FixedWidthOptions();
        }

        /// <summary>
        /// Reads one row of values.
        /// </summary>
        /// <param name="values">Returns the values read when this method returns true.</param>
        /// <returns>True if successful, or false if there are no more rows.</returns>
#if NETSTANDARD2_0
        public bool Read(out string[] values)
#else
        [MemberNotNullWhen(true, nameof(CurrentLine))]
        [MemberNotNullWhen(true, nameof(Values))]
        public bool Read([NotNullWhen(true)] out string[]? values)
#endif
        {
            // Get next line
            CurrentLine = Reader.ReadLine();
            if (CurrentLine != null)
            {
                ParseLine();
                values = Values;
                return true;
            }
            values = null;
            return false;
        }

        /// <summary>
        /// Reads one row of values and stores them in the <see cref="Values"/> property.
        /// </summary>
        /// <returns>True if successful, or false if there are no more rows.</returns>
#if !NETSTANDARD2_0
        [MemberNotNullWhen(true, nameof(CurrentLine))]
        [MemberNotNullWhen(true, nameof(Values))]
#endif
        public bool Read()
        {
            // Get next line
            CurrentLine = Reader.ReadLine();
            if (CurrentLine != null)
            {
                ParseLine();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Asynchronously reads one row of values and stores them in the <see cref="Values"/> property.
        /// </summary>
        /// <returns>True if successful, or false if there are no more rows.</returns>
        /// <remarks>
        /// Note: The <see cref="Values"/> property is guaranteed not to be null when this method returns true. However, .NET does not currently
        /// support the <see cref="MemberNotNullWhenAttribute"/> attribute for async methods. So the compiler may generate warnings
        /// when nullable reference types are enabled. In this case, you can safely use the null-forgiving operator (!) when this
        /// method returns true.
        /// </remarks>
#if !NETSTANDARD2_0
        [MemberNotNullWhen(true, nameof(CurrentLine))]  // Note: Ignored with async methods
        [MemberNotNullWhen(true, nameof(Values))]       // Note: Ignored with async methods
#endif
        public async Task<bool> ReadAsync()
        {
            // Get next line
            CurrentLine = await Reader.ReadLineAsync();
            if (CurrentLine != null)
            {
                ParseLine();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Closes the <see cref="FixedWidthReader"/> object and underlying stream, and releases
        /// any system resources associated with the reader.
        /// </summary>
        public void Close() => Reader.Close();

        /// <summary>
        /// Parses a single fixed-width line.
        /// </summary>
#if !NETSTANDARD2_0
        [MemberNotNull(nameof(Values))]
#endif
        private void ParseLine()
        {
            Debug.Assert(CurrentLine != null);

            // If needed, allocate array to return values
            if (Values == null || Values.Length != Fields.Count)
                Values = new string[Fields.Count];

            // Read fields
            int position = 0;
            for (int i = 0; i < Fields.Count; i++)
                Values[i] = ParseField(CurrentLine, Fields[i], ref position);
        }

        /// <summary>
        /// Reads the specified field from the given line.
        /// </summary>
        /// <param name="line">The line that contains all the fields.</param>
        /// <param name="field">Field descriptor.</param>
        /// <param name="position">The current position within the liine.</param>
        /// <returns>The extracted field.</returns>
        private string ParseField(string line, FixedWidthField field, ref int position)
        {
            Debug.Assert(line != null);
            Debug.Assert(field != null);
            Debug.Assert(position >= 0);

            // Skip requested characters
            position += field.Skip;

            // Extract field
            int start, end;
            if (line.Length < position + field.Length)
            {
                if (Options.ThrowOutOfRangeException)
                    throw new FixedWidthOutOfRangeException();
                start = (position <= line.Length) ? position : line.Length;
                end = line.Length;
            }
            else
            {
                start = position;
                end = position + field.Length;
            }

            // Trim field if requested
            if (field.TrimField ?? Options.TrimFields)
            {
                char padCharacter = field.PadCharacter ?? Options.DefaultPadCharacter;
                // Trim start
                while (start < end && line[start] == padCharacter)
                    start++;
                // Trim end
                while (end > start && line[end - 1] == padCharacter)
                    end--;
            }

            // Advance line position
            position += field.Length;

            // Return field value
#if NETSTANDARD2_0
            return line.Substring(start, end - start);
#else
            return line[start..end];
#endif
        }

        /// <summary>
        /// Releases all resources used by the <see cref="FixedWidthReader"/> object.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Reader.Dispose();
        }
    }
}
