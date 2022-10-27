// Copyright (c) 2020-2022 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using NUnit.Framework;
using SoftCircuits.Parsers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FixedWidthParserTests
{
    public class ManualMappingTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public class Person
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public string Country { get; set; }
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public List<Person> People = new()
        {
            new() { Id = 1, FirstName = "Bob", LastName = "Villa", Age = 27, Country = "USA" },
            new() { Id = 2, FirstName = "Jack", LastName = "Carpenter", Age = 18, Country = "USA" },
            new() { Id = 3, FirstName = "Susan", LastName = "Smith", Age = 33, Country = "France" },
            new() { Id = 4, FirstName = "Fred", LastName = "Wilson", Age = 21, Country = "USA" },
            new() { Id = 5, FirstName = "Matt", LastName = "Faust", Age = 29, Country = "USA" },
            new() { Id = 6, FirstName = "Ann", LastName = "Carter", Age = 41, Country = "UK" },
            new() { Id = 7, FirstName = "Sam", LastName = "Allen", Age = 38, Country = "USA" },
            new() { Id = 8, FirstName = "Roxanne", LastName = "Olsen", Age = 14, Country = "UK" },
            new() { Id = 9, FirstName = "Billy", LastName = "Benson", Age = 2, Country = "Canada" },
            new() { Id = 10, FirstName = "Janice", LastName = "Biden", Age = 80, Country = "USA" },
        };

        private class PersonComparer : IComparer, IComparer<Person>
        {
            public int Compare(object? a, object? b)
            {
                if (a is not Person ta || b is not Person tb)
                    throw new InvalidOperationException();
                return Compare(ta, tb);
            }

            public int Compare(Person? a, Person? b)
            {
                if (a != null && b != null)
                {
                    int result;

                    result = a.Id.CompareTo(b.Id);
                    if (result != 0) return result;
                    result = a.FirstName.CompareTo(b.FirstName);
                    if (result != 0) return result;
                    result = a.LastName.CompareTo(b.LastName);
                    if (result != 0) return result;
                    result = a.Age.CompareTo(b.Age);
                    if (result != 0) return result;
                    result = a.Country.CompareTo(b.Country);
                    return result;
                }

                if (a == null && b == null)
                    return 0;
                if (a == null)
                    return -1;
                return 1;
            }
        }


        [Test]
        public void Test()
        {
            ObjectMappingTests x = new();
            CollectionAssert.AreEqual(People, x.WriteReadValues(People, null, w => MapWriter(w), r => MapReader(r)), new PersonComparer());
        }

        private static void MapWriter(FixedWidthWriter<Person> writer)
        {
            writer.MapField(m => m.Id, 6);
            writer.MapField(m => m.FirstName, 20).SetPadCharacter('~');
            writer.MapField(m => m.LastName, 20);
            writer.MapField(m => m.Age, 6).SetAlignment(FieldAlignment.Right);
            writer.MapField(m => m.Country, 20);
        }

        private static void MapReader(FixedWidthReader<Person> reader)
        {
            reader.MapField(m => m.Id, 6);
            reader.MapField(m => m.FirstName, 20).SetPadCharacter('~');
            reader.MapField(m => m.LastName, 20);
            reader.MapField(m => m.Age, 6).SetAlignment(FieldAlignment.Right);
            reader.MapField(m => m.Country, 20);
        }
    }
}
