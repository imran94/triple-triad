using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    abstract class Control
    {
        //responsible for handling logics
        //setting states of the Card and Tile for GUI to update
        //calculates score


        protected Card[] cards;
        static protected Card card = null;
        static protected Tile[] tiles = null;
        static protected Tile tile = null;

        

        protected static int score = 0; //end game if 0 draw, if +ve player wins, if -ve bot wins
            public static int Score { get { return score; } set { score = value; } }


        

        public Control(ref Card[] cards, ref Tile[] _tiles)
        {
            score = 0;
            this.cards = cards;
            tiles = _tiles;

            lastDealtCardID = -1;
            lastOccupiedTileID = -1;

            for (int i = 0; i < _tiles.Length; i++)
            {
                pTiles[i] = new PseudoTile(i);
            }
        }

        protected SoundEffect cardPick, cardPlace, cardCapture;

        public virtual void LoadContent(ContentManager content)
        {
            cardPick = content.Load<SoundEffect>("audio\\cardPick");
            cardPlace = content.Load<SoundEffect>("audio\\cardPlace");
            cardCapture = content.Load<SoundEffect>("audio\\cardCapture");
        }

        public virtual void Reset(ref Card[] cards, ref Tile[] _tiles)
        {
            this.cards = cards;
            tiles = _tiles;

            lastDealtCardID = -1;
            lastOccupiedTileID = -1;

            for (int i = 0; i < _tiles.Length; i++)
            {
                pTiles[i] = new PseudoTile(i);
            }
        }

        public abstract bool Update(GameTime gameTime);

        protected void moveCard()
        {
            //move card to tile
            //make sure to set child class card and tile before calling this function

            score += card.Player;
            lastDealtCardID = card.ID; //recognise last dealt card
            lastOccupiedTileID = tile.ID; //recognise last occupied tile

            card.Rectangle = tile.Rectangle;
            setCard();  
            tryCapture();

            pTiles[lastOccupiedTileID].Exclude = true;

            if (card.Player == (int)Enum.Player.Human)
                cardCount_player--;
            else //if (card.Player == (int)Enum.Player.Bot)
                cardCount_bot--;

            setPTileAdjacent(); //sets open sides & threat multiplier
            setPTileEach(); //sets pTile gain and waste per side

            reset();
        }

        private void setCard()
        {
            //wrote as separate function in case this is handled differently
            tile.Card = card;
            cardPlace.Play();
        }

        private void reset()
        {
            card.State = (int)Enum.CardState.Dealt;
            cards[lastDealtCardID].State = (int)Enum.CardState.Dealt;
            card = null;
            tile.State = (int)Enum.TileState.Occupied;
            tile = null;
        }

        //start try to capture////////////////////////=====
        //public enum TargetDirection : int { Up = -3, Down = +3, Left = -1, Right = +1 }
        private void tryCapture()
        {
            /* the cheat is real
                0|1|2
                3|4|5
                6|7|8
             //*/

            if (tile.ID > 2)
            {
                if (capture((int)Enum.TargetDirection.Up))
                {
                    score += card.Player;
                    cardCapture.Play();
                }
            }
            if (tile.ID < 6)
            {
                if (capture((int)Enum.TargetDirection.Down))
                {
                    score += card.Player;
                    cardCapture.Play();
                }
            }
            if (tile.ID % 3 > 0)
            {
                if (capture((int)Enum.TargetDirection.Left))
                {
                    score += card.Player;
                    cardCapture.Play();
                }
            }
            if (tile.ID % 3 < 2)
            {
                if (capture((int)Enum.TargetDirection.Right))
                {
                    score += card.Player;
                    cardCapture.Play();
                }
            }
        } //end try to capture//////////////////=====

        
        private bool capture(int idx_shift)
        {
            //logics for comparing opposing side's value
            if (tiles[tile.ID + idx_shift].Card != null && tiles[tile.ID + idx_shift].Card.Player != tile.Card.Player) //if exist card at adjacent tile and not current player's
            {
                int self=0, target=0; //stores appropriate value of opposing card sides
                switch (idx_shift)
                {
                    case (int)Enum.TargetDirection.Up:
                    {
                        self = tile.Card.North;
                        target = tiles[tile.ID + idx_shift].Card.South;
                        break;
                    }
                    case (int)Enum.TargetDirection.Down:
                    {
                        self = tile.Card.South;
                        target = tiles[tile.ID + idx_shift].Card.North;
                        break;
                    }
                    case (int)Enum.TargetDirection.Left:
                    {
                        self = tile.Card.West;
                        target = tiles[tile.ID + idx_shift].Card.East;
                        break;
                    }
                    case (int)Enum.TargetDirection.Right:
                    {
                        self = tile.Card.East;
                        target = tiles[tile.ID + idx_shift].Card.West;
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
                if (self > target)
                {
                    tiles[tile.ID + idx_shift].Card.Swapped = true; //to update card owner
                    tiles[tile.ID + idx_shift].Card.Player = tile.Card.Player; //capture
                    tiles[tile.ID + idx_shift].Card.Swapped = false; //to update states
                    return true;
                }
            }
            return false;
        } //end capture////////////////////////==














        //bot cheats here, shhhhhh
        protected static PseudoTile[] pTiles = new PseudoTile[9];

        static protected int lastDealtCardID;
        static protected int lastOccupiedTileID;

        static protected int cardCount_player;
        static protected int cardCount_bot;


        static protected void setPTileAdjacent()
        {
            if (lastOccupiedTileID > 2) //if got pTile above
            {
                pTiles[lastOccupiedTileID + (int)Enum.TargetDirection.Up].OpenSouth = false; //close open space SOUTH for pTile above
                pTiles[lastOccupiedTileID + (int)Enum.TargetDirection.Up].ThreatMultiplier--; //reduce threat multiplier for pTile above
            }
            if (lastOccupiedTileID < 6) //below
            {
                pTiles[lastOccupiedTileID + (int)Enum.TargetDirection.Down].OpenNorth = false;
                pTiles[lastOccupiedTileID + (int)Enum.TargetDirection.Down].ThreatMultiplier--;
            }
            if (lastOccupiedTileID % 3 > 0) //left
            {
                pTiles[lastOccupiedTileID + (int)Enum.TargetDirection.Left].OpenEast = false;
                pTiles[lastOccupiedTileID + (int)Enum.TargetDirection.Left].ThreatMultiplier--;
            }
            if (lastOccupiedTileID % 3 < 2) //right
            {
                pTiles[lastOccupiedTileID + (int)Enum.TargetDirection.Right].OpenWest = false;
                pTiles[lastOccupiedTileID + (int)Enum.TargetDirection.Right].ThreatMultiplier--;
            }


            pTiles[lastOccupiedTileID].ThreatMultiplier = -1; //0 for cannot be targeted, < 0 for occupied

        }



        static private void setPTileEach()
        {
            //not exactly elegant, i know, don't ask

            for (int i = 0; i < tiles.Length; i++ ) //for all pTiles
            {
                if (i > 2) //if exists above
                {
                    if (tiles[i].Card != null && tiles[i].Card.Player == (int)Enum.Player.Human) //if current tile has card and is human's
                    {
                        if (tiles[i + (int)Enum.TargetDirection.Up].Card == null) //if tile above has no card
                        {
                            pTiles[i + (int)Enum.TargetDirection.Up].GainSouth = calcGain((int)Enum.TargetDirection.Up, i);
                        }
                        else //if tile above has card
                        { 
                            pTiles[i + (int)Enum.TargetDirection.Up].GainSouth = 0;
                        }
                    }
                    else
                    {
                        pTiles[i + (int)Enum.TargetDirection.Up].GainSouth = 0;

                        calcWaste((int)Enum.TargetDirection.Up, i);
                    }

                    if (tiles[i + (int)Enum.TargetDirection.Up].Card != null && tiles[i + (int)Enum.TargetDirection.Up].Card.Player == (int)Enum.Player.Human) //if tile above has card and is human's
                    {
                        pTiles[i].ChallengeNorth = tiles[i + (int)Enum.TargetDirection.Up].Card.South; //set challenge north
                    }
                    else
                    {
                        pTiles[i].ChallengeNorth = 0; //reset challenge north
                    }

                    if (tiles[i].Card != null) //if current tile has no card
                    {
                        pTiles[i].WasteNorth = 0;
                    }
                    
                }
                if (i < 6)
                {
                    if (tiles[i].Card != null && tiles[i].Card.Player == (int)Enum.Player.Human)
                    {
                        if (tiles[i + (int)Enum.TargetDirection.Down].Card == null)
                        {
                            pTiles[i + (int)Enum.TargetDirection.Down].GainNorth = calcGain((int)Enum.TargetDirection.Down, i);
                        }
                        else
                        {
                            pTiles[i + (int)Enum.TargetDirection.Down].GainNorth = 0;
                        }
                    }
                    else
                    {
                        pTiles[i + (int)Enum.TargetDirection.Down].GainNorth = 0;

                        calcWaste((int)Enum.TargetDirection.Down, i);
                    }

                    if (tiles[i + (int)Enum.TargetDirection.Down].Card != null && tiles[i + (int)Enum.TargetDirection.Down].Card.Player == (int)Enum.Player.Human)
                    {
                        pTiles[i].ChallengeSouth = tiles[i + (int)Enum.TargetDirection.Down].Card.North; //set challenge 
                    }
                    else
                    {
                        pTiles[i].ChallengeSouth = 0; //reset challenge 
                    }


                    if (tiles[i].Card != null)
                    {
                        pTiles[i].WasteSouth = 0;
                    }
                }
                if (i % 3 > 0)
                {
                    if (tiles[i].Card != null && tiles[i].Card.Player == (int)Enum.Player.Human)
                    {
                        if (tiles[i + (int)Enum.TargetDirection.Left].Card == null)
                        {
                            pTiles[i + (int)Enum.TargetDirection.Left].GainEast = calcGain((int)Enum.TargetDirection.Left, i);
                        }
                        else
                        {
                            pTiles[i + (int)Enum.TargetDirection.Left].GainEast = 0;
                        }
                    }
                    else
                    {
                        pTiles[i + (int)Enum.TargetDirection.Left].GainEast = 0;

                        calcWaste((int)Enum.TargetDirection.Left, i);
                    }

                    if (tiles[i + (int)Enum.TargetDirection.Left].Card != null && tiles[i + (int)Enum.TargetDirection.Left].Card.Player == (int)Enum.Player.Human) //if tile above has card and is human's
                    {
                        pTiles[i].ChallengeWest = tiles[i + (int)Enum.TargetDirection.Left].Card.East; //set challenge 
                    }
                    else
                    {
                        pTiles[i].ChallengeWest = 0; //reset challenge 
                    }

                    if (tiles[i].Card != null)
                    {
                        pTiles[i].WasteWest = 0;
                    }
                }
                if (i % 3 < 2)
                {
                    if (tiles[i].Card != null && tiles[i].Card.Player == (int)Enum.Player.Human)
                    {
                        if (tiles[i + (int)Enum.TargetDirection.Right].Card == null)
                        {
                            pTiles[i + (int)Enum.TargetDirection.Right].GainWest = calcGain((int)Enum.TargetDirection.Right, i);
                        }
                        else
                        {
                            pTiles[i + (int)Enum.TargetDirection.Right].GainWest = 0;
                        }
                    }
                    else
                    {
                        pTiles[i + (int)Enum.TargetDirection.Right].GainWest = 0;

                        calcWaste((int)Enum.TargetDirection.Right, i);
                    }

                    if (tiles[i + (int)Enum.TargetDirection.Right].Card != null && tiles[i + (int)Enum.TargetDirection.Right].Card.Player == (int)Enum.Player.Human) //if tile above has card and is human's
                    {
                        pTiles[i].ChallengeEast = tiles[i + (int)Enum.TargetDirection.Right].Card.West; //set challenge 
                    }
                    else
                    {
                        pTiles[i].ChallengeEast = 0; //reset challenge 
                    }

                    if (tiles[i].Card != null)
                    {
                        pTiles[i].WasteEast = 0;
                    }
                }
            }
        }

        static private int calcGain(int direction, int i)
        {
            return

                (direction != (int)Enum.TargetDirection.Up ? ((pTiles[i].OpenNorth ? tiles[i].Card.North : 0)) : 0) +

                (direction != (int)Enum.TargetDirection.Down ? ((pTiles[i].OpenSouth ? tiles[i].Card.South : 0)) : 0) +

                (direction != (int)Enum.TargetDirection.Right ? ((pTiles[i].OpenEast ? tiles[i].Card.East : 0)) : 0) +

                (direction != (int)Enum.TargetDirection.Left ? ((pTiles[i].OpenWest ? tiles[i].Card.West : 0)) : 0);
        }

        static private void calcWaste(int direction, int i)
        {
            if (direction == (int)Enum.TargetDirection.Up)
            {
                if (tiles[i + (int)Enum.TargetDirection.Up].Card != null && tiles[i + (int)Enum.TargetDirection.Up].Card.Player == (int)Enum.Player.Human) //card above exist and human's
                {
                    pTiles[i].WasteNorth = tiles[i + (int)Enum.TargetDirection.Up].Card.South;
                }
            }
            if (direction == (int)Enum.TargetDirection.Down)
            {
                if (tiles[i + (int)Enum.TargetDirection.Down].Card != null && tiles[i + (int)Enum.TargetDirection.Down].Card.Player == (int)Enum.Player.Human)
                {
                    pTiles[i].WasteSouth = tiles[i + (int)Enum.TargetDirection.Down].Card.North;
                }
            }
            if (direction == (int)Enum.TargetDirection.Left)
            {
                if (tiles[i + (int)Enum.TargetDirection.Left].Card != null && tiles[i + (int)Enum.TargetDirection.Left].Card.Player == (int)Enum.Player.Human)
                {
                    pTiles[i].WasteWest = tiles[i + (int)Enum.TargetDirection.Left].Card.East;
                }
            }
            if (direction == (int)Enum.TargetDirection.Right)
            {
                if (tiles[i + (int)Enum.TargetDirection.Right].Card != null && tiles[i + (int)Enum.TargetDirection.Right].Card.Player == (int)Enum.Player.Human)
                {
                    pTiles[i].WasteEast = tiles[i + (int)Enum.TargetDirection.Right].Card.West;
                }
            }
        }

    }//end class
}//end namespace
