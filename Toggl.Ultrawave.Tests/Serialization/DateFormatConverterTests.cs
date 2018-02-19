﻿using System;
using FluentAssertions;
using Newtonsoft.Json;
using Toggl.Multivac;
using Toggl.Ultrawave.Serialization.Converters;
using Xunit;

namespace Toggl.Ultrawave.Tests.Serialization
{
    public sealed class DateFormatConverterTests
    {
        public sealed class TheCanConvertMethod
        {
            private readonly DateFormatConverter converter = new DateFormatConverter();

            [Fact]
            public void ReturnsTrueWhenObjectTypeIsDateFormat()
            {
                converter.CanConvert(typeof(DateFormat)).Should().BeTrue();
            }

            public void ReturnsFalseForAnyOtherType(Type type)
            {
                if (type == typeof(DateFormat)) return;

                converter.CanConvert(type).Should().BeFalse();
            }
        }

        public sealed class TheDateFormatStruct
        {
            private sealed class ClassWithDateFormat
            {
                [JsonConverter(typeof(DateFormatConverter))]
                public DateFormat DateFormat { get; set; }
            }

            private readonly string validJson = "{\"date_format\":\"MM.DD.YYYY\"}";
            private readonly ClassWithDateFormat validObject = new ClassWithDateFormat
            {
                DateFormat = DateFormat.FromLocalizedDateFormat("MM.DD.YYYY")
            };

            [Fact]
            public void CanBeSerialized()
            {
                SerializationHelper.CanBeSerialized(validJson, validObject);
            }

            [Fact]
            public void CanBeDeserialized()
            {
                SerializationHelper.CanBeDeserialized(validJson, validObject);
            }
        }
    }
}
