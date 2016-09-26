using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class Human : Control
    {

        //responsible for handling mouse event by human

        MouseState mouse;
        public Human(ref Card[] cards, ref Tile[] tiles) : base(ref cards, ref tiles)
        {
            cardCount_player = cards.Length;
        }

        public override void Reset(ref Card[] cards, ref Tile[] _tiles)
        {
            cardCount_player = cards.Length;

            base.Reset(ref cards, ref _tiles);
        }

        //begin update=================================================
        public override bool Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();

            foreach (Tile t in tiles)
            {
                if (t.State != (int)Enum.TileState.Occupied)
                {
                    if (t.Rectangle.Contains(Mouse.GetState().Position))
                    {
                        t.State = (int)Enum.TileState.Hover;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            Debug.WriteLine("Clicked");
                            tile = t;
                            tile.State = (int)Enum.TileState.Selected;
                            if (card != null)
                            {
                                Debug.WriteLine("Card != null");
                                moveCard();
                                return true;
                            }
                        }
                    }
                    else
                    {
                        t.State = (int)Enum.TileState.Rest;
                    }
                    tile = null; //contingency
                }
                t.Update();
            }

            foreach (Card c in cards)
            {
                if (c.State != (int)Enum.CardState.Dealt) //if card is not dealt
                {
                    if (c.Rectangle.Contains(Mouse.GetState().Position)) //if mouse contacts card
                    {
                        if (c.State == (int)Enum.CardState.Rest) //if card is at rest
                            c.State = (int)Enum.CardState.Hover; //then card is hover
                        else if (mouse.LeftButton == ButtonState.Pressed) //else if left click
                        {
                            if (card != null && c.ID != card.ID) //if card was selected and not currently clicking
                            {
                                card.State = (int)Enum.CardState.Rest; //reset and forget selected card
                                card = null;
                            }
                            card = c; //remember this card
                            card.State = (int)Enum.CardState.Selected; //change state of selected card
                        }
                    }
                    else //mouse doesn't contact card
                    {
                        if (mouse.LeftButton == ButtonState.Pressed) //if left click
                        {
                            c.State = (int)Enum.CardState.Rest; //deselects all card
                            if (card != null && c == card) //if a card was selected forget selected
                                card = null;
                        }
                        else //if do nothing
                        {
                            if (card == null) //if no card selected
                                c.State = (int)Enum.CardState.Rest; //deselects all card
                            else if (card != null) //if card is selected
                            {   // && c != card
                                if (c != card) //if c is not the selected card
                                    c.State = (int)Enum.CardState.Rest; //deselects c
                            }
                        }
                    }
                }
                c.Update();
            }
            return false;


        }//end update=================================================







    }//end class
}//end namespace
