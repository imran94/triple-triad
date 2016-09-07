using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Game4
{
    class Tile : Obj
    {
        private Card card;
            public Card Card { get { return card; } set { card = value; } }

        private int id;
            public int ID { get { return id; } }

        private Rectangle rectangle;
            public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }

        //private Color color, col_rest = Color.Transparent, col_hover = Color.Red, col_selected = Color.Blue, col_occupied = Color.Green;
        private Color color, col_rest = Color.Transparent;


        


        //public enum TileState : int { Rest, Hover, Selected, Occupied };
        private int state;
            public int State { get { return state; } set { state = value; } }
        public Tile()
        {
            id = -1;
            card = null;
        }

        public Tile(int id) 
        {
            this.id = id;

            card = null;

            state = (int)Enum.TileState.Rest;
            color = col_rest;

        }

        public void Reset()
        {
            card = null;
            state = (int)Enum.TileState.Rest;
            color = col_rest;
        }


        public void LoadContent(Texture2D texture)
        {
            sprite = texture;
        }

        public void Unload()
        {
            sprite.Dispose();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rectangle, color);
        }

        public void Update()
        {
            //updates the appearance based on state, display handled by GUI, logics handled by Control

            /* //enable for testing events
            if (state == (int)States.hover)
                color = col_hover;
            else if (state == (int)States.selected)
                color = col_selected;
            else if (state == (int)States.occupied)
                color = col_occupied;
            else
                color = col_rest;
             //*/
        }


        //for AI
        
        


    }//end class========================================================
}//end namespace========================================================