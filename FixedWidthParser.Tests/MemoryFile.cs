// Copyright (c) 2020-2025 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System.IO;
using System.Text;

namespace FixedWidthParserTests
{
    /// <summary>
    /// Helper class for reading and writing to memory files.
    /// </summary>
    public class MemoryFile
    {
        private MemoryStream? _Stream;

        /// <summary>
        /// Gets a memory stream for reading or writing.
        /// </summary>
        /// <param name="loadLastStreamContent">If true, the returned stream is loaded with the content
        /// written to the previous stream returned by this method.</param>
        public Stream GetStream(bool loadLastStreamContent = true)
        {
            MemoryStream? oldStream = _Stream;
            _Stream = new();
            if (loadLastStreamContent && oldStream != null)
            {
                _Stream.Write(oldStream.ToArray());
                _Stream.Seek(0, SeekOrigin.Begin);
            }
            return _Stream;
        }

        /// <summary>
        /// Gets the content of the most recent stream returned by <see cref="GetStream(bool)"/>.
        /// </summary>
        public string GetContent()
        {
            if (_Stream != null)
            {
                byte[] buffer = _Stream.ToArray();
                return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
            return string.Empty;
        }
    }
}
