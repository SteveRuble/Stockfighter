using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Stockfighter.Core.Book;
using Stockfighter.Core.ValueTypes;

namespace Stockfighter.Core.Subjects
{

    public static class PositionBroadcastBlock
    {
        public static BroadcastBlock<Position> Create(Symbol symbol)
        {

            var block = new BroadcastBlock<Position>(null);

            block.Post(new Position(symbol));

            return block;

        } 
    }


}
