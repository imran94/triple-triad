using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class PseudoTile
    {
        //represents the numerical significant of tile
        //index coincides with the tiles in game
        //can be built as part of Tile class, but separated due to its inherent functionality

        //for the AI to decide based on threat vs gain vs waste?
        //waste should represent the strength wasted trying to capture or when facing board edge
        //gain should represent the exposed sides of the adjacent card, ie how much can the card tank for the waste to obtain it
        //threat should represent the danger of a tile based on open sides and threat values for that side

        private int id;
            public int ID { get { return id; } }

        //private static int[] threatMultiplier = { 2,3,2,
                                              //3,4,3,
                                              //2,3,2 };

            private bool exclude = false;
            public bool Exclude { get { return exclude; } set { exclude = value; } }

        private int threatMultiplier;
            public int ThreatMultiplier { get { return threatMultiplier; } set { threatMultiplier = value; } }

        private float threatNorth = 0, threatSouth = 0, threatEast = 0, threatWest = 0;
        private int waste = 0, gain = 0;
        private float threat=0;
            public int Waste { get { return waste; } }
            public int Gain { get { return gain; } }
            public float Threat { get { return threat; } }

        private int deadZone;

        private int challengeNorth = 0, challengeSouth = 0, challengeEast = 0, challengeWest = 0;
            public int ChallengeNorth { get { return challengeNorth; } set { challengeNorth = value; } }
            public int ChallengeSouth { get { return challengeSouth; } set { challengeSouth = value; } }
            public int ChallengeEast { get { return challengeEast; } set { challengeEast = value; } }
            public int ChallengeWest { get { return challengeWest; } set { challengeWest = value; } }


        private bool openNorth = true, openSouth = true, openEast = true, openWest = true;
            public bool OpenNorth { get { return openNorth; } set { openNorth = value; } }
            public bool OpenSouth { get { return openSouth; } set { openSouth = value; } }
            public bool OpenEast { get { return openEast; } set { openEast = value; } }
            public bool OpenWest { get { return openWest; } set { openWest = value; } }

        private int gainNorth = 0, gainSouth = 0, gainEast = 0, gainWest = 0;
            public int GainNorth { get { return gainNorth; } set { gainNorth = value; } }
            public int GainSouth { get { return gainSouth; } set { gainSouth = value; } }
            public int GainEast { get { return gainEast; } set { gainEast = value; } }
            public int GainWest { get { return gainWest; } set { gainWest = value; } }

        private int wasteNorth = 0, wasteSouth = 0, wasteEast = 0, wasteWest = 0;
            public int WasteNorth { get { return wasteNorth; } set { wasteNorth = value; } }
            public int WasteSouth { get { return wasteSouth; } set { wasteSouth = value; } }
            public int WasteEast { get { return wasteEast; } set { wasteEast = value; } }
            public int WasteWest { get { return wasteWest; } set { wasteWest = value; } }

        private const int DEAD_ZONE = 10;

        public PseudoTile(int id)
        {
            this.id = id;

            if (id == 4)
                threatMultiplier = 4;
            else if (id % 2 == 0)
            {
                threatMultiplier = 2;
                deadZone += DEAD_ZONE * 2;
            }
            else if (id % 2 == 1)
            {
                threatMultiplier = 3;
                deadZone += DEAD_ZONE;
            }

            if (id < 3)
                openNorth = false;
            else if (id > 5)
                openSouth = false;
            if (id % 3 < 1)
                openWest = false;
            else if (id % 3 > 1)
                openEast = false;
        }

        public void updateThreat(int[] sumThreat, int cardCount_player)
        {
            if (openNorth)
                threatNorth = (float)sumThreat[(int)Enum.CardSide.N] / (float)cardCount_player;
            else
                threatNorth = 0;

            if (openSouth)
                threatSouth = (float)sumThreat[(int)Enum.CardSide.S] / (float)cardCount_player;
            else
                threatSouth = 0;

            if (openEast)
                threatEast = (float)sumThreat[(int)Enum.CardSide.E] / (float)cardCount_player;
            else
                threatEast = 0;

            if (openWest)
                threatWest = (float)sumThreat[(int)Enum.CardSide.W] / (float)cardCount_player;
            else
                threatWest = 0;

            //printThreat();
        }

        public void updateGain()
        {
            gain = gainNorth + gainSouth + gainEast + gainWest;

            //Debug.WriteLine(id + "|| " + gain);
            //printGain();
            //printOpen();
            //printWaste();
            //printChallenge();
        }


        private const int CHALLENGED = 10;

        private int bestCardID = -1;
            public int BestCardID { get{return bestCardID;}}

        private float bestDeal = -99999; //deal in the context discount, bargain, etc.
            public float BestDeal{get{return bestDeal;}}

        //waste, gain, threat
        public void crunch(ref Card[] cards, int opponent_remaining, int mode)
        {
            bestCardID = -1;
            bestDeal = -99999;
            

            foreach (Card c in cards)
            {
                int challengeCount = 0;

                if (c.State == (int)Enum.CardState.Rest && !exclude) //if c is still available && ptile is not occupied
                {
                    //if blank opposing space or edge or bot's own card, waste is the value of card not used (wasted)
                    //otherwise it's the card value - the opposing card value, which
                    //if <= 0 means no worth, if > 0 means capture for that side
                    //want the least waste possible

                    //if card val greater than waste val, waste = card val - waste val
                    //else                                waste = card val
                    waste = (c.North > wasteNorth ? c.North - wasteNorth : c.North) +
                           (c.South > wasteSouth ? c.South - wasteSouth : c.South) +
                           (c.East > wasteEast ? c.East - wasteEast : c.East) +
                           (c.West > wasteWest ? c.West - wasteWest : c.West) +
                           deadZone;

                    //Debug.Write((float)waste/remaining + "|");
                    //Debug.Write(waste + "|");

                    //if opposing tile is empty, threat is the average of the card value of that opposing side
                    //otherwise that opposing side poses no threat
                    //want the least threat possible

                    //if open side cont, else threat at side = 0
                    //cont => if card val > threat val, threat =  card val - threat val
                    //        else                      threat = threat val
                    threat = (openNorth ? (c.North > threatNorth ? c.North - threatNorth : threatNorth) : 0) +
                            (openNorth ? (c.South > threatSouth ? c.South - threatSouth : threatSouth) : 0) +
                            (openNorth ? (c.East > threatEast ? c.East - threatEast  : threatEast) : 0) +
                            (openNorth ? (c.West > threatWest ? c.West - threatWest : threatWest) : 0);

                            //(openWest ? c.North - threatWest : 0);
                    //gain is the same regardless of any card to be placed
                    //want the most gain possible


                    //challenge is how many opposing cards can be won with current card
                    //no need breakdown for this calculation
                    challengeCount += (c.North > challengeNorth ? 1 : 0) +
                                    (c.South > challengeSouth ? 1 : 0) +
                                    (c.East > challengeEast ? 1 : 0) +
                                    (c.West > challengeWest ? 1 : 0);

                    float temp = 0;

                    switch (mode)
                    {
                        case (int)Enum.BotDecision.Offense:
                            {
                                temp = (challengeCount * (float)gain / (float)opponent_remaining + challengeCount * CHALLENGED) * 4
                                - ((threatMultiplier >= 0 ? threatMultiplier : 0) * threat)
                                - (waste)
                                ;
                                break;
                            }

                        case (int)Enum.BotDecision.Defense:
                            {
                                temp = (challengeCount * (float)gain / (float)opponent_remaining + challengeCount * CHALLENGED)
                                - ((threatMultiplier >= 0 ? threatMultiplier : 0) * threat) * 4
                                - (waste)
                                ;
                                break;
                            }

                        default: //case (int)Enum.BotDecision.Defense:
                            {
                                temp = (challengeCount * (float)gain / (float)opponent_remaining + challengeCount * CHALLENGED)
                                - ((threatMultiplier >= 0 ? threatMultiplier : 0) * threat)
                                - (waste)
                                ;
                                break;
                            }
                    }
                    

                    /*
                    float temp = ((float)gain / (float)opponent_remaining) * 1
                                - ((threatMultiplier >= 0 ? threatMultiplier : 0) * threat) * 4
                                - (waste) * 6  ;
                     * //*/

                    //float temp = ((float)gain / (float)opponent_remaining) - (threat) - (waste);

                    if (bestDeal < temp) //if bestDeal not as good as the calculated value
                    {
                        bestDeal = temp; //bestDeal is the calculated value
                        bestCardID = c.ID; //save the best choice
                    }
                }
            }

            if (exclude) //just to reset if necessary
            {
                bestCardID = -1;
                bestDeal = -99999;
            }
        }


        //DEBUG DE-BUGGER Da-BURGER====================================================
        public void printThreat()
        {
            Debug.WriteLine(id + "|| " + threatNorth + " | " + threatSouth + " | " + threatEast + " | " + threatWest);
        }

        public void printGain()
        {
            Debug.WriteLine(id + "|| " + gainNorth + " | " + gainSouth + " | " + gainEast + " | " + gainWest);
        }

        public void printOpen()
        {
            Debug.WriteLine(id + "|| " + openNorth + " | " + openSouth + " | " + openEast + " | " + openWest);
        }

        public void printWaste()
        {
            Debug.WriteLine(id + "|| " + wasteNorth + " | " + wasteSouth + " | " + wasteEast + " | " + wasteWest);
        }

        public void printChallenge()
        {
            Debug.WriteLine(id + "|| " + challengeNorth + " | " + challengeSouth + " | " + challengeEast + " | " + challengeWest);
        }

        








    } //end class
} //end namespace
