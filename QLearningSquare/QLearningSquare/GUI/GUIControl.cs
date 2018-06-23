using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLearningSquare.GUI
{

    class GUIControl : IGUIController
    {
        AppMediator.Mediator ctrl = AppMediator.Mediator.pMediator;
        public MainWindow pMainWindow;

        Action onGUILoaded = null;
        public Action OnGuiLoaded { get => onGUILoaded; set => onGUILoaded = value; }

        public void MoveSquare(string state)
        {
            throw new NotImplementedException();
        }

        public void OnError(string errorMessage)
        {
            MessageBox.Show(errorMessage);
        }

        public void onMainWindowLoaded()
        {
            onGUILoaded();
        }

        public void SetStatesMatrix(List<List<string>> statesMatrix)
        {

        }
    }
}
