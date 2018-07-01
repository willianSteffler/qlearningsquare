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
using Microsoft.Win32;

namespace QLearningSquare
{
    [ValueConversion(typeof(string), typeof(int))]
    public class ColumnConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(int))
                throw new InvalidOperationException("The target must be a interger");

            return GUIControl.statePositions[(string)value].Column;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    [ValueConversion(typeof(string), typeof(int))]
    public class RowConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(int))
                throw new InvalidOperationException("The target must be a interger");

            return GUIControl.statePositions[(string)value].Row;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private QLearningWorker worker;
        public ObservableCollection<QLearningState> Qtable { get; set; }

        private int animateInterval;
        private bool autoAnimate;
        private double gridStatesSize;
        private int currentsteps;

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

        public QLearningWorker Worker { get => worker;
            set
            {
                worker = value;
                RaisePropertyChanged("Worker");
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
            double maxValue = e.NewSize.Height < (e.NewSize.Width) ? e.NewSize.Height : (e.NewSize.Width);

            ViewModel.GridStatesSize = gridStates.ColumnDefinitions.Count > gridStates.RowDefinitions.Count ?
                maxValue / gridStates.ColumnDefinitions.Count : maxValue / gridStates.RowDefinitions.Count;
        }

        private void btNext_Click(object sender, RoutedEventArgs e)
        {
            ctrl.nextClick();
        }

        private void btStop_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox && ViewModel.AutoAnimate)
                return;

            ctrl.stopClick();
        }

        private void btInit_Click(object sender, RoutedEventArgs e)
        {
            ctrl.initClick();
        }

        private void btReset_Click(object sender, RoutedEventArgs e)
        {
            ctrl.resetClick();
        }

        private void btOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName != "")

            ctrl.openFile(openFileDialog.FileName);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ctrl.onClose();
        }
    }
}
