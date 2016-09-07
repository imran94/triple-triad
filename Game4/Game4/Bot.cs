using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class Bot : Control
    {
        //responsible for the artificially intelligent

        private Random random = new Random();
        private Card[] cards_opponent; //for evaluation? discard if useless

        private int decision;
            public int Decision { get { return decision; } set { decision = value; } }

            private bool initialised = false;

        public Bot(ref Card[] cards_self, ref Tile[] tiles, ref Card[] cards_opponent, int decision=(int)Enum.BotDecision.Basic) 
            : base(ref cards_self, ref tiles)
        {
            this.cards_opponent = cards_opponent;
            this.decision = decision;

            cardCount_bot = cards.Length;
            

        }//end constructor====================================================================

        public override void Reset(ref Card[] cards, ref Tile[] _tiles)
        {
            this.cards = cards;
            cardCount_bot = cards.Length;

            decision = (int)Enum.BotDecision.Basic;
            initialised = false;

            base.Reset(ref cards, ref _tiles);
        }

        //begin update=================================================
        public override bool Update()
        {
            if (!initialised)
                threatInit();

            if (score > (int)Enum.Player.Human)
                decision = (int)Enum.BotDecision.Offense;
            else if (score < (int)Enum.Player.Bot)
                decision = (int)Enum.BotDecision.Defense;
            else
                decision = (int)Enum.BotDecision.Basic;

            //Debug.WriteLine(score + " thus bot : " +decision);

                switch (decision) //decide card and tile
                {
                    case (int)Enum.BotDecision.Basic:
                        {
                            decide((int)Enum.BotDecision.Basic);
                            break;
                        }

                    case (int)Enum.BotDecision.Offense:
                        {
                            decide((int)Enum.BotDecision.Offense);
                            break;
                        }

                    case (int)Enum.BotDecision.Defense:
                        {
                            decide((int)Enum.BotDecision.Defense);
                            break;
                        }

                    default: //case (int)Enum.BotDecision.Random:
                        {
                            decide((int)Enum.BotDecision.Basic);
                            break;
                        }

                }
                moveCard();

                //Debug.WriteLine("=====>" + lastDealtCardID + " on " + lastOccupiedTileID + "===========");
                //Debug.WriteLine(score + " thus bot : " + decision + "\n====================");

            return true;


        }//end update=================================================
        //==================================================================================
        //==================================================================================










        //==================================================================================
        //==================================================================================
        //begin random AI====================================================================
        private void decideRandom()
        {
            int cardID, tileID;
            do
            {
                cardID = random.Next(0, cards.Length);
            }while(cards[cardID].State == (int)Enum.CardState.Dealt); //keep choosing dealt card

            do
            {
                tileID = random.Next(0, tiles.Length);
            } while (tiles[tileID].State == (int)Enum.TileState.Occupied); //keep choosing occupied tile

            card = cards[cardID];
            tile = tiles[tileID];

        }//end random AI====================================================================
        //==================================================================================
        //==================================================================================

        

        //==================================================================================
        //==================================================================================
        //begin basic AI====================================================================


        private void decide(int mode)
        {
            //*
            float tempDeal = -99999;
            if(lastDealtCardID > -1) reAssessThreat(lastDealtCardID); //skip re-assessment if bot moves 1st, where last dealt card doesn't exist
            foreach (PseudoTile pt in pTiles)
            {
                pt.updateThreat(sumThreat, cardCount_player);
                pt.updateGain();

                pt.crunch(ref cards, cardCount_player, mode);


                if (pt.BestDeal > tempDeal)
                {
                    tempDeal = pt.BestDeal;
                    card = cards[pt.BestCardID];
                    tile = tiles[pt.ID];
                }
            }//*/





            /*/ for displaying best best card and value on console
            for (int i = 0; i < 9; i++)
            {
                if (!pTiles[i].Exclude)
                {
                    Debug.WriteLine(i + "::" + pTiles[i].BestCardID + " => " + pTiles[i].BestDeal);

                }
                if (i == 8) Debug.WriteLine("===================");
            }//*/
        }



        
        
        
        
        
        
        //end AI====================================================================
        //==================================================================================
        //==================================================================================

        
        private int[] sumThreat = { 0, 0, 0, 0 };
        private int[] threatMax = { 1, 1, 1, 1 };
        private int[] threatMin = { 9, 9, 9, 9 };

        private int[] sumStrength = { 0, 0, 0, 0 };
        private int[] strengthMax = { 1, 1, 1, 1 };
        private int[] strengthMin = { 9, 9, 9, 9 };

        private void threatInit()
        {
            for (int i = 0; i < cards.Length; i++)
            {
                sumThreat[(int)Enum.CardSide.N] += cards[i].North;
                sumThreat[(int)Enum.CardSide.S] += cards[i].South;
                sumThreat[(int)Enum.CardSide.E] += cards[i].East;
                sumThreat[(int)Enum.CardSide.W] += cards[i].West;
            }

            sumStrength[(int)Enum.CardSide.N] = sumThreat[(int)Enum.CardSide.N];
            sumStrength[(int)Enum.CardSide.S] = sumThreat[(int)Enum.CardSide.S];
            sumStrength[(int)Enum.CardSide.E] = sumThreat[(int)Enum.CardSide.E];
            sumStrength[(int)Enum.CardSide.W] = sumThreat[(int)Enum.CardSide.W];

            initialised = true;
        }

        private void reAssessThreat(int lastDealtCardID)
        {
            sumThreat[(int)Enum.CardSide.N] -= cards[lastDealtCardID].North;
            sumThreat[(int)Enum.CardSide.S] -= cards[lastDealtCardID].South;
            sumThreat[(int)Enum.CardSide.E] -= cards[lastDealtCardID].East;
            sumThreat[(int)Enum.CardSide.W] -= cards[lastDealtCardID].West;
        }

        /* //currently can't find a use for this
        private void reAssessStrength(int lastDealtCardID)
        {
            sumStrength[(int)Enum.CardSide.N] -= cards[lastDealtCardID].North;
            sumStrength[(int)Enum.CardSide.S] -= cards[lastDealtCardID].South;
            sumStrength[(int)Enum.CardSide.E] -= cards[lastDealtCardID].East;
            sumStrength[(int)Enum.CardSide.W] -= cards[lastDealtCardID].West;
        }//*/




        //DEBUG for days!=======================================
        protected void printTileThreatMultiplier()
        {//*
            Debug.WriteLine("\ntile threat multiplier");
            Debug.WriteLine(pTiles[0].ThreatMultiplier + "," + pTiles[1].ThreatMultiplier + "," + pTiles[2].ThreatMultiplier);
            Debug.WriteLine(pTiles[3].ThreatMultiplier + "," + pTiles[4].ThreatMultiplier + "," + pTiles[5].ThreatMultiplier);
            Debug.WriteLine(pTiles[6].ThreatMultiplier + "," + pTiles[7].ThreatMultiplier + "," + pTiles[8].ThreatMultiplier);
            Debug.WriteLine("");
          //*/
        }

        protected void printThreat()
        {
            Debug.WriteLine("threat LVL");
            Debug.Write(sumThreat[0] + ",");
            Debug.Write(sumThreat[1] + ",");
            Debug.Write(sumThreat[2] + ",");
            Debug.WriteLine(sumThreat[3] + "\n");
        }

        protected void printStrength()
        {
            Debug.WriteLine("eat stiitake");
            Debug.Write(sumStrength[0] + ",");
            Debug.Write(sumStrength[1] + ",");
            Debug.Write(sumStrength[2] + ",");
            Debug.WriteLine(sumStrength[3] + "\n");
        }


    }//end class
}//end namespace
