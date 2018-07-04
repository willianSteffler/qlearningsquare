using QLearningSquare.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare
{
    public enum StateType
    {
        AvoidState,
        GoalState,
        NormalState,
        Invalid
    }

    public class QLearningState : INotifyPropertyChanged
    {
        private Dictionary<string, QLearningAction> actions;

        private double stateReward;
        private string name;
        private StateType type;
        public int Hits;

        public double StateReward { get => stateReward;
            set
            {
                if (stateReward != value)
                {
                    stateReward = value;
                    RaisePropertyChanged("StateReward");
                }
            }
        }
        public string Name { get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public Dictionary<string, QLearningAction> Actions { get => actions;
            set
            {
                actions = value;
                RaisePropertyChanged("Actions");
            }
        }

        public StateType Type { get => type;
            set
            {
                if (type != value)
                {
                    type = value;
                    RaisePropertyChanged("Type");
                }
            }
        }

        public void AddAction(QLearningAction action)
        {
            Actions.Add(action.Name, action);
            RaisePropertyChanged("Actions["+ action.Name+"]");
        }

        public QLearningAction GetAction(string actionName)
        {
            if (Actions.ContainsKey(actionName))
                return Actions[actionName];
            else
                return null;
        }

        public int GetActionsCount()
        {
            return Actions.Count;
        }

        public QLearningAction Max(bool useRandom)
        {
            QLearningAction ret;
            if (type == StateType.GoalState)
                ret = new QLearningAction() { Reward = StateReward };
            else
            {
                ret = useRandom ? Enumerable.ToList(actions.Values)[RandomExtensions.GetPseudoRandom(0, Actions.Values.Count)] :
                    (Enumerable.ToList(actions.Values).Max());
            }

            return ret;
        }

        public bool ActionsAreEqual()
        {
            return (Enumerable.ToList(actions.Values).FindAll(act => act.CompareTo(Max(false)) == 0)).Count > 1;
        } 

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
