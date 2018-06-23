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

                    //load the state Rewards and names
                    List<List<int>> rewards = pDAO.getStateRewards();
                    List<List<string>> stateNames = new List<List<string>>();

                    int n = 0;
                    for (int i = 0; i < rewards.Count; i++)
                    {
                        List<string> row = new List<string>();
                        stateNames.Add(row);

                        for (int j = 0; j < rewards[i].Count; j++)
                        {
                            QLearningState s = new QLearningState();
                            s.Name = "S" + (++n);
                            s.StateReward = rewards[i][j];
                            QLCtrl.AddState(s);

                            row.Add(s.Name);
                        }
                        n++;
                    }

                    pGUI.SetStatesMatrix(stateNames);
                    parametersLoaded = true;
                }
                catch(Exception e)
                {
                    LogHelper.cat("Mediator", "Exception loading parameters -> " + e.Message);
                    pGUI.OnError("Falha na inicialização dos parâmetros dos estados");
                }

            };
        }
    }
}
