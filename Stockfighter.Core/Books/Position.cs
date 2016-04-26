using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stockfighter.Core.Api;
using Stockfighter.Core.ValueTypes;

namespace Stockfighter.Core.Book
{
    public class Position
    {
        public Symbol Symbol { get; }
        public Quantity Quantity { get; }

        public Price Basis { get; }

        public Price AverageCostBasis => Basis / Quantity;

        public Position(Symbol symbol)
        {
            Symbol = symbol;
        }

        private Position(Symbol symbol, Quantity quantity, Price basis)
        {
            Symbol = symbol;
            Quantity = quantity;
            Basis = basis;
        }

        public Position Increase(Quantity quantity, Price atPrice)
        {
            var newQuantity = quantity + Quantity;
            var newAverage = ((Quantity.Value*Basis.Value) + (quantity.Value*atPrice.Value))/newQuantity.Value;

            return new Position(Symbol, newQuantity, new Price(newAverage));
        }

        public Position Reduce(Quantity quantity, Price atPrice)
        {
            if (Quantity < quantity)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), quantity, $"Only {Quantity} in inventory.");
            }

            var newQuantity = Quantity - quantity;

            return new Position(Symbol, newQuantity, Basis);
        }
    }




}
