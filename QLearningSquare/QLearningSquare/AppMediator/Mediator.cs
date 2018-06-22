using QLearningSquare.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearningSquare.DAO;
using Shared;

namespace QLearningSquare.AppMediator
{
    class Mediator
    {
        private static Mediator mediator = new Mediator();

        //initialize classes
        public IGUIController pGUI = new GUIControl();
        public DataAcessObject pDAO = new DataAcessObject();
        QLearningController QLCtrl = new QLearningController();

        public static Mediator pMediator { get => mediator; }

        bool parametersLoaded = false;

        public Mediator()
        {
            pGUI.OnGuiLoaded = delegate ()
            {
                try
                {
                    pDAO.loadParameters(@"Parameters.json");
                    parametersLoaded = true;
                }
                catch(Exception e)
                {
                    LogHelper.cat("Mediator", "Exception opening parameters file: " + e.Message);
                    pGUI.onError("Falha na inicialização dos parâmetros");
                }

                if (parametersLoaded)
                {
                    //load the state Rewards
                    List<List<int>> rewards = pDAO.getStateRewards();
                    int n = 0;
                    for (int i = 0; i < rewards.Count; i++)
                    {
                        for (int j = 0; j < rewards[i].Count; j++)
                        {
                            QLearningState s = new QLearningState();
                            s.Name = "S" + (++n);
                            s.StateReward = rewards[i][j];
                            QLCtrl.AddState(s);
                        }
                        n++;
                    }
                }

            };
        }
    }
}
