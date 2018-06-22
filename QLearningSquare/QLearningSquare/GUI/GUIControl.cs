using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare.GUI
{

    class GUIControl : IGUIController
    {
        AppMediator.Mediator ctrl = AppMediator.Mediator.pMediator;

        Action onGUILoaded = null;
        public Action OnGuiLoaded { get => onGUILoaded; set => onGUILoaded = value; }

        public void MoveSquare(SquareDirections dir)
        {
            throw new NotImplementedException();
        }

        public void onError(string errorMessage)
        {
            throw new NotImplementedException();
        }

        public void onMainWindowLoaded()
        {
            onGUILoaded();
        }
    }
}
