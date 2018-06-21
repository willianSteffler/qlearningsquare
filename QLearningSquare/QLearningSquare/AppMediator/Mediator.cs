using QLearningSquare.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearningSquare.AppMediator
{
    class Mediator
    {
        private static Mediator mediator = new Mediator();
        public GUIControl GUI = new GUIControl();

        internal static Mediator pMediator { get => mediator; }


    }
}
