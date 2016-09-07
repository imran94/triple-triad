using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    abstract class Deck
    {
        protected enum States : int { Deal, Capture, Wait };

        protected int state { get; set; }
        protected List<Card> deck;
        protected Tile[] tiles;
        protected Card selectedCard;

        public Deck() { }

        public Deck(List<Card> deck, Tile[] tiles)
        {
            this.deck = deck;
            this.tiles = tiles;
        }

        abstract public void Update(ref List<Card> ownDeck, ref List<Card> otherDeck, ref Tile[] tiles);

        public void dealCard(ref Card card, ref Tile tile)
        {
            //tile.addCard(ref card);
        }

        public bool hasCard(Card checkCard, List<Card> cardList)
        {
            foreach (Card c in cardList)
            {
                if (checkCard.ID == c.ID)
                {
                    return true;
                }
            }

            return false;
        }

        public bool captureCard(Card toCapture, List<Card> otherDeck)
        {
            for (int i = 0; i < otherDeck.Count(); i++)
            {
                if (otherDeck[i].ID == toCapture.ID)
                {
                    otherDeck.RemoveAt(i);
                    deck.Add(toCapture);
                    return true;
                }
            }

            return false;
        }

        public void selectCard(ref Card card, ref List<Card> deck)
        {
            card.color = card.colorMarked;
            card.Selected = true;
            selectedCard = card;

            for (int i = 0; i < deck.Count(); i++)
            {
                if (deck[i].ID == card.ID)
                {
                    deck[i] = card;
                }
                else
                {
                    deck[i].Selected = false;
                }
            }
        }

    }
}
