using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flood_Task
{
    class Cell
    {
        public State CellState { get; set; }
        public Cell()
        {
            CellState = State.Air;
        }
    }
}
