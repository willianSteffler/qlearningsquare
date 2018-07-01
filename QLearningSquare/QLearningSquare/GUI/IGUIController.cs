using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare.GUI
{
    interface IGUIController
    {
        int AnimateInterval { get; set; }
        bool AutoAnimate { get; set; }
        void OnError(string errorMessage);
        void SetWoker(QLearningWorker worker);
        void SetStatesMatrix(List<List<QLearningState>> statesMatrix);
        Action OnGuiLoaded { get; set; }
        Action OnGuiClose { get; set; }
        void ResetViews();
    }
}
