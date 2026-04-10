using NUnit.Framework;
using NUnit.Framework.Legacy;
using SoftCircuits.Parsers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FixedWidthParserTests
{
    internal class CustomConverterTests
    {
        private const int BirthDateLength = 20;

        private class Person
        {
            [FixedWidthField(8)]
            public int Id { get; set; }
            [FixedWidthField(12)]
            public string FirstName { get; set; }
            [FixedWidthField(12)]
            public string LastName { get; set; }
            [FixedWidthField(BirthDateLength, ConverterType = typeof(BirthdateConverter))]
            public DateTime BirthDate { get; set; }

            public Person()
            {
                FirstName = LastName = string.Empty;
            }
        }

        private class PersonWithDescriptor
        {
            [FixedWidthField(8)]
            public int Id { get; set; }
            [FixedWidthField(12)]
            public string FirstName { get; set; }
            [FixedWidthField(12)]
            public string LastName { get; set; }
            [FixedWidthField(BirthDateLength, ConverterType = typeof(BirthdateConverterWithDescriptor))]
            public DateTime BirthDate { get; set; }

            public PersonWithDescriptor()
            {
                FirstName = LastName = string.Empty;
            }
        }

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
                    result = a.BirthDate.CompareTo(b.BirthDate);
                    return result;
                }

                if (a == null && b == null)
                    return 0;
                if (a == null)
                    return -1;
                return 1;
            }
        }

        private class PersonWithDescriptorComparer : IComparer, IComparer<PersonWithDescriptor>
        {
            public int Compare(object? a, object? b)
            {
                if (a is not PersonWithDescriptor ta || b is not PersonWithDescriptor tb)
                    throw new InvalidOperationException();
                return Compare(ta, tb);
            }

            public int Compare(PersonWithDescriptor? a, PersonWithDescriptor? b)
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
                    result = a.BirthDate.CompareTo(b.BirthDate);
                    return result;
                }

                if (a == null && b == null)
                    return 0;
                if (a == null)
                    return -1;
                return 1;
            }
        }

        private class BirthdateConverter() : DataConverter<DateTime>
        {
            private const string Format = "yyyyMMdd";

            public override string ConvertToString(DateTime value) => value.ToString(Format);

            public override bool TryConvertFromString(string? s, out DateTime value)
            {
                return DateTime.TryParseExact(s, Format, null, System.Globalization.DateTimeStyles.None, out value);
            }
        }

        private class BirthdateConverterWithDescriptor(FixedWidthDescriptor field) : DataConverter<DateTime>
        {
            private const string Format = "yyyyMMdd";

            private readonly FixedWidthDescriptor Field = field;

            public override string ConvertToString(DateTime value) => value.ToString(Format);

            public override bool TryConvertFromString(string? s, out DateTime value)
            {
                Assert.That(Field != null);
                Assert.That(Field!.Name == nameof(PersonWithDescriptor.BirthDate));
                Assert.That(Field!.Length == BirthDateLength);
                return DateTime.TryParseExact(s, Format, null, System.Globalization.DateTimeStyles.None, out value);
            }
        }

        private readonly List<Person> People =
        [
            new Person { Id = 1, FirstName = "Bill", LastName = "Smith", BirthDate = new DateTime(1982, 2, 7) },
            new Person { Id = 2, FirstName = "Gary", LastName = "Parker", BirthDate = new DateTime(1989, 8, 2) },
            new Person { Id = 3, FirstName = "Karen", LastName = "Wilson", BirthDate = new DateTime(1978, 6, 24) },
            new Person { Id = 4, FirstName = "Jeff", LastName = "Johnson", BirthDate = new DateTime(1972, 4, 18) },
            new Person { Id = 5, FirstName = "John", LastName = "Carter", BirthDate = new DateTime(1982, 12, 21) },
        ];

        private readonly List<PersonWithDescriptor> PeopleWithDescriptors =
        [
            new PersonWithDescriptor { Id = 1, FirstName = "Bill", LastName = "Smith", BirthDate = new DateTime(1982, 2, 7) },
            new PersonWithDescriptor { Id = 2, FirstName = "Gary", LastName = "Parker", BirthDate = new DateTime(1989, 8, 2) },
            new PersonWithDescriptor { Id = 3, FirstName = "Karen", LastName = "Wilson", BirthDate = new DateTime(1978, 6, 24) },
            new PersonWithDescriptor { Id = 4, FirstName = "Jeff", LastName = "Johnson", BirthDate = new DateTime(1972, 4, 18) },
            new PersonWithDescriptor { Id = 5, FirstName = "John", LastName = "Carter", BirthDate = new DateTime(1982, 12, 21) },
        ];

        [Test]
        public void TestCustomConverter()
        {
            ObjectMappingTests x = new();
            CollectionAssert.AreEqual(People, ObjectMappingTests.WriteReadValues(People), new PersonComparer());

            // Test predefined custom converters
            CollectionAssert.AreEqual(People, ObjectMappingTests.WriteReadValues(People, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(UniversalDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(UniversalDateTimeConverter))),
                new PersonComparer());
            CollectionAssert.AreEqual(People, ObjectMappingTests.WriteReadValues(People, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(CompactDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(CompactDateTimeConverter))),
                new PersonComparer());
            CollectionAssert.AreEqual(People, ObjectMappingTests.WriteReadValues(People, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(DateOnlyDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(DateOnlyDateTimeConverter))),
                new PersonComparer());
        }

        /// <summary>
        /// Test custom converter with a constructor that accepts a <see cref="FixedWidthDescriptor"/> parameter.
        /// </summary>
        [Test]
        public void TestCustomConverterWithDescriptors()
        {
            ObjectMappingTests x = new();
            CollectionAssert.AreEqual(PeopleWithDescriptors, ObjectMappingTests.WriteReadValues(PeopleWithDescriptors), new PersonWithDescriptorComparer());

            // Test predefined custom converters
            CollectionAssert.AreEqual(PeopleWithDescriptors, ObjectMappingTests.WriteReadValues(PeopleWithDescriptors, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(UniversalDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(UniversalDateTimeConverter))),
                new PersonWithDescriptorComparer());
            CollectionAssert.AreEqual(PeopleWithDescriptors, ObjectMappingTests.WriteReadValues(PeopleWithDescriptors, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(CompactDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(CompactDateTimeConverter))),
                new PersonWithDescriptorComparer());
            CollectionAssert.AreEqual(PeopleWithDescriptors, ObjectMappingTests.WriteReadValues(PeopleWithDescriptors, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(DateOnlyDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(DateOnlyDateTimeConverter))),
                new PersonWithDescriptorComparer());
        }
    }
}
