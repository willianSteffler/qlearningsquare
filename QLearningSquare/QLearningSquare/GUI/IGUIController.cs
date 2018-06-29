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
        void OnError(string errorMessage);
        void setWorkerState(QLearningState workerState);
        void SetStatesMatrix(List<List<QLearningState>> statesMatrix);
        Action OnGuiLoaded { get; set; }
    }
}
