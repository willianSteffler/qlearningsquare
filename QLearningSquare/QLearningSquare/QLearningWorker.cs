using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare
{
    public class QLearningWorker : INotifyPropertyChanged
    {
        int steps;
        public QLearningState IntialState = null;
        QLearningState currentState = null;
        

        public int Steps { get => steps;
            set
            {
                if (steps != value)
                {
                    steps = value;
                    RaisePropertyChanged("Steps");
                }
            }
        }
        public QLearningState CurrentState { get => currentState;
            set
            {
                currentState = value;
                RaisePropertyChanged("CurrentState");
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
