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
    class Card : Obj
    {

        string asset_card = "card_blank", asset_val="cardVal_";
        private Texture2D[] cardVal = new Texture2D[9];

        //added a few layers to fail-proof setting of card values
        private int north=1, south=1, east=1, west=1;
            public int North { get { return north; } }
            public int South { get { return south; } }
            public int East { get { return east; } }
            public int West { get { return west; } }

        private int id;
            public int ID { get { return id; } }
        private bool moved;
            public bool Moved { get { return moved; } set { moved = value; } }
        private Rectangle rectangle;
            public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        //public enum Owner : int { Human = 1, Bot = -1 } //the value doubles up as the score for the game
        private int player;
            public int Player { get { return player; } set { player = value; } }

        //public enum CardState : int { Rest, Hover, Selected, Dealt };
        private int state;
            public int State { get { return state; } set { state = value; } }

        private bool swapped;
            public bool Swapped { get { return swapped; } set { swapped = value; } }

        //http://www.flounder.com/csharp_color_table.htm because i'm lazy to evaluate rgba
        //public Color color, col_rest = Color.White, col_hover = Color.Red, col_selected = Color.Blue, col_dealt = Color.Green, col_human = Color.PaleTurquoise, col_bot = Color.PapayaWhip;
        private Color color, col_rest = Color.White, col_hover = Color.LightCyan ,col_selected = Color.DarkTurquoise, col_human = Color.PaleTurquoise, col_bot = Color.PapayaWhip;
        
        public Card()
        {
            id = -1;
        }

        public Card(int player, int id, int n, int s, int e, int w)
        {
            this.id = id;
            this.player = player;
            setValues(n, s, e, w);

            swapped = false;

            spriteAsset = asset_card;
            if (player == (int)Enum.Player.Human)
                color = col_human;
            if (player == (int)Enum.Player.Bot)
                color = col_bot;

            state = (int)Enum.CardState.Rest;
        }

        public void Reset(int player, int n, int s, int e, int w)
        {
            this.player = player;
            setValues(n, s, e, w);

            swapped = false;

            spriteAsset = asset_card;
            if (player == (int)Enum.Player.Human)
                color = col_human;
            if (player == (int)Enum.Player.Bot)
                color = col_bot;

            state = (int)Enum.CardState.Rest;
        }

        public void setValues(int n, int s, int e, int w)
        {
            north = n % 10 < 1 ? 1 : n % 10;
            south = s % 10 < 1 ? 1 : s % 10;
            east = e % 10 < 1 ? 1 : e % 10;
            west = w % 10 < 1 ? 1 : w % 10;
        }

        public virtual void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>(spriteFolder + spriteAsset);

            for(int i=0;i<9;i++)
                cardVal[i] = content.Load<Texture2D>(spriteFolder+asset_val + (i + 1).ToString());
        }

        /*
        public virtual void ReLoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>(spriteFolder + spriteAsset);
        }//*/
        
        public void Unload()
        {
            sprite.Dispose();

            foreach (Texture2D t in cardVal)
                t.Dispose();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rectangle, color);

            int width = rectangle.Width;
            int height = rectangle.Height;
            int numW = 36, numH = 40, offsetX = rectangle.Width / 4+1, offsetY = rectangle.Height/4+2;
            spriteBatch.Draw(cardVal[north - 1], new Rectangle(width / 2 - numW / 2 + rectangle.X, height / 2 - numH / 2 - offsetY + rectangle.Y, numW, numH), col_rest);
            spriteBatch.Draw(cardVal[south - 1], new Rectangle(width / 2 - numW / 2 + rectangle.X, height / 2 - numH / 2 + offsetY + rectangle.Y, numW, numH), col_rest);
            spriteBatch.Draw(cardVal[east - 1], new Rectangle(width / 2 - numW / 2 + offsetX + rectangle.X, height / 2 - numH / 2 + rectangle.Y, numW, numH), col_rest);
            spriteBatch.Draw(cardVal[west - 1], new Rectangle(width / 2 - numW / 2 - offsetX + rectangle.X, height / 2 - numH / 2 + rectangle.Y, numW, numH), col_rest);
        }

        //begin update================================================================
        public virtual void Update() 
        {
            //updates the appearance based on state, display handled by GUI, logics handled by Control

            if (state == (int)Enum.CardState.Hover)
            {
                color = col_hover;
            }
            else if (state == (int)Enum.CardState.Selected)
            {
                color = col_selected;
            }
            else if (state == (int)Enum.CardState.Dealt)
            {
                //color = col_dealt;
                //color = col_rest;

                if (player == (int)Enum.Player.Human)
                    color = col_human;
                if (player == (int)Enum.Player.Bot)
                    color = col_bot;
            }
            else
            {
                //color = col_rest;
                if (player == (int)Enum.Player.Human)
                    color = col_human;
                if (player == (int)Enum.Player.Bot)
                    color = col_bot;
            }

            if (swapped)
            {
                if (player == (int)Enum.Player.Human)
                    color = col_human;
                if (player == (int)Enum.Player.Bot)
                    color = col_bot;
            }
        }//end update================================================================


        


    }//end class========================================================
}//end namespace========================================================
