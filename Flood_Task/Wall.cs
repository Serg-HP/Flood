using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flood_Task
{
    class Wall
    {
        public Point FirstPoint { get; set; }
        public Point SecondPoint { get; set; }

        public Wall(Point first, Point second)
        {
            this.FirstPoint = first;
            this.SecondPoint = second;
        }

        public bool IsVertical()
        {
            return this.FirstPoint.Y == this.SecondPoint.Y;
        }

        public bool IsHorizontal()
        {
            return this.FirstPoint.X == this.SecondPoint.X;
        }

        public void Swap()
        {
            Point temp = this.FirstPoint;
            this.FirstPoint = this.SecondPoint;
            this.SecondPoint = temp;
        }

        public void MoveFirstPointRight()
        {
            var copy = this.FirstPoint;
            copy.Y++;
            this.FirstPoint = copy;
        }

        public void MoveFirstPointTop()
        {
            var copy = this.FirstPoint;
            copy.X++;
            this.FirstPoint = copy;
        }

        public void MoveSecondPointRight()
        {
            var copy = this.SecondPoint;
            copy.Y++;
            this.SecondPoint = copy;
        }

        public void MoveSecondPointTop()
        {
            var copy = this.SecondPoint;
            copy.X++;
            this.SecondPoint = copy;
        }
    }
}
