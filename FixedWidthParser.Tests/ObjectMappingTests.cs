// Copyright (c) 2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using NUnit.Framework;
using SoftCircuits.Parsers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FixedWidthParserTests
{
    public class ObjectMappingTests
    {
        private class Product
        {
            [FixedWidthField(36)]
            public Guid Id { get; set; }
            [FixedWidthField(12)]
            public string Description { get; set; }
            [FixedWidthField(12)]
            public string Category { get; set; }
            [FixedWidthField(10)]
            public double Rating { get; set; }

            public override string ToString() => $"{Id}/{Category}/{Description}/{Rating}";
        }

        private class ProductComparer : IComparer, IComparer<Product>
        {
            public int Compare(object a, object b)
            {
                if (!(a is Product ta) || !(b is Product tb))
                    throw new InvalidOperationException();
                return Compare(ta, tb);
            }

            public int Compare([AllowNull] Product a, [AllowNull] Product b)
            {
                int result;

                result = a.Id.CompareTo(b.Id);
                if (result != 0) return result;
                result = a.Description.CompareTo(b.Description);
                if (result != 0) return result;
                result = a.Category.CompareTo(b.Category);
                if (result != 0) return result;
                result = a.Rating.CompareTo(b.Rating);
                return result;
            }
        }

        private readonly List<Product> Products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Description = "Coffee Table", Category = "Furniture", Rating = 4.5 },
            new Product { Id = Guid.NewGuid(), Description = "Spoons", Category = "Utensils", Rating = 4.2 },
            new Product { Id = Guid.NewGuid(), Description = "Carpet", Category = "Flooring", Rating = 4.5 },
            new Product { Id = Guid.NewGuid(), Description = "Knives", Category = "Utensils", Rating = 4.7 },
            new Product { Id = Guid.NewGuid(), Description = "Recliner", Category = "Furniture", Rating = 4.5 },
            new Product { Id = Guid.NewGuid(), Description = "Floor Tiles", Category = "Flooring", Rating = 4.5 },
        };

        [Test]
        public void BasicTest()
        {
            WriteReadValues(Products, out List<Product> results);
            CollectionAssert.AreEqual(Products, results, new ProductComparer());
        }

        #region Support methods

        internal void WriteReadValues<T1, T2>(List<T1> items, out List<T2> results, FixedWidthOptions options = null) where T1 : class, new() where T2 : class, new()
        {
            string path = Path.GetTempFileName();

            try
            {
                using (FixedWidthWriter<T1> writer = new FixedWidthWriter<T1>(path, options))
                {
                    foreach (var item in items)
                        writer.WriteItem(item);
                }

                results = new List<T2>();
                using (FixedWidthReader<T2> reader = new FixedWidthReader<T2>(path, options))
                {
                    while (reader.ReadItem(out T2 item))
                        results.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                File.Delete(path);
            }
        }

        #endregion

    }
}
