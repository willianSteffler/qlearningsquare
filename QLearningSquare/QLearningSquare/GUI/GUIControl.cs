using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QLearningSquare.AppMediator;

namespace QLearningSquare.GUI
{

    class GUIControl : IGUIController
    {
        public class GridPosition
        {
            public int Row;
            public int Column;

            public Border BorderElement { get; internal set; }
        }

        public static Dictionary<string, GridPosition> statePositions = new Dictionary<string,GridPosition>();

        AppMediator.Mediator ctrl = AppMediator.Mediator.pMediator;
        public MainWindow pMainWindow;

        Action onGUILoaded = null;
        Action onGUIClose = null;

        public Action OnGuiLoaded { get => onGUILoaded; set => onGUILoaded = value; }
        public int AnimateInterval { get => pMainWindow.ViewModel.AnimateInterval; set => pMainWindow.ViewModel.AnimateInterval = value; }
        public bool AutoAnimate { get => pMainWindow.ViewModel.AutoAnimate; set => pMainWindow.ViewModel.AutoAnimate = value; }
        public Action OnGuiClose { get => onGUIClose; set => onGUIClose = value; }

        public void OnError(string errorMessage)
        {
            MessageBox.Show(errorMessage);
        }

        public void onMainWindowLoaded()
        {
            onGUILoaded();
        }

        public void SetStatesMatrix(List<List<QLearningState>> statesMatrix)
        {
            pMainWindow.ViewModel.Qtable = new ObservableCollection<QLearningState>();
            statePositions = new Dictionary<string, GridPosition>();

            for (int i = 0; i < statesMatrix.Count; i++)
            {
                if (pMainWindow.gridStates.RowDefinitions.Count < statesMatrix.Count)
                {
                    pMainWindow.gridStates.RowDefinitions.Add(new RowDefinition());
                }
                for (int j = 0; j < statesMatrix[i].Count; j++)
                {
                    if(pMainWindow.gridStates.ColumnDefinitions.Count < statesMatrix[i].Count)
                    {
                        pMainWindow.gridStates.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    pMainWindow.ViewModel.Qtable.Add(statesMatrix[i][j]);
                    Border b = new Border();

                    b.Style = (Style)pMainWindow.FindResource(statesMatrix[i][j].Type.ToString());
                    pMainWindow.gridStates.Children.Add(b);
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);

                    statePositions[statesMatrix[i][j].Name] = new GridPosition() { Row = i, Column = j,BorderElement = b };
                }
            }

            pMainWindow.QtableView.ItemsSource = pMainWindow.ViewModel.Qtable;
            Border gb = (Border)pMainWindow.gridStates.Parent;
            double maxValue = gb.ActualHeight < (gb.ActualWidth) ? gb.ActualHeight : (gb.ActualWidth);

            pMainWindow.ViewModel.GridStatesSize = pMainWindow.gridStates.ColumnDefinitions.Count > pMainWindow.gridStates.RowDefinitions.Count ?
                maxValue / pMainWindow.gridStates.ColumnDefinitions.Count : maxValue / pMainWindow.gridStates.RowDefinitions.Count;

        }

        public void SetWoker(QLearningWorker worker)
        {
            pMainWindow.ViewModel.Worker = worker;
            pMainWindow.Worker.Visibility = Visibility.Visible;

            statePositions[worker.CurrentState.Name].BorderElement.Style = (Style)pMainWindow.FindResource("InitialState");
            Grid.SetZIndex(pMainWindow.Worker, 10);
        }

        internal void resetClick()
        {
            Mediator.pMediator.ResetQL();
        }

        internal void initClick()
        {
            Mediator.pMediator.InitQL();
        }

        internal void stopClick()
        {
            Mediator.pMediator.StopQL();
        }

        internal void nextClick()
        {
            Mediator.pMediator.AdvanceQL();
        }

        internal void openFile(string fileName)
        {
            Mediator.pMediator.OpenFile(fileName);
        }

        public void ResetViews()
        {

            foreach(var gp in statePositions.Values)
            {
                pMainWindow.gridStates.Children.Remove(gp.BorderElement);
            }

            pMainWindow.gridStates.RowDefinitions.Clear();
            pMainWindow.gridStates.ColumnDefinitions.Clear();
            statePositions.Clear();
        }

        internal void onClose()
        {
            OnGuiClose();
        }
    }
}
