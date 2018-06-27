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

                    bool haveLeft;
                    bool haveRight;
                    bool haveUp;
                    bool haveDown;

                    int n = 0;
                    for (int i = 0; i < rewards.Count; i++)
                    {
                        haveUp = !(i == 0);
                        haveDown = !(i == rewards.Count -1);

                        List<string> row = new List<string>();
                        stateNames.Add(row);

                        for (int j = 0; j < rewards[i].Count; j++)
                        {
                            haveLeft = !(j == 0);
                            haveRight = !(j == rewards[i].Count - 1);

                            QLearningState s = new QLearningState();
                            s.Name = "S" + (++n);
                            s.StateReward = rewards[i][j];
                            s.Siblings = new List<string>();
                            s.Actions = new Dictionary<string, int>();

                            if (haveUp)
                            {
                                s.Actions["up"] = pDAO.getActionReward(s.Name, "up");
                                s.Siblings.Add("S" + (n - rewards[i - 1].Count));
                            }
                            if (haveDown)
                            {
                                s.Actions["down"] = pDAO.getActionReward(s.Name, "down");
                                s.Siblings.Add("S" + (n + rewards[i + 1].Count));
                            }
                            if (haveLeft)
                            {
                                s.Actions["left"] = pDAO.getActionReward(s.Name, "left");
                                s.Siblings.Add("S" + (n-1));
                            }
                            if (haveRight)
                            {
                                s.Actions["right"] = pDAO.getActionReward(s.Name, "right");
                                s.Siblings.Add("S" + (n +1));
                            }

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
