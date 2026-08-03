// Copyright (c) 2020-2026 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.Parsers;

namespace FixedWidthParser.Tests
{
    public class CustomConverterTests
    {
        private const int BirthDateLength = 20;

        public class Person
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

        public class PersonWithDescriptor
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

        public class PersonComparer : IEqualityComparer<Person>
        {
            public bool Equals(Person? x, Person? y)
            {
                if (x is null && y is null) return true;
                if (x is null || y is null) return false;
                return x.Id == y.Id && x.FirstName == y.FirstName && x.LastName == y.LastName && x.BirthDate == y.BirthDate;
            }

            public int GetHashCode(Person obj) =>
                HashCode.Combine(obj.Id, obj.FirstName, obj.LastName, obj.BirthDate);
        }

        public class PersonWithDescriptorComparer : IEqualityComparer<PersonWithDescriptor>
        {
            public bool Equals(PersonWithDescriptor? x, PersonWithDescriptor? y)
            {
                if (x is null && y is null) return true;
                if (x is null || y is null) return false;
                return x.Id == y.Id && x.FirstName == y.FirstName && x.LastName == y.LastName && x.BirthDate == y.BirthDate;
            }

            public int GetHashCode(PersonWithDescriptor obj) =>
                HashCode.Combine(obj.Id, obj.FirstName, obj.LastName, obj.BirthDate);
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
                Assert.NotNull(Field);
                Assert.Equal(nameof(PersonWithDescriptor.BirthDate), Field!.Name);
                Assert.Equal(BirthDateLength, Field!.Length);
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

        [Fact]
        public void TestCustomConverter()
        {
            ObjectMappingTests x = new();
            Assert.Equal(People, ObjectMappingTests.WriteReadValues(People), new PersonComparer());

            // Test predefined custom converters
            Assert.Equal(People, ObjectMappingTests.WriteReadValues(People, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(UniversalDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(UniversalDateTimeConverter))),
                new PersonComparer());
            Assert.Equal(People, ObjectMappingTests.WriteReadValues(People, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(CompactDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(CompactDateTimeConverter))),
                new PersonComparer());
            Assert.Equal(People, ObjectMappingTests.WriteReadValues(People, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(DateOnlyDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(DateOnlyDateTimeConverter))),
                new PersonComparer());
        }

        /// <summary>
        /// Test custom converter with a constructor that accepts a <see cref="FixedWidthDescriptor"/> parameter.
        /// </summary>
        [Fact]
        public void TestCustomConverterWithDescriptors()
        {
            ObjectMappingTests x = new();
            Assert.Equal(PeopleWithDescriptors, ObjectMappingTests.WriteReadValues(PeopleWithDescriptors), new PersonWithDescriptorComparer());

            // Test predefined custom converters
            Assert.Equal(PeopleWithDescriptors, ObjectMappingTests.WriteReadValues(PeopleWithDescriptors, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(UniversalDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(UniversalDateTimeConverter))),
                new PersonWithDescriptorComparer());
            Assert.Equal(PeopleWithDescriptors, ObjectMappingTests.WriteReadValues(PeopleWithDescriptors, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(CompactDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(CompactDateTimeConverter))),
                new PersonWithDescriptorComparer());
            Assert.Equal(PeopleWithDescriptors, ObjectMappingTests.WriteReadValues(PeopleWithDescriptors, null,
                w => w.MapField(m => m.BirthDate, 20).SetConverterType(typeof(DateOnlyDateTimeConverter)),
                r => r.MapField(m => m.BirthDate, 20).SetConverterType(typeof(DateOnlyDateTimeConverter))),
                new PersonWithDescriptorComparer());
        }
    }
}
