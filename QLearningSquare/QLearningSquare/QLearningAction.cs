using System.ComponentModel;

namespace QLearningSquare
{
    public class QLearningAction : INotifyPropertyChanged
    {
        private string name;
        private string stateResult;
        private int reward;

        public int Reward { get => reward;
            set
            {
                if (reward != value)
                {
                    reward = value;
                    RaisePropertyChanged("Reward");
                }
            }
        }

        public string StateResult { get => stateResult;
            set
            {
                if (stateResult != value)
                {
                    stateResult = value;
                    RaisePropertyChanged("StateResult");
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