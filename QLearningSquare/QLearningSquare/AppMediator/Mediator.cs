using QLearningSquare.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearningSquare.DAO;
using Shared;
using QLearningSquare.Shared;

namespace QLearningSquare.AppMediator
{
    class Mediator
    {
        private static Mediator mediator = new Mediator();
        public static Dictionary<string, string> TranslateStates = new Dictionary<string, string>() {
                                { "up","Cima"},
                                { "down","Baixo"},
                                { "left","Esquerda"},
                                { "right","Direita"}
                            };

        //initialize classes
        public IGUIController pGUI = new GUIControl();
        public DataAcessObject pDAO = new DataAcessObject();
        QLearningController QLCtrl;
        Dictionary<int, StateType> TypeRewards = new Dictionary<int, StateType>();

        public static Mediator pMediator { get => mediator; }
        SafeTh RunStates=null;

        public Mediator()
        {
            pGUI.OnGuiLoaded = delegate ()
            {
                if (OpenParameters(@"Parameters.json"));
                    Init();
            };
            pGUI.OnGuiClose = delegate ()
            {
                SafeTh.StopAllThreads();
            };
        }

        private void Init()
        {
            try
            {

                //load the state Rewards and names
                QLCtrl = new QLearningController();
                List<List<int>> rewards = pDAO.getStateRewards();
                List<List<QLearningState>> states = new List<List<QLearningState>>();
                TypeRewards = pDAO.getStateRewardsTypes();
                
                bool haveLeft;
                bool haveRight;
                bool haveUp;
                bool haveDown;

                int n = 0;
                for (int i = 0; i < rewards.Count; i++)
                {

                    List<QLearningState> row = new List<QLearningState>();
                    states.Add(row);

                    for (int j = 0; j < rewards[i].Count; j++)
                    {
                        haveUp = !(i == 0) && (rewards[i - 1].Count > j);
                        haveDown = !(i == rewards.Count - 1) && (rewards[i + 1].Count > j);
                        haveLeft = !(j == 0);
                        haveRight = !(j == rewards[i].Count - 1);

                        QLearningState s = new QLearningState();
                        s.Name = "S" + (++n);
                        s.StateReward = rewards[i][j];
                        s.Actions = new Dictionary<string, QLearningAction>();
                        s.Type = GetStateType(rewards[i][j]);

                        if (haveUp)
                            s.Actions["up"] = new QLearningAction() { Name = "up", Reward = pDAO.getActionReward(s.Name, "up"),
                                StateResult = "S" + (n - rewards[i].Count) };
                        if (haveDown)
                            s.Actions["down"] = new QLearningAction() { Name = "down", Reward = pDAO.getActionReward(s.Name, "down"),
                                StateResult = "S" + (n + rewards[i].Count) };
                        if (haveLeft)
                            s.Actions["left"] = new QLearningAction() { Name = "left", Reward = pDAO.getActionReward(s.Name, "left"),
                                StateResult = "S" + (n - 1) };
                        if (haveRight)
                            s.Actions["right"] = new QLearningAction() { Name = "right", Reward = pDAO.getActionReward(s.Name, "right"),
                                StateResult = "S" + (n + 1) };

                        QLCtrl.AddState(s);
                        row.Add(s);
                    }
                }

                QLearningState sinitialState = QLCtrl.GetState( pDAO.getInitialStateName());
                QLearningWorker worker = new QLearningWorker() { CurrentState = sinitialState,IntialState = sinitialState };
                QLCtrl.SetWorker(worker);
                QLCtrl.Init();

                QLCtrl.ItsTerminal = delegate (QLearningEpisode e)
                {
                    StopQL();
                    string acts = "";
                    e.takenActions.ForEach(a => acts += TranslateStates[a.Name] + " -> ");
                    acts += "Estado final.";

                    pGUI.OnMessage("Provavelmente a melhor solução foi encontrada pelo agente !\r\n" +
                                    "Ações :" + acts + "\r\n"+
                                    "Total de Passos : " + e.takenActions.Count());
                };

                pGUI.SetStatesMatrix(states);
                pGUI.SetWoker(worker);
            }
            catch (Exception e)
            {
                LogHelper.cat("Mediator", "Exception loading parameters -> " + e.Message);
                pGUI.OnError("Falha na inicialização dos parâmetros dos estados");
            }
        }

        private bool OpenParameters(string filename)
        {
            bool parametersLoaded = false;

            try
            {
                pDAO.loadParameters(filename);
                parametersLoaded = true;
            }
            catch (Exception e)
            {
                LogHelper.cat("Mediator", "Exception loading parameters -> " + e.Message);
                pGUI.OnError("Falha ao abrir arquivo de parametros");
            }

            return parametersLoaded;
        }

        internal void OpenFile(string name)
        {
            StopQL();
            if (OpenParameters(name))
            {
                ResetQL();
            }
        }

        internal string getInitialStateName()
        {
            return pDAO.getInitialStateName();
        }

        internal void ResetQL()
        {
            StopQL();
            QLCtrl.Dispose();
            pGUI.ResetViews();
            Init();
        }

        internal void InitQL()
        {
            if(RunStates == null)
            {
                RunStates = new SafeTh() { Loop = true };
                RunStates.OnException = delegate (SafeTh sender, Exception e)
                {
                    LogHelper.cat("Mediator", "Exception running QLearningStates -> " + e.Message);
                    pGUI.OnError("Falha ao executar estados");
                };
            }

            if (RunStates.Running)
                RunStates.Stop();

            RunStates.LoopIntervalms = pGUI.AnimateInterval;
            RunStates.Start(delegate (SafeTh sender, object[] args)
            {
                QLCtrl.DoWork();
            });

        }

        internal void StopQL()
        {
            if (RunStates != null)
            {
                RunStates.Dispose();
                RunStates = null;
            }
        }

        public StateType GetStateType(int stateReward)
        {
            return TypeRewards[stateReward];
        }

        internal void AdvanceQL()
        {
            QLCtrl.DoWork();
        }
    }
}
