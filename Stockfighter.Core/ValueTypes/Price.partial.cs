using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockfighter.Core.ValueTypes
{
    public partial struct Price
    {
        /// <summary>
        /// Returns the value of <paramref name="price"/> divided by <paramref name="quantity"/>. 
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static Price operator /(Price price, Quantity quantity)
        {
            return new Price((int) Math.Round((double) price.Value/(double) quantity.Value, MidpointRounding.AwayFromZero));
        }
    }
}
