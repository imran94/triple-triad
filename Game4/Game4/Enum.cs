using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class Enum
    {
        public enum CardSide : int { N, S, E, W };
        public enum Player : int { Human = 1, Bot = -1 } //the value doubles up as the score for the game
        public enum CardState : int { Rest, Hover, Selected, Dealt };

        public enum TileState : int { Rest, Hover, Selected, Occupied };

        public enum BotDecision : int { Random, Basic , Offense, Defense}

        public enum TargetDirection : int { Up = -3, Down = +3, Left = -1, Right = +1 }



    }
}
