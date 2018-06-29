using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QLearningSquare.GUI;

namespace QLearningSquare
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private QLearningState workerState;
        public ObservableCollection<QLearningState> Qtable { get; set; }

        private int animateInterval;
        private bool autoAnimate;
        private double gridStatesSize;
        private int currentsteps;

        private int workerRow;
        private int workerColumn;
        private double workerSpacing;


        public int AnimateInterval { get => animateInterval;
            set
            {
                if (animateInterval != value)
                {
                    animateInterval = value;
                    RaisePropertyChanged("AnimateInterval");
                }
            }
        }
        public bool AutoAnimate { get => autoAnimate;
            set
            {
                if (autoAnimate != value)
                {
                    autoAnimate = value;
                    RaisePropertyChanged("AutoAnimate");
                }
            }
        }

        public double GridStatesSize { get => gridStatesSize;
            set
            {
                gridStatesSize = value;
                WorkerSpacing = gridStatesSize * 0.2;
                RaisePropertyChanged("GridStatesSize");
            }
        }

        public int CurrentSteps
        {
            get => currentsteps;
            set
            {
                currentsteps = value;

                RaisePropertyChanged("CurrentSteps");
            }
        }

        public QLearningState WorkerState { get => workerState;
            set
            {
                workerState = value;

                RaisePropertyChanged("WorkerState");
            }
        }

        public int WorkerRow { get => workerRow;
            set
            {
                if (workerRow != value)
                {
                    workerRow = value;
                    RaisePropertyChanged("WorkerRow");
                }
            }
        }
        public int WorkerColumn { get => workerColumn;
            set
            {
                if (workerColumn != value)
                {
                    workerColumn = value;
                    RaisePropertyChanged("WorkerColumn");
                }
            }
        }

        public double WorkerSpacing { get => workerSpacing;
            set
            {
                workerSpacing = value;

                RaisePropertyChanged("WorkerSpacing");
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

    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        GUIControl ctrl = (GUIControl)AppMediator.Mediator.pMediator.pGUI;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            this.DataContext = ViewModel;
            ctrl.pMainWindow = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrl.onMainWindowLoaded();
        }

        private void gridStates_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double maxValue = e.NewSize.Height < (e.NewSize.Width - 300) ? e.NewSize.Height : (e.NewSize.Width - 300);

            ViewModel.GridStatesSize = gridStates.ColumnDefinitions.Count > gridStates.RowDefinitions.Count ?
                maxValue / gridStates.ColumnDefinitions.Count : maxValue / gridStates.RowDefinitions.Count;
            
        }
    }
}
