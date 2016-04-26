using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Stockfighter.Core;
using Stockfighter.Core.ValueTypes;
using Xunit;

namespace Stockfighter.Tests.ValueTypes
{
    public class JsonConversionTests
    {
        private class Container
        {
            public OrderDirection OrderDirection { get; set; }
            public Price Price { get; set; }
        }

        [Fact]
        public void RoundTrip()
        {
            var expected = new Container()
                           {
                               OrderDirection = OrderDirection.Buy,
                               Price = new Price(100)
                           };

            var json = JsonConvert.SerializeObject(expected, CoreModule.JsonSerializerSettings);
            Console.WriteLine(json);

            var actual = JsonConvert.DeserializeObject<Container>(json, CoreModule.JsonSerializerSettings);

            actual.OrderDirection.Value.Should().Be(expected.OrderDirection.Value);
            actual.Price.Value.Should().Be(expected.Price.Value);
        }
        
    }
}
