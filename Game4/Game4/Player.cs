using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Game4
{
    class Player : Deck
    {
        public Player() : base() { }

        public Player(List<Card> deck, Tile[] tiles) : base(deck, tiles)
        {
            state = (int) States.Deal;
        }

        MouseState mouse;
        public override void Update(ref List<Card> ownDeck, ref List<Card> otherDeck, ref Tile[] tiles)
        {
            Card c;
            Tile t;
            mouse = Mouse.GetState();

            // if player is in the dealing phase
            if (state == (int) States.Deal)
            {
                for (int i = 0; i < ownDeck.Count(); i++)
                {
                    c = ownDeck.ElementAt(i);

                    // Highlight card when hovering over 
                    if (!c.Dealt && c.Rectangle.Contains(Mouse.GetState().Position))
                    {
                        c.color = c.colorHover;

                        // select card when clicked
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            selectCard(ref c, ref ownDeck);
                        }       
                    }

                    // if a card has been selected
                    if (c.Selected && !c.Dealt)
                    {
                        // Highlight a tile when hovering over
                        for(int j = 0; j < tiles.Length; j++)
                        {
                            if (tiles[j].Rectangle.Contains(Mouse.GetState().Position) && !selectedCard.Dealt)
                            {
                                tiles[j].color = tiles[j].colorHover;

                                // Deal a card by moving it to the tile
                                // and change player state to capturing
                                if (Mouse.GetState().LeftButton == ButtonState.Pressed && tiles[j].Available)
                                {
                                    dealCard(ref c, ref tiles[j]);
                                    ownDeck[i] = c;

                                    state = (int)States.Capture;
                                }
                            }
                            else
                            {
                                tiles[j].color = tiles[j].colorNeutral;
                            }
                        }
                    }
                    else if (!c.Rectangle.Contains(Mouse.GetState().Position))
                    {
                        c.color = c.colorNeutral;
                    }
                }

                
            }
        }
    }
}
