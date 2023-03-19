using ExchangeRateTransfer.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xunit;

namespace ExchangeRateTransfer.UnitTests.Units
{
    public class JsonExtensionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("{\"Name\":\"serialize name\",\"No\":123,\"NoNullable\":123.4,\"Date\":\"2021-09-26T00:00:00+00:00\",\"DateNullable\":\"2021-09-26T00:00:00+00:00\",\"AEnum\":\"Fail\",\"AEnumNullable\":\"Success\"}")]
        public void CanDeserialize(string value)
        {
            var obj = JsonExtensions.FromJson<AClass>(value);

            if (string.IsNullOrWhiteSpace(value))
                Assert.Null(obj);
            else
                Assert.NotNull(obj);

            if (obj != null && obj.Name == "serialize name")
            {
                AClass a = new()
                {
                    AEnum = AEnum.Fail,
                    AEnumNullable = AEnum.Success,
                    Date = new DateTimeOffset(2021, 09, 26, 0, 0, 0, TimeSpan.Zero),
                    DateNullable = new DateTimeOffset(2021, 09, 26, 0, 0, 0, TimeSpan.Zero),
                    Name = "serialize name",
                    No = 123,
                    NoNullable = 123.4m,
                };

                Assert.Equal(a.AEnum, obj.AEnum);
                Assert.Equal(a.AEnumNullable, obj.AEnumNullable);
                Assert.Equal(a.Date, obj.Date);
                Assert.Equal(a.DateNullable, obj.DateNullable);
                Assert.Equal(a.Name, obj.Name);
                Assert.Equal(a.No, obj.No);
                Assert.Equal(a.NoNullable, obj.NoNullable);
            }
        }

        [Fact]
        public void CanSerialize()
        {
            AClass a = new()
            {
                AEnum = AEnum.Fail,
                AEnum2 = AEnum.Cancel,
                AEnumNullable = AEnum.Success,
                Date = new DateTimeOffset(2021, 09, 26, 0, 0, 0, TimeSpan.Zero),
                DateNullable = new DateTimeOffset(2021, 09, 26, 0, 0, 0, TimeSpan.Zero),
                Name = "serialize name",
                No = 123,
                NoNullable = 123.4m,
            };

            var str = JsonExtensions.ToJson(a);

            Assert.NotNull(str);

            Assert.Equal(
                "{\"Name\":\"serialize name\",\"No\":123,\"NoNullable\":123.4,\"Date\":\"2021-09-26T00:00:00+00:00\",\"DateNullable\":\"2021-09-26T00:00:00+00:00\",\"AEnum\":\"Fail\",\"AEnumNullable\":\"Success\",\"AEnum2\":2}",
                str
                );
        }
    }

    internal class AClass
    {
        public string? Name { get; set; }
        public int No { get; set; }
        public decimal? NoNullable { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset? DateNullable { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AEnum AEnum { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AEnum? AEnumNullable { get; set; }

        public AEnum AEnum2 { get; set; }
    }

#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
    internal enum AEnum
#pragma warning restore S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
    {
        Success,
        Fail,
        Cancel,
    }
}
