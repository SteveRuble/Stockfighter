using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Stockfighter.Core.Book;
using Stockfighter.Core.ValueTypes;
using Xunit;
using static Stockfighter.Core.ValueTypes.ValueTypeFactory;
using Quantity = Stockfighter.Core.ValueTypes.Quantity;

namespace Stockfighter.Tests.Books
{
    public class PositionTests
    {
        private static readonly Symbol Symbol = Symbol("TEST");

        [Fact]
        public void increase()
        {
            var expectedQuantity = Quantity(10);
            var expectedPrice = Price(100);
            var target = new Position(Symbol);
            var actual = target.Increase(expectedQuantity, expectedPrice);
            actual.Quantity.Should().Be(expectedQuantity);
            actual.Basis.Should().Be(expectedPrice);
        }

        [Fact]
        public void reduce_decrements_count()
        {
            var target = new Position(Symbol).Increase(Quantity(10), Price(200));
            var actual = target.Reduce(Quantity(5), Price(100));

            actual.Quantity.Should().Be(Quantity(5));
            actual.Basis.Should().Be(Price(200));
        }

        [Fact]
        public void reduce_throws_when_quantity_sold_is_greater_than_current_quantity()
        {
            var target = new Position(Symbol).Increase(Quantity(10), Price(200));
            Action action = () => target.Reduce(Quantity(15), Price(100));
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }


    }
}
