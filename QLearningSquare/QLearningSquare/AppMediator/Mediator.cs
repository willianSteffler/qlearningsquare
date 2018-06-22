using QLearningSquare.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLearningSquare.DAO;

namespace QLearningSquare.AppMediator
{
    public delegate void GlobalBusMessage(string message);

    class Mediator
    {
        private static Mediator mediator = new Mediator();
        public List<GlobalBusMessage> GlobalBus = new List<GlobalBusMessage>();

        //initialize classes
        public GUIControl pGUI = new GUIControl();
        public DataAcessObject pDAO = new DataAcessObject();

        internal static Mediator pMediator { get => mediator; }
    }
}
