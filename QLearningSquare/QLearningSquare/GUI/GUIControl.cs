using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare.GUI
{
    public enum SquareDirections
    {
        left,
        right,
        up,
        down
    }

    class GUIControl
    {
        AppMediator.Mediator ctrl = AppMediator.Mediator.pMediator;

        public void MoveSquare(SquareDirections dir)
        {

        }
    }
}
