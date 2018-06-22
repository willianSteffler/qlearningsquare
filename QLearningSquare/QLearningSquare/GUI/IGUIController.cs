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

    interface IGUIController
    {
        void onError(string errorMessage);
        void MoveSquare(SquareDirections dir);

        Action OnGuiLoaded { get; set; }

    }
}
