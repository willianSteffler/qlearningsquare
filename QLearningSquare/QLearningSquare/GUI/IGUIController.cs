﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare.GUI
{

    public enum SquareDirections
    {
        left,
        right,
        up,
        down
    }

    interface IGUIController
    {
        void OnError(string errorMessage);
        void MoveSquare(string state);
        void SetStatesMatrix(List<List<string>> statesMatrix);
        Action OnGuiLoaded { get; set; }

    }
}